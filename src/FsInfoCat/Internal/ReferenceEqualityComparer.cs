using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Internal
{
    class ReferenceEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
        where T : class
    {
        public static readonly ReferenceEqualityComparer<T> Default = new ReferenceEqualityComparer<T>();
        EqualityComparer<T> _backingComparer = EqualityComparer<T>.Default;
        internal static readonly ReferenceEqualityComparer<T> Instance = new ReferenceEqualityComparer<T>();
        public bool Equals(T x, T y) => (x is null) ? y is null : !(y is null) && _backingComparer.Equals(x, y);

        public int GetHashCode(T obj) => (obj is null) ? 0 : _backingComparer.GetHashCode(obj);

        bool IEqualityComparer.Equals(object x, object y) => (x is null) ? y is null : ((x is string a) ? (y is string b) && a == b : !(y is string) && x.Equals(y));

        int IEqualityComparer.GetHashCode(object obj) => (obj is null) ? 0 : ((obj is T t) ? GetHashCode(t) : obj.GetHashCode());
    }
}
