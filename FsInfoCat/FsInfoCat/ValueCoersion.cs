using System.Collections.Generic;

namespace FsInfoCat
{
    internal class ValueCoersion<T> : Coersion<T>
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

}
