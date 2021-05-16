using System.Collections.Generic;

namespace FsInfoCat.Internal
{
    class SuspendableReferenceQueue<T> : SuspendableQueue<T>
        where T : class
    {
        private readonly IEqualityComparer<T> _itemComparer;

        public SuspendableReferenceQueue(IEqualityComparer<T> itemComparer)
        {
            _itemComparer = itemComparer ?? Services.GetComparisonService().GetEqualityComparer<T>();
        }

        protected override bool AreEqual(T x, T y) => _itemComparer.Equals(x, y);

        protected override int GetHashcode(T obj) => _itemComparer.GetHashCode(obj);
    }
}
