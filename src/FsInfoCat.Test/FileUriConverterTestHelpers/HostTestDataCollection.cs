using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class HostTestDataCollection : Collection<BaseHostType>
    {
        private readonly object _syncRoot = new object();

        public HostTestDataCollection() : base() { }

        public HostTestDataCollection(IList<BaseHostType> collection) : base(collection) { }

        protected override void InsertItem(int index, BaseHostType item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            lock (_syncRoot)
            {
                if (Items.Contains(item))
                    throw new ArgumentOutOfRangeException(nameof(index));
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, BaseHostType item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            lock (_syncRoot)
            {
                BaseHostType replacing = Items[index];
                if (ReferenceEquals(item, replacing))
                    return;
                base.SetItem(index, item);
            }
        }
    }

}
