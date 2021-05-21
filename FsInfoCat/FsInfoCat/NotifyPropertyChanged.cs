using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FsInfoCat
{
    public abstract class NotifyPropertyChanged : INotifyPropertyValueChanged
    {
        public event PropertyValueChangedEventHandler PropertyValueChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged<T>(T oldValue, T newValue, [CallerMemberName] string propertyName = null) =>
            OnPropertyChanged(new PropertyValueChangedEventArgs(propertyName, oldValue, newValue));

        protected virtual void OnPropertyChanged(PropertyValueChangedEventArgs args)
        {
            try { PropertyValueChanged?.Invoke(this, args); }
            finally { PropertyChanged?.Invoke(this, args); }
        }

        protected virtual bool CheckHashSetChanged<T>(HashSet<T> oldValue, HashSet<T> newValue, Action<HashSet<T>> setter, [CallerMemberName] string propertyName = null)
        {
            if (newValue is null)
            {
                if (oldValue.Count == 0)
                    return false;
                setter(new HashSet<T>());
            }
            else
            {
                if (ReferenceEquals(oldValue, newValue))
                    return false;
                setter(newValue);
            }
            RaisePropertyChanged(oldValue, newValue, propertyName);
            return true;
        }

        protected IPropertyChangeTracker<T> CreateChangeTracker<T>([DisallowNull] string propertyName, [AllowNull] T initialValue, ICoersion<T> coersion = null) =>
            new PropertyChangeTracker<T>(this, propertyName, initialValue, coersion);

        public interface IPropertyChangeTracker : IChangeTracking
        {
            bool IsSet { get; }

            object GetValue();

            bool SetValue(object newValue);
        }

        public interface IPropertyChangeTracker<T> : IPropertyChangeTracker
        {
            new T GetValue();

            bool SetValue(T newValue);
        }

        private class PropertyChangeTracker<T> : IPropertyChangeTracker<T>
        {
            private readonly object _syncRoot = new object();
            private readonly WeakReference<NotifyPropertyChanged> _target;
            private T _value;

            internal PropertyChangeTracker(NotifyPropertyChanged target, string propertyName, T initialValue, ICoersion<T> coersion)
            {
                _value = initialValue;
                _target = new WeakReference<NotifyPropertyChanged>(target);
                PropertyName = propertyName;
                Coersion = coersion ?? Coersion<T>.Default;
            }

            public string PropertyName { get; }

            public IEqualityComparer<T> EqualityComparer { get; }

            public ICoersion<T> Coersion { get; }

            public bool IsSet { get; private set; }

            public bool IsChanged { get; private set; }

            public void AcceptChanges() => IsChanged = false;

            public T GetValue() => _value;

            public bool SetValue(T newValue)
            {
                IsSet = true;
                T oldValue = _value;
                if (Coersion.Equals(oldValue, newValue))
                    return false;
                IsChanged = true;
                _value = newValue;
                if (_target.TryGetTarget(out NotifyPropertyChanged target))
                    target.RaisePropertyChanged(oldValue, newValue, PropertyName);
                return true;
            }

            bool IPropertyChangeTracker.SetValue(object newValue) => SetValue(Coersion.Cast(newValue));

            object IPropertyChangeTracker.GetValue() => GetValue();
        }
    }

}
