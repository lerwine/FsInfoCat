using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Collections
{
    public abstract class GeneralizableReadOnlyListBase<T> : IGeneralizableReadOnlyList<T>
    {
        public abstract T this[int index] { get; }
        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }
        public abstract int Count { get; }
        protected virtual bool IsSynchronized => false;
        protected virtual object SyncRoot => null;
        bool IList.IsFixedSize => false;
        bool IList.IsReadOnly => true;
        bool ICollection.IsSynchronized => IsSynchronized;
        object ICollection.SyncRoot => SyncRoot;
        protected virtual bool GenericContains(object value) => Coersion<T>.Default.TryCoerce(value, out T t) && Contains(t);
        public abstract bool Contains(T item);
        public abstract IEnumerator<T> GetEnumerator();
        protected virtual int GenericIndexOf(object value) => Coersion<T>.Default.TryCoerce(value, out T t) ? IndexOf(t) : -1;
        public abstract int IndexOf(T item);
        IEnumerator IEnumerable.GetEnumerator() => GetGenericEnumerator();
        protected abstract IEnumerator GetGenericEnumerator();
        int IList.Add(object value) => throw new NotSupportedException();
        void IList.Clear() => throw new NotSupportedException();
        int IList.IndexOf(object value) => GenericIndexOf(value);
        void IList.Insert(int index, object value) => throw new NotSupportedException();
        void IList.Remove(object value) => throw new NotSupportedException();
        void ICollection.CopyTo(Array array, int index) => this.ToArray().CopyTo(array, index);
        bool IList.Contains(object value) => GenericContains(value);
        void IList.RemoveAt(int index) => throw new NotSupportedException();
    }
}
