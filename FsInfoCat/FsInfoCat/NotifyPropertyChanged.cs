using FsInfoCat.Collections;
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
    public abstract class NotifyPropertyChanged : INotifyPropertyValueChanged, IDataErrorInfo, IValidatableObject, INotifyDataErrorInfo
    {
        private readonly object _syncroot = new();
        private static readonly ValidationResultDictionary _lastValidationResults = new();

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
                    if (_lastValidationResults.ValidationResultCount > 0)
                        return string.Join("\n\n", _lastValidationResults.Keys.Select(g =>
                        {
                            string[] msgs = _lastValidationResults[g].ToArray();
                            if (msgs.Length == 1)
                                return $"{g}: {msgs[0]}";
                            return $"{g}: {string.Join("\n\t", msgs)}";
                        }));
                }
                finally { Monitor.Exit(_syncroot); }
                return null;
            }
        }

        protected bool HasErrors => _lastValidationResults.ValidationResultCount > 0;

        bool INotifyDataErrorInfo.HasErrors => HasErrors;

        public string this[string columnName]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(columnName))
                    columnName = "";
                Monitor.Enter(_syncroot);
                try
                {
                    string[] msgs = ValidationResultComparer.SelectManyByMessageAndPropertyNames(_lastValidationResults, (m, p) => p.Select(n => new { Key = n, Value = m }))
                        .Where(a => a.Key == columnName).Select(a => a.Value).ToArray();
                    if (msgs.Length > 0)
                    {
                        if (msgs.Length == 1)
                            return msgs[0];
                        return string.Join("\n", msgs);
                    }
                }
                finally { Monitor.Exit(_syncroot); }
                return null;
            }
        }

        protected void RaisePropertyChanged<T>(T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            IList<DataErrorsChangedEventArgs> errorsChanged;
            try
            {
                Collection<ValidationResult> validationResults = new();
                if (Validator.TryValidateProperty(newValue, new ValidationContext(this) { MemberName = propertyName }, validationResults))
                    errorsChanged = ClearValidationResults(propertyName);
                else
                    errorsChanged = SetValidationResults(validationResults, propertyName);
            }
            finally { OnPropertyChanged(new PropertyValueChangedEventArgs(propertyName, oldValue, newValue)); }
            foreach (var item in errorsChanged)
                OnErrorsChanged(item);
        }

        private IList<DataErrorsChangedEventArgs> ClearValidationResults([NotNull] string propertyName)
        {
            Monitor.Enter(_syncroot);
            try
            {
                if (!_lastValidationResults.Remove(propertyName))
                    return Array.Empty<DataErrorsChangedEventArgs>();
            }
            finally { Monitor.Exit(_syncroot); }
            return new DataErrorsChangedEventArgs[] { new DataErrorsChangedEventArgs(propertyName) };
        }

        private  IList<DataErrorsChangedEventArgs> SetValidationResults([NotNull] IEnumerable<ValidationResult> validationResults, string propertyName = null)
        {
            List<DataErrorsChangedEventArgs> result = new();
            Monitor.Enter(_syncroot);
            try
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                    propertyName = "";
                bool currentPropertyChanged =_lastValidationResults.Remove(propertyName);
                var itemsToAdd = validationResults.Where(v => !(v is null || _lastValidationResults.Contains(v, ValidationResultComparer.Instance)))
                    .Distinct(ValidationResultComparer.Instance).ToArray();
                if (itemsToAdd.Length == 0)
                    return Array.Empty<DataErrorsChangedEventArgs>();

                foreach (var byPropertyName in ValidationResultComparer.SelectManyByMessageAndPropertyNames(validationResults.Where(v => !(v is null))
                    .Distinct(ValidationResultComparer.Instance), (m, p) =>
                    p.Select(n => new { Name = n, Message = m })).GroupBy(a => a.Name, a => a.Message))
                {
                    if (byPropertyName.Key == propertyName)
                    {
                        if (_lastValidationResults.ContainsKey(propertyName))
                        {
                            List<string> messages = _lastValidationResults[propertyName].ToList();
                            string[] toAdd = byPropertyName.Where(m => !messages.Contains(m)).ToArray();
                            string[] toRemove = messages.Where(m => !byPropertyName.Contains(m)).ToArray();
                            if (toAdd.Length > 0)
                            {
                                if (toRemove.Length > 0)
                                    messages.RemoveAll(m => toRemove.Contains(m));
                                messages.AddRange(toAdd);
                            }
                            else if (toRemove.Length > 0)
                            {
                                if (toRemove.Length == messages.Count)
                                    _lastValidationResults.Remove(propertyName);
                                else
                                    messages.RemoveAll(m => toRemove.Contains(m));
                            }
                            else
                                continue;
                        }
                        else
                        {
                            string[] toAdd = byPropertyName.ToArray();
                            if (toAdd.Length > 0)
                                _lastValidationResults.Add(propertyName, new List<string>(toAdd));
                            else
                                continue;
                        }
                    }
                    else
                    {
                        string[] toAdd = byPropertyName.ToArray();
                        if (toAdd.Length == 0)
                            continue;
                        if (_lastValidationResults.ContainsKey(byPropertyName.Key))
                        {
                            List<string> messages = _lastValidationResults[byPropertyName.Key].ToList();
                            if ((toAdd = toAdd.Where(m => !messages.Contains(m)).ToArray()).Length == 0)
                                continue;
                            messages.AddRange(toAdd);
                        }
                        else
                            _lastValidationResults.Add(propertyName, new List<string>(toAdd));
                    }
                    result.Add(new DataErrorsChangedEventArgs(byPropertyName.Key));
                }
            }
            finally { Monitor.Exit(_syncroot); }
            return result;
        }

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

        protected IPropertyChangeTracker<T> CreateChangeTracker<T>([DisallowNull] string propertyName, [AllowNull] T initialValue, ICoersion<T> coersion = null) =>
            new PropertyChangeTracker<T>(this, propertyName, initialValue, null, coersion);

        protected IPropertyChangeTracker<T> CreateChangeTracker<T>([DisallowNull] string propertyName, [AllowNull] T initialValue,
                Func<T, ValidationResult> customValidation, ICoersion<T> coersion = null) =>
            new PropertyChangeTracker<T>(this, propertyName, initialValue, customValidation, coersion);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        public interface IPropertyChangeTracker : IChangeTracking
        {
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
            private readonly object _syncRoot = new object();
            private readonly WeakReference<NotifyPropertyChanged> _target;
            private T _value;

            internal PropertyChangeTracker(NotifyPropertyChanged target, string propertyName, T initialValue, Func<T, ValidationResult> customValidation,
                ICoersion<T> coersion)
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

            public bool IsEqualTo(T other) => EqualityComparer.Equals(_value, other);

            bool IPropertyChangeTracker.IsEqualTo(object obj) => Coersion.TryCast(obj, out T t) && IsEqualTo(t);
        }
    }

    public class ValidationResultComparer : IEqualityComparer<ValidationResult>
    {
        public static readonly ValidationResultComparer Instance = new();

        private ValidationResultComparer() { }

        private static string GetErrorMessage(ValidationResult validationResult) => (validationResult.ErrorMessage is null) ? "" : validationResult.ErrorMessage.Trim();

        private static IEnumerable<string> DistinctNames(IEnumerable<string> source) => (source is null) ? new string[] { "" } :
            source.Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().OrderBy(n => n).DefaultIfEmpty("");

        //public static IEnumerable<IGrouping<string, Tuple<ValidationResult, int>>> GroupByPropertyNameWithIndex(IEnumerable<ValidationResult> input) =>
        //    input.Select((v, i) => (v is null) ? null : new { Index = i, Names = DistinctNames(v.MemberNames), Original = v }).Where(a => !(a is null))
        //    .SelectMany(a => a.Names.Select(n => new { a.Index, Name = n, Original = a.Original }))
        //    .GroupBy(a => a.Name, a => new Tuple<ValidationResult, int>(a.Original, a.Index));

        //public static IEnumerable<IGrouping<string, ValidationResult>> GroupByPropertyName(IEnumerable<ValidationResult> input) => input.Select(v =>
        //    (v is null) ? null : new { Names = DistinctNames(v.MemberNames), Original = v }).Where(a => !(a is null))
        //    .SelectMany(a => a.Names.Select(n => new { Name = n, Original = a.Original }))
        //    .GroupBy(a => a.Name, a => a.Original);

        internal static bool SetPropertyMessage(List<ValidationResult> list, string propertyName, string message)
        {
            if (message is null || (message = message.Trim()).Length == 0)
                throw new ArgumentOutOfRangeException(nameof(message));
            if (string.IsNullOrWhiteSpace(propertyName))
                propertyName = "";
            ValidationResult[] items = list.Where(m => DistinctNames(m.MemberNames).Contains(propertyName) && GetErrorMessage(m) == message).ToArray();
            int index;
            if (items.Length == 1)
            {
                string[] names = DistinctNames(items[0].MemberNames).ToArray();
                if (names.Length == 1 && names[0] == propertyName)
                    return false;
                index = list.IndexOf(items[0]);
            }
            else if (items.Length > 0)
            {
                index = list.IndexOf(items[0]);
                foreach (ValidationResult r in items.Skip(1))
                    list.Remove(r);
            }
            else
            {
                list.Add((propertyName.Length > 0) ? new ValidationResult(message, new string[] { propertyName }) : new ValidationResult(message));
                return true;
            }
            list[index] = (propertyName.Length > 0) ? new ValidationResult(message, new string[] { propertyName }) : new ValidationResult(message);
            return true;
        }

        internal static bool AddPropertyMessage(List<ValidationResult> list, string propertyName, string message)
        {
            if (message is null || (message = message.Trim()).Length == 0)
                throw new ArgumentOutOfRangeException(nameof(message));
            if (string.IsNullOrWhiteSpace(propertyName))
                propertyName = "";
            if (list.Any(m => DistinctNames(m.MemberNames).Contains(propertyName) && GetErrorMessage(m) == message))
                return false;
            list.Add((propertyName.Length > 0) ? new ValidationResult(message, new string[] { propertyName }) : new ValidationResult(message));
            return true;
        }

        internal static bool RemovePropertyErrors(List<ValidationResult> list, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                propertyName = "";
            var items = list.Select((m, i) => new { Names = DistinctNames(m.MemberNames).ToArray(), Item = m, Index = i }).Where(a => a.Names.Contains(propertyName)).ToArray();
            if (items.Length == 0)
                return false;
            foreach (var i in items)
            {
                if (i.Names.Length == 1)
                    list.Remove(i.Item);
                else
                    list[i.Index] = new ValidationResult(i.Item.ErrorMessage, i.Names.Where(n => n != propertyName).ToArray());
            }
            return true;
        }

        internal static bool ImportResult(List<ValidationResult> list, ValidationResult validationResult)
        {
            if (list.Contains(validationResult, Instance))
                return false;
            string message = GetErrorMessage(validationResult);
            string[] names = DistinctNames(validationResult.MemberNames).ToArray();
            string[] toAdd = names.Where(n => !list.Any(m => DistinctNames(m.MemberNames).Contains(n) && GetErrorMessage(m) == message)).ToArray();
            if (toAdd.Length == 0)
                return false;
            if (names.Length == toAdd.Length)
                list.Add(validationResult);
            else
                list.Add(new ValidationResult(validationResult.ErrorMessage, toAdd));

            return true;
        }

        public static IEnumerable<IGrouping<string, TResult>> SelectByPropertyName<TResult>(IEnumerable<ValidationResult> input,
                Func<string, ValidationResult, int, TResult> selector) =>
            input.Select((v, i) => (v is null) ? null : new { Index = i, Names = DistinctNames(v.MemberNames), Original = v }).Where(a => !(a is null))
            .SelectMany(a => a.Names.Select(n => new { a.Index, Name = n, a.Original }))
            .GroupBy(a => a.Name, a => selector(a.Name, a.Original, a.Index));

        public static IEnumerable<IGrouping<string, TResult>> SelectByPropertyName<TResult>(IEnumerable<ValidationResult> input,
                Func<string, int, TResult> selector) =>
            input.Select((v, i) => (v is null) ? null : new { Index = i, Names = DistinctNames(v.MemberNames) }).Where(a => !(a is null))
            .SelectMany(a => a.Names.Select(n => new { a.Index, Name = n }))
            .GroupBy(a => a.Name, a => selector(a.Name, a.Index));

        public static IEnumerable<IGrouping<string, TResult>> SelectByPropertyName<TResult>(IEnumerable<ValidationResult> input,
                Func<string, ValidationResult, TResult> selector) =>
            input.Select(v => (v is null) ? null : new { Names = DistinctNames(v.MemberNames), Original = v }).Where(a => !(a is null))
            .SelectMany(a => a.Names.Select(n => new { Name = n, Original = a.Original }))
            .GroupBy(a => a.Name, a => selector(a.Name, a.Original));

        public static IEnumerable<TResult> SelectByPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<IEnumerable<string>, ValidationResult, int, TResult> selector) =>
            input.Select((v, i) => (v is null) ? selector(Array.Empty<string>(), null, i) : selector(DistinctNames(v.MemberNames), v, i));

        public static IEnumerable<TResult> SelectByPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<IEnumerable<string>, int, TResult> selector) =>
            input.Select((v, i) => (v is null) ? selector(Array.Empty<string>(), i) : selector(DistinctNames(v.MemberNames), i));

        public static IEnumerable<TResult> SelectByPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<IEnumerable<string>, ValidationResult, TResult> selector) =>
            input.Select(v => (v is null) ? selector(Array.Empty<string>(), null) : selector(DistinctNames(v.MemberNames), v));

        public static IEnumerable<TResult> SelectManyByPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<IEnumerable<string>, ValidationResult, int, IEnumerable<TResult>> selector) =>
            input.SelectMany((v, i) => (v is null) ? selector(Array.Empty<string>(), null, i) : selector(DistinctNames(v.MemberNames), v, i));

        public static IEnumerable<TResult> SelectManyByPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<IEnumerable<string>, int, IEnumerable<TResult>> selector) =>
            input.SelectMany((v, i) => (v is null) ? selector(Array.Empty<string>(), i) : selector(DistinctNames(v.MemberNames), i));

        public static IEnumerable<TResult> SelectManyByPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<IEnumerable<string>, ValidationResult, IEnumerable<TResult>> selector) =>
            input.SelectMany(v => (v is null) ? selector(Array.Empty<string>(), null) : selector(DistinctNames(v.MemberNames), v));

        public static IEnumerable<IGrouping<string, TResult>> SelectByMessageAndPropertyName<TResult>(IEnumerable<ValidationResult> input,
                Func<string, string, ValidationResult, int, TResult> selector) =>
            input.Select((v, i) => (v is null) ? null : new { Index = i, Message = (v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), Names = DistinctNames(v.MemberNames), Original = v }).Where(a => !(a is null))
            .SelectMany(a => a.Names.Select(n => new { a.Index, a.Message, Name = n, a.Original }))
            .GroupBy(a => a.Name, a => selector(a.Message, a.Name, a.Original, a.Index));

        public static IEnumerable<IGrouping<string, TResult>> SelectByMessageAndPropertyName<TResult>(IEnumerable<ValidationResult> input,
                Func<string, string, int, TResult> selector) =>
            input.Select((v, i) => (v is null) ? null : new { Index = i, Message = (v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), Names = DistinctNames(v.MemberNames) }).Where(a => !(a is null))
            .SelectMany(a => a.Names.Select(n => new { a.Index, a.Message, Name = n }))
            .GroupBy(a => a.Name, a => selector(a.Message, a.Name, a.Index));

        public static IEnumerable<IGrouping<string, TResult>> SelectByMessageAndPropertyName<TResult>(IEnumerable<ValidationResult> input,
                Func<string, string, ValidationResult, TResult> selector) =>
            input.Select(v => (v is null) ? null : new { Message = (v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), Names = DistinctNames(v.MemberNames), Original = v }).Where(a => !(a is null))
            .SelectMany(a => a.Names.Select(n => new { a.Message, Name = n, a.Original }))
            .GroupBy(a => a.Name, a => selector(a.Message, a.Name, a.Original));

        public static IEnumerable<IGrouping<string, TResult>> SelectByMessageAndPropertyName<TResult>(IEnumerable<ValidationResult> input,
                Func<string, string, TResult> selector) =>
            input.Select(v => (v is null) ? null : new { Message = (v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), Names = DistinctNames(v.MemberNames) }).Where(a => !(a is null))
            .SelectMany(a => a.Names.Select(n => new { a.Message, Name = n }))
            .GroupBy(a => a.Name, a => selector(a.Message, a.Name));

        public static IEnumerable<TResult> SelectByMessageAndPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<string, IEnumerable<string>, ValidationResult, int, TResult> selector) =>
            input.Select((v, i) => (v is null) ? selector(null, Array.Empty<string>(), null, i) : selector((v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), DistinctNames(v.MemberNames), v, i));

        public static IEnumerable<TResult> SelectByMessageAndPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<string, IEnumerable<string>, int, TResult> selector) =>
            input.Select((v, i) => (v is null) ? selector(null, Array.Empty<string>(), i) : selector((v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), DistinctNames(v.MemberNames), i));

        public static IEnumerable<TResult> SelectByMessageAndPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<string, IEnumerable<string>, ValidationResult, TResult> selector) =>
            input.Select(v => (v is null) ? selector(null, Array.Empty<string>(), null) : selector((v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), DistinctNames(v.MemberNames), v));

        public static IEnumerable<TResult> SelectByMessageAndPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<string, IEnumerable<string>, TResult> selector) =>
            input.Select(v => (v is null) ? selector(null, Array.Empty<string>()) : selector((v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), DistinctNames(v.MemberNames)));

        public static IEnumerable<TResult> SelectManyByMessageAndPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<string, IEnumerable<string>, ValidationResult, int, IEnumerable<TResult>> selector) =>
            input.SelectMany((v, i) => (v is null) ? selector(null, Array.Empty<string>(), null, i) : selector((v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), DistinctNames(v.MemberNames), v, i));

        public static IEnumerable<TResult> SelectManyByMessageAndPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<string, IEnumerable<string>, int, IEnumerable<TResult>> selector) =>
            input.SelectMany((v, i) => (v is null) ? selector(null, Array.Empty<string>(), i) : selector((v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), DistinctNames(v.MemberNames), i));

        public static IEnumerable<TResult> SelectManyByMessageAndPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<string, IEnumerable<string>, ValidationResult, IEnumerable<TResult>> selector) =>
            input.SelectMany(v => (v is null) ? selector(null, Array.Empty<string>(), null) : selector((v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), DistinctNames(v.MemberNames), v));

        public static IEnumerable<TResult> SelectManyByMessageAndPropertyNames<TResult>(IEnumerable<ValidationResult> input,
                Func<string, IEnumerable<string>, IEnumerable<TResult>> selector) =>
            input.SelectMany(v => (v is null) ? selector(null, Array.Empty<string>()) : selector((v.ErrorMessage is null) ? "" : v.ErrorMessage.Trim(), DistinctNames(v.MemberNames)));

        public bool Equals(ValidationResult x, ValidationResult y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            string m = x.ErrorMessage;
            if (m is null || (m = m.Trim()).Length == 0)
                return string.IsNullOrWhiteSpace(y.ErrorMessage);
            if (y.ErrorMessage is null || !m.Equals(y.ErrorMessage.Trim()))
                return false;
            return DistinctNames(x.MemberNames).SequenceEqual(DistinctNames(y.MemberNames));
        }

        public int GetHashCode([DisallowNull] ValidationResult obj)
        {
            return new string[] { (obj.ErrorMessage is null) ? "" : obj.ErrorMessage.Trim() }.Concat(DistinctNames(obj.MemberNames)).GetAggregateHashCode();
        }
    }
}
