using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Desktop.Util
{
    public class LinkedComponentList<T> : IList<T>, IList, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler EmptyChanged;

        private readonly object _syncRoot = new object();

        public Node FirstNode { get; private set; }

        public Node LastNode { get; private set; }

        protected IEqualityComparer<T> Comparer { get; }

        public T this[int index]
        {
            get => (Get(index) ?? throw new IndexOutOfRangeException()).Value;
            set => Node.Replace(Get(index) ?? throw new IndexOutOfRangeException(), new Node(value), this);
        }

        object IList.this[int index]
        {
            get => (Get(index) ?? throw new IndexOutOfRangeException()).Value;
            set
            {
                Node node = Get(index) ?? throw new IndexOutOfRangeException();
                if (value is T item || TryCoerce(value, out item))
                    Node.Replace(node, new Node(item), this);
                else
                    throw new InvalidCastException();
            }
        }

        public int Count { get; private set; }

        public bool IsEmpty { get; private set; }

        bool ICollection<T>.IsReadOnly => false;

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => _syncRoot;

        public LinkedComponentList() : this(null, EqualityComparer<T>.Default) { }

        public LinkedComponentList(params T[] items) : this(items, null) { }

        public LinkedComponentList(IEqualityComparer<T> equalityComparer) : this(null, equalityComparer) { }

        public LinkedComponentList(IEqualityComparer<T> equalityComparer, params T[] items) : this(items, equalityComparer) { }

        public LinkedComponentList(IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        {
            Comparer = equalityComparer ?? EqualityComparer<T>.Default;
            if (!(items is null))
                Node.AddRange(items, this);
        }

        public void Add(T item) => Node.Add(new Node(item), this);

        int IList.Add(object value)
        {
            if (value is T item || TryCoerce(value, out item))
                return Node.Add(new Node(item), this);
            throw new InvalidCastException();
        }

        public void AddRange(IEnumerable<T> items) => Node.AddRange(items, this);

        public void Clear() => Node.Clear(this);

        public bool Contains(T item)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                for (Node node = FirstNode; !(node is null); node = node.Next)
                {
                    if (Comparer.Equals(item, node.Value))
                        return true;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return false;
        }

        bool IList.Contains(object value) => (value is T item || TryCoerce(value, out item)) && Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Values().ToList().CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => Values().ToArray().CopyTo(array, index);

        public Node Find(T item)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                for (Node node = FirstNode; !(node is null); node = node.Next)
                {
                    if (Comparer.Equals(node.Value, item))
                        return node;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return null;
        }

        public IEnumerator<T> GetEnumerator() => Values().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Values()).GetEnumerator();

        public int IndexOf(T item)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                int index = -1;
                for (Node node = FirstNode; !(node is null); node = node.Next)
                {
                    ++index;
                    if (Comparer.Equals(item, node.Value))
                        return index;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return -1;
        }

        int IList.IndexOf(object value) => (value is T item || TryCoerce(value, out item)) ? IndexOf(item) : -1;

        public void Insert(int index, T item) => Node.Insert(Get(index) ?? throw new ArgumentOutOfRangeException(nameof(index)),
            new Node(item), true, this);

        public void InsertBefore(Node node, T item) => Node.Insert(node, new Node(item), true, this);

        public void InsertAfter(Node node, T item) => Node.Insert(node, new Node(item), false, this);

        void IList.Insert(int index, object value)
        {
            if (value is T item || TryCoerce(value, out item))
                Insert(index, item);
            else
                throw new InvalidCastException();
        }

        protected virtual void OnEmptyChanged() => EmptyChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        private void RaiseEmptyChanged()
        {
            try { RaisePropertyChanged(nameof(IsEmpty)); }
            finally { OnEmptyChanged(); }
        }

        private void RaisePropertyChanged(string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        public bool Remove(T item)
        {
            Node node = Find(item);
            return !(node is null) && Node.Remove(node, this);
        }

        void IList.Remove(object value)
        {
            if (value is T item || TryCoerce(value, out item))
                Remove(item);
        }

        public void RemoveAt(int index) => Node.Remove(Get(index) ?? throw new ArgumentOutOfRangeException(nameof(index)), this);

        private Node Get(int index)
        {
            if (index < 0)
                return null;
            Monitor.Enter(_syncRoot);
            try
            {
                if (index < Count)
                {
                    if (index == 0)
                        return FirstNode;
                    Node node;
                    if (index > Count << 1)
                    {
                        node = LastNode;
                        int c = Count;
                        while (--c > index)
                            node = node.Previous;
                    }
                    else
                    {
                        node = FirstNode.Next;
                        int c = 0;
                        while (++c < index)
                            node = node.Next;
                    }
                    return node;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return null;
        }

        protected virtual bool TryCoerce(object value, out T result)
        {
            result = default;
            return false;
        }

        protected IEnumerable<T> Values()
        {
            Node node = FirstNode;
            while (!(node is null))
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (!ReferenceEquals(this, node.Owner))
                        throw new InvalidOperationException("Collection was modified");
                    yield return node.Value;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public class Node
        {
            private object _syncRoot = new object();
            public T Value { get; }
            public LinkedComponentList<T> Owner { get; private set; }
            public Node Previous { get; private set; }
            public Node Next { get; private set; }
            internal Node(T value) { Value = value; }

            internal static bool Remove(Node item, LinkedComponentList<T> owner)
            {
                if (owner is null)
                    throw new ArgumentNullException(nameof(owner));
                if (item is null)
                    return false;
                bool raiseFirstNodeChanged, raiseLastNodehanged;
                Monitor.Enter(item._syncRoot);
                try
                {
                    Monitor.Enter(owner._syncRoot);
                    try
                    {
                        if (!ReferenceEquals(item.Owner, owner))
                            return false;
                        item.Owner = null;
                        raiseFirstNodeChanged = item.Previous is null;
                        if (raiseFirstNodeChanged)
                        {
                            raiseLastNodehanged = (owner.FirstNode = item.Next) is null;
                            if (raiseLastNodehanged)
                            {
                                owner.LastNode = null;
                                owner.IsEmpty = true;
                            }
                            else
                                item.Next = item.Next.Previous = null;
                        }
                        else
                        {
                            raiseLastNodehanged = (item.Previous.Next = item.Next) is null;
                            if (raiseLastNodehanged)
                                item.Previous = (owner.LastNode = item.Previous).Next = null;
                            else
                            {
                                item.Next.Previous = item.Previous;
                                item.Previous = item.Next = null;
                            }
                        }
                        owner.Count--;
                    }
                    finally { Monitor.Exit(owner._syncRoot); }
                }
                finally { Monitor.Exit(item._syncRoot); }
                try
                {
                    if (raiseFirstNodeChanged)
                        try { owner.RaisePropertyChanged(nameof(owner.FirstNode)); }
                        finally
                        {
                            if (raiseLastNodehanged)
                                owner.RaisePropertyChanged(nameof(owner.LastNode));
                        }
                    else if (raiseLastNodehanged)
                        owner.RaisePropertyChanged(nameof(owner.LastNode));
                }
                finally
                {
                    try { owner.RaisePropertyChanged(nameof(owner.Count)); }
                    finally
                    {
                        if (raiseFirstNodeChanged && raiseLastNodehanged)
                            owner.RaiseEmptyChanged();
                    }
                }
                return true;
            }

            internal static int Add(Node item, LinkedComponentList<T> owner)
            {
                if (item is null)
                    throw new ArgumentNullException(nameof(item));
                if (owner is null)
                    throw new ArgumentNullException(nameof(owner));
                int result;
                bool raiseFirstNodeChanged;
                Monitor.Enter(item._syncRoot);
                try
                {
                    Monitor.Enter(owner._syncRoot);
                    try
                    {
                        if (!(item.Owner is null))
                            throw new ArgumentOutOfRangeException(nameof(item));
                        lock (owner._syncRoot)
                        {
                            result = owner.Count++;
                            raiseFirstNodeChanged = (item.Previous = owner.LastNode) is null;
                            if (raiseFirstNodeChanged)
                            {
                                owner.IsEmpty = false;
                                (item.Owner = owner).LastNode = owner.FirstNode = item;
                            }
                            else
                                (item.Owner = owner).LastNode = item.Previous.Next = item;
                        }
                    }
                    finally { Monitor.Exit(owner._syncRoot); }
                }
                finally { Monitor.Exit(item._syncRoot); }
                try
                {
                    if (raiseFirstNodeChanged)
                        owner.RaisePropertyChanged(nameof(owner.FirstNode));
                }
                finally
                {
                    try { owner.RaisePropertyChanged(nameof(owner.LastNode)); }
                    finally
                    {
                        try { owner.RaisePropertyChanged(nameof(owner.Count)); }
                        finally
                        {
                            if (raiseFirstNodeChanged)
                                owner.RaiseEmptyChanged();
                        }
                    }
                }
                return result;
            }

            internal static void Clear(LinkedComponentList<T> owner)
            {
                if (owner is null)
                    throw new ArgumentNullException(nameof(owner));
                Monitor.Enter(owner._syncRoot);
                try
                {
                    Node node = owner.FirstNode;
                    if (node is null)
                        return;
                    do
                    {
                        Node next = node.Next;
                        node.Owner = null;
                        next.Previous = node.Next = null;
                        node = next;
                    }
                    while (!(node is null));
                    owner.FirstNode = owner.LastNode = null;
                    owner.Count = 0;
                    owner.IsEmpty = true;
                }
                finally { Monitor.Exit(owner._syncRoot); }
                try { owner.RaisePropertyChanged(nameof(owner.FirstNode)); }
                finally
                {
                    try { owner.RaisePropertyChanged(nameof(owner.LastNode)); }
                    finally
                    {
                        try { owner.RaisePropertyChanged(nameof(owner.Count)); }
                        finally { owner.RaiseEmptyChanged(); }
                    }
                }
            }

            internal static void AddRange(IEnumerable<T> items, LinkedComponentList<T> owner)
            {
                if (owner is null)
                    throw new ArgumentNullException(nameof(owner));
                if (items is null)
                    return;
                bool raiseFirstNodeChanged;
                Monitor.Enter(owner._syncRoot);
                try
                {
                    using (IEnumerator<T> enumerator = items.GetEnumerator())
                    {
                        if (!enumerator.MoveNext())
                            return;
                        Node node = new Node(enumerator.Current) { Owner = owner };
                        raiseFirstNodeChanged = (node.Previous = owner.LastNode) is null;
                        if (raiseFirstNodeChanged)
                            owner.FirstNode = owner.LastNode = node;
                        else
                            owner.LastNode = node.Previous.Next = node;
                        owner.Count++;
                        while (enumerator.MoveNext())
                        {
                            node = new Node(enumerator.Current) { Owner = owner, Previous = node };
                            owner.LastNode = node.Previous.Next = node;
                            owner.Count++;
                        }
                    }
                }
                finally { Monitor.Exit(owner._syncRoot); }
                try
                {
                    if (raiseFirstNodeChanged)
                        owner.RaisePropertyChanged(nameof(owner.FirstNode));
                }
                finally
                {
                    try { owner.RaisePropertyChanged(nameof(owner.LastNode)); }
                    finally
                    {
                        try { owner.RaisePropertyChanged(nameof(owner.Count)); }
                        finally
                        {
                            if (raiseFirstNodeChanged)
                                owner.RaiseEmptyChanged();
                        }
                    }
                }
            }

            internal static void Insert(Node referenceItem, Node newItem, bool before, LinkedComponentList<T> owner)
            {
                if (newItem is null)
                    throw new ArgumentNullException(nameof(newItem));
                if (owner is null)
                    throw new ArgumentNullException(nameof(owner));
                bool raiseFirstNodeChanged, raiseLastNodehanged;
                Monitor.Enter(newItem._syncRoot);
                try
                {
                    if (!(newItem.Owner is null) || ReferenceEquals(referenceItem, newItem))
                        throw new ArgumentOutOfRangeException(nameof(newItem));
                    if (referenceItem is null)
                    {
                        Monitor.Enter(owner._syncRoot);
                        try
                        {
                            raiseFirstNodeChanged = raiseLastNodehanged = owner.FirstNode is null;
                            if (raiseFirstNodeChanged)
                            {
                                (newItem.Owner = owner).IsEmpty = false;
                                owner.FirstNode = owner.LastNode = newItem;
                            }
                            else
                                throw new ArgumentNullException(nameof(referenceItem));
                        }
                        finally { Monitor.Exit(owner._syncRoot); }
                    }
                    else
                    {
                        Monitor.Enter(referenceItem._syncRoot);
                        try
                        {
                            if (!ReferenceEquals(referenceItem.Owner, owner))
                                throw new ArgumentOutOfRangeException(nameof(referenceItem));
                            Monitor.Enter(owner._syncRoot);
                            try
                            {
                                if (before)
                                {
                                    raiseLastNodehanged = false;
                                    raiseFirstNodeChanged = (newItem.Previous = (newItem.Next = referenceItem).Previous) is null;
                                    if (raiseFirstNodeChanged)
                                        referenceItem.Previous = owner.FirstNode = newItem;
                                    else
                                        newItem.Previous.Next = referenceItem.Previous = newItem;
                                }
                                else
                                {
                                    raiseFirstNodeChanged = false;
                                    raiseLastNodehanged = (newItem.Next = (newItem.Previous = referenceItem).Next) is null;
                                    if (raiseLastNodehanged)
                                        referenceItem.Next = owner.LastNode = newItem;
                                    else
                                        newItem.Next.Previous = referenceItem.Next = newItem;
                                }
                                newItem.Owner = owner;
                                owner.Count++;
                            }
                            finally { Monitor.Exit(owner._syncRoot); }
                        }
                        finally { Monitor.Exit(referenceItem._syncRoot); }
                    }
                }
                finally { Monitor.Exit(newItem._syncRoot); }
                try
                {
                    if (raiseFirstNodeChanged)
                        try { owner.RaisePropertyChanged(nameof(owner.FirstNode)); }
                        finally
                        {
                            if (raiseLastNodehanged)
                                owner.RaisePropertyChanged(nameof(owner.LastNode));
                        }
                    else if (raiseLastNodehanged)
                        owner.RaisePropertyChanged(nameof(owner.LastNode));
                }
                finally
                {
                    try { owner.RaisePropertyChanged(nameof(owner.Count)); }
                    finally
                    {
                        if (raiseFirstNodeChanged && raiseLastNodehanged)
                            owner.RaiseEmptyChanged();
                    }
                }
            }

            internal static void Replace(Node oldNode, Node newNode, LinkedComponentList<T> owner)
            {
                if (oldNode is null)
                    throw new ArgumentNullException(nameof(oldNode));
                if (newNode is null)
                    throw new ArgumentNullException(nameof(newNode));
                if (owner is null)
                    throw new ArgumentNullException(nameof(owner));
                bool raiseFirstNodeChanged, raiseLastNodehanged;
                Monitor.Enter(newNode._syncRoot);
                try
                {
                    if (!(newNode.Owner is null) || ReferenceEquals(oldNode, newNode))
                        throw new ArgumentOutOfRangeException(nameof(newNode));
                    Monitor.Enter(oldNode._syncRoot);
                    try
                    {
                        if (!ReferenceEquals(oldNode.Owner, owner))
                            throw new ArgumentOutOfRangeException(nameof(oldNode));
                        if (owner.Comparer.Equals(oldNode.Value, newNode.Value))
                            return;
                        Monitor.Enter(owner._syncRoot);
                        try
                        {
                            raiseFirstNodeChanged = (newNode.Previous = oldNode.Previous) is null;
                            if (raiseFirstNodeChanged)
                                owner.FirstNode = newNode;
                            else
                                oldNode.Previous = null;
                            raiseLastNodehanged = (newNode.Next = oldNode.Next) is null;
                            if (raiseFirstNodeChanged)
                                owner.LastNode = newNode;
                            else
                                oldNode.Next = null;
                            oldNode.Owner = null;
                            newNode.Owner = owner;
                        }
                        finally { Monitor.Exit(owner._syncRoot); }
                    }
                    finally { Monitor.Exit(oldNode._syncRoot); }
                }
                finally { Monitor.Exit(newNode._syncRoot); }
                try
                {
                    if (raiseFirstNodeChanged)
                        owner.RaisePropertyChanged(nameof(owner.FirstNode));
                }
                finally
                {
                    if (raiseLastNodehanged)
                        owner.RaisePropertyChanged(nameof(owner.LastNode));
                }
            }
        }
    }
}
