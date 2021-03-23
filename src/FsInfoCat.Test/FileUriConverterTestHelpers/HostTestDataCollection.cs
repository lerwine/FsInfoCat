using System;
using System.Collections.ObjectModel;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class HostTestDataCollection : Collection<IHostType>
    {
        private readonly object _syncRoot = new object();
        protected override void InsertItem(int index, IHostType item)
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
            
        protected override void SetItem(int index, IHostType item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            lock (_syncRoot)
            {
                IHostType replacing = Items[index];
                if (ReferenceEquals(item, replacing))
                    return;
                base.SetItem(index, item);
            }
        }
    }

}
