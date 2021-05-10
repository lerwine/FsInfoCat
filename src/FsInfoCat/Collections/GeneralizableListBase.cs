using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Collections
{
    /// <summary>
    /// A base <see cref="IList{T}"/> class which is compatible with usages that require a generic <seealso cref="IList"/>.
    /// </summary>
    /// <typeparam name="T">The list element type.</typeparam>
    public abstract class GeneralizableListBase<T> : IList<T>, IList
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
        /// <summary>
        /// This provides the functionality for the generic <see cref="IList.Add(object)"/> method.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>The index of the added element.</returns>
        protected virtual int GenericAdd(object value)
        {
            int result = Count;
            Add((T)value);
            return result;
        }
        public abstract void Clear();
        public abstract bool Contains(T item);
        /// <summary>
        /// This provides the functionality for the generic <see cref="IList.Contains(object)"/> method.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>
        /// <see langword="true"/> ifteh specified <paramref name="value"/> exists in the current list; otherwise, <see langword="false"/>.
        /// </returns>
        protected virtual bool GenericContains(object value) => Coersion<T>.Default.TryCoerce(value, out T t) && Contains(t);
        public abstract void CopyTo(T[] array, int arrayIndex);
        protected abstract void CopyTo(Array array, int index);
        public abstract IEnumerator<T> GetEnumerator();
        public abstract int IndexOf(T item);
        /// <summary>
        /// This provides the functionality for the generic <see cref="IList.IndexOf(object)"/> method.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The zero-based index of the matched item or <c>-1</c> if the item was not found.</returns>
        protected virtual int GenericIndexOf(object value) => Coersion<T>.Default.TryCoerce(value, out T t) ? IndexOf(t) : -1;
        public abstract void Insert(int index, T item);
        /// <summary>
        /// This provides the functionality for the generic <see cref="IList.Insert(int, object)"/> method.
        /// </summary>
        /// <param name="index">The index at which to insert the new value.</param>
        /// <param name="value">The value to insert.</param>
        protected virtual void GenericInsert(int index, object value) => Insert(index, (T)value);
        public abstract bool Remove(T item);
        /// <summary>
        /// This provides the functionality for the generic <see cref="IList.Remove(object)"/> method.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        protected virtual void GenericRemove(object value)
        {
            if (Coersion<T>.Default.TryCoerce(value, out T t))
                Remove(t);
        }
        public abstract void RemoveAt(int index);
        /// <summary>
        /// This provides the functionality for the generic <see cref="IList"/> indexed mutator.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        protected virtual void GenericSet(int index, object value) => this[index] = (T)value;
        IEnumerator IEnumerable.GetEnumerator() => GetGenericEnumerator();
        /// <summary>
        /// This provides the functionality for the generic <see cref="IEnumerable.GetEnumerator"/> method.
        /// </summary>
        /// <returns>The generic <see cref="IEnumerator"/>.</returns>
        protected abstract IEnumerator GetGenericEnumerator();
        int IList.Add(object value) => GenericAdd(value);
        bool IList.Contains(object value) => GenericContains(value);
        int IList.IndexOf(object value) => GenericIndexOf(value);
        void IList.Insert(int index, object value) => GenericInsert(index, value);
        void IList.Remove(object value) => GenericRemove(value);
        void ICollection.CopyTo(Array array, int index) => this.ToArray().CopyTo(array, index);
    }
}
