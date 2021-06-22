using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemEventArgs<T> : EventArgs
    {
        public ItemEventArgs(T item)
        {
            Item = item;
        }
        public T Item { get; }
    }
}
