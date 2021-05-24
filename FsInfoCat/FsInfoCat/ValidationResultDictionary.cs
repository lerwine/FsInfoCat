using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat
{
    public sealed class ValidationResultDictionary : ICollection<ValidationResult>, IDictionary<string, IEnumerable<string>>
    {
        private readonly object _syncroot = new();

        public event PropertyChangedEventHandler MemberStateChanged;

        public ValidationResultDictionary()
        {
            Keys = new KeyCollection(this);
            Values = new ValueCollection(this);
        }

        public IEnumerable<string> this[string key]
        {
            get => _backingCollection.Where(i => i.MemberNames.Contains(key)).Select(i => i.ErrorMessage);
            set
            {
                Monitor.Enter(_syncroot);
                try
                {
                    var newValues = value.AsNonNullTrimmedValues().AsOrderedDistinct().EmptyIfNull();
                    var oldItems = _backingCollection.ToIndexValuePairs(r => r).Where(kvp => kvp.Value.MemberNames.Contains(key));
                    var toAdd = newValues.Where(t => !oldItems.Any(i => i.Value.ErrorMessage == t)).ToArray();
                    var toRemove = oldItems.Where(p => !newValues.Contains(p.Value.ErrorMessage)).Reverse().ToArray();
                    if (toAdd.Length == 0 && toRemove.Length == 0)
                        return;
                    foreach (var kvp in toRemove)
                    {
                        if (kvp.Value.MemberNames.Length == 1)
                            _backingCollection.RemoveAt(kvp.Key);
                        else
                            _backingCollection[kvp.Key] = new ResultItem(kvp.Value.ErrorMessage, kvp.Value.MemberNames.Where(n => n != key));
                    }
                    foreach (string message in toAdd)
                        _backingCollection.Add(new ResultItem(message, key));
                }
                finally { Monitor.Exit(_syncroot); }
                RaiseMemberStateChanged(key);
            }
        }

        public ICollection<string> Keys { get; }

        public ICollection<IEnumerable<string>> Values { get; }

        public int Count { get; private set; }

        public int NameCount { get; private set; }

        public void Add(ValidationResult item)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, IEnumerable<string> value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ValidationResult item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ValidationResult[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, IEnumerable<string>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ValidationResult> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<string, IEnumerable<string>>> IEnumerable<KeyValuePair<string, IEnumerable<string>>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<string, string>> GetMessagesByPropertyName() =>
            _backingCollection.SelectMany(i => i.MemberNames.Select(n => (n, i.ErrorMessage))).GroupBy(a => a.n, a => a.ErrorMessage);

        private void RaiseMemberStateChanged(string memberName) => MemberStateChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));

        public bool Remove(ValidationResult item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out IEnumerable<string> value)
        {
            throw new NotImplementedException();
        }

        bool ICollection<ValidationResult>.IsReadOnly => false;

        int ICollection<KeyValuePair<string, IEnumerable<string>>>.Count => Keys.Count;

        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.IsReadOnly => false;

        private LinkedList<ValidationResultBuilder> _validationResults = new();
        private Collection<MemberNameItem> _memberNames = new();

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

            internal static bool Set(string memberName, IEnumerable<string> errorMessages, [NotNull] ValidationResultDictionary target, bool appendOnly = false)
            {
                memberName = memberName.EmptyIfNullOrWhiteSpace();
                errorMessages = errorMessages.AsNonNullTrimmedValues().Distinct();
                lock (target._syncroot)
                {
                    ValidationResultBuilder item = target._validationResults.FirstOrDefault(v => v.ErrorMessage == errorMessage);
                    MemberNameItem memberItem = target._memberNames.FirstOrDefault(m => m.Value == memberName);

                    if (item is null)
                    {
                        if (memberItem is null)
                        {
                            item = new ValidationResultBuilder(errorMessage, new MemberNameItem(memberName));
                            target._memberNames.Add(item._memberNames[0]);
                        }
                        else
                            item = new ValidationResultBuilder(errorMessage, memberItem);
                    }
                    else
                    {
                        if (memberItem is null)
                        {
                            target._memberNames.Add(new MemberNameItem(memberName));
                            item._memberNames.Add(target._memberNames.Last());
                        }
                        else
                        {
                            if (item._memberNames.Contains(memberItem))
                                return false;
                            item._memberNames.Add(memberItem);
                        }
                    }
                    if (appendOnly || memberItem is null)
                        return true;
                    var node = target._validationResults.First;
                    while (!(node is null))
                    {
                        var next = node.Next;
                        if (!node.Value.ErrorMessage.Equals(errorMessage) && node.Value._memberNames.Remove(memberItem))
                            target._validationResults.Remove(node);
                        node = next;
                    }
                }
                return true;
            }

            internal static bool Set(string memberName, string errorMessage, [NotNull] ValidationResultDictionary target, bool appendOnly = false)
            {
                memberName = memberName.EmptyIfNullOrWhiteSpace();
                errorMessage = errorMessage.AsNonNullTrimmed();
                lock (target._syncroot)
                {
                    ValidationResultBuilder item = target._validationResults.FirstOrDefault(v => v.ErrorMessage == errorMessage);
                    MemberNameItem memberItem = target._memberNames.FirstOrDefault(m => m.Value == memberName);
                    
                    if (item is null)
                    {
                        if (memberItem is null)
                        {
                            item = new ValidationResultBuilder(errorMessage, new MemberNameItem(memberName));
                            target._memberNames.Add(item._memberNames[0]);
                        }
                        else
                            item = new ValidationResultBuilder(errorMessage, memberItem);
                    }
                    else
                    {
                        if (memberItem is null)
                        {
                            target._memberNames.Add(new MemberNameItem(memberName));
                            item._memberNames.Add(target._memberNames.Last());
                        }
                        else
                        {
                            if (item._memberNames.Contains(memberItem))
                                return false;
                            item._memberNames.Add(memberItem);
                        }
                    }
                    if (appendOnly || memberItem is null)
                        return true;
                    var node = target._validationResults.First;
                    while (!(node is null))
                    {
                        var next = node.Next;
                        if (!node.Value.ErrorMessage.Equals(errorMessage) && node.Value._memberNames.Remove(memberItem))
                            target._validationResults.Remove(node);
                        node = next;
                    }
                }
                return true;
            }

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
                                result._memberNames[i] = target._memberNames[index];
                        }
                    }
                }
            }

            public bool Equals(ValidationResultBuilder other) => !(other is null) && (ReferenceEquals(this, other) || (ErrorMessage.Equals(other.ErrorMessage) && _memberNames.SequenceEqual(other._memberNames)));

            public override bool Equals(object obj) => obj is ValidationResultBuilder other && Equals(other);

            public override int GetHashCode() => new string[] { ErrorMessage }.Concat(_memberNames.Select(m=> m.Value)).GetAggregateHashCode();

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

        private class ResultItem
        {
            internal ResultItem Previous { get; private set; }
            internal ResultItem Next { get; private set; }
            internal static IEnumerable<ResultItem> GetItems([NotNull] ValidationResultDictionary target)
            {
                for (ResultItem item = target._firstResultItem; !(item is null); item = item.Next)
                    yield return item;
            }
            void Append([NotNull] ValidationResult source, [NotNull] ValidationResultDictionary target)
            {
                lock (target._syncroot)
                {

                }
            }
            internal ResultItem(ValidationResult source)
            {
                Source = source;
                MemberNames = source.MemberNames.AsOrderedDistinct().EmptyIfNull().ToArray();
                ErrorMessage = source.ErrorMessage.AsNonNullTrimmed();
            }
            internal ResultItem(string message, IEnumerable<string> memberNames) : this(new ValidationResult(message, memberNames)) { }
            internal ResultItem(string message, params string[] memberNames) : this(new ValidationResult(message, memberNames)) { }
            internal ValidationResult Source { get; }
            internal string[] MemberNames { get; }
            internal string ErrorMessage { get; }
            public bool Equals(ResultItem other) => !(other is null) && (ReferenceEquals(this, other) ||
                (ErrorMessage == other.ErrorMessage && MemberNames.SequenceEqual(other.MemberNames)));
        }

        class KeyCollection : ICollection<string>
        {
            private readonly ValidationResultDictionary _source;
            internal KeyCollection(ValidationResultDictionary source) { _source = source; }
            int ICollection<string>.Count => _source._backingCollection.SelectMany(i => i.MemberNames).Distinct().Count();

            bool ICollection<string>.IsReadOnly => true;

            void ICollection<string>.Add(string item) => throw new NotSupportedException();

            void ICollection<string>.Clear() => throw new NotSupportedException();

            public bool Contains(string item) => _source._backingCollection.Any(i => i.MemberNames.Contains(item));

            void ICollection<string>.CopyTo(string[] array, int arrayIndex) => _source._backingCollection.SelectMany(i => i.MemberNames).Distinct()
                .ToList().CopyTo(array, arrayIndex);

            public IEnumerator<string> GetEnumerator() => _source._backingCollection.SelectMany(i => i.MemberNames).Distinct().GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_source._backingCollection.SelectMany(i => i.MemberNames).Distinct()).GetEnumerator();

            bool ICollection<string>.Remove(string item) => throw new NotSupportedException();
        }

        class ValueCollection : ICollection<IEnumerable<string>>
        {
            private readonly ValidationResultDictionary _source;
            internal ValueCollection(ValidationResultDictionary source) { _source = source; }
            int ICollection<IEnumerable<string>>.Count => _source._backingCollection.SelectMany(i => i.MemberNames).Distinct().Count();

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
