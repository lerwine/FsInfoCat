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
    public sealed class ValidationResultDictionary : ICollection<ValidationResult>, IDictionary<string, IEnumerable<string>>, IDictionary<string, string>
    {
        private readonly object _syncroot = new();
        private LinkedList<ValidationResultBuilder> _validationResults = new();
        private Collection<MemberNameItem> _memberNames = new();

        public event PropertyChangedEventHandler MemberStateChanged;

        public IEnumerable<string> this[string key]
        {
            get => _validationResults.Where(i => i.ContainsMemberName(key)).Select(i => i.ErrorMessage);
            set => Add(key, value);
        }

        public int Count { get; private set; }

        public ICollection<string> Keys { get; }

        public ICollection<IEnumerable<string>> Values { get; }

        public ValidationResultDictionary()
        {
            Keys = new KeyCollection(this);
            Values = new ValueCollection(this);
        }

        public void Add(string key, string value)
        {
            key = key.EmptyIfNullOrWhiteSpace();
            if (ValidationResultBuilder.Set(key, value, this))
                RaiseMemberStateChanged(key);
        }

        public void Add(ValidationResult item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            string[] memberNames = ValidationResultBuilder.Upsert(item, this).ToArray();
            foreach (string n in memberNames)
                RaiseMemberStateChanged(n);
        }

        public void Add(string key, IEnumerable<string> value)
        {
            key = key.EmptyIfNullOrWhiteSpace();
            if (ValidationResultBuilder.Set(key, value, this))
                RaiseMemberStateChanged(key);
        }

        public void Clear()
        {
            string[] memberNames;
            lock (_syncroot)
            {
                memberNames = _memberNames.Select(m => m.Value).ToArray();
                _memberNames.Clear();
                _validationResults.Clear();
            }
            foreach (string n in memberNames)
                RaiseMemberStateChanged(n);
        }

        public bool Contains(ValidationResult item) => !(item is null) && ValidationResultBuilder.TryFindMatching(item, this, out _);

        public bool ContainsKey(string key)
        {
            key = key.EmptyIfNullOrWhiteSpace();
            return _memberNames.Any(m => m.Value.Equals(key));
        }

        public void CopyTo(ValidationResult[] array, int arrayIndex) => _validationResults.Select(v => v.ValidationResult).ToList().CopyTo(array, arrayIndex);

        public IEnumerator<ValidationResult> GetEnumerator() => _validationResults.Select(v => v.ValidationResult).GetEnumerator();

        public IEnumerable<IGrouping<string, string>> GetMessagesByPropertyName() =>
            _validationResults.SelectMany(i => i.GetMemberNames().Select(n => (n, i.ErrorMessage))).GroupBy(a => a.n, a => a.ErrorMessage);

        private void RaiseMemberStateChanged(string memberName) => MemberStateChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));

        public bool Remove(ValidationResult item)
        {
            string[] memberNames = ValidationResultBuilder.Remove(item, this).ToArray();
            if (memberNames.Length == 0)
                return false;
            foreach (string n in memberNames)
                RaiseMemberStateChanged(n);
            return true;
        }

        public bool Remove(string key)
        {
            key = key.EmptyIfNullOrWhiteSpace();
            if (ValidationResultBuilder.Remove(key, this))
            {
                RaiseMemberStateChanged(key);
                return true;
            }
            return false;
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out IEnumerable<string> value)
        {
            key = key.EmptyIfNullOrWhiteSpace();
            value = _validationResults.Where(i => i.ContainsMemberName(key)).Select(i => i.ErrorMessage);
            return value.Any();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        {
            key = key.EmptyIfNullOrWhiteSpace();
            value = _validationResults.Where(i => i.ContainsMemberName(key)).Select(i => i.ErrorMessage).JoinWithNewLines();
            return !(value is null);
        }

        #region Explicit Member

        string IDictionary<string, string>.this[string key]
        {
            get
            {
                string[] result = this[key].ToArray();
                if (result.Length == 1)
                    return result[0];
                if (result.Length > 1)
                    return string.Join(Environment.NewLine, result);
                throw new KeyNotFoundException();
            }
            set => Add(key, value);
        }

        bool ICollection<ValidationResult>.IsReadOnly => false;

        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.IsReadOnly => false;

        ICollection<string> IDictionary<string, string>.Values => throw new NotImplementedException();

        bool ICollection<KeyValuePair<string, string>>.IsReadOnly => false;

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Add(KeyValuePair<string, IEnumerable<string>> item) => Add(item.Key, item.Value);

        void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item) => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.Contains(KeyValuePair<string, IEnumerable<string>> item)
        {
            string propertyName = item.Key.EmptyIfNullOrWhiteSpace();
            string[] messages = item.Value.AsOrderedDistinct().ToArray();
            return GetMessagesByPropertyName().Any(g => g.Key.Equals(propertyName) && g.OrderBy(v => v).SequenceEqual(messages));
        }

        bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
        {
            string propertyName = item.Key.EmptyIfNullOrWhiteSpace();
            string[] messages = item.Value.AsNonNullTrimmed().SplitLines();
            return GetMessagesByPropertyName().Any(g => g.Key.Equals(propertyName) && g.OrderBy(v => v).SequenceEqual(messages));
        }

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.CopyTo(KeyValuePair<string, IEnumerable<string>>[] array, int arrayIndex) =>
            GetMessagesByPropertyName().ToKeyValuePairs(g => g.Key, g => (IEnumerable<string>)g).ToList().CopyTo(array, arrayIndex);

        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) =>
            GetMessagesByPropertyName().ToKeyValuePairs(g => g.Key, g => g.JoinWithNewLines()).ToList().CopyTo(array, arrayIndex);

        IEnumerator<KeyValuePair<string, IEnumerable<string>>> IEnumerable<KeyValuePair<string, IEnumerable<string>>>.GetEnumerator() =>
            GetMessagesByPropertyName().ToKeyValuePairs(g => g.Key, g => (IEnumerable<string>)g).GetEnumerator();

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator() => GetMessagesByPropertyName().ToKeyValuePairs(g => g.Key, g => g.JoinWithNewLines()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_validationResults.Select(v => v.ValidationResult)).GetEnumerator();

        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.Remove(KeyValuePair<string, IEnumerable<string>> item)
        {
            string key = item.Key.EmptyIfNullOrWhiteSpace();
            if (ValidationResultBuilder.Remove(key, item.Value, this))
            {
                RaiseMemberStateChanged(key);
                return true;
            }
            return false;
        }

        bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
        {
            string key = item.Key.EmptyIfNullOrWhiteSpace();
            string[] messages = item.Value.AsNonNullTrimmed().SplitLines();
            if (ValidationResultBuilder.Remove(key, messages, this))
            {
                RaiseMemberStateChanged(key);
                return true;
            }
            return false;
        }

        #endregion

        private class ErrorMessageItem : IEquatable<ErrorMessageItem>
        {
            internal string Value { get; private set; }

            public bool Equals(ErrorMessageItem other) => !(other is null) && (ReferenceEquals(this, other) || Value.Equals(other.Value));

            public override bool Equals(object obj) => obj is ErrorMessageItem other && Equals(other);

            public override int GetHashCode() => Value.GetHashCode();
        }

        private class MemberNameItem : IEquatable<MemberNameItem>
        {
            public MemberNameItem([NotNull] string memberName) { Value = memberName; }

            internal string Value { get; private set; }

            public bool Equals(MemberNameItem other) => !(other is null) && (ReferenceEquals(this, other) || Value.Equals(other.Value));

            public override bool Equals(object obj) => obj is MemberNameItem other && Equals(other);

            public override int GetHashCode() => Value.GetHashCode();
        }

        private class ValidationResultBuilder : IEquatable<ValidationResultBuilder>
        {
            private readonly Collection<MemberNameItem> _memberNames = new();
            private ValidationResult _validationResult;

            internal string ErrorMessage { get; private set; }

            internal ValidationResult ValidationResult
            {
                get
                {
                    ValidationResult validationResult = _validationResult;
                    if (validationResult is null)
                        _validationResult = validationResult = new ValidationResult(ErrorMessage, _memberNames.Select(m => m.Value).ToArray());
                    return validationResult;
                }
            }

            internal static bool TryFindMatching([NotNull] ValidationResult validationResult, [NotNull] ValidationResultDictionary target, out ValidationResultBuilder result)
            {
                lock (target._syncroot)
                {
                    if ((result = target._validationResults.FirstOrDefault(item => ReferenceEquals(item._validationResult, validationResult))) is null)
                    {
                        var item = new ValidationResultBuilder(validationResult);
                        if ((result = target._validationResults.FirstOrDefault(e => e.Equals(item))) is null)
                        {
                            result = item;
                            return false;
                        }
                    }
                }
                return true;
            }

            /// <summary>
            /// Sets the error message(s) for a named member.
            /// </summary>
            /// <param name="memberName">The member name to apply the messages to.</param>
            /// <param name="errorMessages">The error messages.</param>
            /// <param name="target">The <see cref="ValidationResultDictionary"/> to apply the error messages to.</param>
            /// <param name="appendOnly">If <see langword="true"/>, then new messages are upserted; otherwise, <see langword="false"/> to remove previous error messages for the named member.</param>
            /// <returns><see langword="true"/> if validation message(s) were changed for the specified <paramref name="memberName"/>; otherwise, <see langword="false"/>.</returns>
            internal static bool Set(string memberName, IEnumerable<string> errorMessages, [NotNull] ValidationResultDictionary target, bool appendOnly = false)
            {
                memberName = memberName.EmptyIfNullOrWhiteSpace();
                errorMessages = errorMessages.AsNonNullTrimmedValues().Distinct();
                lock (target._syncroot)
                {
                    MemberNameItem memberItem = target._memberNames.FirstOrDefault(m => m.Value == memberName);
                    bool changed = memberItem is null;
                    IEnumerable<string> toAdd;
                    if (changed)
                    {
                        memberItem = new MemberNameItem(memberName);
                        target._memberNames.Add(memberItem);
                        toAdd = errorMessages;
                    }
                    else
                    {
                        changed = (toAdd = errorMessages.Where(e => !target._validationResults.Any(r => r.ErrorMessage == e && r._memberNames.Any(n => n.Value == memberName))).ToArray()).Any();
                        if (!appendOnly)
                        {
                            var node = target._validationResults.First;
                            while (!(node is null))
                            {
                                var next = node.Next;
                                if (!errorMessages.Contains(node.Value.ErrorMessage))
                                {
                                    var memberNames = node.Value._memberNames;
                                    var item = memberNames.FirstOrDefault(n => n.Value.Equals(memberName));
                                    if (!(item is null))
                                    {
                                        changed = true;
                                        if (memberNames.Count == 1)
                                            target._validationResults.Remove(node);
                                        else
                                            memberNames.Remove(item);
                                    }
                                }
                            }
                        }
                    }

                    foreach (string msg in toAdd)
                        target._validationResults.AddLast(new ValidationResultBuilder(msg, memberItem));
                    return changed;
                }
            }

            /// <summary>
            /// Sets the error message of a named member.
            /// </summary>
            /// <param name="memberName">The member name to apply the message to.</param>
            /// <param name="errorMessage">The error message.</param>
            /// <param name="target">The <see cref="ValidationResultDictionary"/> to apply the error message to.</param>
            /// <param name="appendOnly">If <see langword="true"/>, then a new message is upserted; otherwise, <see langword="false"/> to remove previous error messages for the named member.</param>
            /// <returns><see langword="true"/> if validation message(s) were changed for the specified <paramref name="memberName"/>; otherwise, <see langword="false"/>.</returns>
            internal static bool Set(string memberName, string errorMessage, [NotNull] ValidationResultDictionary target, bool appendOnly = false)
            {
                memberName = memberName.EmptyIfNullOrWhiteSpace();
                errorMessage = errorMessage.AsNonNullTrimmed();
                lock (target._syncroot)
                {
                    MemberNameItem memberItem = target._memberNames.FirstOrDefault(m => m.Value == memberName);
                    ValidationResultBuilder item;
                    if (memberItem is null)
                    {
                        memberItem = new MemberNameItem(memberName);
                        item = new ValidationResultBuilder(errorMessage, memberItem);
                        target._validationResults.AddLast(item);
                        target._memberNames.Add(memberItem);
                    }
                    else if ((item = target._validationResults.FirstOrDefault(v => v.ErrorMessage == errorMessage && v._memberNames.Contains(memberItem))) is null)
                    {
                        if (!appendOnly)
                        {
                            var node = target._validationResults.First;
                            while (!(node is null))
                            {
                                var next = node.Next;
                                if (node.Value._memberNames.Remove(memberItem) && node.Value._memberNames.Count == 0)
                                    target._validationResults.Remove(node);
                                node = next;
                            }
                        }
                        item = new ValidationResultBuilder(errorMessage, memberItem);
                        target._validationResults.AddLast(item);
                    }
                    else
                    {
                        if (appendOnly)
                            return false;
                        var node = target._validationResults.First;
                        while (!(node is null))
                        {
                            var next = node.Next;
                            if (!ReferenceEquals(node.Value, item) && node.Value._memberNames.Remove(memberItem) && node.Value._memberNames.Count == 0)
                            {
                                appendOnly = true;
                                target._validationResults.Remove(node);
                            }
                            node = next;
                        }
                        return appendOnly;
                    }
                }
                return true;
            }

            /// <summary>
            /// Appends a <see cref="ValidationResult"/> if a matching item does not already exist.
            /// </summary>
            /// <param name="validationResult">The item to add.</param>
            /// <param name="target">The <see cref="ValidationResultDictionary"/> to add the item to.</param>
            /// <returns>The names of the members where the validation has changed.</returns>
            internal static IEnumerable<string> Upsert([NotNull] ValidationResult validationResult, [NotNull] ValidationResultDictionary target)
            {
                if (TryFindMatching(validationResult, target, out ValidationResultBuilder result))
                    result._validationResult = validationResult;
                else
                {
                    lock (target._syncroot)
                    {
                        for (int i = 0; i < result._memberNames.Count; i++)
                        {
                            var item = result._memberNames[i];
                            int index = target._memberNames.IndexOf(item);
                            if (index < 0)
                            {
                                target._memberNames.Add(item);
                                yield return item.Value;
                            }
                            else
                            {
                                result._memberNames[i] = item = target._memberNames[index];
                                var node = target._validationResults.First;
                                while (!(node is null))
                                {
                                    var next = node.Next;
                                    if (!ReferenceEquals(node.Value, item) && node.Value._memberNames.Remove(item) && node.Value._memberNames.Count == 0)
                                        target._validationResults.Remove(node);
                                    node = next;
                                }
                            }
                        }
                        target._validationResults.AddLast(result);
                    }
                }
            }

            /// <summary>
            /// Removes error message associated with the specified <see cref="ValidationResult.MemberNames"/>.
            /// </summary>
            /// <param name="item">The item that contains the member names.</param>
            /// <param name="target">The <see cref="ValidationResultDictionary"/> to remove the error messages from.</param>
            /// <returns>The names of the members where the validation has changed.</returns>
            internal static IEnumerable<string> Remove(ValidationResult item, ValidationResultDictionary target)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Removes all messages for the specified member name.
            /// </summary>
            /// <param name="memberName">The member name to remove messages from.</param>
            /// <param name="target">The <see cref="ValidationResultDictionary"/> to remove the member name and its error messages from.</param>
            /// <returns><see langword="true"/> if validation message(s) were changed for the specified <paramref name="memberName"/>; otherwise, <see langword="false"/>.</returns>
            internal static bool Remove(string memberName, ValidationResultDictionary target)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Removes messages for the specified member name if all the specified messages exist.
            /// </summary>
            /// <param name="memberName"></param>
            /// <param name="errorMessages"></param>
            /// <param name="validationResultDictionary"></param>
            /// <returns></returns>
            internal static bool Remove(string memberName, IEnumerable<string> errorMessages, ValidationResultDictionary validationResultDictionary)
            {
                throw new NotImplementedException();
            }

            public bool Equals(ValidationResultBuilder other) => !(other is null) && (ReferenceEquals(this, other) || (ErrorMessage.Equals(other.ErrorMessage) && _memberNames.SequenceEqual(other._memberNames)));

            public override bool Equals(object obj) => obj is ValidationResultBuilder other && Equals(other);

            public override int GetHashCode() => new string[] { ErrorMessage }.Concat(_memberNames.Select(m=> m.Value)).GetAggregateHashCode();

            internal bool ContainsMemberName(string key)
            {
                key = key.EmptyIfNullOrWhiteSpace();
                return _memberNames.Any(n => n.Value.Equals(key));
            }

            internal IEnumerable<string> GetMemberNames() => _memberNames.Select(m => m.Value);

            private ValidationResultBuilder([NotNull] ValidationResult validationResult)
            {
                ErrorMessage = (_validationResult = validationResult).ErrorMessage.AsNonNullTrimmed();
                foreach (string memberName in validationResult.MemberNames.AsNonNullValues().EmptyIfNull().Distinct())
                    _memberNames.Add(new MemberNameItem(memberName));
            }

            private ValidationResultBuilder(string errorMessage, MemberNameItem memberName)
            {
                ErrorMessage = errorMessage;
                _memberNames.Add(memberName);
            }
        }

        class KeyCollection : ICollection<string>
        {
            private readonly ValidationResultDictionary _source;
            internal KeyCollection(ValidationResultDictionary source) { _source = source; }
            public int Count => _source._memberNames.Count;

            bool ICollection<string>.IsReadOnly => true;

            void ICollection<string>.Add(string item) => throw new NotSupportedException();

            void ICollection<string>.Clear() => throw new NotSupportedException();

            public bool Contains(string item) => _source._memberNames.Any(i => i.Value.Equals(item));

            void ICollection<string>.CopyTo(string[] array, int arrayIndex) => _source._memberNames.Select(i => i.Value).ToList().CopyTo(array, arrayIndex);

            public IEnumerator<string> GetEnumerator() => _source._memberNames.Select(i => i.Value).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_source._memberNames.Select(i => i.Value)).GetEnumerator();

            bool ICollection<string>.Remove(string item) => throw new NotSupportedException();
        }

        class ValueCollection : ICollection<IEnumerable<string>>
        {
            private readonly ValidationResultDictionary _source;
            internal ValueCollection(ValidationResultDictionary source) { _source = source; }
            public int Count => _source._memberNames.Count;

            bool ICollection<IEnumerable<string>>.IsReadOnly => true;

            void ICollection<IEnumerable<string>>.Add(IEnumerable<string> item) => throw new NotSupportedException();

            void ICollection<IEnumerable<string>>.Clear() => throw new NotSupportedException();

            bool ICollection<IEnumerable<string>>.Contains(IEnumerable<string> item)
            {
                if (item is null)
                    return false;
                string[] ordered = item.AsOrderedDistinct().EmptyIfNull().ToArray();
                if (ordered.Length == 0)
                    return false;
                return _source.GetMessagesByPropertyName().Any(m => ordered.SequenceEqual(m.AsNonNullTrimmedValues().AsOrderedDistinct().EmptyIfNull()));
            }

            void ICollection<IEnumerable<string>>.CopyTo(IEnumerable<string>[] array, int arrayIndex) =>
                _source.GetMessagesByPropertyName().Cast<IEnumerable<string>>().ToList().CopyTo(array, arrayIndex);

            public IEnumerator<IEnumerable<string>> GetEnumerator() => _source.GetMessagesByPropertyName().Cast<IEnumerable<string>>().GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() =>
                ((IEnumerable)_source.GetMessagesByPropertyName().Cast<IEnumerable<string>>()).GetEnumerator();

            bool ICollection<IEnumerable<string>>.Remove(IEnumerable<string> item) => throw new NotSupportedException();
        }
    }
}
