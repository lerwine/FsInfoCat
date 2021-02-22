using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Test.Helpers
{
    public class StructEqualityHelper<T> : IEqualityComparer<T?>
        where T : struct
    {
        private readonly IEqualityComparer<T> _underlyingComparer;

        public StructEqualityHelper(IEqualityComparer<T> underlyingComparer = null)
        {
            _underlyingComparer = (underlyingComparer is null) ? EqualityComparer<T>.Default : underlyingComparer;
        }

        public bool Equals([AllowNull] T? x, [AllowNull] T? y) => (x.HasValue) ? y.HasValue && _underlyingComparer.Equals(x.Value, y.Value) : !y.HasValue;

        public int GetHashCode(T? obj) => (obj.HasValue) ? _underlyingComparer.GetHashCode(obj.Value) : 0;
    }
}
