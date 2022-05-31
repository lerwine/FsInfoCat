using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Collections
{
    // TODO: Document WeakReferenceList class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class WeakReferenceList<T> : IReadOnlyList<T>, IList<T>, IList
        where T : class
    {
        private Node _first;
        private Node _last;
        private IEnumerator<T> _latestEnumerator;

        T IReadOnlyList<T>.this[int index] => Node.TryGet(this, index, out T value) ? value : throw new ArgumentOutOfRangeException(nameof(index));

        T IList<T>.this[int index] { get => Node.TryGet(this, index, out T value) ? value : throw new ArgumentOutOfRangeException(nameof(index)); set => throw new NotSupportedException(); }

        object IList.this[int index] { get => Node.TryGet(this, index, out T value) ? value : throw new ArgumentOutOfRangeException(nameof(index)); set => throw new NotSupportedException(); }

        public object SyncRoot { get; } = new();

        int IReadOnlyCollection<T>.Count => Node.Count(this);

        int ICollection<T>.Count => Node.Count(this);

        int ICollection.Count => Node.Count(this);

        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsReadOnly => true;

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => true;

        public void AddFirst(T item) => Node.AddFirst(this, item ?? throw new ArgumentNullException(nameof(item)));

        public void AddLast(T item) => Node.AddLast(this, item ?? throw new ArgumentNullException(nameof(item)));

        public void Clear() => Node.Clear(this);

        public bool Contains(T item) => Node.Contains(this, item);

        public IEnumerator<T> GetEnumerator() => Node.GetEnumerator(this);

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

        protected class Node
        {
            private readonly WeakReferenceList<T> _owner;

            public Node Previous { get; private set; }

            public Node Next { get; private set; }

            public WeakReference<T> Value { get; }

            private Node([DisallowNull] WeakReferenceList<T> owner, [DisallowNull] T value)
            {
                _owner = owner;
                Value = new(value);
            }

            internal static void AddFirst([DisallowNull] WeakReferenceList<T> owner, [DisallowNull] T item)
            {
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    Node node = new(owner, item);
                    if ((node.Next = owner._first) is null)
                        owner._first = owner._last = node;
                    else
                        owner._first = node.Next.Previous = node;
                    owner._latestEnumerator = null;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
            }

            internal static void AddLast([DisallowNull] WeakReferenceList<T> owner, [DisallowNull] T item)
            {
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    Node node = new(owner, item);
                    if ((node.Previous = owner._last) is null)
                        owner._last = owner._first = node;
                    else
                        owner._last = node.Previous.Next = node;
                    owner._latestEnumerator = null;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
            }

            internal static void Clear([DisallowNull] WeakReferenceList<T> owner)
            {
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    owner._first = owner._last = null;
                    owner._latestEnumerator = null;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
            }

            internal static bool Contains([DisallowNull] WeakReferenceList<T> owner, T item)
            {
                if (item is null)
                    return false;
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    using Enumerator enumerator = new(owner);
                    while (enumerator.MoveNext())
                    {
                        if (ReferenceEquals(enumerator.Current, item))
                            return true;
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return false;
            }

            internal static int Count([DisallowNull] WeakReferenceList<T> owner)
            {
                int result = 0;
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    using Enumerator enumerator = new(owner);
                    while (enumerator.MoveNext())
                        result++;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return result;
            }

            internal static bool TryGet([DisallowNull] WeakReferenceList<T> owner, int index, out T result)
            {
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    using Enumerator enumerator = new(owner);
                    int count = -1;
                    while (enumerator.MoveNext())
                    {
                        if (++count == index)
                        {
                            result = enumerator.Current;
                            return true;
                        }
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                result = null;
                return false;
            }

            internal static IEnumerator<T> GetEnumerator([DisallowNull] WeakReferenceList<T> owner) => new Enumerator(owner);

            internal static int IndexOf([DisallowNull] WeakReferenceList<T> owner, T item)
            {
                if (item is null)
                    return -1;
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    int index = -1;
                    using Enumerator enumerator = new(owner);
                    while (enumerator.MoveNext())
                    {
                        index++;
                        if (ReferenceEquals(enumerator.Current, item))
                            return index;
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return -1;
            }

            internal static IEnumerable<T> GetItems([DisallowNull] WeakReferenceList<T> owner)
            {
                for (Node currentNode = owner._first; currentNode is not null;)
                {
                    if (currentNode.Value.TryGetTarget(out T value))
                        yield return value;
                    else
                        currentNode = currentNode.Remove();
                }
            }

            internal static bool Remove([DisallowNull] WeakReferenceList<T> owner, T item)
            {
                if (item is null)
                    return false;
                Monitor.Enter(owner.SyncRoot);
                try
                {
                    for (Node currentNode = owner._first; currentNode is not null;)
                    {
                        if (currentNode.Value.TryGetTarget(out T value))
                        {
                            if (ReferenceEquals(item, value))
                            {
                                currentNode.Remove();
                                owner._latestEnumerator = null;
                                return true;
                            }
                            currentNode = currentNode.Next;
                        }
                        else
                            currentNode = currentNode.Remove();
                    }
                }
                finally { Monitor.Exit(owner.SyncRoot); }
                return false;
            }

            private Node Remove()
            {
                Node result = Next;
                if (result is null)
                {
                    if ((_owner._last = Previous) is null)
                        _owner._first = null;
                    else
                        Previous = Previous.Next = null;
                    return null;
                }
                Next = null;
                if ((result.Previous = Previous) is null)
                    _owner._first = result;
                else
                    Previous.Next = result;
                return result;
            }

            sealed class Enumerator : IEnumerator<T>
            {
                private readonly object _syncRoot;
                private Enumerator _preceding;
                private Enumerator _following;
                private WeakReferenceList<T> _owner;
                private Node _currentNode;
                private T _currentValue;
                private bool _endOfEnumeration;

                internal Enumerator([DisallowNull] WeakReferenceList<T> owner)
                {
                    _syncRoot = (_owner = owner).SyncRoot;
                    if (owner._latestEnumerator is Enumerator preceding)
                        (_preceding = preceding)._following = this;
                    owner._latestEnumerator = this;
                }

                public T Current => _currentValue ?? throw new InvalidOperationException();

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

                        Node nextNode = _currentNode?.Next;
                        if (nextNode is null && (_currentNode is not null || (nextNode = _owner._first) is null))
                        {
                            _currentValue = null;
                            _currentNode = null;
                            _endOfEnumeration = true;
                            return false;
                        }
                        while (!(_currentNode = nextNode).Value.TryGetTarget(out _currentValue))
                        {
                            if ((nextNode = _currentNode.Next) is null)
                            {
                                if ((_owner._last = _currentNode.Previous) is null)
                                    _owner._first = null;
                                else
                                    _owner._last.Next = null;
                                _currentValue = null;
                                _currentNode = null;
                                _endOfEnumeration = true;
                                return false;
                            }
                            if ((nextNode.Previous = _currentNode.Previous) is null)
                                _owner._first = nextNode;
                            else
                                nextNode.Previous.Next = nextNode;
                        }
                    }
                    finally { Monitor.Exit(_syncRoot); }
                    return true;
                }

                public void Reset()
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if ((_owner ?? throw new ObjectDisposedException(GetType().FullName))._latestEnumerator is null || !(ReferenceEquals(this, _owner._latestEnumerator) || _preceding is not null || _following is not null))
                            throw new InvalidOperationException();
                        _currentValue = null;
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
                            _currentValue = null;
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
