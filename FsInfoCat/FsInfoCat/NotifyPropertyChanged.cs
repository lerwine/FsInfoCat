using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FsInfoCat
{
    public abstract class NotifyPropertyChanged : INotifyPropertyValueChanged, IDataErrorInfo, INotifyDataErrorInfo, IRevertibleChangeTracking
    {
        private readonly ILogger<NotifyPropertyChanged> _logger = Services.ServiceProvider.GetRequiredService<ILogger<NotifyPropertyChanged>>();
        private readonly object _syncroot = new();
        private readonly Dictionary<string, IPropertyChangeTracker> _changeTrackers = new();
        private readonly Dictionary<string, string[]> _lastValidationResults = new();

        public event PropertyValueChangedEventHandler PropertyValueChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        string IDataErrorInfo.Error
        {
            get
            {
                Monitor.Enter(_syncroot);
                try
                {
                    if (_lastValidationResults.Count > 0)
                        return string.Join("\n\n", _lastValidationResults.Keys.Select(n =>
                        {
                            string[] msgs = _lastValidationResults[n];
                            if (msgs.Length == 1)
                                return $"{n}: {msgs[0]}";
                            return $"{n}: {string.Join("\n\t", msgs)}";
                        }));
                }
                finally { Monitor.Exit(_syncroot); }
                return null;
            }
        }

        bool INotifyDataErrorInfo.HasErrors => HasErrors();

        bool IChangeTracking.IsChanged => IsChanged();

        public string this[string columnName]
        {
            get
            {
                Monitor.Enter(_syncroot);
                try { return _lastValidationResults.ContainsKey(columnName) ? _lastValidationResults[columnName].JoinWithNewLines() : null; }
                finally { Monitor.Exit(_syncroot); }
            }
        }

        public virtual void AcceptChanges()
        {
            lock (_changeTrackers)
            {
                foreach (IPropertyChangeTracker tracker in _changeTrackers.Values)
                    tracker.AcceptChanges();
            }
        }

        protected IPropertyChangeTracker<T> AddChangeTracker<T>([DisallowNull] string propertyName, [AllowNull] T initialValue, ICoersion<T> coersion = null) =>
            new PropertyChangeTracker<T>(this, propertyName, initialValue, coersion);

        protected bool ClearError([NotNull] string propertyName)
        {
            Monitor.Enter(_syncroot);
            try
            {
                throw new NotImplementedException();
            }
            finally { Monitor.Exit(_syncroot); }
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            if (_lastValidationResults.TryGetValue(propertyName, out string[] result))
                return result;
            return null;
        }

        public bool HasErrors() => _lastValidationResults.Count > 0;

        public bool IsChanged() => _changeTrackers.Values.Any(t => t.IsChanged);

        protected virtual void OnPropertyChanged(PropertyValueChangedEventArgs args)
        {
            try { PropertyValueChanged?.Invoke(this, args); }
            finally { PropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs args) => ErrorsChanged?.Invoke(this, args);

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

        protected void RaisePropertyChanged<T>(T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            using var scope = _logger.BeginScope("Property {propertyName} changed", propertyName);
            DataErrorsChangedEventArgs[] errorsChanged;
            try
            {
                Collection<ValidationResult> validationResults = new();
                if (Validator.TryValidateProperty(newValue, new ValidationContext(this) { MemberName = propertyName }, validationResults))
                {
                    Monitor.Enter(_syncroot);
                    try
                    {
                        errorsChanged = _lastValidationResults.Remove(propertyName) ? new DataErrorsChangedEventArgs[] { new DataErrorsChangedEventArgs(propertyName) } : Array.Empty<DataErrorsChangedEventArgs>();
                    }
                    finally { Monitor.Exit(_syncroot); }
                }
                else
                {
                    string[] changed;
                    Monitor.Enter(_syncroot);
                    try
                    {
                        KeyValuePair<string, string[]>[] keyValuePairs = _lastValidationResults.ToArray();
                        IEnumerable<KeyValuePair<string, string[]>> added = validationResults.SelectMany(r =>
                                r.MemberNames.Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => new { MemberName = string.IsNullOrWhiteSpace(n) ? "" : n, r.ErrorMessage }))
                            .GroupBy(a => a.MemberName, a => a.ErrorMessage ?? "").ToKeyValuePairs(g => g.Key, g => g.Distinct().ToArray());
                        if (string.IsNullOrWhiteSpace(propertyName))
                        {
                            _lastValidationResults.Clear();
                            foreach (var kvp in (added = added.ToArray()))
                                _lastValidationResults.Add(kvp.Key, kvp.Value);
                            changed = keyValuePairs.Where(a => !added.Any(b => a.Key == b.Key && a.Value.OrderBy(t => t).SequenceEqual(b.Value.OrderBy(t => t))))
                                .Concat(added.Where(a => !keyValuePairs.Any(b => a.Key == b.Key))).Select(a => a.Key).ToArray();
                        }
                        else if (added.Any(kvp => kvp.Key == propertyName))
                            changed = added.Where(kvp =>
                            {
                                if (kvp.Key == propertyName)
                                {
                                    if (_lastValidationResults.ContainsKey(propertyName))
                                    {
                                        if (_lastValidationResults[propertyName].OrderBy(t => t).SequenceEqual(kvp.Value.OrderBy(t => t)))
                                            return false;
                                        _lastValidationResults[propertyName] = kvp.Value;
                                    }
                                    else
                                        _lastValidationResults.Add(propertyName, kvp.Value);
                                }
                                else if (_lastValidationResults.ContainsKey(propertyName))
                                {
                                    string[] m = _lastValidationResults[propertyName];
                                    if (_lastValidationResults[propertyName].OrderBy(t => t).SequenceEqual(kvp.Value.OrderBy(t => t)))
                                        return false;
                                    _lastValidationResults[propertyName] = kvp.Value;
                                }
                                else
                                    _lastValidationResults.Add(propertyName, kvp.Value);
                                return true;
                            }).Select(kvp => kvp.Key).ToArray();
                        else
                        {
                            changed = added.Where(kvp =>
                            {
                                if (_lastValidationResults.ContainsKey(propertyName))
                                {
                                    string[] m = _lastValidationResults[propertyName];
                                    if (!kvp.Value.Any(v => !m.Contains(v)))
                                        return false;
                                    _lastValidationResults[propertyName] = m.Concat(kvp.Value).Distinct().ToArray();
                                }
                                else
                                    _lastValidationResults.Add(propertyName, kvp.Value);
                                return true;
                            }).Select(kvp => kvp.Key).ToArray();
                            if (keyValuePairs.Any(kvp => kvp.Key == propertyName))
                                changed = changed.Concat(new string[] { propertyName }).ToArray();
                        }
                        errorsChanged = changed.Select(p => new DataErrorsChangedEventArgs(p)).ToArray();
                    }
                    finally { Monitor.Exit(_syncroot); }
                }
            }
            finally { OnPropertyChanged(new PropertyValueChangedEventArgs(propertyName, oldValue, newValue)); }
            foreach (var item in errorsChanged)
                OnErrorsChanged(item);
        }

        public virtual void RejectChanges()
        {
            IPropertyChangeTracker[] changeTrackers = _changeTrackers.Values.ToArray();
            foreach (IPropertyChangeTracker tracker in changeTrackers)
                tracker.RejectChanges();
        }

        protected bool RemoveChangeTracker<T>(IPropertyChangeTracker<T> changeTracker)
        {
            lock (_changeTrackers)
            {
                if (_changeTrackers.TryGetValue(changeTracker.PropertyName, out IPropertyChangeTracker ct) && ReferenceEquals(ct, changeTracker))
                {
                    _changeTrackers.Remove(changeTracker.PropertyName);
                    return true;
                }
            }
            return false;
        }

        protected bool SetError(string propertyName, string message)
        {
            Monitor.Enter(_syncroot);
            try
            {
                throw new NotImplementedException();
            }
            finally { Monitor.Exit(_syncroot); }
        }

        public interface IPropertyChangeTracker : IRevertibleChangeTracking
        {
            string PropertyName { get; }

            bool IsSet { get; }

            object GetValue();

            bool SetValue(object newValue);

            bool IsEqualTo(object obj);
        }

        public interface IPropertyChangeTracker<T> : IPropertyChangeTracker
        {
            new T GetValue();

            bool SetValue(T newValue);

            bool IsEqualTo(T other);
        }

        private class PropertyChangeTracker<T> : IPropertyChangeTracker<T>
        {
            private readonly WeakReference<NotifyPropertyChanged> _target;
            private bool _originalValueIsSet;
            private T _originalValue;
            private T _value;

            internal PropertyChangeTracker([NotNull] NotifyPropertyChanged target, [NotNull] string propertyName, T initialValue, ICoersion<T> coersion)
            {
                lock (target._changeTrackers)
                {
                    if (target._changeTrackers.ContainsKey(propertyName))
                        throw new ArgumentOutOfRangeException(nameof(propertyName));
                    target._changeTrackers.Add(propertyName, this);
                }
                Coersion = coersion ?? Coersion<T>.Default;
                _value = Coersion.Normalize(initialValue);
                _target = new WeakReference<NotifyPropertyChanged>(target);
                PropertyName = propertyName;
            }

            public string PropertyName { get; }

            public ICoersion<T> Coersion { get; }

            public bool IsSet { get; private set; }

            public bool IsChanged => !Coersion.Equals(_originalValue, _value);

            public void AcceptChanges()
            {
                _originalValueIsSet = IsSet;
                _originalValue = _value;
            }

            public void RejectChanges()
            {
                IsSet = _originalValueIsSet;
                T oldValue = _value;
                _value = _originalValue;
                if (!Coersion.Equals(oldValue, _value) && _target.TryGetTarget(out NotifyPropertyChanged target) && target._changeTrackers.TryGetValue(PropertyName, out IPropertyChangeTracker changeTracker) && ReferenceEquals(target, changeTracker))
                    target.RaisePropertyChanged(oldValue, _value, PropertyName);
            }

            public T GetValue() => _value;

            public bool SetValue(T newValue)
            {
                IsSet = true;
                T oldValue = _value;
                newValue = Coersion.Normalize(newValue);
                if (Coersion.Equals(oldValue, newValue))
                    return false;
                _value = newValue;
                if (_target.TryGetTarget(out NotifyPropertyChanged target) && target._changeTrackers.TryGetValue(PropertyName, out IPropertyChangeTracker changeTracker) && ReferenceEquals(target, changeTracker))
                    target.RaisePropertyChanged(oldValue, newValue, PropertyName);
                return true;
            }

            bool IPropertyChangeTracker.SetValue(object newValue) => SetValue(Coersion.Cast(newValue));

            object IPropertyChangeTracker.GetValue() => GetValue();

            public bool IsEqualTo(T other) => Coersion.Equals(_value, other);

            bool IPropertyChangeTracker.IsEqualTo(object obj) => Coersion.TryCast(obj, out T t) && IsEqualTo(t);
        }
    }
}
