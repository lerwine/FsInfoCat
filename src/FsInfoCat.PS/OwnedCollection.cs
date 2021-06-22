using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.PS
{
    public abstract class OwnedCollection<TOwner, TElement> : Collection<TElement>
        where TOwner : class, ISynchonizedElement
        where TElement : class, IOwnedElement<TOwner>
    {
        public TOwner Owner { get; }

        protected OwnedCollection(TOwner owner) { Owner = owner ?? throw new ArgumentNullException(nameof(owner)); }

        protected OwnedCollection(TOwner owner, IEnumerable<TElement> items) : this(owner) { AddRange(items); }

        public void AddRange(IEnumerable<TElement> items)
        {
            if (items is null)
                return;
            foreach (TElement e in items.Where(i => i is not null).Distinct())
                Add(e);
        }

        protected override void ClearItems()
        {
            Monitor.Enter(Owner.SyncRoot);
            try
            {
                TElement[] removed = Items.ToArray();
                base.ClearItems();
                IEnumerator enumerator = removed.GetEnumerator();
                OnItemsRemoved(enumerator);
            }
            finally { Monitor.Exit(Owner.SyncRoot); }
        }

        private void OnItemsRemoved(IEnumerator enumerator)
        {
            if (enumerator.MoveNext())
                try
                {
                    TElement item = (TElement)enumerator.Current;
                    if (ReferenceEquals(item.Owner, Owner))
                        OnItemRemoved(item);
                }
                finally { OnItemsRemoved(enumerator); }
        }

        protected abstract void OnItemAdding(TElement item);

        protected abstract void OnItemRemoved(TElement item);

        protected override void InsertItem(int index, TElement item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            Monitor.Enter(Owner.SyncRoot);
            try
            {
                Monitor.Enter(item.SyncRoot);
                try
                {
                    if (item.Owner is not null)
                        throw new ArgumentOutOfRangeException(nameof(item));
                    OnItemAdding(item);
                    if (!ReferenceEquals(item.Owner, Owner))
                        throw new InvalidOperationException();
                    base.InsertItem(index, item);
                }
                finally { Monitor.Exit(item.SyncRoot); }
            }
            finally { Monitor.Exit(Owner.SyncRoot); }
        }

        protected override void RemoveItem(int index)
        {
            Monitor.Enter(Owner.SyncRoot);
            try
            {
                TElement item = Items[index];
                Monitor.Enter(item.SyncRoot);
                try
                {
                    base.RemoveItem(index);
                    OnItemRemoved(item);
                }
                finally { Monitor.Exit(item.SyncRoot); }
            }
            finally { Monitor.Exit(Owner.SyncRoot); }
        }

        protected override void SetItem(int index, TElement item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            Monitor.Enter(Owner.SyncRoot);
            try
            {
                Monitor.Enter(item.SyncRoot);
                try
                {
                    TElement oldItem = Items[index];
                    if (ReferenceEquals(item, oldItem))
                    {
                        base.SetItem(index, item);
                        return;
                    }
                    Monitor.Enter(oldItem.SyncRoot);
                    try
                    {
                        OnItemAdding(item);
                        if (!ReferenceEquals(item.Owner, Owner))
                            throw new InvalidOperationException();
                        base.SetItem(index, item);
                        OnItemRemoved(oldItem);
                    }
                    finally { Monitor.Exit(oldItem.SyncRoot); }
                }
                finally { Monitor.Exit(item.SyncRoot); }
            }
            finally { Monitor.Exit(Owner.SyncRoot); }
        }
    }
}
