using FsInfoCat.Collections;
using System;
using System.Collections;

namespace FsInfoCat.Internal
{
    internal class ComparableEqualityComparer<T> : EqualityComparerBase<T>, IGeneralizableOrderAndEqualityComparer<T> where T : class, IComparable<T>
    {
        private static readonly Coersion<T> _coersion = Coersion<T>.Default;
        internal static readonly IGeneralizableOrderAndEqualityComparer<T> Default = new ComparableEqualityComparer<T>();

        public int Compare(T x, T y) => (x is null) ? ((y is null) ? 0 : 1) : ((y is null) ? -1 : x.CompareTo(y));

        int IComparer.Compare(object x, object y) => _coersion.TryCoerce(x, out T a) ?
            (_coersion.TryCoerce(y, out T b) ? Compare(a, b) : -1) : (_coersion.TryCoerce(y, out _) ? 1 :
            ((x is null) ? ((y is null) ? 0 : -1) : (ReferenceEquals(x, y) ? 0 : Comparer.Default.Compare(x, y))));

        public override bool Equals(T x, T y) => (x is null) ? y is null : !(y is null) && x.CompareTo(y) == 0;

        public override int GetHashCode(T obj) => (obj is null) ? 0 : obj.GetHashCode();
    }
}
