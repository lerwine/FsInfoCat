using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public partial class NestedCollectionComponentContainer<TOwner, TItem> where TOwner : IComponent
        where TItem : IComponent
    {
        public partial class Site
        {
            public class NestedCollection : Collection<TItem>
            {
                private object _syncRoot = new object();
                protected internal NestedCollectionComponentContainer<TOwner, TItem> Container { get; private set; }

                public NestedCollection() { }

                internal NestedCollection(NestedCollectionComponentContainer<TOwner, TItem> container)
                {
                    if (container is null)
                        throw new ArgumentNullException(nameof(container));
                    if (null != container._items)
                        throw new InvalidOperationException();
                    Container = container;
                }

                protected override void ClearItems()
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (null == Container)
                            base.ClearItems();
                        else
                        {
                            Monitor.Enter(Container._syncRoot);
                            try
                            {
                                TItem[] removedItems = Items.ToArray();
                                base.ClearItems();
                                Site.Unlink(Container, removedItems);
                            }
                            finally { Monitor.Exit(Container._syncRoot); }
                        }
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }

                protected override void InsertItem(int index, TItem item)
                {
                    if (item is null)
                        throw new ArgumentNullException(nameof(item));
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (null == Container)
                            base.InsertItem(index, item);
                        else
                        {
                            Monitor.Enter(Container._syncRoot);
                            try
                            {
                                Site.Add(item, Container, item.GetComponentName(), true);
                                base.InsertItem(index, item);
                            }
                            finally { Monitor.Exit(Container._syncRoot); }
                        }
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }

                protected override void RemoveItem(int index)
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (null == Container)
                            base.RemoveItem(index);
                        else
                        {
                            Monitor.Enter(Container._syncRoot);
                            try
                            {
                                if (this[index].Site is Site site)
                                    site.Unlink();
                                base.RemoveItem(index);
                            }
                            finally { Monitor.Exit(Container._syncRoot); }
                        }
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }

                protected override void SetItem(int index, TItem item)
                {
                    if (item is null)
                        throw new ArgumentNullException(nameof(item));
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (null == Container)
                            base.SetItem(index, item);
                        else
                        {
                            Monitor.Enter(Container._syncRoot);
                            try
                            {
                                TItem oldItem = this[index];
                                if (ReferenceEquals(oldItem, item))
                                    return;

                                if (oldItem.Site is Site site)
                                    site.Unlink();
                                Site.Add(item, Container, item.GetComponentName(), true);
                                base.SetItem(index, item);
                            }
                            finally { Monitor.Exit(Container._syncRoot); }
                        }
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }

                internal static void SetItems(Collection<TItem> value, NestedCollectionComponentContainer<TOwner, TItem> container)
                {
                    if (container is null)
                        throw new ArgumentNullException(nameof(container));
                    Monitor.Enter(container._syncRoot);
                    try
                    {
                        if (null == value)
                        {
                            foreach (TItem item in container._items)
                                ((Site)item.Site).Unlink();
                            container._items.Container = null;
                            container._items = null;
                            container._items = new NestedCollection(container);
                        }
                        else
                        {
                            if (value.Select(v => v.GetComponentName()).Where(n => null != n).GroupBy(n => n, container.NameComparer).Any(g => g.Count() > 1))
                                throw new InvalidOperationException("Duplicate names not allowed");
                            if (value is NestedCollection nc)
                            {
                                if (null != nc.Container)
                                {
                                    if (ReferenceEquals(nc.Container, container))
                                        return;
                                    throw new InvalidOperationException();
                                }
                                foreach (TItem item in container._items)
                                    ((Site)item.Site).Unlink();
                                container._items.Container = null;
                                (container._items = nc).Container = container;
                                foreach (TItem item in value)
                                    Site.Add(item, container, item.GetComponentName(), true);
                            }
                            else
                            {
                                foreach (TItem item in container._items)
                                    ((Site)item.Site).Unlink();
                                container._items.Container = null;
                                container._items = null;
                                container._items = new NestedCollection(container);
                                foreach (TItem item in value)
                                {
                                    if (null != item)
                                        container._items.Add(item);
                                }
                            }
                        }
                    }
                    finally { Monitor.Exit(container._syncRoot); }
                }
            }
        }
    }
}
