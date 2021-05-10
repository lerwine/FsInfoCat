using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.Collections
{
    public abstract class GeneralizableDictionaryBase<TKey, TValue> : IGeneralizableDictionary<TKey, TValue>
    {
        public abstract TValue this[TKey key] { get; set; }
        public abstract object this[object key] { get; set; }

        public abstract ICollection<TKey> Keys { get; }
        public abstract ICollection<TValue> Values { get; }
        public abstract int Count { get; }
        public abstract bool IsReadOnly { get; }
        public abstract bool IsFixedSize { get; }
        public abstract bool IsSynchronized { get; }
        public abstract object SyncRoot { get; }

        ICollection IDictionary.Keys => throw new NotImplementedException();

        ICollection IDictionary.Values => throw new NotImplementedException();

        public abstract void Add(TKey key, TValue value);
        public abstract void Add(KeyValuePair<TKey, TValue> item);
        public abstract void Add(object key, object value);
        public abstract void Clear();
        public abstract bool Contains(KeyValuePair<TKey, TValue> item);
        public abstract bool Contains(object key);
        public abstract bool ContainsKey(TKey key);
        public abstract void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex);
        public abstract void CopyTo(Array array, int index);
        public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
        public abstract bool Remove(TKey key);
        public abstract bool Remove(KeyValuePair<TKey, TValue> item);
        public abstract void Remove(object key);
        public abstract bool TryGetValue(TKey key, out TValue value);

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public abstract class GeneralizableReadOnlyDictionaryBase<TKey, TValue> : IGeneralizableReadOnlyDictionary<TKey, TValue>
    {
        public abstract TValue this[TKey key] { get; set; }
        protected abstract object this[object key] { get; set; }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => throw new NotImplementedException();

        public abstract ICollection<TKey> Keys { get; }
        public abstract ICollection<TValue> Values { get; }
        public abstract int Count { get; }
        protected abstract bool IsReadOnly { get; }
        protected abstract bool IsFixedSize { get; }
        protected abstract bool IsSynchronized { get; }
        protected abstract object SyncRoot { get; }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw new NotImplementedException();

        ICollection IDictionary.Keys => throw new NotImplementedException();

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => throw new NotImplementedException();

        ICollection IDictionary.Values => throw new NotImplementedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => throw new NotImplementedException();

        bool IDictionary.IsFixedSize => throw new NotImplementedException();

        bool IDictionary.IsReadOnly => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        object IDictionary.this[object key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public abstract void Add(TKey key, TValue value);
        protected abstract void Add(KeyValuePair<TKey, TValue> item);
        protected abstract void Add(object key, object value);
        public abstract void Clear();
        protected abstract bool Contains(KeyValuePair<TKey, TValue> item);
        protected abstract bool Contains(object key);
        public abstract bool ContainsKey(TKey key);
        public abstract void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex);
        public abstract void CopyTo(Array array, int index);
        public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
        public abstract bool Remove(TKey key);
        public abstract bool Remove(KeyValuePair<TKey, TValue> item);
        public abstract void Remove(object key);
        public abstract bool TryGetValue(TKey key, out TValue value);

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        void IDictionary.Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary.Contains(object key)
        {
            throw new NotImplementedException();
        }
    }
    public class GeneralizableReadOnlyCollectionWrapper<T> : IList, ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, ICollection, IGeneralizableReadOnlyList<T>
    {
        private ReadOnlyCollection<T> _backingList;

        public T this[int index] { get => _backingList[index]; }

        T IList<T>.this[int index] { get => _backingList[index]; set => throw new NotSupportedException(); }

        object IList.this[int index] { get => _backingList[index]; set => throw new NotSupportedException(); }

        public int Count => _backingList.Count;

        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsReadOnly => true;

        protected bool IsSynchronized => ((ICollection)_backingList).IsSynchronized;

        bool ICollection.IsSynchronized => IsSynchronized;

        protected object SyncRoot => ((ICollection)_backingList).SyncRoot;

        object ICollection.SyncRoot => SyncRoot;

        protected bool IsFixedSize => ((IList)_backingList).IsFixedSize;

        bool IList.IsFixedSize => IsFixedSize;

        public bool Contains(T item) => _backingList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _backingList.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _backingList.GetEnumerator();

        public int IndexOf(T item) => _backingList.IndexOf(item);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_backingList).GetEnumerator();

        void ICollection<T>.Add(T item) => throw new NotSupportedException();

        void ICollection<T>.Clear() => throw new NotSupportedException();

        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        bool IList.Contains(object value) => Coersion<T>.Default.TryCoerce(value, out T result) && Contains(result);

        int IList.IndexOf(object value) => Coersion<T>.Default.TryCoerce(value, out T result) ? IndexOf(result) : -1;

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        public void CopyTo(Array array, int index) => ((ICollection)_backingList).CopyTo(array, index);
    }
}
