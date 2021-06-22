using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Collections
{
    /// <summary>
    /// A base <see cref="IReadOnlyList{T}"/> class which is compatible with usages that require a generic <seealso cref="IList"/>.
    /// </summary>
    /// <typeparam name="T">The list element type.</typeparam>
    public abstract class GeneralizableReadOnlyListBase<T> : IReadOnlyList<T>, IList
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
        /// <summary>
        /// This provides the functionality for the generic <see cref="IList.Contains(object)"/> method.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>
        /// <see langword="true"/> ifteh specified <paramref name="value"/> exists in the current list; otherwise, <see langword="false"/>.
        /// </returns>
        protected virtual bool GenericContains(object value) => Coersion<T>.Default.TryCoerce(value, out T t) && Contains(t);
        public abstract bool Contains(T item);
        public abstract IEnumerator<T> GetEnumerator();
        /// <summary>
        /// This provides the functionality for the generic <see cref="IList.IndexOf(object)"/> method.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The zero-based index of the matched item or <c>-1</c> if the item was not found.</returns>
        protected virtual int GenericIndexOf(object value) => Coersion<T>.Default.TryCoerce(value, out T t) ? IndexOf(t) : -1;
        public abstract int IndexOf(T item);
        IEnumerator IEnumerable.GetEnumerator() => GetGenericEnumerator();
        /// <summary>
        /// This provides the functionality for the generic <see cref="IEnumerable.GetEnumerator"/> method.
        /// </summary>
        /// <returns>The generic <see cref="IEnumerator"/>.</returns>
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
