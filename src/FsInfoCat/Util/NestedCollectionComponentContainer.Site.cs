using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public partial class NestedCollectionComponentContainer<TOwner, TItem> where TOwner : IComponent
        where TItem : IComponent
    {
        public partial class Site : INestedSite
        {
            private readonly object _syncRoot = new object();
            private Site _previous = null;
            private Site _next = null;
            private string _name;
            public bool IsInCollection { get; }

            public string FullName => throw new NotImplementedException();

            public IComponent Component { get; }

            public NestedCollectionComponentContainer<TOwner, TItem> Container { get; }

            IContainer ISite.Container => Container;

            public string Name
            {
                get => _name;
                set
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (null != value && !IsOrphaned() && (null == _name || !Container.NameComparer.Equals(value, _name)) &&
                                Site.GetAllSites(Container).Where(s => !ReferenceEquals(s, this)).Select(s => s.Name)
                                .Where(s => null != s).GroupBy(s => s, Container.NameComparer).Any(g => g.Count() > 0))
                            throw new InvalidOperationException("Another component with that name already exists");
                        _name = value;
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }
            }

            bool ISite.DesignMode => false;

            private Site(IComponent component, NestedCollectionComponentContainer<TOwner, TItem> container, string name, bool isInCollection)
            {
                Component = component;
                Container = container;
                _name = name;
                IsInCollection = isInCollection;
            }

            public object GetService(Type serviceType) => (serviceType == typeof(ISite)) ? this : Container.GetService(serviceType);

            public bool IsOrphaned() => _previous is null && _next is null && (Container._first is null || !ReferenceEquals(this, Container._first));

            private bool Unlink()
            {
                if (null != _previous)
                {
                    if (null == (_previous._next = _next))
                        _previous = (Container._last = _previous)._next = null;
                    else
                    {
                        _next._previous = _previous;
                        _next = _previous = null;
                    }
                    Component.Site = null;
                    Container._componentCount--;
                }
                else if (ReferenceEquals(Container._first, this))
                {
                    if (null == (Container._first = _next))
                        Container._last = null;
                    else
                        _next = _next._previous = null;
                    Container._componentCount--;
                    Component.Site = null;
                }
                else
                    return false;
                return true;
            }

            internal bool Remove()
            {
                if (Component is TItem item && Container._items.Remove(item))
                    return true;
                Monitor.Enter(Container._syncRoot);
                try { return Unlink(); }
                finally { Monitor.Exit(Container._syncRoot); }
            }

            internal static IEnumerable<Site> GetAllSites(NestedCollectionComponentContainer<TOwner, TItem> container)
            {
                for (Site site = container._first; null != site; site = site._next)
                    yield return site;
            }

            internal static bool Add(IComponent component, NestedCollectionComponentContainer<TOwner, TItem> container, string name)
            {
                return Add(component, container, name, false);
            }

            private static bool Add(IComponent component, NestedCollectionComponentContainer<TOwner, TItem> container, string name, bool isInCollection)
            {
                if (component is null)
                    throw new ArgumentNullException(nameof(component));
                if (container is null)
                    throw new ArgumentNullException(nameof(container));
                Monitor.Enter(container._syncRoot);
                try
                {
                    ISite anyOldSite = component.Site;
                    IContainer oldContainer;
                    if (null != name)
                    {
                        StringComparer comparer = container.NameComparer;
                        if (container.GetNames().Any(n => comparer.Equals(name)))
                        {
                            if (null == anyOldSite || !(anyOldSite is Site oldSite && (oldSite.IsOrphaned() || ReferenceEquals(anyOldSite.Container, container))))
                                throw new ArgumentException("Duplicate component name", nameof(name));
                            return false;
                        }
                    }

                    if (null != anyOldSite && null != (oldContainer = anyOldSite.Container))
                    {
                        if (ReferenceEquals(oldContainer, container))
                        {
                            if (anyOldSite is Site oldSite && !oldSite.IsOrphaned())
                                return false;
                        }
                        else
                            oldContainer.Remove(component);
                    }
                    Site site = new Site(component, container, name, isInCollection);
                    component.Site = site;
                    if (null == (site._previous = container._last))
                        container._first = container._last = site;
                    else
                        container._last = site._previous._next = site;
                    container._componentCount++;
                }
                finally { Monitor.Exit(container._syncRoot); }
                return true;
            }

            private static void Unlink(NestedCollectionComponentContainer<TOwner, TItem> container, IEnumerable<TItem> itemCollection)
            {
                Monitor.Enter(container._syncRoot);
                try
                {
                    foreach (Site site in itemCollection.Select(i => i.Site).OfType<Site>().Where(s => ReferenceEquals(s.Container, container)))
                        site.Unlink();
                }
                finally { Monitor.Exit(container._syncRoot); }
            }
        }
    }
}
