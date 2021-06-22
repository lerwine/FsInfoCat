using System.Collections.Generic;

namespace FsInfoCat
{
    internal class NullableCoersion<T> : Coersion<T?>
        where T : struct
    {
        private static readonly EqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public override bool Equals(T? x, T? y) => x.HasValue ? (y.HasValue && _comparer.Equals(x.Value, y.Value)) : !y.HasValue;

        public override int GetHashCode(T? obj) => obj.HasValue ? _comparer.GetHashCode(obj.Value) : 0;

        public override bool TryCast(object obj, out T? result)
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

}
