using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Collections
{
    public abstract class GeneralizableListBase<T> : IGeneralizableList<T>
    {
        public abstract T this[int index] { get; set; }
        object IList.this[int index] { get => this[index]; set => GenericSet(index, value); }
        public abstract int Count { get; }
        protected virtual bool IsSynchronized => false;
        protected virtual object SyncRoot => null;
        bool ICollection<T>.IsReadOnly => false;
        bool IList.IsFixedSize => false;
        bool IList.IsReadOnly => false;
        bool ICollection.IsSynchronized => IsSynchronized;
        object ICollection.SyncRoot => SyncRoot;
        public abstract void Add(T item);
        protected virtual int GenericAdd(object value)
        {
            int result = Count;
            Add((T)value);
            return result;
        }
        public abstract void Clear();
        public abstract bool Contains(T item);
        protected virtual bool GenericContains(object value) => Coersion<T>.Default.TryCoerce(value, out T t) && Contains(t);
        public abstract void CopyTo(T[] array, int arrayIndex);
        protected abstract void CopyTo(Array array, int index);
        public abstract IEnumerator<T> GetEnumerator();
        public abstract int IndexOf(T item);
        protected virtual int GenericIndexOf(object value) => Coersion<T>.Default.TryCoerce(value, out T t) ? IndexOf(t) : -1;
        public abstract void Insert(int index, T item);
        protected virtual void GenericInsert(int index, object value) => Insert(index, (T)value);
        public abstract bool Remove(T item);
        protected virtual void GenericRemove(object value)
        {
            if (Coersion<T>.Default.TryCoerce(value, out T t))
                Remove(t);
        }
        public abstract void RemoveAt(int index);
        protected virtual void GenericSet(int index, object value) => this[index] = (T)value;
        IEnumerator IEnumerable.GetEnumerator() => GetGenericEnumerator();
        protected abstract IEnumerator GetGenericEnumerator();
        int IList.Add(object value) => GenericAdd(value);
        bool IList.Contains(object value) => GenericContains(value);
        int IList.IndexOf(object value) => GenericIndexOf(value);
        void IList.Insert(int index, object value) => GenericInsert(index, value);
        void IList.Remove(object value) => GenericRemove(value);
        void ICollection.CopyTo(Array array, int index) => this.ToArray().CopyTo(array, index);
    }
}
