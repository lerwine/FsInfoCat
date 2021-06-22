using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Collections
{
    public class DictionaryEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
    {
        private readonly object _syncRoot = new object();
        private readonly IEnumerator<TKey> _backingEnumerator;
        private readonly IDictionary<TKey, TValue> _source;

        private KeyValuePair<TKey, TValue>? _current;
        private DictionaryEntry? _entry;

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                lock (_syncRoot)
                {
                    KeyValuePair<TKey, TValue>? current = _current;
                    if (!current.HasValue)
                        _current = current = new KeyValuePair<TKey, TValue>(Key, Value);
                    return current.Value;
                }
            }
        }

        public DictionaryEntry Entry
        {
            get
            {
                lock (_syncRoot)
                {
                    DictionaryEntry? entry = _entry;
                    if (!entry.HasValue)
                        _entry = entry = new DictionaryEntry(Current.Key, Current.Value);
                    return entry.Value;
                }
            }
        }

        public TKey Key { get; private set; }

        object IDictionaryEnumerator.Key => Key;

        public TValue Value { get; private set; }

        object IDictionaryEnumerator.Value => Value;

        object IEnumerator.Current => Current;

        public DictionaryEnumerator(IDictionary<TKey, TValue> source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _backingEnumerator = _source.Keys.GetEnumerator();
        }

        public void Dispose() => _backingEnumerator.Dispose();

        public bool MoveNext()
        {
            lock (_syncRoot)
            {
                _entry = null;
                _current = null;
                if (_backingEnumerator.MoveNext())
                {
                    Key = _backingEnumerator.Current;
                    Value = _source[Key];
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            lock (_syncRoot)
            {
                _backingEnumerator.Reset();
                Key = default;
                Value = default;
                _entry = null;
                _current = null;
            }
        }
    }
}
