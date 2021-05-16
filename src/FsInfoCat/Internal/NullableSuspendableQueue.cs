using System.Collections.Generic;

namespace FsInfoCat.Internal
{
    class NullableSuspendableQueue<T> : SuspendableQueue<T?>
        where T : struct
    {
        private readonly IEqualityComparer<T?> _itemComparer;

        public NullableSuspendableQueue(IEqualityComparer<T?> itemComparer)
        {
            _itemComparer = itemComparer ?? Extensions.GetComparisonService().GetEqualityComparer<T?>();
        }

        protected override bool AreEqual(T? x, T? y)
        {
            throw new System.NotImplementedException();
        }

        protected override int GetHashcode(T? obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
