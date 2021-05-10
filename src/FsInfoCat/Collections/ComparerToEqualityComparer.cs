using System;
using System.Collections.Generic;

namespace FsInfoCat.Collections
{
    internal class ComparerToEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly IComparer<T> _comparer;
        private readonly Func<T, int> _getHashCode;

        public ComparerToEqualityComparer(IComparer<T> comparer) : this(comparer, null) { }

        public ComparerToEqualityComparer(IComparer<T> comparer, Func<T, int> getHashCode)
        {
            _comparer = comparer;
            _getHashCode = getHashCode ?? ((comparer is IEqualityComparer<T> equalityComparer) ? equalityComparer.GetHashCode : new Func<T, int>(t => EqualityComparer<T>.Default.GetHashCode(t)));
        }

        public bool Equals(T x, T y) => _comparer.Compare(x, y) == 0;

        public int GetHashCode(T obj) => _getHashCode(obj);
    }
}
