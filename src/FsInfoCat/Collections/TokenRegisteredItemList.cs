using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Collections
{
    // TODO: Document TokenRegisteredItemList class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TokenRegisteredItemList<T> : IReadOnlyList<T>, IList<T>, IList
        where T : class
    {
        private readonly object _syncRoot = new();
        private Node.Enumerator _latestEnumerator;

        T IReadOnlyList<T>.this[int index] => Node.TryGet(this, index, out T value) ? value : throw new ArgumentOutOfRangeException(nameof(index));

        T IList<T>.this[int index] { get => Node.TryGet(this, index, out T value) ? value : throw new ArgumentOutOfRangeException(nameof(index)); set => throw new NotSupportedException(); }

        object IList.this[int index] { get => Node.TryGet(this, index, out T value) ? value : throw new ArgumentOutOfRangeException(nameof(index)); set => throw new NotSupportedException(); }

        public Node FirstNode { get; private set; }

        public Node LastNode { get; private set; }

        public object SyncRoot { get; } = new();

        int IReadOnlyCollection<T>.Count => Node.Count(this);

        int ICollection<T>.Count => Node.Count(this);

        int ICollection.Count => Node.Count(this);

        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsReadOnly => true;

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => true;

        public Node AddFirst(T value, CancellationToken removeToken) => Node.AddFirst(this, value ?? throw new ArgumentNullException(nameof(value)), removeToken);

        public Node AddLast(T value, CancellationToken removeToken) => Node.AddLast(this, value ?? throw new ArgumentNullException(nameof(value)), removeToken);

        public void Clear() => Node.Clear(this);

        public bool Contains(T item) => Node.Contains(this, item);

        public IEnumerator<T> GetEnumerator() => new Node.Enumerator(this);

        public bool Remove(T item) => Node.Remove(this, item);

        public T[] ToArray()
        {
            Monitor.Enter(SyncRoot);
            try { return Node.GetItems(this).ToArray(); }
            finally { Monitor.Exit(SyncRoot); }
        }

        void ICollection<T>.Add(T item) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        bool IList.Contains(object value) => value is T item && Node.Contains(this, item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            Monitor.Enter(SyncRoot);
            try { Node.GetItems(this).ToList().CopyTo(array, arrayIndex); }
            finally { Monitor.Exit(SyncRoot); }
        }

        void ICollection.CopyTo(Array array, int index) => ToArray().CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator() => ToArray().GetEnumerator();

        int IList<T>.IndexOf(T item) => Node.IndexOf(this, item);

        int IList.IndexOf(object value) => (value is T item) ? Node.IndexOf(this, item) : -1;

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        public class Node
        {
            private readonly TokenRegisteredItemList<T> _owner;

            public Node Next { get; private set; }

            public Node Previous { get; private set; }

            public T Value { get; }

            private Node([DisallowNull] TokenRegisteredItemList<T> owner, [DisallowNull] T item)
            {
                _owner = owner;
                Value = item;
            }

            internal static IEnumerable<T> GetItems([DisallowNull] TokenRegisteredItemList<T> owner)
            {
                for (Node currentNode = owner.FirstNode; currentNode is not null; currentNode = currentNode.Next)
                    yield return currentNode.Value;
            }

            internal static bool Remove([DisallowNull] TokenRegisteredItemList<T> owner, Node node)
            {
                if (node is null)
                    return false;

                Monitor.Enter(owner.SyncRoot);
                try
                {
                    if (node.Next is null)
                    {
                        if (node.Previous is null)
                        {
                            if (owner.LastNode is not null && ReferenceEquals(owner.LastNode, node))
                                owner.FirstNode = owner.LastNode = null;
                            else
                                return false;
                        }
                        else
                            node.Previous = (owner.LastNode = node.Previous).Next = null;
                    }
                    else
                    {
                        if (((owner.LastNode = node.Next).Previous = node.Previous) is null)
                            owner.FirstNode = node.Next;
                        else
                        {
                            node.Previous.Next = node.Next;
                            node.Previous = null;
                        }
                        node.Next = null;
                    }
                    owner._latestEnumerator = null;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return true;
            }

            internal static Node AddFirst(TokenRegisteredItemList<T> owner, T item, CancellationToken removeToken)
            {
                Node node = new(owner, item);
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    if ((node.Next = owner.FirstNode) is null)
                        owner.FirstNode = owner.LastNode = node;
                    else
                        owner.FirstNode = node.Next.Previous = node;
                    owner._latestEnumerator = null;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                removeToken.Register(() => Remove(owner, node));
                return node;
            }

            internal static Node AddLast(TokenRegisteredItemList<T> owner, T item, CancellationToken removeToken)
            {
                Node node = new(owner, item);
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    if ((node.Previous = owner.LastNode) is null)
                        owner.LastNode = owner.FirstNode = node;
                    else
                        owner.LastNode = node.Previous.Next = node;
                    owner._latestEnumerator = null;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                removeToken.Register(() => Remove(owner, node));
                return node;
            }

            internal static int Count(TokenRegisteredItemList<T> owner)
            {
                int count = 0;
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    for (Node currentNode = owner.FirstNode; currentNode is not null; currentNode = currentNode.Next)
                        count++;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return count;
            }

            internal static bool TryGet(TokenRegisteredItemList<T> owner, int index, out T item)
            {
                if (index < 0)
                {
                    item = null;
                    return false;
                }
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    int i = -1;
                    for (Node currentNode = owner.FirstNode; currentNode is not null; currentNode = currentNode.Next)
                    {
                        if (++i == index)
                        {
                            item = currentNode.Value;
                            return true;
                        }
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                item = null;
                return false;
            }

            internal static void Clear(TokenRegisteredItemList<T> owner)
            {
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    Node previousNode = owner.FirstNode;
                    owner.FirstNode = owner.LastNode = null;
                    owner._latestEnumerator = null;
                    if (previousNode is not null)
                    {
                        Node currentNode = previousNode.Next;
                        while (currentNode is not null)
                        {
                            currentNode.Previous = null;
                            previousNode.Next = null;
                            previousNode = currentNode;
                        }
                        previousNode.Previous = null;
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
            }

            internal static bool Contains(TokenRegisteredItemList<T> owner, T item)
            {
                if (item is null)
                    return false;
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    for (Node currentNode = owner.FirstNode; currentNode is not null; currentNode = currentNode.Next)
                    {
                        if (ReferenceEquals(currentNode.Value, item))
                            return true;
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return false;
            }

            internal static bool Remove(TokenRegisteredItemList<T> owner, T item)
            {
                if (item is null)
                    return false;
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    for (Node currentNode = owner.FirstNode; currentNode is not null; currentNode = currentNode.Next)
                    {
                        if (ReferenceEquals(currentNode.Value, item))
                            return Remove(owner, currentNode);
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return false;
            }

            internal static int IndexOf(TokenRegisteredItemList<T> owner, T item)
            {
                if (item is null)
                    return -1;
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    int index = -1;
                    for (Node currentNode = owner.FirstNode; currentNode is not null; currentNode = currentNode.Next)
                    {
                        index++;
                        if (ReferenceEquals(currentNode.Value, item))
                            return index;
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return -1;
            }

            internal sealed class Enumerator : IEnumerator<T>
            {
                private readonly object _syncRoot;
                private Enumerator _preceding;
                private Enumerator _following;
                private TokenRegisteredItemList<T> _owner;
                private Node _currentNode;
                private bool _endOfEnumeration;

                internal Enumerator([DisallowNull] TokenRegisteredItemList<T> owner)
                {
                    _syncRoot = (_owner = owner).SyncRoot;
                    if (owner._latestEnumerator is not null)
                        (_preceding = owner._latestEnumerator)._following = this;
                    owner._latestEnumerator = this;
                }

                public T Current => (_currentNode ?? throw new InvalidOperationException()).Value;

                object IEnumerator.Current => Current;

                public bool MoveNext()
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if ((_owner ?? throw new ObjectDisposedException(GetType().FullName))._latestEnumerator is null || !(ReferenceEquals(this, _owner._latestEnumerator) || _preceding is not null || _following is not null))
                            throw new InvalidOperationException();
                        if (_endOfEnumeration)
                            return false;

                        if (_currentNode is not null)
                        {
                            _currentNode = _currentNode.Next;
                            if (_currentNode is not null)
                                return true;
                        }
                        else if ((_currentNode = _owner.FirstNode) is not null)
                            return true;
                        _currentNode = null;
                        _endOfEnumeration = true;
                    }
                    finally { Monitor.Exit(_syncRoot); }
                    return false;
                }

                public void Reset()
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if ((_owner ?? throw new ObjectDisposedException(GetType().FullName))._latestEnumerator is null || !(ReferenceEquals(this, _owner._latestEnumerator) || _preceding is not null || _following is not null))
                            throw new InvalidOperationException();
                        _currentNode = null;
                        _endOfEnumeration = false;
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }

                private void Dispose(bool disposing)
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (disposing && _owner is not null)
                        {
                            if (_following is null)
                            {
                                if ((_owner._latestEnumerator = _preceding) is not null)
                                    _preceding = _preceding._following = null;
                            }
                            else
                            {
                                if ((_following._preceding = _preceding) is not null)
                                {
                                    _preceding._following = _following;
                                    _preceding = null;
                                }
                                _following = null;
                            }
                            _currentNode = null;
                            _endOfEnumeration = true;
                        }
                        _owner = null;
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }

                public void Dispose()
                {
                    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                    Dispose(disposing: true);
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
