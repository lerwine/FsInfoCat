using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Models.Crawl
{
    public abstract partial class CrawlComponent
    {
        public abstract class CrawlComponentContainer : IContainer, IServiceProvider
        {
            protected Collection<CrawlComponentSite> Sites { get; } = new Collection<CrawlComponentSite>();

            private bool _disposed = false;
            private ComponentCollection _components;

            ComponentCollection IContainer.Components
            {
                get
                {
                    ComponentCollection components = _components;
                    if (null == components)
                        _components = components = new ComponentCollection(Sites.Select(s => s.Component).ToArray());
                    return components;
                }
            }

            public int Count => Sites.Count;

            protected object SyncRoot { get; } = new object();

            protected virtual int Add(CrawlComponent component)
            {
                if (null == component)
                    throw new ArgumentNullException(nameof(component));

                Monitor.Enter(SyncRoot);
                try
                {
                    Monitor.Enter(component.SyncRoot);
                    try
                    {
                        CrawlComponentSite oldSite = component.Site;
                        CrawlComponentSite newSite;
                        if (null != oldSite)
                        {
                            if (ReferenceEquals(oldSite.Container, this))
                                return IndexOf(component);
                            newSite = CreateSite(this, component);
                            ValidateName(component.GetName(), -1);
                            oldSite.Container.Remove(component);
                            if (null != oldSite)
                                throw new InvalidOperationException("Failed to remove component from current container before adding to new container");
                        }
                        else
                        {
                            newSite = CreateSite(this, component);
                            ValidateName(component.GetName(), -1);
                        }
                        component._site = newSite;
                        int result = Sites.Count;
                        Sites.Add(newSite);
                        return result;
                    }
                    finally { Monitor.Exit(component.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            public void Clear()
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    CrawlComponentSite[] removed = Sites.ToArray();
                    Sites.Clear();
                    foreach (CrawlComponentSite site in removed)
                    {
                        CrawlComponent item = site.Component;
                        Monitor.Enter(item.SyncRoot);
                        try
                        {
                            if (null != item._site && ReferenceEquals(item._site, site))
                                item._site = null;
                        }
                        finally { Monitor.Exit(item.SyncRoot); }
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            protected bool Contains(CrawlComponent item)
            {
                if (null == item)
                    return false;

                Monitor.Enter(SyncRoot);
                try
                {
                    if (_disposed)
                        return false;
                    Monitor.Enter(item.SyncRoot);
                    try
                    {
                        return null != item._site && ReferenceEquals(item._site.Container, this);
                    }
                    finally { Monitor.Exit(item.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            protected abstract CrawlComponentSite CreateSite(CrawlComponentContainer container, CrawlComponent component);

            public virtual object GetService(Type serviceType)
            {
                return (serviceType == typeof(IContainer)) ? this : null;
            }

            protected int IndexOf(CrawlComponent item)
            {
                if (null == item)
                    return -1;

                Monitor.Enter(SyncRoot);
                try
                {
                    if (_disposed)
                        return -1;
                    Monitor.Enter(item.SyncRoot);
                    try
                    {
                        return (null != item._site && ReferenceEquals(item._site.Container, this)) ? Sites.IndexOf(item._site) : -1;
                    }
                    finally { Monitor.Exit(item.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            protected virtual void Insert(int index, CrawlComponent item)
            {
                if (null == item)
                    throw new ArgumentNullException(nameof(item));

                Monitor.Enter(SyncRoot);
                try
                {
                    Monitor.Enter(item.SyncRoot);
                    try
                    {
                        CrawlComponentSite newSite;
                        if (null != item._site)
                        {
                            if (ReferenceEquals(item._site.Container, this))
                            {
                                int oldIndex = Sites.IndexOf(item._site);
                                if (oldIndex != index)
                                {
                                    Sites.Insert(index, item._site);
                                    Sites.RemoveAt((index > oldIndex) ? oldIndex : oldIndex + 1);
                                }
                                return;
                            }
                            newSite = CreateSite(this, item);
                            ValidateName(item.GetName(), -1);
                            item._site.Container.Remove(item);
                            if (null != item._site)
                                return;
                        }
                        else
                        {
                            newSite = CreateSite(this, item);
                            ValidateName(item.GetName(), -1);
                        }
                            newSite = CreateSite(this, item);
                        Sites.Insert(index, newSite);
                        item._site = newSite;
                    }
                    finally { Monitor.Exit(item.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            protected virtual bool Remove(CrawlComponent component)
            {
                if (null == component)
                    return false;

                Monitor.Enter(SyncRoot);
                try
                {
                    Monitor.Enter(component.SyncRoot);
                    try
                    {
                        if (null != component._site && ReferenceEquals(component._site.Container, this))
                        {
                            Sites.Remove(component._site);
                            component._site = null;
                            return true;
                        }
                    }
                    finally { Monitor.Exit(component.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
                return false;
            }

            public void RemoveAt(int index)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    CrawlComponentSite site = Sites[index];
                    Monitor.Enter(site.Component.SyncRoot);
                    try
                    {
                        Sites.Remove(site);
                        site.Component._site = null;
                    }
                    finally { Monitor.Exit(site.Component.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            protected virtual void SetAt(int index, CrawlComponent item)
            {
                if (null == item)
                    throw new ArgumentNullException(nameof(item));

                Monitor.Enter(SyncRoot);
                try
                {
                    Monitor.Enter(item.SyncRoot);
                    try
                    {
                        CrawlComponentSite newSite;
                        if (null != item._site)
                        {
                            if (ReferenceEquals(item._site.Container, this))
                                throw new InvalidOperationException();

                            newSite = CreateSite(this, item);
                            ValidateName(item.GetName(), index);
                            item._site.Container.Remove(item);
                            if (null != item._site)
                                return;
                        }
                        else
                        {
                            newSite = CreateSite(this, item);
                            ValidateName(item.GetName(), index);
                        }
                        CrawlComponentSite existing = Sites[index];
                        Monitor.Enter(existing.Component.SyncRoot);
                        try
                        {
                            item._site = Sites[index] = newSite;
                            existing.Component._site = null;
                        }
                        finally { Monitor.Exit(existing.Component.SyncRoot); }
                    }
                    finally { Monitor.Exit(item.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            public abstract StringComparer GetNameComparer();

            private void ValidateName(string name, int index)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    StringComparer comparer = GetNameComparer();
                    for (int i = 0; i < Sites.Count; i++)
                    {
                        if (i != index && comparer.Equals(Sites[i].Name, name))
                            throw new InvalidOperationException("Item with the same name already exists");
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            void IContainer.Add(IComponent component) => Add((CrawlComponent)component);
            void IContainer.Add(IComponent component, string name)
            {
                CrawlComponent c = (CrawlComponent)component;
                if (null == c)
                    throw new ArgumentNullException(nameof(component));
                if (c.GetName() != name)
                    throw new ArgumentException("Name does not match component name", nameof(name));
                Add(c);
            }
            void IContainer.Remove(IComponent component) => Remove((CrawlComponent)component);

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects)
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    _disposed = true;
                }
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            // ~CrawlComponentContainer()
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

        public abstract class CrawlComponentContainer<TComponent> : CrawlComponentContainer, IList<TComponent>, IList
            where TComponent : CrawlComponent
        {
            public TComponent this[int index] { get => (TComponent)Sites[index].Component; set => SetAt(index, value); }

            object IList.this[int index] { get => Sites[index].Component; set => SetAt(index, (TComponent)value); }

            bool ICollection<TComponent>.IsReadOnly => false;

            bool IList.IsFixedSize => false;

            bool IList.IsReadOnly => false;

            bool ICollection.IsSynchronized => true;

            object ICollection.SyncRoot => SyncRoot;

            public void Add(TComponent item) => base.Add(item);

            protected override int Add(CrawlComponent component) => base.Add((TComponent)component);

            public bool Contains(TComponent item) => base.Contains(item);

            public void CopyTo(TComponent[] array, int arrayIndex) => Sites.Select(s => s.Component).ToList().CopyTo(array, arrayIndex);

            protected override CrawlComponentSite CreateSite(CrawlComponentContainer container, CrawlComponent component) => new CrawlComponentSite<TComponent>((CrawlComponentContainer<TComponent>)container, (TComponent)component);

            public IEnumerator<TComponent> GetEnumerator() => Sites.Select(s => (TComponent)s.Component).GetEnumerator();

            public int IndexOf(TComponent item) => base.IndexOf(item);

            public void Insert(int index, TComponent item) => base.Insert(index, item);

            protected override void Insert(int index, CrawlComponent item) => base.Insert(index, (TComponent)item);

            public bool Remove(TComponent item) => base.Remove((TComponent)item);

            protected override void SetAt(int index, CrawlComponent value) => base.SetAt(index, (TComponent)value);

            int IList.Add(object value) => base.Add((TComponent)value);

            bool IList.Contains(object value) => Contains(value as TComponent);

            IEnumerator IEnumerable.GetEnumerator() => Sites.Select(s => (TComponent)s.Component).ToArray().GetEnumerator();

            int IList.IndexOf(object value) => IndexOf(value as TComponent);

            void IList.Insert(int index, object value) => base.Insert(index, (TComponent)value);

            void IList.Remove(object value) => base.Remove(value as TComponent);

            void ICollection.CopyTo(Array array, int index) => Sites.Select(s => s.Component).ToArray().CopyTo(array, index);
        }
    }
}
