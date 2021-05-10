using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Collections
{
    public abstract class GeneralizableDictionaryBase<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable,
        IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, ICollection, IDictionary
    {
        public abstract TValue this[TKey key] { get; set; }
        public abstract IGeneralizedCollection<TKey> Keys { get; }
        public abstract IGeneralizedCollection<TValue> Values { get; }
        public abstract int Count { get; }
        protected virtual bool IsSynchronized => false;
        protected virtual object SyncRoot => null;
        public abstract void Add(TKey key, TValue value);
        public abstract void Clear();
        public abstract bool ContainsKey(TKey key);
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            Keys.Select(k => new KeyValuePair<TKey, TValue>(k, this[k])).ToList().CopyTo(array, arrayIndex);
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);
        public abstract bool Remove(TKey key);
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

        protected virtual void CollectionAdd(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        protected virtual bool CollectionContains(KeyValuePair<TKey, TValue> item) => ContainsKey(item.Key) &&
            EqualityComparer<TValue>.Default.Equals(this[item.Key], item.Value);

        protected virtual bool CollectionRemove(KeyValuePair<TKey, TValue> item) => ContainsKey(item.Key) &&
            EqualityComparer<TValue>.Default.Equals(this[item.Key], item.Value) && Remove(item.Key);

        protected virtual object GenericGet(object key) => this[(TKey)key];

        protected virtual void GenericSet(object key, object value) => this[(TKey)key] = (TValue)value;

        protected virtual void GenericAdd(object key, object value) => Add((TKey)key, (TValue)value);

        protected virtual bool GenericContains(object key) => Coersion<TKey>.Default.TryCoerce(key, out TKey k) && ContainsKey(k);

        protected virtual void GenericCopyTo(Array array, int index) => Keys.Select(k => new KeyValuePair<TKey, TValue>(k, this[k])).ToArray().CopyTo(array, index);

        protected virtual void GenericRemove(object key)
        {
            if (Coersion<TKey>.Default.TryCoerce(key, out TKey k))
                Remove(k);
        }

        protected virtual IDictionaryEnumerator GetGenericDictionaryEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);

        #region Explicit Members

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        ICollection IDictionary.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        ICollection IDictionary.Values => Values;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        bool ICollection.IsSynchronized => IsSynchronized;

        object ICollection.SyncRoot => SyncRoot;

        bool IDictionary.IsFixedSize => false;

        bool IDictionary.IsReadOnly => false;

        object IDictionary.this[object key] { get => GenericGet(key); set => GenericSet(key, value); }

        IDictionaryEnumerator IDictionary.GetEnumerator() => GetGenericDictionaryEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetGenericEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => CollectionAdd(item);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => CollectionContains(item);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => CollectionRemove(item);

        void ICollection.CopyTo(Array array, int index) => GenericCopyTo(array, index);

        void IDictionary.Add(object key, object value) => GenericAdd(key, value);

        bool IDictionary.Contains(object key) => GenericContains(key);

        void IDictionary.Remove(object key) => GenericRemove(key);

        #endregion
    }
}
