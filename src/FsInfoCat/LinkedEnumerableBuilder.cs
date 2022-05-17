using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    // TODO: Document LinkedEnumerableBuilder class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class LinkedEnumerableBuilder<T> : IEnumerable<T>
    {
        private readonly object _syncRoot = new();
        private Node _first;
        private Node _last;
        public int Count { get; private set; } = 0;
        public void Add(T value)
        {
            lock (_syncRoot)
            {
                _last = new Node(value, _last);
                if (_first is null)
                    _first = _last;
                Count++;
            }
        }
        public IEnumerable<T> Build() => new Enumerable(this);
        public IEnumerator<T> GetEnumerator() => new Enumerator(_first, _last);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        private sealed class Enumerable : IEnumerable<T>
        {
            private readonly Node _first;
            private readonly Node _last;
            internal Enumerable([DisallowNull] LinkedEnumerableBuilder<T> builder)
            {
                _first = builder._first;
                _last = builder._last;
            }
            public IEnumerator<T> GetEnumerator() => new Enumerator(_first, _last);
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        }
        private sealed class Node
        {
            internal Node(T value, Node previous)
            {
                Value = value;
                if (previous is not null)
                    previous.Next = this;
            }
            internal T Value { get; }
            internal Node Next { get; private set; }
        }
        private sealed class Enumerator : IEnumerator<T>
        {
            private bool _isDisposed = false;
            private readonly object _syncRoot = new();
            private bool _enumerated = false;
            private readonly Node _first;
            private readonly Node _last;
            private Node _current;

            internal Enumerator([AllowNull] Node first, [AllowNull] Node last)
            {
                _first = first;
                _last = last;
            }

            public T Current
            {
                get
                {
                    lock (_syncRoot)
                    {
                        if (_isDisposed)
                            throw new ObjectDisposedException(nameof(ByteArrayCoersion));
                        if (_current is null)
                            throw new InvalidOperationException(_enumerated ? "Index is after the last value" : "Index is before the first value");
                        return _current.Value;
                    }
                }
            }


            object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext()
            {
                lock (_syncRoot)
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(ByteArrayCoersion));
                    if (_enumerated)
                    {
                        if (_current is null || (_current = _current?.Next) is null)
                            return false;
                        if (ReferenceEquals(_current, _last))
                        {
                            _current = null;
                            return false;
                        }
                    }
                    _enumerated = true;
                    return !((_current = _first) is null);
                }
            }

            public void Reset()
            {
                lock (_syncRoot)
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(ByteArrayCoersion));
                    _enumerated = false;
                    _current = null;
                }
            }

            public void Dispose()
            {
                lock (_syncRoot)
                    _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
