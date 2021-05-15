using FsInfoCat.Services;
using System.Collections.Generic;

namespace FsInfoCat.Internal
{
    class SuspendableReferenceQueue<T> : SuspendableQueue<T>
        where T : class
    {
        private readonly IEqualityComparer<T> _itemComparer;

        public SuspendableReferenceQueue(IEqualityComparer<T> itemComparer)
        {
            _itemComparer = itemComparer ?? Extensions.GetComparisonService().GetEqualityComparer<T>();
        }

        protected override bool AreEqual(T x, T y)
        {
            throw new System.NotImplementedException();
        }

        protected override int GetHashcode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
