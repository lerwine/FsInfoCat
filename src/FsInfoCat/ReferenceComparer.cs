using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    // TODO: Document ReferenceComparer class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ReferenceComparer<T> : IEqualityComparer<T>
    {
        public static ReferenceComparer<T> Default = new();

        private ReferenceComparer() { }

        public bool Equals(T x, T y) => (x is null) ? y is null : x is not null && ReferenceEquals(x, y);

        public int GetHashCode([DisallowNull] T obj) => (obj is null) ? 0 : obj.GetHashCode();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
