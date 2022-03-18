using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public class ReferenceComparer<T> : IEqualityComparer<T>
    {
        public static ReferenceComparer<T> Default = new();

        private ReferenceComparer() { }

        public bool Equals(T x, T y) => (x is null) ? y is null : x is not null && ReferenceEquals(x, y);

        public int GetHashCode([DisallowNull] T obj) => (obj is null) ? 0 : obj.GetHashCode();
    }
}
