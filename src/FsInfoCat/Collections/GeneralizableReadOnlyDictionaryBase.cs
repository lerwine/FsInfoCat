using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Collections
{
    public abstract class GeneralizableReadOnlyDictionaryBase<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>,
        IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary
    {
        public abstract TValue this[TKey key] { get; }
        public abstract IGeneralizedCollection<TKey> Keys { get; }
        public abstract IGeneralizedCollection<TValue> Values { get; }
        public abstract int Count { get; }
        protected virtual bool IsSynchronized => false;
        protected virtual object SyncRoot => null;
        public abstract bool ContainsKey(TKey key);
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            Keys.Select(k => new KeyValuePair<TKey, TValue>(k, this[k])).ToList().CopyTo(array, arrayIndex);
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);
        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            if (ContainsKey(key))
                try
                {
                    value = this[key];
                    return true;
                }
                catch { /* ignore on purpose */ }
            value = default;
            return false;
        }

        protected IEnumerator GetGenericEnumerator() => ((IEnumerable)Keys.Select(k => new KeyValuePair<TKey, TValue>(k, this[k]))).GetEnumerator();

        protected virtual bool CollectionContains(KeyValuePair<TKey, TValue> item) => ContainsKey(item.Key) &&
            EqualityComparer<TValue>.Default.Equals(this[item.Key], item.Value);

        protected virtual object GenericGet(object key) => this[(TKey)key];

        protected virtual bool GenericContains(object key) => Coersion<TKey>.Default.TryCoerce(key, out TKey k) && ContainsKey(k);

        protected virtual void GenericCopyTo(Array array, int index) => Keys.Select(k => new KeyValuePair<TKey, TValue>(k, this[k])).ToArray().CopyTo(array, index);

        protected virtual IDictionaryEnumerator GetGenericDictionaryEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);

        #region Explicit members

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        ICollection IDictionary.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        ICollection IDictionary.Values => Values;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

        bool IDictionary.IsReadOnly => true;

        bool IDictionary.IsFixedSize => false;

        bool ICollection.IsSynchronized => IsSynchronized;

        object ICollection.SyncRoot => SyncRoot;

        TValue IDictionary<TKey, TValue>.this[TKey key] { get => this[key]; set => throw new NotSupportedException(); }

        object IDictionary.this[object key] { get => GenericGet(key); set => throw new NotSupportedException(); }

        IDictionaryEnumerator IDictionary.GetEnumerator() => GetGenericDictionaryEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetGenericEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        void IDictionary.Add(object key, object value) => throw new NotSupportedException();

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw new NotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => CollectionContains(item);

        bool IDictionary.Contains(object key) => GenericContains(key);

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => throw new NotSupportedException();

        void IDictionary.Clear() => throw new NotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        bool IDictionary<TKey, TValue>.Remove(TKey key) => throw new NotSupportedException();

        void IDictionary.Remove(object key) => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index) => GenericCopyTo(array, index);

        #endregion
    }
}
