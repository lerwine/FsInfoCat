using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{

    public abstract partial class RevertibleChangeTracking
    {
        private class PropertyChangeTracker<T> : IPropertyChangeTracker<T>
        {
            private readonly WeakReference<RevertibleChangeTracking> _target;
            private bool _originalValueIsSet;
            private T _originalValue;
            private T _value;

            public event EventHandler ValueChanged;

            internal PropertyChangeTracker([NotNull] RevertibleChangeTracking target, [NotNull] string propertyName, T initialValue, ICoersion<T> coersion)
            {
                lock (target.SyncRoot)
                {
                    if (target._changeTrackers.ContainsKey(propertyName))
                        throw new ArgumentOutOfRangeException(nameof(propertyName));
                    target._changeTrackers.Add(propertyName, this);
                }
                Coersion = coersion ?? Coersion<T>.Default;
                _value = Coersion.Normalize(initialValue);
                _target = new WeakReference<RevertibleChangeTracking>(target);
                PropertyName = propertyName;
            }

            public string PropertyName { get; }

            public ICoersion<T> Coersion { get; }

            public bool IsSet { get; private set; }

            public bool IsChanged => !Coersion.Equals(_originalValue, _value);

            public void AcceptChanges()
            {
                if (_target.TryGetTarget(out RevertibleChangeTracking target))
                    lock (target.SyncRoot)
                    {
                        _originalValueIsSet = IsSet;
                        _originalValue = _value;
                    }
                else
                {
                    _originalValueIsSet = IsSet;
                    _originalValue = _value;
                }
            }

            public void RejectChanges()
            {
                T oldValue, newValue;
                if (_target.TryGetTarget(out RevertibleChangeTracking target))
                {
                    lock (target.SyncRoot)
                    {
                        IsSet = _originalValueIsSet;
                        oldValue = _value;
                        newValue = _originalValue;
                        if (Coersion.Equals(oldValue, newValue) || !(target._changeTrackers.TryGetValue(PropertyName, out IPropertyChangeTracker changeTracker) && ReferenceEquals(target, changeTracker)))
                            return;
                    }
                    target.RaisePropertyChanging(PropertyName);
                    lock (target.SyncRoot)
                    {
                        IsSet = _originalValueIsSet;
                        _value = _originalValue;
                    }
                    try { ValueChanged?.Invoke(this, EventArgs.Empty); }
                    finally { target.RaisePropertyChanged(oldValue, newValue, PropertyName); }
                }
                else
                {
                    oldValue = _value;
                    _value = newValue = _originalValue;
                    IsSet = _originalValueIsSet;
                    _value = _originalValue;
                    if (!Coersion.Equals(oldValue, newValue))
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            public T GetValue() => _value;

            public bool SetValue(T newValue)
            {
                T oldValue;
                if (_target.TryGetTarget(out RevertibleChangeTracking target))
                {
                    lock (target.SyncRoot)
                    {
                        oldValue = _value;
                        newValue = Coersion.Normalize(newValue);
                        if (Coersion.Equals(oldValue, newValue))
                            return false;
                        if (!(target._changeTrackers.TryGetValue(PropertyName, out IPropertyChangeTracker changeTracker) && ReferenceEquals(target, changeTracker)))
                        {
                            _value = newValue;
                            return true;
                        }
                    }
                    target.RaisePropertyChanging(PropertyName);
                    lock (target.SyncRoot)
                    {
                        IsSet = true;
                        _value = newValue;
                    }
                    try { ValueChanged?.Invoke(this, EventArgs.Empty); }
                    finally { target.RaisePropertyChanged(oldValue, newValue, PropertyName); }
                }
                else
                {
                    IsSet = true;
                    oldValue = _value;
                    _value = newValue = Coersion.Normalize(newValue);
                    if (Coersion.Equals(oldValue, newValue))
                        return false;
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
                return true;
            }

            bool IPropertyChangeTracker.SetValue(object newValue) => SetValue(Coersion.Cast(newValue));

            object IPropertyChangeTracker.GetValue() => GetValue();

            public bool IsEqualTo(T other) => Coersion.Equals(_value, other);

            bool IPropertyChangeTracker.IsEqualTo(object obj) => Coersion.TryCast(obj, out T t) && IsEqualTo(t);
        }
    }
}
