using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Collections
{
    // TODO: Document WeakReferenceSet class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class WeakReferenceSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, IReadOnlySet<T>
        where T : class
    {
        private static readonly EqualityComparer<T> _comparer = EqualityComparer<T>.Default;
        private readonly HashSet<WeakReference<T>> _backingSet = new(new Comparer());

        int ICollection<T>.Count => _backingSet.Count;

        int IReadOnlyCollection<T>.Count => _backingSet.Count;

        bool ICollection<T>.IsReadOnly => ((ICollection<WeakReference<T>>)_backingSet).IsReadOnly;

        public bool Add(T item)
        {
            if (item is null)
                return false;
            Monitor.Enter(_backingSet);
            try
            {
                if (AsEnumerable().Contains(item, _comparer))
                    return false;
                _backingSet.Add(new(item));
            }
            finally { Monitor.Exit(_backingSet); }
            return true;
        }

        void ICollection<T>.Add(T item) => Add(item);

        public void Clear()
        {
            Monitor.Enter(_backingSet);
            try { _backingSet.Clear(); }
            finally { Monitor.Exit(_backingSet); }
        }

        public bool Contains(T item)
        {
            if (item is null)
                return false;
            Monitor.Enter(_backingSet);
            try { return AsEnumerable().Contains(item, _comparer); }
            finally { Monitor.Exit(_backingSet); }
        }

        public void CopyTo(T[] array, int arrayIndex) => AsEnumerable().ToList().CopyTo(array, arrayIndex);

        public void ExceptWith(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            Monitor.Enter(_backingSet);
            try
            {
                if (_backingSet.Count == 0)
                    return;
                other = other.Where(o => o is not null);
                foreach (WeakReference<T> toRemove in _backingSet.Where(w => !w.TryGetTarget(out T v) || other.Contains(v, _comparer)).ToArray())
                    _backingSet.Remove(toRemove);
            }
            finally { Monitor.Exit(_backingSet); }
        }

        public IEnumerable<T> AsEnumerable()
        {
            using Enumerator enumerator = new(this);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)AsEnumerable()).GetEnumerator();

        public void IntersectWith(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            Monitor.Enter(_backingSet);
            try
            {
                if (_backingSet.Count == 0)
                    return;
                foreach (WeakReference<T> toRemove in _backingSet.Where(w => !(w.TryGetTarget(out T v) && other.Contains(v))).ToArray())
                    _backingSet.Remove(toRemove);
            }
            finally { Monitor.Exit(_backingSet); }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            using IEnumerator<T> x = GetEnumerator();
            using IEnumerator<T> y = other.GetEnumerator();
            if (x.MoveNext())
            {
                T a = x.Current;
                if (y.MoveNext())
                {
                    T b = y.Current;
                    while (!_comparer.Equals(a, b))
                    {
                        if (!y.MoveNext())
                            return false;
                        b = y.Current;
                    }
                    while (x.MoveNext())
                    {
                        if (!(y.MoveNext() && _comparer.Equals(x.Current, y.Current)))
                            return false;
                    }
                }
                else
                    return false;
            }
            else
                return !y.MoveNext();
            return true;
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            using IEnumerator<T> x = GetEnumerator();
            using IEnumerator<T> y = other.GetEnumerator();
            if (y.MoveNext())
            {
                T b = y.Current;
                if (x.MoveNext())
                {
                    T a = x.Current;
                    while (!_comparer.Equals(a, b))
                    {
                        if (!x.MoveNext())
                            return false;
                        a = x.Current;
                    }
                    while (y.MoveNext())
                    {
                        if (!(x.MoveNext() && _comparer.Equals(x.Current, y.Current)))
                            return false;
                    }
                }
                else
                    return false;
            }
            else
                return !x.MoveNext();
            return true;
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            other = other.Where(o => o is not null);
            return AsEnumerable().All(i => other.Contains(i, _comparer));
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            IEnumerable<T> x = AsEnumerable();
            return other.All(o => o is not null && x.Contains(o));
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            other = other.Where(o => o is not null);
            return AsEnumerable().Any(i => other.Contains(i));
        }

        private bool Remove([DisallowNull] WeakReference<T> item)
        {
            Monitor.Enter(_backingSet);
            try { return _backingSet.Remove(item); }
            finally { Monitor.Exit(_backingSet); }
        }

        public bool Remove(T item)
        {
            if (item is null)
                return false;
            Monitor.Enter(_backingSet);
            try
            {
                if (_backingSet.Count == 0)
                    return false;
                Stack<WeakReference<T>> toRemove = new();
                using IEnumerator<WeakReference<T>> enumerator = _backingSet.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    WeakReference<T> w = enumerator.Current;
                    if (w.TryGetTarget(out T value))
                    {
                        if (_comparer.Equals(value, item))
                        {
                            foreach (WeakReference<T> r in toRemove)
                                _backingSet.Remove(r);
                            return true;
                        }
                    }
                    else
                        toRemove.Push(w);
                }
                foreach (WeakReference<T> r in toRemove)
                    _backingSet.Remove(r);
            }
            finally { Monitor.Exit(_backingSet); }
            return false;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            return AsEnumerable().SequenceEqual(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            Monitor.Enter(_backingSet);
            try
            {
                if (_backingSet.Count == 0)
                {
                    foreach (T item in other.Where(o => o is not null).Distinct(_comparer))
                        _backingSet.Add(new(item));
                }
                else
                {
                    Stack<WeakReference<T>> toRemove = new();
                    (T v, WeakReference<T> w)[] current = _backingSet.Select(w => w.TryGetTarget(out T v) ? (v, w) : (v: (T)null, w)).Where(t =>
                    {
                        if (t.v is null)
                        {
                            toRemove.Push(t.w);
                            return false;
                        }
                        return true;
                    }).ToArray();
                    foreach (WeakReference<T> w in toRemove)
                        _backingSet.Remove(w);
                    foreach ((T v, WeakReference<T> w) in current.Where(c => other.Contains(c.v, _comparer)))
                        _backingSet.Remove(w);
                    IEnumerable<T> values = current.Select(c => c.v);
                    foreach (T v in other.Where(o => o is not null && !values.Contains(o, _comparer)).Distinct(_comparer))
                        _backingSet.Add(new(v));
                }
            }
            finally { Monitor.Exit(_backingSet); }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            Monitor.Enter(_backingSet);
            try
            {
                foreach (T item in other.Where(o => o is not null).Distinct(_comparer).Where(o => !AsEnumerable().Contains(o, _comparer)).ToArray())
                    _backingSet.Add(new(item));
            }
            finally { Monitor.Exit(_backingSet); }
        }

        class Comparer : IEqualityComparer<WeakReference<T>>
        {
            public bool Equals(WeakReference<T> x, WeakReference<T> y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                return ReferenceEquals(x, y) || (x.TryGetTarget(out T a) && y.TryGetTarget(out T b) && _comparer.Equals(a, b));
            }

            public int GetHashCode([DisallowNull] WeakReference<T> obj) => (obj is not null && obj.TryGetTarget(out T v)) ? _comparer.GetHashCode(v) : 0;
        }

        class Enumerator : IEnumerator<T>
        {
            private IEnumerator<WeakReference<T>> _backingEnumerator;
            private readonly Stack<WeakReference<T>> _toRemove = new();
            private readonly WeakReferenceSet<T> _target;

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            internal Enumerator([DisallowNull] WeakReferenceSet<T> target)
            {
                _backingEnumerator = (_target = target)._backingSet.GetEnumerator();
            }

            public bool MoveNext()
            {
                Monitor.Enter(_toRemove);
                try
                {
                    while (_backingEnumerator.MoveNext())
                    {
                        if (_backingEnumerator.Current.TryGetTarget(out T value))
                        {
                            Current = value;
                            return true;
                        }
                        _toRemove.Push(_backingEnumerator.Current);
                    }
                }
                finally { Monitor.Exit(_toRemove); }
                return false;
            }

            public void Reset()
            {
                Monitor.Enter(_backingEnumerator);
                try
                {
                    _backingEnumerator.Reset();
                    while (_toRemove.TryPop(out WeakReference<T> item))
                        _target.Remove(item);
                    IEnumerator<WeakReference<T>> oldEnumerator = _backingEnumerator;
                    _backingEnumerator = _target._backingSet.GetEnumerator();
                    oldEnumerator.Dispose();
                }
                finally { Monitor.Exit(_backingEnumerator); }
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposing)
                    return;
                Monitor.Enter(_backingEnumerator);
                try { _backingEnumerator.Dispose(); }
                finally { Monitor.Exit(_backingEnumerator); }
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
