using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FsInfoCat.Services
{
    public interface ISuspendableQueue<T> : ISuspendable, IEnumerable<T>, IEnumerable, INotifyCollectionChanged
    {
        new event EventHandler<NotifyCollectionChangedEventArgs<T>> CollectionChanged;
        event EventHandler<NotifyCollectionChangedEventArgs<T>> SuspendedCollectionChanged;
        event EventHandler CountChanged;
        event EventHandler HasItemsChanged;
        event EventHandler SuspendedCountChanged;
        event EventHandler HasSuspendedItemsChanged;

        bool HasItems { get; }

        bool HasSuspendedItems { get; }

        int Count { get; }

        int SuspendedCount { get; }

        IReadOnlyList<T> AsReadOnlyList();

        IList<T> AsList();

        IList AsGenericList();

        void Clear();

        void Clear(bool includeSuspended);

        bool Contains(T item);

        bool Contains(T item, bool includeSuspended);

        T Dequeue();

        T Dequeue(bool includeSuspended);

        IEnumerable<T> DequeueAll();

        IEnumerable<T> DequeueAll(bool includeSuspended);

        bool Enqueue(T item);

        IEnumerator<T> GetEnumerator(bool includeSuspended);

            bool IsItemSuspended(T item);

        T Pop();

        T Pop(bool includeSuspended);

        bool Push(T item);

        bool Remove(T item);

        bool Remove(T item, bool includeSuspended);

        ISuspension Suspend(bool noThreadLock, bool suspendCurrentItems);

        bool TryDequeue(out T item);

        bool TryDequeue(bool includeSuspended, out T item);

        bool TryPop(out T item);

        bool TryPop(bool includeSuspended, out T item);
    }
}
