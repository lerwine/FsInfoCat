using System.Collections.Generic;

namespace FsInfoCat
{
    // TODO: Document ReferenceCoersion class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    internal class ReferenceCoersion<T> : Coersion<T>
        where T : class
    {
        private static readonly EqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public override bool Equals(T x, T y) => (x is null) ? y is null : !(y is null) && _comparer.Equals(x, y);

        public override int GetHashCode(T obj) => (obj is null) ? 0 : _comparer.GetHashCode(obj);

        public override bool TryCast(object obj, out T result)
        {
            if (obj is null)
                result = null;
            else if (obj is T t)
                result = t;
            else
            {
                result = null;
                return false;
            }
            return true;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
