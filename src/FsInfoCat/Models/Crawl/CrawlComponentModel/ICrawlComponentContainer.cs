using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface ICrawlComponentContainer : IContainer, IServiceProvider, ICollection<CrawlComponent>, ICollection
    {
        void Add(ICrawlComponent component);
        void Add(ICrawlComponent component, string name);
        void Remove(ICrawlComponent component);
    }

    public partial class CrawlComponent
    {
        public partial class CrawlSite
        {
            private CrawlSite _previous = null;
            private CrawlSite _next = null;
            public abstract class CrawlComponentContainer : ICrawlComponentContainer
            {
                private int _count = 0;
                private CrawlSite _first = null;
                private CrawlSite _last = null;
                private ComponentCollection _components = null;
                private bool _isDisposed = false;

                ComponentCollection IContainer.Components
                {
                    get
                    {
                        ComponentCollection components = _components;
                        if (null == components)
                            _components = components = new ComponentCollection(GetAllComponents().ToArray());
                        return components;
                    }
                }

                int ICollection<CrawlComponent>.Count => _count;

                int ICollection.Count => _count;

                bool ICollection<CrawlComponent>.IsReadOnly => false;

                bool ICollection.IsSynchronized => true;

                protected internal object SyncRoot { get; } = new object();

                object ICollection.SyncRoot => SyncRoot;

                protected void Add(CrawlComponent component)
                {
                    if (null == component)
                        throw new ArgumentNullException(nameof(component));
                    Monitor.Enter(component.SyncRoot);
                    try
                    {
                        if (component._isDisposed)
                            throw new ObjectDisposedException(component.GetType().FullName);
                        if (null != component.Site)
                        {
                            if (ReferenceEquals(component.Site.Container, this))
                                return;
                            component.Site.Container.Remove(component);
                            if (null != component.Site)
                                throw new InvalidOperationException("Unable to move component from its current container");
                        }
                        Monitor.Enter(SyncRoot);
                        try
                        {
                            if (_isDisposed)
                                throw new ObjectDisposedException(GetType().FullName);
                            CrawlSite site = CreateCrawlSite(component);
                            if (null == site || !ReferenceEquals(site.Component, component) || !ReferenceEquals(site.Container, this) || null != site._next || null != site._previous || (null != _first && ReferenceEquals(_first, site)))
                                throw new InvalidOperationException("Invalid result from CreateCrawlSite method");
                            string name = site.Name;
                            if (null != name)
                                AssertValidName(GetAllSites().Select(s => s.Name).Where(n => null != n), name);
                            if (null == (site._previous = _last))
                                _last = _first = site;
                            else
                                _last = _last._next = site;
                            _count++;
                            _components = null;
                        }
                        finally { Monitor.Exit(SyncRoot); }
                    }
                    finally { Monitor.Exit(component.SyncRoot); }
                }

                void ICrawlComponentContainer.Add(ICrawlComponent component) => Add((CrawlComponent)component);

                void IContainer.Add(IComponent component) => Add((CrawlComponent)component);

                protected void Add(CrawlComponent component, string name)
                {
                    if (null == component)
                        throw new ArgumentNullException(nameof(component));
                    if (component.GetName() != name)
                        throw new ArgumentException("Component name mismatch", nameof(name));
                    Add(component);
                }

                void ICollection<CrawlComponent>.Add(CrawlComponent item) => Add(item);

                void ICrawlComponentContainer.Add(ICrawlComponent component, string name) => Add((CrawlComponent)component, name);

                void IContainer.Add(IComponent component, string name) => Add((CrawlComponent)component, name);

                protected virtual void AssertValidName(IEnumerable<string> currentNames, string name)
                {
                    if (currentNames.Contains(name))
                        throw new ArgumentException("An item with that name already exists", nameof(name));
                }

                private void Clear(CrawlSite site)
                {
                    site.Component.Site = null;
                    CrawlSite next = site._next;
                    if (null == next)
                        return;
                    site._next = null;
                    Monitor.Enter(next.Component.SyncRoot);
                    try
                    {
                        next._previous = null;
                        Clear(next);
                    }
                    finally { Monitor.Exit(site.Component.SyncRoot); }
                }
                public void Clear()
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (null != _first)
                        {
                            Monitor.Enter(_first.Component.SyncRoot);
                            try { Clear(_first); }
                            finally { Monitor.Exit(_first.Component.SyncRoot); }
                            _count = 0;
                            _first = _last = null;
                            _components = null;
                        }
                    }
                    finally { Monitor.Exit(SyncRoot); }
                }

                protected bool Contains(CrawlComponent item)
                {
                    if (null == item)
                        return false;
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    Monitor.Enter(item.SyncRoot);
                    try
                    {
                        CrawlSite site = item.Site;
                        return null != site && ReferenceEquals(site.Container, this);
                    }
                    finally { Monitor.Exit(item.SyncRoot); }
                }

                bool ICollection<CrawlComponent>.Contains(CrawlComponent item) => Contains(item);

                void ICollection<CrawlComponent>.CopyTo(CrawlComponent[] array, int arrayIndex)
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    GetAllComponents().ToList().CopyTo(array, arrayIndex);
                }

                void ICollection.CopyTo(Array array, int index)
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    GetAllComponents().ToArray().CopyTo(array, index);
                }

                protected abstract CrawlSite CreateCrawlSite(CrawlComponent component);

                private IEnumerable<CrawlSite> GetAllSites()
                {
                    for (CrawlSite site = _first; null != site; site = site._next)
                        yield return site;
                }

                public IEnumerable<CrawlComponent> GetAllComponents() => GetAllSites().Select(s => s.Component);

                IEnumerator<CrawlComponent> IEnumerable<CrawlComponent>.GetEnumerator()
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    return GetAllComponents().GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    return GetAllComponents().ToArray().GetEnumerator();
                }

                public object GetService(Type serviceType)
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    return (serviceType == typeof(IContainer)) ? this : null;
                }

                protected bool Remove(CrawlComponent component)
                {
                    if (null == component)
                        return false;
                    Monitor.Enter(component.SyncRoot);
                    try
                    {
                        CrawlSite site = component.Site;
                        if (null != site && ReferenceEquals(site.Container, this))
                        {
                            Monitor.Enter(SyncRoot);
                            try
                            {
                                if (_isDisposed)
                                    throw new ObjectDisposedException(GetType().FullName);
                                if (null == site._next)
                                {
                                    if (null == (_last = site._previous))
                                        _first = null;
                                    else
                                        _last._next = site._previous = null;
                                }
                                else if (null == (site._next._previous = site._previous))
                                    site._next = (_first = site._next)._previous = null;
                                else
                                    site._previous._next = site._next;
                                _count--;
                            }
                            finally { Monitor.Exit(SyncRoot); }
                        }
                    }
                    finally { Monitor.Exit(component.SyncRoot); }
                    return false;
                }

                bool ICollection<CrawlComponent>.Remove(CrawlComponent item) => Remove(item);

                void ICrawlComponentContainer.Remove(ICrawlComponent component) => Remove(component as CrawlComponent);

                void IContainer.Remove(IComponent component) => Remove(component as CrawlComponent);

                protected virtual void Dispose(bool disposing)
                {
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_isDisposed)
                            return;
                        if (disposing && null != _first)
                        {
                            Monitor.Enter(_first.Component.SyncRoot);
                            try { Clear(_first); }
                            finally { Monitor.Exit(_first.Component.SyncRoot); }
                            _count = 0;
                            _first = _last = null;
                            _components = null;
                        }
                        _isDisposed = true;
                    }
                    finally { Monitor.Exit(SyncRoot); }
                }

                public void Dispose()
                {
                    Dispose(disposing: true);
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}
