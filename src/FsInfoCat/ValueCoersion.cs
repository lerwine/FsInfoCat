using System.Collections.Generic;

namespace FsInfoCat
{
    // TODO: Document ValueCoersion class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ValueCoersion<T> : Coersion<T>
        where T : struct
    {
        private static readonly EqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public override bool Equals(T x, T y) => _comparer.Equals(x, y);

        public override int GetHashCode(T obj) => _comparer.GetHashCode(obj);

        public override bool TryCast(object obj, out T result)
        {
            if (obj is T t)
            {
                result = t;
                return true;
            }
            result = default;
            return false;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
