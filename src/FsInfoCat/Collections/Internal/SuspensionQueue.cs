using FsInfoCat.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Collections.Internal
{
    /// <summary>
    /// Suspendable access to a backing <seealso cref="IList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <seealso cref="SuspensionProvider" />
    /// <seealso cref="ISuspensionQueue{T}" />
    public class SuspensionQueue<T> : SuspensionProvider, ISuspensionQueue<T>
        where T : class
    {
        private event NotifyCollectionChangedEventHandler GenericCollectionChanged;
        private readonly Collection<T> _queuedItems = new Collection<T>();
        private readonly Collection<T> _availableQueue = new Collection<T>();

        public event EventHandler<NotifyCollectionChangedEventArgs<T>> QueuedItemsChanged;

        public event EventHandler<NotifyCollectionChangedEventArgs<T>> CollectionChanged;

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add => GenericCollectionChanged += value;
            remove => GenericCollectionChanged -= value;
        }

        /// <summary>
        /// Gets the number of elements that can be retrieved from the backing <seealso cref="Queue{T}"/>.
        /// </summary>
        public int Count => _availableQueue.Count;

        bool ICollection.IsSynchronized => true;

        public int QueuedItemsCount => _queuedItems.Count;

        object ICollection.SyncRoot => SyncRoot;

        protected internal SuspensionQueue() { }

        /// <summary>
        /// Removes all from the backing <seealso cref="Queue{T}"/> if <see cref="SuspensionProvider.IsSuspended"/> is <see langword="false"/>.
        /// </summary>
        public virtual void Clear()
        {
            T[] oldItems;
            Monitor.Enter(SyncRoot);
            try
            {
                oldItems = _availableQueue.ToArray();
                _availableQueue.Clear();
            }
            finally { Monitor.Exit(SyncRoot); }
            if (oldItems.Length > 0)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, oldItems, 0));
        }

        /// <summary>
        /// Removes all from the backing <seealso cref="Queue{T}"/> if <see cref="SuspensionProvider.IsSuspended"/> is <see langword="false"/>.
        /// </summary>
        public virtual void ClearQueuedItems()
        {
            T[] oldItems;
            Monitor.Enter(SyncRoot);
            try
            {
                oldItems = _queuedItems.ToArray();
                _queuedItems.Clear();
            }
            finally { Monitor.Exit(SyncRoot); }
            if (oldItems.Length > 0)
                RaiseQueuedItemsChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, oldItems, 0));
        }

        /// <summary>
        /// Removes all retrievable objects from the backing <seealso cref="Queue{T}"/>.
        /// </summary>
        public virtual void ForceClearAll()
        {
            T[] oldQueuedItems;
            T[] oldAvailableItems;
            Monitor.Enter(SyncRoot);
            try
            {
                oldQueuedItems = _queuedItems.ToArray();
                oldAvailableItems = _availableQueue.ToArray();
                _queuedItems.Clear();
                _availableQueue.Clear();
            }
            finally { Monitor.Exit(SyncRoot); }
            try
            {
                if (oldQueuedItems.Length > 0)
                    RaiseQueuedItemsChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, oldQueuedItems, 0));
            }
            finally
            {
                if (oldAvailableItems.Length > 0)
                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, oldAvailableItems, 0));
            }
        }

        public bool Contains(T item, bool includeQueued)
        {
            if (item is null)
                return false;
            Monitor.Enter(SyncRoot);
            try
            {
                if (_availableQueue.Contains(item))
                    return true;
                return includeQueued && _queuedItems.Contains(item);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public bool Contains(T item)
        {
            if (item is null)
                return false;
            Monitor.Enter(SyncRoot);
            try { return _availableQueue.Contains(item); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public bool ContainsQueuedItem(T item)
        {
            Monitor.Enter(SyncRoot);
            try { return _queuedItems.Contains(item); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public void CopyTo(T[] array, int arrayIndex) => _availableQueue.CopyTo(array, arrayIndex);

        public virtual T Dequeue()
        {
            T result;
            Monitor.Enter(SyncRoot);
            try
            {
                result = _availableQueue[0];
                _availableQueue.RemoveAt(0);
            }
            finally { Monitor.Exit(SyncRoot); }
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, result, 0));
            return result;
        }

        public ISuspension Suspend(bool enqueueAvailable)
        {
            if (enqueueAvailable)
            {
                ISuspension result;
                T[] enqueued;
                int index;
                Monitor.Enter(SyncRoot);
                try
                {
                    index = _queuedItems.Count;
                    enqueued = _availableQueue.ToArray();
                    if (enqueued.Length > 0)
                    {
                        foreach (T item in enqueued)
                            _queuedItems.Add(item);
                        _availableQueue.Clear();
                    }
                    result = Suspend();
                }
                finally { Monitor.Exit(SyncRoot); }
                if (enqueued.Length > 0)
                    try
                    {
                        try { RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, enqueued, 0)); }
                        finally { RaiseQueuedItemsChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, enqueued, index)); }
                    }
                    catch
                    {
                        result.Dispose();
                        throw;
                    }
                return result;
            }
            return Suspend();
        }

        public virtual IList<T> DequeueAvailable()
        {
            List<T> result;
            Monitor.Enter(SyncRoot);
            try
            {
                result = _availableQueue.ToList();
                _availableQueue.Clear();
            }
            finally { Monitor.Exit(SyncRoot); }
            if (result.Count > 0)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, result.ToArray(), 0));
            return result;
        }

        public virtual IList<T> DequeueAll()
        {
            List<T> available, enqueued;
            Monitor.Enter(SyncRoot);
            try
            {
                available = _availableQueue.ToList();
                enqueued = _queuedItems.ToList();
                _availableQueue.Clear();
                _queuedItems.Clear();
            }
            finally { Monitor.Exit(SyncRoot); }
            try
            {
                if (available.Count > 0)
                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, available.ToArray(), 0));
            }
            finally
            {
                if (enqueued.Count > 0)
                {
                    RaiseQueuedItemsChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, enqueued.ToArray(), 0));
                    available.AddRange(enqueued);
                }
            }
            return available;
        }

        /// <summary>
        /// Adds an item to the end of the <see cref="SuspensionQueue{T}"/>.
        /// </summary>
        /// <param name="item">The item to be added to the end of the <see cref="SuspensionQueue{T}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the item was immediately available (<see cref="SuspensionProvider.IsSuspended"/> is <see langword="true"/>);
        /// otherwise <see langword="false"/> if it was enqueued to be made available after the suspended state has ended.
        /// </returns>
        public virtual bool Enqueue(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            bool wasSuspended;
            int index;
            int oldIndex;
            T oldItem;
            Monitor.Enter(SyncRoot);
            try
            {
                wasSuspended = IsSuspended;
                if (wasSuspended)
                {
                    oldIndex = _queuedItems.IndexOf(item);
                    index = _queuedItems.Count;
                    _queuedItems.Add(item);
                    if (oldIndex >= 0)
                    {
                        index--;
                        oldItem = _queuedItems[oldIndex];
                        _queuedItems.RemoveAt(oldIndex);
                    }
                    else
                        oldItem = default;
                }
                else
                {
                    oldIndex = _availableQueue.IndexOf(item);
                    index = _availableQueue.Count;
                    _availableQueue.Add(item);
                    if (oldIndex >= 0)
                    {
                        index--;
                        oldItem = _availableQueue[oldIndex];
                        _availableQueue.RemoveAt(oldIndex);
                    }
                    else
                        oldItem = default;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            if (wasSuspended)
            {
                if (oldIndex >= 0)
                    RaiseQueuedItemsChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, oldItem, oldIndex));
                RaiseQueuedItemsChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, item, index));
                return false;
            }
            if (oldIndex >= 0)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, oldItem, oldIndex));
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, item, index));
            return true;

        }

        public virtual T ForceDequeue(out bool fromAvailable)
        {
            T result;
            Monitor.Enter(SyncRoot);
            try
            {
                fromAvailable = _availableQueue.Count > 0;
                Collection<T> collection = fromAvailable ? _availableQueue : _queuedItems;
                result = collection[0];
                collection.RemoveAt(0);
            }
            finally { Monitor.Exit(SyncRoot); }
            if (fromAvailable)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, result, 0));
            else
                RaiseQueuedItemsChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, result, 0));
            return result;
        }

        public IEnumerator<T> GetEnumerator() => _availableQueue.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_availableQueue).CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator() => ((ICollection)_availableQueue).GetEnumerator();

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs<T> args) => CollectionChanged?.Invoke(this, args);

        protected virtual void OnQueuedItemsChanged(NotifyCollectionChangedEventArgs<T> args) => QueuedItemsChanged?.Invoke(this, args);

        private void RaiseQueuedItemsChanged(NotifyCollectionChangedEventArgs<T> args)
        {
            try
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                        break;
                    default:
                        RaisePropertyChanged(nameof(QueuedItemsCount));
                        break;
                }
            }
            finally { OnQueuedItemsChanged(args); }
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs<T> args)
        {
            try
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                        break;
                    default:
                        RaisePropertyChanged(nameof(QueuedItemsCount));
                        break;
                }
            }
            finally { OnCollectionChanged(args); }
        }
    }
}
