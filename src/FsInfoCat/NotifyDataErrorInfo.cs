using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    // TODO: Document NotifyDataErrorInfo class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class NotifyDataErrorInfo : RevertibleChangeTracking, IDataErrorInfo, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, string[]> _lastValidationResults = [];

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        string IDataErrorInfo.Error
        {
            get
            {
                lock (SyncRoot)
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
                return null;
            }
        }

        bool INotifyDataErrorInfo.HasErrors => HasErrors();

        public string this[string columnName] => _lastValidationResults.TryGetValue(columnName, out string[] a) ? a.JoinWithNewLines() : null;

        protected bool ClearError([DisallowNull] string propertyName)
        {
            lock (SyncRoot)
            {
                if (!_lastValidationResults.Remove(propertyName))
                    return false;
            }
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            return true;
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName) => _lastValidationResults.TryGetValue(propertyName, out string[] result) ? result : null;

        public bool HasErrors() => _lastValidationResults.Count > 0;

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs args) => ErrorsChanged?.Invoke(this, args);

        protected override void OnPropertyChanged(PropertyValueChangedEventArgs args)
        {
            DataErrorsChangedEventArgs[] errorsChanged;
            try
            {
                Collection<ValidationResult> validationResults = [];
                if (Validator.TryValidateProperty(args.NewValue, new ValidationContext(this) { MemberName = args.PropertyName }, validationResults))
                {
                    lock (SyncRoot)
                        errorsChanged = _lastValidationResults.Remove(args.PropertyName) ?
                            [new DataErrorsChangedEventArgs(args.PropertyName)] : [];
                }
                else
                {
                    string[] changed;
                    lock (SyncRoot)
                    {
                        KeyValuePair<string, string[]>[] keyValuePairs = [.. _lastValidationResults];
                        IEnumerable<KeyValuePair<string, string[]>> added = validationResults.SelectMany(r =>
                                r.MemberNames.Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => (MemberName: string.IsNullOrWhiteSpace(n) ? "" : n, r.ErrorMessage)))
                            .GroupBy(a => a.MemberName, a => a.ErrorMessage ?? "").ToKeyValuePairs(g => g.Key, g => g.Distinct().ToArray());
                        if (string.IsNullOrWhiteSpace(args.PropertyName))
                        {
                            _lastValidationResults.Clear();
                            foreach (var kvp in (added = added.ToArray()))
                                _lastValidationResults.Add(kvp.Key, kvp.Value);
                            changed = [.. keyValuePairs.Where(a => !added.Any(b => a.Key == b.Key && a.Value.OrderBy(t => t).SequenceEqual(b.Value.OrderBy(t => t))))
                                .Concat(added.Where(a => !keyValuePairs.Any(b => a.Key == b.Key))).Select(a => a.Key)];
                        }
                        else if (added.Any(kvp => kvp.Key == args.PropertyName))
                            changed = [.. added.Where(kvp =>
                            {
                                if (kvp.Key == args.PropertyName)
                                {
                                    if (_lastValidationResults.TryGetValue(args.PropertyName, out string[] a))
                                    {
                                        if (a.OrderBy(t => t).SequenceEqual(kvp.Value.OrderBy(t => t)))
                                            return false;
                                        _lastValidationResults[args.PropertyName] = kvp.Value;
                                    }
                                    else
                                        _lastValidationResults.Add(args.PropertyName, kvp.Value);
                                }
                                else if (_lastValidationResults.TryGetValue(args.PropertyName, out string[] validationResults))
                                {
                                    if (validationResults.OrderBy(t => t).SequenceEqual(kvp.Value.OrderBy(t => t)))
                                        return false;
                                    _lastValidationResults[args.PropertyName] = kvp.Value;
                                }
                                else
                                    _lastValidationResults.Add(args.PropertyName, kvp.Value);
                                return true;
                            }).Select(kvp => kvp.Key)];
                        else
                        {
                            changed = [.. added.Where(kvp =>
                            {
                                if (_lastValidationResults.TryGetValue(args.PropertyName, out string[] m))
                                {
                                    if (!kvp.Value.Any(v => !m.Contains(v)))
                                        return false;
                                    _lastValidationResults[args.PropertyName] = [.. m.Concat(kvp.Value).Distinct()];
                                }
                                else
                                    _lastValidationResults.Add(args.PropertyName, kvp.Value);
                                return true;
                            }).Select(kvp => kvp.Key)];
                            if (keyValuePairs.Any(kvp => kvp.Key == args.PropertyName))
                                changed = [.. changed, .. new string[] { args.PropertyName }];
                        }
                        errorsChanged = [.. changed.Select(p => new DataErrorsChangedEventArgs(p))];
                    }
                }
            }
            finally { base.OnPropertyChanged(args); }
            foreach (var item in errorsChanged)
                OnErrorsChanged(item);
        }

        protected bool SetError([DisallowNull] string propertyName, [DisallowNull] string message)
        {
            lock (SyncRoot)
            {
                if (_lastValidationResults.TryGetValue(propertyName, out string[] a))
                {
                    if (a.Length == 1 && a[0] == message)
                        return false;
                    _lastValidationResults[propertyName] = [propertyName];
                }
                else
                    _lastValidationResults.Add(propertyName, [propertyName]);
            }
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            return true;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
