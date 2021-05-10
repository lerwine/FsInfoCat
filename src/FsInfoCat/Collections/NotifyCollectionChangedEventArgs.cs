using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace FsInfoCat.Collections
{
    /// <summary>
    /// Provides a strongly-typed version of the <see cref="NotifyCollectionChangedEventArgs"/> class.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <seealso cref="NotifyCollectionChangedEventArgs" />
    public class NotifyCollectionChangedEventArgs<T> : NotifyCollectionChangedEventArgs
    {
        private static ReadOnlyCollection<T> Coerce(IEnumerable<T> value) => (value is null) ? null :
            ((value is ReadOnlyCollection<T> r) ? r : new ReadOnlyCollection<T>((value is IList<T> l) ? l : value.ToArray()));

        public new ReadOnlyCollection<T> NewItems => (ReadOnlyCollection<T>)base.NewItems;

        public new ReadOnlyCollection<T> OldItems => (ReadOnlyCollection<T>)base.OldItems;

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action) : base(action)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IEnumerable<T> changedItems)
            : base(action, Coerce(changedItems))
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T changedItem) : base(action, changedItem)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IEnumerable<T> newItems, IEnumerable<T> oldItems)
            : base(action, Coerce(newItems), Coerce(oldItems))
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IEnumerable<T> changedItems, int startingIndex)
            : base(action, Coerce(changedItems), startingIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T changedItem, int index) : base(action, changedItem, index)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T newItem, T oldItem) : base(action, newItem, oldItem)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IEnumerable<T> newItems, IEnumerable<T> oldItems, int startingIndex)
            : base(action, Coerce(newItems), Coerce(oldItems), startingIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IEnumerable<T> changedItems, int index, int oldIndex)
            : base(action, Coerce(changedItems), index, oldIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T changedItem, int index, int oldIndex)
            : base(action, changedItem, index, oldIndex)
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T newItem, T oldItem, int index) : base(action, newItem, oldItem, index)
        {
        }
    }
}
