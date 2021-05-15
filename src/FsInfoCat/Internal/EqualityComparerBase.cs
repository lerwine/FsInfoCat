using FsInfoCat.Collections;
using System.Collections;

namespace FsInfoCat.Internal
{
    internal abstract class EqualityComparerBase<T> : IGeneralizableEqualityComparer<T>
    {
        public abstract bool Equals(T x, T y);

        bool IEqualityComparer.Equals(object x, object y) => Coersion<T>.Default.TryCoerce(x, out T a) ? (Coersion<T>.Default.TryCoerce(y, out T b) && Equals(a, b)) :
            ((x is null) ? y is null : x.Equals(y));

        public abstract int GetHashCode(T obj);

        int IEqualityComparer.GetHashCode(object obj) => Coersion<T>.Default.TryCoerce(obj, out T t) ? GetHashCode(t) : ((obj is null) ? 0 : obj.GetHashCode());
    }
}
