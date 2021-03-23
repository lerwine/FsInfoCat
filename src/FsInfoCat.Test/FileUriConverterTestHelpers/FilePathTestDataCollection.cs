using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class FilePathTestDataCollection : Collection<FilePathTestDataItem>, ISynchronized
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        internal FilePathTestData Owner { get; set; }

        protected override void ClearItems()
        {
            lock (_syncRoot)
            {
                IEnumerable<FilePathTestDataItem> removed = Items.ToArray();
                using IEnumerator<FilePathTestDataItem> enumerator = removed.GetEnumerator();
                Clear(enumerator);
            }
        }

        private void Clear(IEnumerator<FilePathTestDataItem> enumerator)
        {
            if (enumerator.MoveNext())
            {
                FilePathTestDataItem item = enumerator.Current;
                lock (item.SyncRoot)
                {
                    if (ReferenceEquals(item.OwnerCollection, this))
                        item.OwnerCollection = null;
                    Clear(enumerator);
                }
            }
            else
                base.ClearItems();
        }

        protected override void InsertItem(int index, FilePathTestDataItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            lock (_syncRoot)
            {
                lock (item.SyncRoot)
                {
                    if (item.OwnerCollection is null)
                    {
                        try
                        {
                            item.OwnerCollection = this;
                            base.InsertItem(index, item);
                        }
                        catch
                        {
                            if (ReferenceEquals(item.OwnerCollection, this))
                                item.OwnerCollection = null;
                            throw;
                        }
                    }
                    else
                        throw new ArgumentOutOfRangeException(nameof(item));
                }
            }
        }

        protected override void RemoveItem(int index)
        {
            lock (_syncRoot)
            {
                FilePathTestDataItem item = Items[index];
                lock (item.SyncRoot)
                {
                    try
                    {
                        if (ReferenceEquals(item.OwnerCollection, this))
                            item.OwnerCollection = null;
                        base.RemoveItem(index);
                    }
                    catch
                    {
                        if (item.OwnerCollection is null)
                            item.OwnerCollection = this;
                        throw;
                    }
                }
            }
        }

        protected override void SetItem(int index, FilePathTestDataItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            lock (_syncRoot)
            {
                lock (item.SyncRoot)
                {
                    FilePathTestDataItem replacing = Items[index];
                    if (ReferenceEquals(item, replacing))
                        return;
                    if (item.Owner is null)
                    {
                        lock (replacing.SyncRoot)
                        {
                            try
                            {
                                if (ReferenceEquals(replacing.OwnerCollection, this))
                                    replacing.OwnerCollection = null;
                                item.OwnerCollection = this;
                                base.SetItem(index, item);
                            }
                            catch
                            {
                                try
                                {
                                    if (replacing.OwnerCollection is null)
                                        replacing.OwnerCollection = this;
                                }
                                finally
                                {
                                    if (ReferenceEquals(item.OwnerCollection, this))
                                        item.OwnerCollection = null;
                                }
                                throw;
                            }
                        }
                    }
                }
            }
        }
    }
}
