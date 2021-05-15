using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FsInfoCat.Collections
{
    public interface ISuspensionQueue<T> : ISuspensionProvider, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyCollectionChanged
        where T : class
    {
        event EventHandler<NotifyCollectionChangedEventArgs<T>> QueuedItemsChanged;

        new event EventHandler<NotifyCollectionChangedEventArgs<T>> CollectionChanged;

        /// <summary>
        /// Gets the number of elements stored in the backing <seealso cref="Queue{T}"/>.
        /// </summary>
        int QueuedItemsCount { get; }

        /// <summary>
        /// Removes all from the backing <seealso cref="Queue{T}"/> if <see cref="SuspensionProvider.IsSuspended"/> is <see langword="false"/>.
        /// </summary>
        void ClearQueuedItems();

        /// <summary>
        /// Removes all retrievable objects from the backing <seealso cref="Queue{T}"/>.
        /// </summary>
        void ForceClearAll();

        bool Contains(T item, bool includeQueued);

        bool ContainsQueuedItem(T item);

        T Dequeue();

        ISuspension_obsolete Suspend(bool enqueueAvailable);

        IList<T> DequeueAvailable();

        IList<T> DequeueAll();

        bool Enqueue(T item);

        T ForceDequeue(out bool fromAvailable);


    }
}
