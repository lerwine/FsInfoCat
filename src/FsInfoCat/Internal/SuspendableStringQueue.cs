using FsInfoCat.Services;
using System.Collections.Generic;

namespace FsInfoCat.Internal
{
    class SuspendableStringQueue : SuspendableQueue<string>
    {
        private readonly IEqualityComparer<string> _itemComparer;

        public SuspendableStringQueue(IEqualityComparer<string> itemComparer)
        {
            _itemComparer = itemComparer ?? Extensions.GetComparisonService().GetEqualityComparer<string>();
        }

        protected override bool AreEqual(string x, string y)
        {
            throw new System.NotImplementedException();
        }

        protected override int GetHashcode(string obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
