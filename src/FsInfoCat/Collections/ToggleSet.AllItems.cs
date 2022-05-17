using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;

namespace FsInfoCat.Collections
{
    // TODO: Document ToggleSet<T>.AllItems class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    partial class ToggleSet<T>
    {
        public class AllItems : IReadOnlySet<T>, ICollection, INotifyCollectionChanged
        {
            private readonly ToggleSet<T> _owner;

            public event NotifyCollectionChangedEventHandler CollectionChanged;
            public event PropertyChangedEventHandler PropertyChanged;

            public int Count => _owner._count;

            bool ICollection.IsSynchronized => true;

            object ICollection.SyncRoot => _owner._syncRoot;

            internal AllItems(ToggleSet<T> owner) { _owner = owner ?? throw new ArgumentNullException(nameof(owner)); }

            public bool Contains(T item) => GetNodesInSet(_owner._firstInSet).Select(n => n.Value).Contains(item, _owner._comparer);

            public void CopyTo(Array array, int index) => GetNodesInSet(_owner._firstInSet).Select(n => n.Value).ToArray().CopyTo(array, index);

            public IEnumerator<T> GetEnumerator() => GetNodesInSet(_owner._firstInSet).Select(n => n.Value).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetNodesInSet(_owner._firstInSet).Select(n => n.Value)).GetEnumerator();

            public bool IsProperSubsetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return other.Any();
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (x.MoveNext())
                    {
                        if (!y.MoveNext() || !other.Contains(x.Current, _owner._comparer))
                            return false;
                        if (!values.Contains(y.Current, _owner._comparer))
                            isProper = true;
                    }
                    if (isProper)
                        return true;
                    while (y.MoveNext())
                    {
                        if (!values.Contains(y.Current, _owner._comparer))
                            return true;
                    }
                }
                finally { Monitor.Exit(_owner._syncRoot); }
                return false;
            }

            public bool IsProperSupersetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return false;
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (y.MoveNext())
                    {
                        if (!x.MoveNext() || !values.Contains(y.Current, _owner._comparer))
                            return false;
                        if (!other.Contains(x.Current, _owner._comparer))
                            isProper = true;
                    }
                    if (isProper)
                        return true;
                    while (x.MoveNext())
                    {
                        if (!other.Contains(x.Current, _owner._comparer))
                            return true;
                    }
                }
                finally { Monitor.Exit(_owner._syncRoot); }
                return false;
            }

            public bool IsSubsetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return true;
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (x.MoveNext())
                    {
                        if (!y.MoveNext() || !other.Contains(x.Current, _owner._comparer))
                            return false;
                        if (!values.Contains(y.Current, _owner._comparer))
                            isProper = true;
                    }
                    if (isProper || !y.MoveNext())
                        return true;
                    do
                    {
                        if (!values.Contains(y.Current, _owner._comparer))
                            return true;
                    } while (y.MoveNext());
                }
                finally { Monitor.Exit(_owner._syncRoot); }
                return false;
            }

            public bool IsSupersetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return !other.Any();
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (y.MoveNext())
                    {
                        if (!x.MoveNext() || !values.Contains(y.Current, _owner._comparer))
                            return false;
                        if (!other.Contains(x.Current, _owner._comparer))
                            isProper = true;
                    }
                    if (isProper || !x.MoveNext())
                        return true;
                    do
                    {
                        if (!other.Contains(x.Current, _owner._comparer))
                            return true;
                    } while (x.MoveNext());
                }
                finally { Monitor.Exit(_owner._syncRoot); }
                return false;
            }

            public bool Overlaps(IEnumerable<T> other)
            {
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (other is null || _owner._count == 0)
                        return false;
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    return _owner._count > 0 && other.Any(o => values.Contains(o, _owner._comparer));
                }
                finally { Monitor.Exit(_owner._syncRoot); }
            }

            public bool SetEquals(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return !other.Any();
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    while (x.MoveNext())
                    {
                        if (!(y.MoveNext() && values.Contains(y.Current, _owner._comparer) && other.Contains(x.Current, _owner._comparer)))
                            return false;
                    }
                    return !y.MoveNext();
                }
                finally { Monitor.Exit(_owner._syncRoot); }
            }

            internal void RaiseCountChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));

            internal void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args) => CollectionChanged?.Invoke(this, args);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
