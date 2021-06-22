using FsInfoCat.Collections;
using System.Collections;

namespace FsInfoCat.Internal
{
    internal abstract class ComparerBase<T> : IGeneralizableComparer<T>
    {
        public abstract int Compare(T x, T y);

        int IComparer.Compare(object x, object y) => Coersion<T>.Default.TryCoerce(x, out T a) ? (Coersion<T>.Default.TryCoerce(y, out T b) ? Compare(a, b) : -1) :
            ((x is null) ? ((y is null) ? 1 : 0) : Comparer.Default.Compare(x, y));
    }
}
