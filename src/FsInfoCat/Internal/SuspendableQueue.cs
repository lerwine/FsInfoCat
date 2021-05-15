using FsInfoCat.Collections;
using FsInfoCat.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Internal
{
    internal abstract class SuspendableQueue<T> : Suspendable, ISuspendableQueue<T>, IReadOnlyList<T>, IList<T>, IList
    {
        private static readonly IThreadLockService _threadLockService = Extensions.GetThreadLockService();
        private readonly LinkedList<T> _suspendedItems = new LinkedList<T>();
        private readonly LinkedList<T> _availableItems = new LinkedList<T>();
        private readonly Coersion<T> _coersion;

        private event NotifyCollectionChangedEventHandler _collectionChanged;

        public event EventHandler<NotifyCollectionChangedEventArgs<T>> CollectionChanged;

        public event EventHandler<NotifyCollectionChangedEventArgs<T>> SuspendedCollectionChanged;

        public event EventHandler CountChanged;

        public event EventHandler HasItemsChanged;

        public event EventHandler SuspendedCountChanged;

        public event EventHandler HasSuspendedItemsChanged;

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add => _collectionChanged += value;
            remove => _collectionChanged -= value;
        }

        T IReadOnlyList<T>.this[int index] => GetItem(index);

        T IList<T>.this[int index] { get => GetItem(index); set => throw new NotSupportedException(); }

        object IList.this[int index] { get => GetItem(index); set => throw new NotSupportedException(); }

        public int Count => _availableItems.Count;

        public int SuspendedCount => _suspendedItems.Count;

        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsReadOnly => true;

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => true;

        public bool HasItems { get; private set; }

        public bool HasSuspendedItems { get; private set; }

        protected SuspendableQueue() : this(null) { }

        protected SuspendableQueue(Coersion<T> genericCoersion) { _coersion = genericCoersion ?? Coersion<T>.Default; }

        private bool Add(T item, Func<LinkedList<T>, T, int> mutator)
        {
            bool? suspendedChanged;
            bool wasSuspended, hadItems;
            int index, oldIndex;
            T oldItem;
            using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
            {
                item = AddItem(item);
                wasSuspended = IsSuspended;
                if (wasSuspended)
                {
                    hadItems = HasSuspendedItems;
                    if ((oldIndex = Find(_suspendedItems, item, out LinkedListNode<T> oldNode)) < 1)
                    {
                        suspendedChanged = true;
                        index = mutator(_suspendedItems, item);
                        oldItem = default;
                    }
                    else
                    {
                        oldItem = oldNode.Value;
                        if (ShouldAddAndRemove(item, oldNode.Value, oldIndex))
                        {
                            suspendedChanged = null;
                            index = mutator(_suspendedItems, item);
                            _suspendedItems.Remove(oldNode);
                        }
                        else
                        {
                            oldIndex = -1;
                            suspendedChanged = true;
                            index = mutator(_suspendedItems, item);
                        }
                    }
                }
                else
                {
                    hadItems = HasItems;
                    if ((oldIndex = Find(_availableItems, item, out LinkedListNode<T> oldNode)) < 1)
                    {
                        suspendedChanged = false;
                        index = mutator(_availableItems, item);
                        oldItem = default;
                    }
                    else
                    {
                        oldItem = oldNode.Value;
                        if (ShouldAddAndRemove(item, oldNode.Value, oldIndex))
                        {
                            suspendedChanged = null;
                            index = mutator(_availableItems, item);
                            _suspendedItems.Remove(oldNode);
                        }
                        else
                        {
                            oldIndex = -1;
                            suspendedChanged = false;
                            index = mutator(_availableItems, item);
                        }
                    }
                }
            }
            if (!suspendedChanged.HasValue)
                return !wasSuspended;
            if (suspendedChanged.Value)
                try
                {
                    try
                    {
                        try { RaisePropertyChanged(nameof(SuspendedCount)); }
                        finally { OnSuspendedCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, item, index)); }
                    }
                    finally
                    {
                        if (oldIndex > -1)
                            OnSuspendedCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, oldItem, index));
                    }
                }
                finally
                {
                    if (!hadItems)
                        OnHasSuspendedItemsChanged(true);
                }
            else
                try
                {
                    try
                    {
                        try { RaisePropertyChanged(nameof(Count)); }
                        finally { OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, item, index)); }
                    }
                    finally
                    {
                        if (oldIndex > -1)
                            OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, oldItem, index));
                    }
                }
                finally
                {
                    if (!hadItems)
                        OnHasItemsChanged(true);
                }
            return !wasSuspended;
        }

        protected abstract bool AreEqual(T x, T y);

        IReadOnlyList<T> ISuspendableQueue<T>.AsReadOnlyList() => this;

        IList<T> ISuspendableQueue<T>.AsList() => this;

        IList ISuspendableQueue<T>.AsGenericList() => this;

        public void Clear() => Clear(false);

        public void Clear(bool includeSuspended)
        {
            T[] clearedAvailable, clearedSuspended;
            using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
            {
                if (includeSuspended)
                {
                    clearedAvailable = _availableItems.ToArray();
                    clearedSuspended = _suspendedItems.ToArray();
                    _availableItems.Clear();
                    _suspendedItems.Clear();
                }
                else
                {
                    clearedAvailable = _availableItems.ToArray();
                    clearedSuspended = Array.Empty<T>();
                    _availableItems.Clear();
                }
            }

            try
            {
                if (clearedAvailable.Length > 0)
                    try
                    {
                        try { OnCountChanged(0); }
                        finally { OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, clearedAvailable, 0)); }
                    }
                    finally { OnHasItemsChanged(false); }
            }
            finally
            {
                if (clearedSuspended.Length > 0)
                    try
                    {
                        try { OnSuspendedCountChanged(0); }
                        finally { OnSuspendedCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, clearedSuspended, 0)); }
                    }
                    finally { OnHasSuspendedItemsChanged(false); }
            }
        }

        private bool Contains(LinkedList<T> list, T item)
        {
            for (LinkedListNode<T> node = list.First; !(node is null); node = node.Next)
            {
                if (AreEqual(node.Value, item))
                    return true;
            }
            return false;
        }

        public bool Contains(T item) => Contains(item, false);

        public bool Contains(T item, bool includeSuspended) => Contains(_availableItems, item) || (includeSuspended && Contains(_suspendedItems, item));

        public T Dequeue() => Dequeue(false);

        public T Dequeue(bool includeSuspended) => Remove(includeSuspended, (LinkedList<T> list, out T result) =>
        {
            LinkedListNode<T> node = list.First;
            if (node is null)
            {
                result = default;
                return -1;
            }
            result = node.Value;
            list.RemoveFirst();
            return 0;
        });

        public IEnumerable<T> DequeueAll() => DequeueAll(false);

        public IEnumerable<T> DequeueAll(bool includeSuspended)
        {
            T[] clearedAvailable, clearedSuspended;
            using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
            {
                if (includeSuspended)
                {
                    clearedAvailable = _availableItems.ToArray();
                    clearedSuspended = _suspendedItems.ToArray();
                    _availableItems.Clear();
                    _suspendedItems.Clear();
                }
                else
                {
                    clearedAvailable = _availableItems.ToArray();
                    clearedSuspended = Array.Empty<T>();
                    _availableItems.Clear();
                }
            }

            try
            {
                if (clearedAvailable.Length > 0)
                    try
                    {
                        try { OnCountChanged(0); }
                        finally { OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, clearedAvailable, 0)); }
                    }
                    finally { OnHasItemsChanged(false); }
            }
            finally
            {
                if (clearedSuspended.Length > 0)
                    try
                    {
                        try { OnSuspendedCountChanged(0); }
                        finally { OnSuspendedCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, clearedSuspended, 0)); }
                    }
                    finally { OnHasSuspendedItemsChanged(false); }
            }

            if (clearedAvailable.Length > 0)
            {
                if (clearedSuspended.Length > 0)
                    return clearedAvailable.Concat(clearedAvailable);
                return clearedAvailable;
            }
            return clearedSuspended;
        }

        protected T AddItem(T item) => item;

#pragma warning disable IDE0060 // Remove unused parameter
        protected bool ShouldAddAndRemove(T newItem, T oldItem, int oldIndex) => true;
#pragma warning restore IDE0060 // Remove unused parameter

        public bool Enqueue(T item) => Add(item, (list, e) =>
        {
            int i = list.Count;
            list.AddLast(e);
            return i;
        });

        private int Find(LinkedList<T> list, T item, out LinkedListNode<T> node)
        {
            int index = -1;
            for (node = list.First; !(node is null); node = node.Next)
            {
                ++index;
                if (AreEqual(node.Value, item))
                    return index;
            }
            node = null;
            return -1;
        }

        protected T GetEnqueued(int index)
        {
            using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
            {
                if (TryGet(_suspendedItems, index, out T result))
                    return result;
            }
            throw new IndexOutOfRangeException();
        }

        public IEnumerator<T> GetEnumerator() => GetEnumerator(false);

        public IEnumerator<T> GetEnumerator(bool includeSuspended) => (includeSuspended ? _availableItems.Concat(_suspendedItems) : _availableItems).GetEnumerator();

        protected abstract int GetHashcode(T obj);

        protected T GetItem(int index)
        {
            using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
            {
                if (TryGet(_availableItems, index, out T result))
                    return result;
            }
            throw new IndexOutOfRangeException();
        }

        private int IndexOf(LinkedList<T> list, T item)
        {
            int index = -1;
            for (LinkedListNode<T> node = list.First; !(node is null); node = node.Next)
            {
                ++index;
                if (AreEqual(node.Value, item))
                    return index;
            }
            return -1;
        }

        public bool IsItemSuspended(T item) => Contains(_suspendedItems, item);

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs<T> args)
        {
            try { CollectionChanged?.Invoke(this, args); }
            finally { _collectionChanged?.Invoke(this, args); }
        }

        protected virtual void OnCountChanged(int count) => CountChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnHasItemsChanged(bool hasItems) => HasItemsChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnHasSuspendedItemsChanged(bool hasItems) => HasSuspendedItemsChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnSuspendedCollectionChanged(NotifyCollectionChangedEventArgs<T> args) =>
            SuspendedCollectionChanged?.Invoke(this, args);

        protected virtual void OnSuspendedCountChanged(int count) => SuspendedCountChanged?.Invoke(this, EventArgs.Empty);

        public T Pop() => Pop(false);

        public T Pop(bool includeSuspended) => Remove(includeSuspended, (LinkedList<T> list, out T result) =>
        {
            LinkedListNode<T> node = list.Last;
            if (node is null)
            {
                result = default;
                return -1;
            }
            int i = list.Count - 1;
            result = node.Value;
            list.RemoveLast();
            return i;
        });

        public bool Push(T item) => Add(item, (list, e) =>
        {
            list.AddFirst(e);
            return 0;
        });

        private T Remove(bool includeSuspended, FuncOut<LinkedList<T>, T, int> mutator)
        {
            bool suspendedChanged;
            T value;
            int index;
            using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
            {
                suspendedChanged = (index = mutator(_availableItems, out value)) < 0;
                if (suspendedChanged && (!includeSuspended || (index = mutator(_suspendedItems, out value)) < 0))
                    throw new InvalidOperationException();
            }
            if (suspendedChanged)
                try { RaisePropertyChanged(nameof(SuspendedCount)); }
                finally { OnSuspendedCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, value, index)); }
            else
                try { RaisePropertyChanged(nameof(Count)); }
                finally { OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, value, index)); }
            return value;
        }

        public bool Remove(T item) => Remove(item, false);

        public bool Remove(T item, bool includeSuspended)
        {
            bool? suspendedChanged;
            int index;
            using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
            {
                if ((index = Find(_availableItems, item, out LinkedListNode<T> node)) < 0)
                {
                    if (includeSuspended && (index = Find(_suspendedItems, item, out node)) > -1)
                    {
                        item = node.Value;
                        suspendedChanged = true;
                        _suspendedItems.Remove(node);
                    }
                    else
                        suspendedChanged = null;
                }
                else
                {
                    suspendedChanged = false;
                    _availableItems.Remove(node);
                }
            }
            if (!suspendedChanged.HasValue)
                return false;
            if (suspendedChanged.Value)
                try { RaisePropertyChanged(nameof(SuspendedCount)); }
                finally { OnSuspendedCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, item, index)); }
            else
                try { RaisePropertyChanged(nameof(Count)); }
                finally { OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, item, index)); }
            return true;
        }

        public ISuspension Suspend(bool noThreadLock, bool suspendCurrentItems)
        {
            ISuspension suspension = Suspend(noThreadLock);
            if (suspendCurrentItems)
            {
                T[] suspended;
                int index;
                try
                {
                    using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
                    {
                        index = _suspendedItems.Count;
                        if ((suspended = _availableItems.ToArray()).Length > 0)
                        {
                            _availableItems.Clear();
                            foreach (T item in suspended)
                                _suspendedItems.AddLast(item);
                        }
                    }
                    if (suspended.Length > 0)
                        try { OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, suspended, 0)); }
                        finally { OnSuspendedCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, suspended, index)); }
                }
                catch
                {
                    suspension.Dispose();
                    throw;
                }
            }
            return suspension;
        }

        public bool TryDequeue(out T item) => TryDequeue(false, out item);

        public bool TryDequeue(bool includeSuspended, out T item) => TryRemove(includeSuspended, (LinkedList<T> list, out T result) =>
        {
            LinkedListNode<T> node = list.First;
            if (node is null)
            {
                result = default;
                return -1;
            }
            result = node.Value;
            list.RemoveFirst();
            return 0;
        }, out item);

        private static bool TryGet(LinkedList<T> list, int index, out T result)
        {
            if (TryGet(list, index, out LinkedListNode<T> node))
            {
                result = default;
                return false;
            }
            result = node.Value;
            return true;
        }

        private static bool TryGet(LinkedList<T> list, int index, out LinkedListNode<T> node)
        {
            int e;
            if (index < 0 || index >= (e = list.Count))
            {
                node = null;
                return false;
            }
            ;
            if (index > e >> 1)
            {
                node = list.Last;
                while (!(node is null) && ++index < e)
                    node = node.Previous;
            }
            else
            {
                if (!((node = list.First) is null) && index > 0)
                {
                    do
                    {
                        if ((node = node.Previous) is null)
                            break;
                    } while (--index > 0);
                }
            }
            return !(node is null);
        }

        public bool TryPop(out T item) => TryPop(false, out item);

        public bool TryPop(bool includeSuspended, out T item) => TryRemove(includeSuspended, (LinkedList<T> list, out T result) =>
        {
            LinkedListNode<T> node = list.Last;
            if (node is null)
            {
                result = default;
                return -1;
            }
            int i = list.Count - 1;
            result = node.Value;
            list.RemoveLast();
            return i;
        }, out item);

        private bool TryRemove(bool includeSuspended, FuncOut<LinkedList<T>, T, int> mutator, out T removed)
        {
            bool suspendedChanged;
            int index;
            using (var threadLock = _threadLockService.GetThreadLock((ISuspendable)this))
            {
                suspendedChanged = (index = mutator(_availableItems, out removed)) < 0;
                if (suspendedChanged && (!includeSuspended || (index = mutator(_suspendedItems, out removed)) < 0))
                    return false;
            }
            if (suspendedChanged)
                try { RaisePropertyChanged(nameof(SuspendedCount)); }
                finally { OnSuspendedCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, removed, index)); }
            else
                try { RaisePropertyChanged(nameof(Count)); }
                finally { OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, removed, index)); }
            return true;
        }

        void ICollection<T>.Add(T item) => Enqueue(item);

        int IList.Add(object value)
        {
            int result = -1;
            Add((T)value, (list, e) =>
            {
                result = list.Count;
                list.AddLast(e);
                return result;
            });
            return result;
        }

        bool IList.Contains(object value) => Coersion<T>.Default.TryCoerce(value, out T item) && Contains(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _availableItems.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_availableItems).CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_availableItems).GetEnumerator();

        int IList<T>.IndexOf(T item) => IndexOf(_availableItems, item);

        int IList.IndexOf(object value) => Coersion<T>.Default.TryCoerce(value, out T item) ? IndexOf(_availableItems, item) : -1;

        private void Insert(int index, T item)
        {
            var result = AssertNotSuspended(() =>
            {
                int oldIndex;
                T removed;
                if (!TryGet(_availableItems, index, out LinkedListNode<T> node))
                    throw new IndexOutOfRangeException();

                item = AddItem(item);
                if ((oldIndex = Find(_availableItems, item, out LinkedListNode<T> oldNode)) < 0)
                {
                    _availableItems.AddBefore(node, item);
                    removed = default;
                }
                else if (ShouldAddAndRemove(item, oldNode.Value, oldIndex))
                {
                    removed = oldNode.Value;
                    _availableItems.AddBefore(node, item);
                    _availableItems.Remove(oldNode);
                }
                else
                {
                    removed = default;
                    _availableItems.AddBefore(node, item);
                    oldIndex = -1;
                }
                return new { OldIndex = oldIndex, OldItem = removed };
            });

            try
            {
                try { RaisePropertyChanged(nameof(Count)); }
                finally { OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, item, index)); }
            }
            finally
            {
                if (result.OldIndex > -1)
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, result.OldItem, result.OldIndex));
            }
        }

        void IList<T>.Insert(int index, T item) => Insert(index, item);

        void IList.Insert(int index, object value) => Insert(index, (T)value);

        void IList.Remove(object value)
        {
            if (value is T item)
                Remove(item);
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
    }
}
