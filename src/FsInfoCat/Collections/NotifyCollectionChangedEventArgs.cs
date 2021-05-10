using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace FsInfoCat.Collections
{
    public class NotifyCollectionChangedEventArgs<T> : NotifyCollectionChangedEventArgs
    {
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action) : base(action)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IGeneralizableList<T> changedItems) : base(action, changedItems)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem) : base(action, changedItem)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems) : base(action, newItems, oldItems)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex) : base(action, changedItems, startingIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index) : base(action, changedItem, index)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem) : base(action, newItem, oldItem)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index) : base(action, newItem, oldItem, index)
        {
        }
    }
}
