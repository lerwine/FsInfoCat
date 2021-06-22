using System.Collections.Generic;

namespace FsInfoCat.Internal
{
    class SuspendableValueQueue<T> : SuspendableQueue<T>
        where T : struct
    {
        private readonly IEqualityComparer<T> _itemComparer;

        public SuspendableValueQueue(IEqualityComparer<T> itemComparer)
        {
            _itemComparer = itemComparer ?? Services.GetComparisonService().GetEqualityComparer<T>();
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
