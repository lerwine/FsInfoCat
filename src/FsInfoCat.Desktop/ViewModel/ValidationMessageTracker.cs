using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public sealed class ValidationMessageTracker : ValidationStateTracker
    {
        private readonly Dictionary<string, MessageCollection> _messages = new();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HasErrors), typeof(bool), typeof(ValidationMessageTracker),
                new PropertyMetadata(false));

        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        public bool HasErrors
        {
            get => (bool)GetValue(HasErrorsProperty);
            private set => SetValue(HasErrorsPropertyKey, value);
        }

        public IEnumerable<string> GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return Dispatcher.CheckInvoke(() =>
                {
                    if (_messages.Count == 0)
                        return null;
                    return _messages.Select(kvp =>
                    {
                        if (kvp.Value.Count == 1)
                            return $"{kvp.Key}: {kvp.Value[0]}";
                        return $"{kvp.Key}: {string.Join("\n\t", kvp.Value)}";
                    });
                });
            return _messages.TryGetValue(propertyName, out MessageCollection messages) ? messages : null;
        }

        public bool ClearErrorMessage(string propertyName, string message)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
            VerifyAccess();
            if (_messages.TryGetValue(propertyName, out MessageCollection messages) && messages.Remove(message, out bool wasLastMessage))
            {
                if (wasLastMessage)
                {
                    _ = _messages.Remove(propertyName);
                    SetValidationState(propertyName, true);
                    try { SetValidationState(propertyName, true); }
                    finally { RaiseErrorsChanged(propertyName); }
                }
                else
                    RaiseErrorsChanged(propertyName);
                return true;
            }
            return false;
        }

        public bool ClearErrorMessages(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            VerifyAccess();
            if (_messages.Remove(propertyName))
            {
                try { SetValidationState(propertyName, true); }
                finally { RaiseErrorsChanged(propertyName); }
                return true;
            }
            return false;
        }

        public bool SetErrorMessage(string propertyName, string message, params string[] additionalMessages)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            VerifyAccess();
            if ((additionalMessages = additionalMessages.AsWsNormalizedOrEmptyValues().Distinct().Where(s => s.Length > 0).ToArray() ?? Array.Empty<string>()).Length == 0 && (message = message.AsWsNormalizedOrEmpty()).Length == 0)
                return ClearErrorMessages(propertyName);
            if (_messages.TryGetValue(propertyName, out MessageCollection messageCollection))
            {
                if (!messageCollection.Set(message, additionalMessages))
                    return false;
                RaiseErrorsChanged(propertyName);
            }
            else
            {
                _messages.Add(propertyName, new(message, additionalMessages));
                try { SetValidationState(propertyName, false); }
                finally { RaiseErrorsChanged(propertyName); }
            }
            return true;
        }

        private void RaiseErrorsChanged(string propertyName) => ErrorsChanged?.Invoke(this, new(propertyName));

        protected override void OnAnyInvalidPropertyChanged(bool oldValue, bool newValue)
        {
            HasErrors = newValue;
            base.OnAnyInvalidPropertyChanged(oldValue, newValue);
        }

        class MessageCollection : ReadOnlyCollection<string>
        {
            internal MessageCollection([DisallowNull] string message, [DisallowNull] string[] additionalMessages) : base(new Collection<string>())
            {
                if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
                if (additionalMessages is null)
                    throw new ArgumentNullException(nameof(additionalMessages));
                Items.Add(message);
            }

            internal bool Add([DisallowNull] string message, [DisallowNull] string[] additionalMessages)
            {
                if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
                if (additionalMessages is null)
                    throw new ArgumentNullException(nameof(additionalMessages));
                if (Items.Contains(message))
#pragma warning disable CA1827 // Do not use Count() or LongCount() when Any() can be used
                    return additionalMessages.Count(m =>
                    {
                        if (Items.Contains(m))
                            return false;
                        Items.Add(m);
                        return true;
                    }) == 0;
#pragma warning restore CA1827 // Do not use Count() or LongCount() when Any() can be used
                Items.Add(message);
                foreach (string m in additionalMessages.Where(s => !Items.Contains(s)))
                    Items.Add(m);
                return true;
            }

            internal bool Set([DisallowNull] string message, [DisallowNull] string[] additionalMessages)
            {
                if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
                if (additionalMessages is null)
                    throw new ArgumentNullException(nameof(additionalMessages));
                if (Items.Count == additionalMessages.Length + 1 && Items[0] == message && (Items.Count == 1 || Items.Skip(1).SequenceEqual(additionalMessages)))
                    return false;
                Items.Clear();
                foreach (string m in additionalMessages)
                    Items.Add(m);
                return true;
            }

            internal bool Remove([DisallowNull] string message, out bool wasLastMessage)
            {
                wasLastMessage = Items.Remove(message);
                if (wasLastMessage)
                {
                    wasLastMessage = Count == 0;
                    return true;
                }
                return false;
            }
        }
    }
}
