using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Test.Helpers
{
    public class ReferenceEqualityHelper<T> : IEqualityComparer<T>
        where T : class
    {
        public static readonly ReferenceEqualityHelper<T> Default = new ReferenceEqualityHelper<T>();

        public bool Equals([AllowNull] T x, [AllowNull] T y) => ReferenceEquals(x, y);

        public int GetHashCode(T obj) => (obj is null) ? 0 : obj.GetHashCode();
    }
}
