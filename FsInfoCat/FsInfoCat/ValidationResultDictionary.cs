using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat
{
    public class ValidationResultDictionary : ICollection<ValidationResult>, IDictionary<string, IEnumerable<string>>
    {
        private readonly object _syncroot = new();
        private readonly Collection<ResultItem> _backingCollection = new();

        public ValidationResultDictionary()
        {
            Keys = new KeyCollection(this);
            Values = new ValueCollection(this);
        }

        public IEnumerable<string> this[string key]
        {
            get => _backingCollection.Where(i => i.MemberNames.Contains(key)).Select(i => i.Message);
            set
            {
                Monitor.Enter(_syncroot);
                try
                {

                }
                finally { Monitor.Exit(_syncroot); }
            }
        }

        public ICollection<string> Keys { get; }

        public ICollection<IEnumerable<string>> Values { get; }

        public int ValidationResultCount => _backingCollection.Count;

        public int GetNameCount() => Keys.Count;

        int ICollection<ValidationResult>.Count => _backingCollection.Count;

        bool ICollection<ValidationResult>.IsReadOnly => false;

        int ICollection<KeyValuePair<string, IEnumerable<string>>>.Count => Keys.Count;

        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.IsReadOnly => false;

        public IEnumerable<string> Add(ValidationResult item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            Monitor.Enter(_syncroot);
            try
            {
                if (Contains(item))
                    return Array.Empty<string>();
                ResultItem resultItem = new ResultItem(item);
                foreach (var matching in _backingCollection.Select((r, i) => new { Index = i, Item = r })
                    .Where(a => a.Item.Message == resultItem.Message && a.Item.MemberNames.Any(n => resultItem.MemberNames.Contains(n))).Reverse().ToArray())
                {
                    string[] remainder = matching.Item.MemberNames.Where(n => !resultItem.MemberNames.Contains(n)).ToArray();
                    if (remainder.Length == 0)
                        _backingCollection.RemoveAt(matching.Index);
                    else
                        _backingCollection[matching.Index] = new ResultItem(matching.Item.Message, remainder);
                }
                _backingCollection.Add(resultItem);
            }
            finally { Monitor.Exit(_syncroot); }
            throw new NotImplementedException();
        }

        public bool Add(string key, IEnumerable<string> value)
        {
            if (string.IsNullOrWhiteSpace(key))
                key = "";
            Monitor.Enter(_syncroot);
            try
            {
                IEnumerable<ResultItem> matching = _backingCollection.Where(i => i.MemberNames.Contains(key));
                foreach (string message in value.Select(m => string.IsNullOrWhiteSpace(m) ? "" : m.Trim()).Where(m => !matching.Any(a => a.Message == m)))
                    _backingCollection.Add(new ResultItem(message, key));
                throw new NotImplementedException();
            }
            finally { Monitor.Exit(_syncroot); }
        }

        public bool Add(KeyValuePair<string, IEnumerable<string>> item) => Add(item.Key, item.Value);

        public IEnumerable<string> Clear()
        {
            string[] result;
            Monitor.Enter(_syncroot);
            try
            {
                result = Keys.ToArray();
                _backingCollection.Clear();
            }
            finally { Monitor.Exit(_syncroot); }
            return result;
        }

        public bool Contains(ValidationResult item) => !(item is null) && (_backingCollection.Any(i => ReferenceEquals(i, item)) || _backingCollection.Contains(new ResultItem(item)));

        public bool Contains(KeyValuePair<string, IEnumerable<string>> item)
        {
            string[] messages = _backingCollection.Where(i => i.MemberNames.Contains(item.Key)).Select(i => i.Message).ToArray();
            if (messages.Length == 0)
                return item.Value is null || !item.Value.Any();
            if (item.Value is null)
                return false;
            return messages.OrderBy(m => m).SequenceEqual(item.Value.Select(m => (m is null) ? "" : m.Trim()).OrderBy(m => m));
        }

        public bool ContainsKey(string key) => _backingCollection.Any(i => i.MemberNames.Contains(key));

        public void CopyTo(ValidationResult[] array, int arrayIndex) => _backingCollection.Select(i => i.Source).ToList().CopyTo(array, arrayIndex);

        public void CopyTo(KeyValuePair<string, IEnumerable<string>>[] array, int arrayIndex) =>
            GetMessagesByPropertyName().Select(g => new KeyValuePair<string, IEnumerable<string>>(g.Key, g));

        public IEnumerable<IGrouping<string, string>> GetMessagesByPropertyName() =>
            _backingCollection.SelectMany(i => i.MemberNames.Select(n => new { Name = n, i.Message })).GroupBy(a => a.Name, a => a.Message);

        public IEnumerator<ValidationResult> GetEnumerator() => _backingCollection.Select(i => i.Source).GetEnumerator();

        public IEnumerable<string> Remove(ValidationResult item)
        {
            if (item is null)
                return Array.Empty<string>();
            Monitor.Enter(_syncroot);
            try
            {
                int index = _backingCollection.Select((r, i) => new { Index = i, r.Source }).Where(a => ReferenceEquals(a.Source, item)).Select(a => a.Index).DefaultIfEmpty(-1).First();
                if (index < 0 && (index = _backingCollection.IndexOf(new ResultItem(item))) < 0)
                    return Array.Empty<string>();
                _backingCollection.RemoveAt(index);
            }
            finally { Monitor.Exit(_syncroot); }
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                key = "";
            Monitor.Enter(_syncroot);
            try
            {
                var toRemove = _backingCollection.Select((r, i) =>
                {
                    if (r.MemberNames.Contains(key))
                    {
                        if (r.MemberNames.Length == 1)
                            return new { Index = i, Item = (ResultItem)null };
                        return new { Index = i, Item = new ResultItem(r.Message, r.MemberNames.Where(n => n != key).ToArray()) };
                    }
                    return null;
                }).Where(a => !(a is null)).ToArray();
                if (toRemove.Length == 0)
                    return false;
                foreach (var r in toRemove.Reverse())
                    if (r.Item is null)
                        _backingCollection.RemoveAt(r.Index);
                    else
                        _backingCollection[r.Index] = r.Item;
            }
            finally { Monitor.Exit(_syncroot); }
            return true;
        }

        public bool Remove(KeyValuePair<string, IEnumerable<string>> item)
        {
            Monitor.Enter(_syncroot);
            try
            {
                throw new NotImplementedException();
            }
            finally { Monitor.Exit(_syncroot); }
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out IEnumerable<string> value)
        {
            value = GetMessagesByPropertyName().FirstOrDefault(g => g.Key == key);
            return !(value is null);
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_backingCollection.Select(i => i.Source)).GetEnumerator();

        IEnumerator<KeyValuePair<string, IEnumerable<string>>> IEnumerable<KeyValuePair<string, IEnumerable<string>>>.GetEnumerator() =>
            GetMessagesByPropertyName().Select(g => new KeyValuePair<string, IEnumerable<string>>(g.Key, g)).GetEnumerator();

        private static string[] GetOrderedNames(IEnumerable<string> source) => (source is null) ? new string[] { "" } : source.Select(n => string.IsNullOrWhiteSpace(n) ? "" : n).Distinct().OrderBy(m => m).ToArray();

        void ICollection<ValidationResult>.Add(ValidationResult item)
        {
            throw new NotImplementedException();
        }

        void ICollection<ValidationResult>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<ValidationResult>.Remove(ValidationResult item)
        {
            throw new NotImplementedException();
        }

        void IDictionary<string, IEnumerable<string>>.Add(string key, IEnumerable<string> value)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Add(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Clear()
        {
            throw new NotImplementedException();
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
                string[] ordered = GetOrderedNames(item);
                if (ordered.Length == 0)
                    return false;
                return _source.GetMessagesByPropertyName().Any(m => ordered.SequenceEqual(GetOrderedNames(m)));
            }

            void ICollection<IEnumerable<string>>.CopyTo(IEnumerable<string>[] array, int arrayIndex) =>
                _source.GetMessagesByPropertyName().Cast<IEnumerable<string>>().ToList().CopyTo(array, arrayIndex);

            public IEnumerator<IEnumerable<string>> GetEnumerator() => _source.GetMessagesByPropertyName().Cast<IEnumerable<string>>().GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() =>
                ((IEnumerable)_source.GetMessagesByPropertyName().Cast<IEnumerable<string>>()).GetEnumerator();

            bool ICollection<IEnumerable<string>>.Remove(IEnumerable<string> item) => throw new NotSupportedException();
        }

        class ResultItem : IEquatable<ResultItem>
        {
            internal ResultItem(ValidationResult source)
            {
                Source = source;
                MemberNames = GetOrderedNames(source.MemberNames);
                Message = string.IsNullOrWhiteSpace(source.ErrorMessage) ? "" : source.ErrorMessage.Trim();
            }
            internal ResultItem(string message, IEnumerable<string> memberNames) : this(new ValidationResult(message, memberNames)) { }
            internal ResultItem(string message, params string[] memberNames) : this(new ValidationResult(message, memberNames)) { }
            internal ValidationResult Source { get; }
            internal string[] MemberNames { get; }
            internal string Message { get; }
            public bool Equals(ResultItem other) => !(other is null) && (ReferenceEquals(this, other) ||
                (Message == other.Message && MemberNames.SequenceEqual(other.MemberNames)));
        }
    }
}
