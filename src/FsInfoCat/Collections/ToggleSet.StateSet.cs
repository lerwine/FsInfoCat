using FsInfoCat.DeferredDelegation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat.Collections
{
    public partial class ToggleSet<T>
    {
        public class StateSet : IReadOnlySet<T>, ICollection, INotifyCollectionChanged
        {
            private readonly object _syncRoot;
            private readonly IDeferredDelegationService _deferredDelegation;
            private readonly IEqualityComparer<T> _comparer;
            internal Node First { get; private set; }
            internal Node Last { get; private set; }

            public event NotifyCollectionChangedEventHandler CollectionChanged;
            public event PropertyChangedEventHandler PropertyChanged;

            public int Count { get; private set; } = 0;

            bool ICollection.IsSynchronized => true;

            object ICollection.SyncRoot => _syncRoot;

            private StateSet([DisallowNull] IDeferredDelegationService deferredDelegation, [DisallowNull] IEqualityComparer<T> comparer, [DisallowNull] object syncRoot)
            {
                _deferredDelegation = deferredDelegation ?? throw new ArgumentNullException(nameof(comparer));
                _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
                _syncRoot = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));
            }

            internal static StateSet Create([DisallowNull] IDeferredDelegationService deferredDelegation, [DisallowNull] IEqualityComparer<T> comparer, [DisallowNull] object syncRoot, [DisallowNull] out ICollection<Node> accessor)
            {
                StateSet result = new(deferredDelegation, comparer, syncRoot);
                accessor = new Accessor(result);
                return result;
            }

            class Accessor : Node.AccessorBase, ICollection<Node>
            {

                internal Accessor(StateSet target) : base(target) { }

                public int Count => Target.Count;

                bool ICollection<Node>.IsReadOnly => false;

                public void Add(Node item)
                {
                    Node previous = Target.Last;
                    if (previous is null)
                    {
                        Target.First = Target.Last = item;
                        Target.Count = 1;
                    }
                    else
                    {
                        Target.Count++;
                        InsertOfState(item, previous);
                        Target.Last = item;
                    }
                }

                public void Clear()
                {
                    if (Target.First is not null)
                        ClearOfState(Target.First);
                    Target.First = Target.Last = null;
                    Target.Count = 0;
                }

                public bool Contains(Node item) => GetNodesOfState(Target.First).Contains(item);

                public void CopyTo(Node[] array, int arrayIndex) => GetNodesOfState(Target.First).ToList().CopyTo(array, arrayIndex);

                public IEnumerator<Node> GetEnumerator() => GetNodesOfState(Target.First).GetEnumerator();

                public bool Remove(Node item)
                {
                    if (item is null || Target.First is null)
                        return false;
                    if (item.NextOfState is null)
                    {
                        if (!ReferenceEquals(item, Target.Last))
                            return false;
                        if ((Target.Last = item.PreviousOfState) is null)
                        {
                            Target.First = null;
                            Target.Count = 0;
                            return true;
                        }
                        RemoveOfState(item);
                    }
                    else if (item.PreviousOfState is null)
                    {
                        if (!ReferenceEquals(item, Target.First))
                            return false;
                        Target.First = item.NextOfState;
                    }
                    else
                    {
                        Node f = item;
                        while (f.PreviousOfState is not null)
                            f = f.PreviousOfState;
                        if (!ReferenceEquals(f, Target.First))
                            return false;
                    }
                    RemoveOfState(item);
                    return true;
                }

                IEnumerator IEnumerable.GetEnumerator() => GetNodesOfState(Target.First).ToArray().GetEnumerator();
            }

            public bool Contains(T item) => GetNodesInSet(First).Select(n => n.Value).Contains(item, _comparer);

            public void CopyTo(Array array, int index) => GetNodesInSet(First).Select(n => n.Value).ToArray().CopyTo(array, index);

            public IEnumerator<T> GetEnumerator() => GetNodesInSet(First).Select(n => n.Value).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetNodesInSet(First).Select(n => n.Value)).GetEnumerator();

            public bool IsProperSubsetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using (_deferredDelegation.EnterSynchronized(this))
                {
                    if (Count == 0)
                        return other.Any();
                    IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (x.MoveNext())
                    {
                        if (!y.MoveNext() || !other.Contains(x.Current, _comparer))
                            return false;
                        if (!values.Contains(y.Current, _comparer))
                            isProper = true;
                    }
                    if (isProper)
                        return true;
                    while (y.MoveNext())
                    {
                        if (!values.Contains(y.Current, _comparer))
                            return true;
                    }
                    return false;
                }
            }

            public bool IsProperSupersetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using (_deferredDelegation.EnterSynchronized(this))
                {
                    if (Count == 0)
                        return false;
                    IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (y.MoveNext())
                    {
                        if (!x.MoveNext() || !values.Contains(y.Current, _comparer))
                            return false;
                        if (!other.Contains(x.Current, _comparer))
                            isProper = true;
                    }
                    if (isProper)
                        return true;
                    while (x.MoveNext())
                    {
                        if (!other.Contains(x.Current, _comparer))
                            return true;
                    }
                    return false;
                }
            }

            public bool IsSubsetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using (_deferredDelegation.EnterSynchronized(this))
                {
                    if (Count == 0)
                        return true;
                    IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (x.MoveNext())
                    {
                        if (!y.MoveNext() || !other.Contains(x.Current, _comparer))
                            return false;
                        if (!values.Contains(y.Current, _comparer))
                            isProper = true;
                    }
                    if (isProper || !y.MoveNext())
                        return true;
                    do
                    {
                        if (!values.Contains(y.Current, _comparer))
                            return true;
                    } while (y.MoveNext());
                    return false;
                }
            }

            public bool IsSupersetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using (_deferredDelegation.EnterSynchronized(this))
                {
                    if (Count == 0)
                        return !other.Any();
                    IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (y.MoveNext())
                    {
                        if (!x.MoveNext() || !values.Contains(y.Current, _comparer))
                            return false;
                        if (!other.Contains(x.Current, _comparer))
                            isProper = true;
                    }
                    if (isProper || !x.MoveNext())
                        return true;
                    do
                    {
                        if (!other.Contains(x.Current, _comparer))
                            return true;
                    } while (x.MoveNext());
                    return false;
                }
            }

            public bool Overlaps(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using (_deferredDelegation.EnterSynchronized(this))
                {
                    if (Count == 0)
                        return false;
                    IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                    return other.Any(o => values.Contains(o, _comparer));
                }
            }

            public bool SetEquals(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using (_deferredDelegation.EnterSynchronized(this))
                {
                    if (Count == 0)
                        return !other.Any();
                    IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    while (x.MoveNext())
                    {
                        if (!(y.MoveNext() && values.Contains(y.Current, _comparer) && other.Contains(x.Current, _comparer)))
                            return false;
                    }
                    return !y.MoveNext();
                }
            }

            internal void RaiseCountChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));

            internal void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args) => CollectionChanged?.Invoke(this, args);
        }
    }
}
