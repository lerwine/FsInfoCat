using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FsInfoCat
{
    // TODO: Document DataErrorInfo class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class DataErrorInfo : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        private readonly Dictionary<string, IEnumerable> _errors = [];

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public object SyncRoot { get; } = new();

        public bool HasErrors => _errors.Count > 0;

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs args) => ErrorsChanged?.Invoke(this, args);

        public bool AddError([DisallowNull] string propertyName, params string[] errors) => AddError(propertyName, errors?.AsEnumerable());

        public bool AddError([DisallowNull] string propertyName, IEnumerable<string> errors)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            bool raiseHasErrorsChanged;
            if (errors is not null && (errors = errors.ElementsNotNullOrWhiteSpace().Distinct().ToArray()).Any())
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_errors.TryGetValue(propertyName, out IEnumerable current))
                    {
                        if (current is IEnumerable<string> values)
                        {
                            IEnumerable<string> toAdd = errors.SkipWhile(e => !values.Contains(e));
                            if (toAdd.Any())
                                _errors[propertyName] = values.Concat(toAdd.Take(1)).Concat(toAdd.Skip(1).Where(e => !values.Contains(e))).ToImmutableArray();
                            else
                                return false;
                        }
                        else
                        {
                            string cs = current as string;
                            if (!errors.Skip(1).Any() && errors.First() == cs)
                                return false;
                            _errors[propertyName] = ImmutableArray.Create(cs).Concat(errors.Where(s => s != cs)).ToImmutableArray();
                        }
                        raiseHasErrorsChanged = false;
                    }
                    else
                    {
                        raiseHasErrorsChanged = _errors.Count == 0;
                        _errors.Add(propertyName, errors.Skip(1).Any() ? errors.ToImmutableArray() : errors.First());
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            else
                return false;
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            if (raiseHasErrorsChanged)
                RaisePropertyChanged(nameof(HasErrors));
            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) => OnPropertyChanged(new(propertyName));

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        public bool RemoveError(string error, [CallerMemberName] string propertyName = null) => RemoveErrors(propertyName, error);

        public bool RemoveErrors(IEnumerable<string> errors, [CallerMemberName] string propertyName = null) => RemoveErrors(propertyName, errors);

        public bool RemoveErrors([DisallowNull] string propertyName, params string[] errors) => RemoveErrors(propertyName, errors?.AsEnumerable());

        public bool RemoveErrors([DisallowNull] string propertyName, IEnumerable<string> errors)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            bool raiseHasErrorsChanged;
            if (errors is not null && (errors = errors.ElementsNotNullOrWhiteSpace().Distinct().ToArray()).Any())
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_errors.TryGetValue(propertyName, out IEnumerable current))
                    {
                        if (current is IEnumerable<string> values)
                        {
                            if (errors.Any(e => values.Contains(e)))
                            {
                                if ((values = values.Except(errors)).Any())
                                {
                                    raiseHasErrorsChanged = false;
                                    _errors[propertyName] = values.ToImmutableArray();
                                }
                                else
                                {
                                    raiseHasErrorsChanged = _errors.Count == 0;
                                    _errors.Remove(propertyName);
                                }
                            }
                            else
                                return false;
                        }
                        else
                        {
                            string cs = current as string;
                            if (!errors.Contains(cs))
                                return false;
                            _errors.Remove(propertyName);
                            raiseHasErrorsChanged = _errors.Count == 0;
                        }
                    }
                    else
                    {
                        raiseHasErrorsChanged = false;
                        _errors.Add(propertyName, errors.Skip(1).Any() ? errors.ToImmutableArray() : errors.First());
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            else
                return false;
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            if (raiseHasErrorsChanged)
                RaisePropertyChanged(nameof(HasErrors));
            return true;
        }

        public bool SetErrors([DisallowNull] string propertyName, params string[] errors) => SetErrors(propertyName, errors?.AsEnumerable());

        public bool SetError(string error, [CallerMemberName] string propertyName = null) => SetErrors(propertyName, error);

        public bool SetErrors(IEnumerable<string> errors, [CallerMemberName] string propertyName = null) => SetErrors(propertyName, errors);

        public bool SetErrors([DisallowNull] string propertyName, IEnumerable<string> errors)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            bool raiseHasErrorsChanged;
            Monitor.Enter(SyncRoot);
            try
            {
                if (errors is not null && (errors = errors.ElementsNotNullOrWhiteSpace().Distinct().ToArray()).Any())
                {
                    if (_errors.TryGetValue(propertyName, out IEnumerable current))
                    {
                        if ((current is string s) ? (errors.First() != s || errors.Skip(1).Any()) : !current.Cast<string>().SequenceEqual(errors))
                            _errors[propertyName] = errors.Skip(1).Any() ? errors.ToImmutableArray() : errors.First();
                        else
                            return false;
                        raiseHasErrorsChanged = false;
                    }
                    else
                    {
                        raiseHasErrorsChanged = _errors.Count == 0;
                        _errors.Add(propertyName, errors.Skip(1).Any() ? errors.ToImmutableArray() : errors.First());
                    }
                }
                else
                {
                    if (!_errors.Remove(propertyName))
                        return false;
                    raiseHasErrorsChanged = _errors.Count == 0;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            if (raiseHasErrorsChanged)
                RaisePropertyChanged(nameof(HasErrors));
            return true;
        }

        public bool ClearErrors([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            Monitor.Enter(SyncRoot);
            try
            {
                if (!_errors.Remove(propertyName))
                    return false;
            }
            finally { Monitor.Exit(SyncRoot); }
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            return true;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.Select(kvp =>
                {
                    if (kvp.Value is string s)
                        return $"{kvp.Key}: {s.AsWsNormalizedOrEmpty()}";
                    string indent = $"\n{new string(' ', kvp.Key.Length + 2)}";
                    return $"{kvp.Key}: {string.Join(indent, ((IEnumerable<string>)kvp.Value).AsWsNormalizedOrEmptyValues())}";
                });
            if (_errors.TryGetValue(propertyName, out IEnumerable messages))
                return messages;
            return Array.Empty<string>();
        }

        public bool HasError(string propertyName) => string.IsNullOrEmpty(propertyName) ? _errors.Count > 0 : _errors.ContainsKey(propertyName);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
