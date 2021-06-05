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
                if (_target.TryGetTarget(out RevertibleChangeTracking target))
                {
                    T oldValue, newValue;
                    lock (target.SyncRoot)
                    {
                        IsSet = _originalValueIsSet;
                        oldValue = _value;
                        _value = newValue = _originalValue;
                        if (Coersion.Equals(oldValue, _value) || !(target._changeTrackers.TryGetValue(PropertyName, out IPropertyChangeTracker changeTracker) && ReferenceEquals(target, changeTracker)))
                            return;
                    }
                    target.RaisePropertyChanged(oldValue, newValue, PropertyName);
                }
                else
                {
                    IsSet = _originalValueIsSet;
                    _value = _originalValue;
                }
            }

            public T GetValue() => _value;

            public bool SetValue(T newValue)
            {
                if (_target.TryGetTarget(out RevertibleChangeTracking target))
                {
                    T oldValue;
                    lock (target.SyncRoot)
                    {
                        IsSet = true;
                        oldValue = _value;
                        newValue = Coersion.Normalize(newValue);
                        if (Coersion.Equals(oldValue, newValue))
                            return false;
                        _value = newValue;
                        if (!(target._changeTrackers.TryGetValue(PropertyName, out IPropertyChangeTracker changeTracker) && ReferenceEquals(target, changeTracker)))
                            return true;
                    }
                    target.RaisePropertyChanged(oldValue, newValue, PropertyName);
                }
                else
                {
                    newValue = Coersion.Normalize(newValue);
                    if (Coersion.Equals(_value, newValue))
                        return false;
                    _value = newValue;
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
