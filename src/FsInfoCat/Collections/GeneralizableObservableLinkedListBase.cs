using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Collections
{
    public abstract class GeneralizableObservableLinkedListBase<T> : GeneralizableListBase<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly object _syncRoot = new object();
        private int _count = 0;
        protected Node FirstNode { get; private set; }
        protected Node LastNode { get; private set; }
        private EventSuspension _eventSuspension;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected abstract IEqualityComparer<T> ValueComparer { get; }

        protected override object SyncRoot => _syncRoot;

        protected override bool IsSynchronized => true;

        public override int Count => _count;

        public override T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected IDisposable BlockReentrancy() => new EventSuspension(this);

        public override void Add(T item)
        {
            Monitor.Enter(_syncRoot);
            try { throw new NotImplementedException(); }
            finally { Monitor.Exit(_syncRoot); }
            throw new NotImplementedException();
        }

        protected void CheckReentrancy()
        {

        }

        public override void Clear()
        {
            Monitor.Enter(_syncRoot);
            try { throw new NotImplementedException(); }
            finally { Monitor.Exit(_syncRoot); }
            throw new NotImplementedException();
        }

        public override bool Contains(T item)
        {
            Monitor.Enter(_syncRoot);
            try { throw new NotImplementedException(); }
            finally { Monitor.Exit(_syncRoot); }
            throw new NotImplementedException();
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        protected override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<T> GetItems()
        {
            for (Node node = FirstNode; !(node is null); node = node.Next)
                yield return node.Value;
        }

        private Node GetNode(int index)
        {
            if (index < 0 || index >= _count)
                return null;
            Node node;
            if (index > _count >> 1)
            {
                node = LastNode;
                while (++index < _count)
                    node = node.Previous;
            }
            else
            {
                if (index == 0)
                    return FirstNode;
                node = FirstNode.Next;
                while (--index > 0)
                    node = node.Next;
            }
            return node;
        }

        protected static Node FindMatching(Node start, Predicate<T> predicate, out int index)
        {
            if (start is null)
            {
                index = -1;
                return null;
            }
            index = 0;
            while (!predicate(start.Value))
            {
                if ((start = start.Next) is null)
                {
                    index = -1;
                    return null;
                }
                index++;
            }
            return start;
        }

        public override int IndexOf(T item)
        {
            Monitor.Enter(_syncRoot);
            try { throw new NotImplementedException(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public override void Insert(int index, T item)
        {
            Monitor.Enter(_syncRoot);
            try { throw new NotImplementedException(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }

        private void RaiseSuspendedEvents(KeyValuePair<string, EventArgs>[] suspendedEvents)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(T item)
        {
            Monitor.Enter(_syncRoot);
            try { throw new NotImplementedException(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public override void RemoveAt(int index)
        {
            Monitor.Enter(_syncRoot);
            try { throw new NotImplementedException(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        protected override IEnumerator GetGenericEnumerator()
        {
            throw new NotImplementedException();
        }

        protected class Node
        {
            private GeneralizableObservableLinkedListBase<T> _collection;
            protected internal Node Previous { get; private set; }
            protected internal Node Next { get; private set; }
            protected internal T Value { get; }
            protected internal int GetIndex() => (Previous is null) ? 0 : Previous.GetIndex() + 1;
            internal Node(T value) { Value = value; }
            internal bool Insert(Node next, GeneralizableObservableLinkedListBase<T> collection)
            {
                _collection = collection;
                if ((Next = next) is null)
                {
                    if (collection.LastNode is null)
                    {
                        collection.FirstNode = collection.LastNode = this;
                        collection._count = 1;
                        return true;
                    }
                    else
                        collection.LastNode = (Previous = collection.LastNode).Next = this;
                }
                else if ((Previous = next.Previous) is null)
                    collection.FirstNode = next.Previous = this;
                else
                    next.Previous = Previous.Next = this;
                collection._count++;
                return false;
            }
            internal bool Append(Node previous, GeneralizableObservableLinkedListBase<T> collection)
            {
                _collection = collection;
                if ((Previous = previous) is null)
                {
                    if (collection.LastNode is null)
                    {
                        collection.FirstNode = collection.LastNode = this;
                        collection._count = 1;
                        return true;
                    }
                    else
                        collection.LastNode = (Next = collection.LastNode).Previous = this;
                }
                else if ((Next = previous.Next) is null)
                    collection.FirstNode = previous.Next = this;
                else
                    previous.Previous = Next.Previous = this;
                collection._count++;
                return false;
            }
            internal bool Remove()
            {
                if (_collection is null)
                    return false;
                if (Previous is null)
                {
                    if (Next is null)
                    {
                        _collection.FirstNode = _collection.LastNode = null;
                        _collection._count = 0;
                        _collection = null;
                        return true;
                    }
                    Next = (_collection.FirstNode = Next).Previous = null;
                }
                else if ((Previous.Next = Next) is null)
                    Previous = (_collection.LastNode = Previous).Next = null;
                else
                {
                    Next.Previous = Previous;
                    Previous = Next = null;
                }
                _collection._count--;
                _collection = null;
                return true;
            }
        }

        private class EventSuspension : IDisposable
        {
            private GeneralizableObservableLinkedListBase<T> _collection;
            private readonly object _syncRoot;
            internal LinkedList<KeyValuePair<string, EventArgs>> SuspendedEvents { get; } = new LinkedList<KeyValuePair<string, EventArgs>>();
            private EventSuspension _previous;
            private EventSuspension _next;

            internal EventSuspension(GeneralizableObservableLinkedListBase<T> collection)
            {
                _syncRoot = (_collection = collection ?? throw new ArgumentNullException(nameof(collection))).SyncRoot;
                Monitor.Enter(_syncRoot);
                collection._eventSuspension = ((_previous = collection._eventSuspension) is null) ? this : _previous._next = this;
            }

            protected virtual void Dispose(bool disposing)
            {
                GeneralizableObservableLinkedListBase<T> collection = _collection;
                if (collection is null || !disposing)
                    return;
                _collection = null;
                try
                {
                    if (_next is null)
                    {
                        if (!((collection._eventSuspension = _previous) is null))
                        {
                            _previous._next = null;
                            return;
                        }
                    }
                    else
                    {
                        if (!((_next._previous = _previous) is null))
                            _previous._next = _next;
                        return;
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
                collection.RaiseSuspendedEvents(SuspendedEvents.ToArray());
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            // ~EventSuspension()
            // {
            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //     Dispose(disposing: false);
            // }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }

    public class GeneralizableReadOnlyObservableLinkedListBase<T> : GeneralizableReadOnlyListBase<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        protected IList<T> BackingList { get; }

        public override T this[int index] => BackingList[index];

        public override int Count => BackingList.Count;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected override object SyncRoot => (BackingList is IList list) ? list.SyncRoot : null;

        protected override bool IsSynchronized => BackingList is IList list && list.IsSynchronized;

        protected GeneralizableReadOnlyObservableLinkedListBase() : this((ObservableCollection<T>)null) { }

        public GeneralizableReadOnlyObservableLinkedListBase(GeneralizableObservableLinkedListBase<T> backingList)
        {
            BackingList = backingList ?? throw new ArgumentNullException(nameof(backingList));
            backingList.CollectionChanged += BackingList_CollectionChanged;
            backingList.PropertyChanged += BackingList_PropertyChanged;
        }

        public GeneralizableReadOnlyObservableLinkedListBase(ObservableCollection<T> backingList)
        {
            BackingList = backingList ?? throw new ArgumentNullException(nameof(backingList));
            backingList.CollectionChanged += BackingList_CollectionChanged;
            ((INotifyPropertyChanged)backingList).PropertyChanged += BackingList_PropertyChanged;
        }

        private void BackingList_PropertyChanged(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);

        private void BackingList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => OnCollectionChanged(e);

        public override bool Contains(T item) => BackingList.Contains(item);

        public override IEnumerator<T> GetEnumerator() => BackingList.GetEnumerator();

        public override int IndexOf(T item) => BackingList.IndexOf(item);

        protected override IEnumerator GetGenericEnumerator() => ((IEnumerable)BackingList).GetEnumerator();

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChanged(string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
}
