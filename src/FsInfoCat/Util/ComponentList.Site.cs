using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public abstract partial class ComponentList
    {
        public abstract partial class ContainerBase
        {
            public abstract class SiteBase : INestedSite
            {
                private string _name;

                string INestedSite.FullName
                {
                    get
                    {
                        Monitor.Enter(Container.SyncRoot);
                        try
                        {
                            if (!(IsOrphaned() || Container._isDisposed))
                            {
                                ISite site = Container.Owner.Site;
                                if (null != site)
                                {
                                    if (site is INestedSite nestedSite)
                                    {
                                        string f = nestedSite.FullName;
                                        if (!string.IsNullOrEmpty(f))
                                        {
                                            if (string.IsNullOrEmpty(_name))
                                                return f;
                                            return $"{f}/{Uri.EscapeDataString(_name)}";
                                        }
                                    }
                                    string n = site.Name;
                                    if (!string.IsNullOrEmpty(n))
                                    {
                                        if (string.IsNullOrEmpty(_name))
                                            return n;
                                        return $"{n}/{Uri.EscapeDataString(_name)}";
                                    }
                                }
                            }
                        }
                        finally { Monitor.Exit(Container.SyncRoot); }

                        return _name ?? "";
                    }
                }

                public IComponent Component { get; }

                public ContainerBase Container { get; }

                IContainer ISite.Container => Container;

                bool ISite.DesignMode => false;

                public string Name
                {
                    get => _name;
                    set
                    {
                        if (value is null || IsOrphaned() || (null != _name && Container.NameComparer.Equals(value, _name)))
                            _name = value;
                        else if (_name != value)
                        {
                            Monitor.Enter(Container.SyncRoot);
                            try
                            {
                                if (!Container._isDisposed)
                                {
                                    IEqualityComparer<string> nameComparer = Container.NameComparer;
                                    if (Container.GetComponents().Where(c => !ReferenceEquals(c, Component))
                                            .Select(c => c.GetComponentName()).Any(n => null != n && nameComparer.Equals(n, value)))
                                        throw new ArgumentOutOfRangeException(nameof(value), ERROR_MESSAGE_ITEM_WITH_NAME_EXISTS);
                                }
                                _name = value;
                            }
                            finally { Monitor.Exit(Container.SyncRoot); }
                        }
                    }
                }

                protected SiteBase(IComponent component, ContainerBase container, string name)
                {
                    Component = component ?? throw new ArgumentNullException(nameof(component));
                    Container = container ?? throw new ArgumentNullException(nameof(container));
                    _name = name;
                }

                protected SiteBase(IComponent component, ContainerBase container)
                {
                    Component = component ?? throw new ArgumentNullException(nameof(component));
                    Container = container ?? throw new ArgumentNullException(nameof(container));
                    _name = component.GetComponentName();
                }

                public abstract bool IsOrphaned();

                public object GetService(Type serviceType) => (serviceType == typeof(ISite) || serviceType == typeof(INestedSite)) ? this :
                    ((IsOrphaned()) ? null : Container.GetService(serviceType));
            }
        }

        private ComponentList _previous = null;
        private ComponentList _next = null;
        public partial class AttachableContainer
        {
            private Site _firstSite = null;
            private Site _lastSite = null;
            private ComponentList _firstAttached = null;
            private ComponentList _lastAttached = null;

            private IEnumerable<ComponentList> GetAttachedLists()
            {
                for (ComponentList list = _firstAttached; null != list; list = list._next)
                    yield return list;
            }

            private sealed class Site : SiteBase
            {
                private Site _previous;
                private Site _next = null;
                public new AttachableContainer Container => (AttachableContainer)base.Container;

                internal Site(IComponent component, AttachableContainer container, string name) : base(component, container, name) { }

                internal Site(IComponent component, AttachableContainer container) : base(component, container) { }

                internal static IEnumerable<Site> GetSites(AttachableContainer container)
                {
                    for (Site site = container._firstSite; null != site; site = site._next)
                        yield return site;
                }

                public override bool IsOrphaned() => _previous is null && _next is null && !ReferenceEquals(this, Container._firstSite);

                private static object ValidateItemAndList(IComponent item, AttachableContainer container, ComponentList componentList, bool listRequired, out object listSyncRoot)
                {
                    if (item is null)
                        throw new ArgumentNullException(nameof(item));
                    return ValidateList(container, componentList, listRequired, out listSyncRoot);
                }

                private static Site GetSite(IComponent item, AttachableContainer container, ComponentList componentList, out int listIndex)
                {
                    if (componentList is null)
                        listIndex = -1;
                    else if ((listIndex = componentList.IndexOf(item)) > -1 && componentList._sites[listIndex] is Site site && ReferenceEquals(site.Container, container) && !site.IsOrphaned())
                        return site;
                    return GetSites(container).FirstOrDefault(s => ReferenceEquals(s.Component, item));
                }

                private static object ValidateList(AttachableContainer container, ComponentList list, bool listRequired, out object listSyncRoot)
                {
                    if (container is null)
                        throw new ArgumentNullException(nameof(container));

                    if (list is null)
                    {
                        if (listRequired)
                            throw new ArgumentNullException(nameof(list));
                        listSyncRoot = null;
                    }
                    else if (ReferenceEquals(list._container, container))
                        listSyncRoot = list._syncRoot;
                    else
                        throw new InvalidOperationException();
                    return container.SyncRoot;
                }

                internal static void Attach(ComponentList list, AttachableContainer container)
                {
                    ContainerBase oldContainer = list._container;
                    if (oldContainer is AttachableContainer)
                    {
                        if (ReferenceEquals(oldContainer, container))
                            return;
                        throw new ArgumentOutOfRangeException("List is attached to another container");
                    }
                    var items = list._sites.Select(s => new
                    {
                        Component = s.Component,
                        Name = s.Name
                    }).ToArray();
                    if (items.Where(i => !container.GetComponents().Any(c => ReferenceEquals(i.Component, c)))
                            .Select(s => s.Name).Where(n => null != n)
                            .Concat(container.GetNames()).GroupBy(n => n, container.NameComparer).Any(g => g.Count() > 1))
                        throw new ArgumentOutOfRangeException(ERROR_MESSAGE_ITEM_WITH_NAME_EXISTS);
                    list._sites.Clear();
                    if (null != oldContainer)
                    {
                        foreach (var a in items)
                            oldContainer.Remove(a.Component);
                    }
                    list._container = container;
                    if ((list._previous = container._lastAttached) is null)
                        container._firstAttached = container._lastAttached = list;
                    else
                        container._lastAttached = list._previous._next = list;
                    foreach (var item in items.ToArray())
                        list._sites.Add(new Site(item.Component, container, item.Name).Add());
                }

                internal static void Detach(ComponentList list)
                {
                    if (!(list._container is AttachableContainer container))
                        return;
                    PlaceHolderContainer newContainer = new PlaceHolderContainer(list, container.NameComparer);
                    if (list._container is AttachableContainer)
                        return;
                    if (list._previous is null)
                    {
                        if (ReferenceEquals(container._firstAttached, list))
                        {
                            if ((container._firstAttached = list._next) is null)
                                container._firstAttached = container._lastAttached = null;
                            else
                                list._next = list._next._previous = null;
                        }
                    }
                    else if ((list._previous._next = list._next) is null)
                        list._previous = (container._lastAttached = list._previous)._next = null;
                    else
                    {
                        list._next._previous = list._previous;
                        list._next = list._previous = null;
                    }
                }

                /// <summary>
                /// Adds the specified <paramref name="component" /> to the <paramref name="container">.
                /// </summary>
                /// <param name="component">The <seealso cref="IComponent" /> to add.</param>
                /// <param name="name">The unique name to assign to the component.
                /// <para>-or-</para>
                /// <para><c>null</c> to leave it unnamed.</para></param>
                /// <param name="container">The <seealso cref="AttachableContainer" /> to add the <paramref name="component" /> to.</param>
                /// <param name="list">The optional <seealso cref="ComponentList" /> to add the <paramref name="component" /> to.</param>
                /// <returns>The zero-based index at which the <paramref name="item" /> to the attached <paramref name="list" />
                /// or <c>-1</c> if <paramref name="list" /> was <c>null</c>.</returns>
                /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
                /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
                /// <paramref name="container">; otherwise, it will be unnamed.</remarks>
                /// <exception cref="ArgumentNullException"><paramref name="component"/> or <paramref name="container"/> was <c>null</c>.</exception>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> was already used by another item.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="list"/> was not attached to the <see cref="container" />.</exception>
                internal static int Add(IComponent component, string name, AttachableContainer container, ComponentList list = null)
                {
                    object containerSyncRoot = ValidateItemAndList(component, container, list, false, out object listSyncRoot);
                    Monitor.Enter(containerSyncRoot);
                    try
                    {
                        if (null != listSyncRoot)
                            Monitor.Enter(listSyncRoot);
                        try
                        {
                            Site site = GetSite(component, container, list, out int index);
                            if (site is null)
                            {
                                if (index > -1)
                                    list._sites.RemoveAt(index);
                                if (null != name)
                                    container.ValidateName(container.GetNames(), name);
                                return new Site(component, container, name).Add(list, index);
                            }

                            site.Name = name;

                            if (list is null)
                            {
                                if (!ReferenceEquals(component.Site, site))
                                    component.Site = site;
                                return index;
                            }

                            if (index < 0)
                            {
                                index = list._sites.Count;
                                list._sites.Add(site);
                            }
                            else
                                list._sites[index] = site;

                            if (!ReferenceEquals(component.Site, site))
                                component.Site = site;

                            return index;
                        }
                        finally
                        {
                            if (null != listSyncRoot)
                                Monitor.Exit(listSyncRoot);
                        }
                    }
                    finally { Monitor.Exit(containerSyncRoot); }
                }

                private int Add(ComponentList list, int index)
                {
                    Add(true);
                    if (list is null)
                    {
                        Component.Site = this;
                        return index;
                    }
                    if (index < 0)
                    {
                        index = list._sites.Count;
                        list._sites.Add(this);
                    }
                    else
                        list._sites[index] = this;
                    Component.Site = this;
                    return index;
                }

                private Site Add(bool skipSetSiteToThis = false)
                {
                    ISite site = Component.Site;
                    if (null != site && !ReferenceEquals(site.Container, Container))
                        site.Container.Remove(Component);
                    if ((_previous = Container._lastSite) is null)
                        Container._firstSite = Container._lastSite = this;
                    else
                        Container._lastSite = _previous._next = this;
                    if (!skipSetSiteToThis)
                        Component.Site = this;
                    return this;
                }

                /// <summary>
                /// Clears all <seealso cref="IComponent" />s from the attached <paramref name="list" />.
                /// </summary>
                /// <param name="list">The attached <seealso cref="ComponentList" /> to remove all items from.</param>
                /// <remarks><seealso cref="IComponent" />s which were removed from
                /// the <paramref name="list" /> will also be removed from the <paramref name="container">.</remarks>
                /// <exception cref="ArgumentNullException"><paramref name="list"/> was <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="list"/> was not attached to the
                /// <paramref name="container">.</exception>
                internal static void Clear(AttachableContainer container, ComponentList list)
                {
                    object containerSyncRoot = ValidateList(container, list, true, out object listSyncRoot);
                    Monitor.Enter(containerSyncRoot);
                    try
                    {
                        Monitor.Enter(listSyncRoot);
                        try
                        {
                            IEnumerable<SiteBase> removedSites = list._sites.ToArray();
                            list._sites.Clear();
                            using (IEnumerator<SiteBase> enumerator = removedSites.GetEnumerator())
                                RemoveSites(container, enumerator);
                        }
                        finally { Monitor.Exit(listSyncRoot); }
                    }
                    finally { Monitor.Exit(containerSyncRoot); }
                }

                private static void RemoveSites(AttachableContainer container, IEnumerator<SiteBase> enumerator)
                {
                    if (!enumerator.MoveNext())
                        return;
                    try
                    {
                        SiteBase siteBase = enumerator.Current;
                        if (siteBase is Site site && ReferenceEquals(site.Container, container) && !site.IsOrphaned())
                            site.Unlink();
                        else
                        {
                            container.Remove(siteBase.Component);
                            if (ReferenceEquals(siteBase, siteBase.Component.Site))
                                siteBase.Component.Site = null;
                        }
                    }
                    finally { RemoveSites(container, enumerator); }
                }

                private bool Unlink()
                {
                    if (_previous is null)
                    {
                        if (ReferenceEquals(Container._firstSite, this))
                        {
                            if ((Container._firstSite = _next) is null)
                                Container._lastSite = null;
                            else
                                _next = _next._previous = null;
                        }
                        else
                            return false;
                    }
                    else if ((_previous._next = _next) is null)
                        _previous = (Container._lastSite = _previous)._next = null;
                    else
                    {
                        _next._previous = _previous;
                        _next = _previous = null;
                    }
                    return true;
                }

                /// <summary>
                /// Inserts the specified <paramref name="item" /> into the attached <paramref name="list" /> at the
                /// specified <paramref name="index" /> as well as adding it to the <paramref name="container">, assigning a name to it.
                /// </summary>
                /// <param name="index">The zero-based index at which to insert the <paramref name="item" /> into the attached <paramref name="list" /></param>
                /// <param name="item">The <seealso cref="IComponent" /> to insert.</param>
                /// <param name="name">The unique name to assign to the component.
                /// <para>-or-</para>
                /// <para><c>null</c> to leave it unnamed.</para></param>
                /// <param name="list">The attached <seealso cref="ComponentList" /> to insert the <paramref name="item" /> into.</param>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
                /// <paramref name="list" />
                /// <para>-or-</para>
                /// <para><paramref name="name"/> was already used by another item.</para></exception>
                /// <exception cref="ArgumentNullException"><paramref name="item"/>, <paramref name="container"/> or <paramref name="list"/> was <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="list"/> was not attached to the
                /// <paramref name="container">.</exception>
                internal static void Insert(int index, IComponent item, string name, AttachableContainer container, ComponentList list)
                {
                    if (index < 0)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    object containerSyncRoot = ValidateItemAndList(item, container, list, true, out object listSyncRoot);
                    Monitor.Enter(containerSyncRoot);
                    try
                    {
                        Monitor.Enter(listSyncRoot);
                        try
                        {
                            Site site = GetSite(item, container, list, out int currentIndex);
                            if (site is null)
                            {
                                if (null != name)
                                    container.ValidateName(container.GetNames(), name);
                                if (currentIndex > -1)
                                    list._sites.RemoveAt(currentIndex);
                                if (index > currentIndex)
                                    index--;
                                if (index == list._sites.Count)
                                    new Site(item, container, name).Add(list, -1);
                                else if (index < list._sites.Count)
                                    list._sites.Insert(index, new Site(item, container, name).Add());
                                else
                                    throw new ArgumentOutOfRangeException(nameof(index));
                                return;
                            }
                            site.Name = name;
                            if (index == currentIndex || index == currentIndex + 1)
                                list._sites[currentIndex] = site;
                            else
                            {
                                list._sites.RemoveAt(currentIndex);
                                if (index < currentIndex)
                                    list._sites.Insert(index, site);
                                else if (index == list._sites.Count)
                                    list._sites.Add(site);
                                else
                                    list._sites.Insert(index - 1, site);
                            }
                            if (!ReferenceEquals(item.Site, site))
                                item.Site = site;
                        }
                        finally { Monitor.Exit(listSyncRoot); }
                    }
                    finally { Monitor.Exit(containerSyncRoot); }
                }

                /// <summary>
                /// Removes the specified <paramref name="item" /> from the attached <paramref name="list" /> and from the
                /// <paramref name="container">.
                /// </summary>
                /// <param name="item">The <seealso cref="IComponent" /> to remove.</param>
                /// <param name="list">The attached <seealso cref="ComponentList" /> to remove the <paramref name="item" /> from.</param>
                /// <returns><c>true</c> if <paramref name="item" /> was removed from the attached <paramref name="list" />;
                /// otherwise, <c>false</c></returns>
                /// <exception cref="ArgumentNullException"><paramref name="container"/> was <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="list"/> was not attached to the
                /// <paramref name="container">.</exception>
                internal static bool Remove(IComponent item, AttachableContainer container, ComponentList list = null)
                {
                    if (item is null)
                        return false;
                    object containerSyncRoot = ValidateList(container, list, false, out object listSyncRoot);
                    Monitor.Enter(containerSyncRoot);
                    try
                    {
                        if (null != listSyncRoot)
                            Monitor.Enter(listSyncRoot);
                        try
                        {
                            Site site = GetSite(item, container, list, out int index);
                            if (index > -1)
                                list._sites.RemoveAt(index);
                            if (site is null)
                                return false;
                            item.Site = null;
                            site.Unlink();
                        }
                        finally
                        {
                            if (null != listSyncRoot)
                                Monitor.Exit(listSyncRoot);
                        }
                    }
                    finally { Monitor.Exit(containerSyncRoot); }
                    return true;
                }

                /// <summary>
                /// Removes the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> of the
                /// <paramref name="list" /> and removes it from the <paramref name="container"> as well.
                /// </summary>
                /// <param name="index">The zero-based index of the <seealso cref="IComponent" /> to remove.</param>
                /// <param name="list">The attached <seealso cref="ComponentList" /> to remove the
                /// <seealso cref="IComponent" /> from.</param>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
                /// <paramref name="list" /></exception>
                /// <exception cref="ArgumentNullException"><paramref name="container"/> pr <paramref name="list"/> was <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="list"/> was not attached to the
                /// <paramref name="container">.</exception>
                internal static void RemoveAt(int index, AttachableContainer container, ComponentList list)
                {
                    if (index < 0)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    object containerSyncRoot = ValidateList(container, list, true, out object listSyncRoot);
                    Monitor.Enter(containerSyncRoot);
                    try
                    {
                        Monitor.Enter(listSyncRoot);
                        try
                        {
                            IComponent item = list._sites[index].Component;
                            list._sites.RemoveAt(index);
                            Site site = GetSite(item, container, list, out index);
                            if (null != site)
                                site.Unlink();
                        }
                        finally { Monitor.Exit(listSyncRoot); }
                    }
                    finally { Monitor.Exit(containerSyncRoot); }
                }

                /// <summary>
                /// Replaces the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> in the
                /// <paramref name="list" /> with another <paramref name="item" />, replacing it in the <paramref name="container"> as well.
                /// </summary>
                /// <param name="index">The zero-based index of the <seealso cref="IComponent" /> to replace.</param>
                /// <param name="item">The new <seealso cref="IComponent" /> to place at the specified index.</param>
                /// <param name="name">The unique name to assign to the component.
                /// <para>-or-</para>
                /// <para><c>null</c> to leave it unnamed.</para></param>
                /// <param name="list">The attached <seealso cref="ComponentList" /> to replace the
                /// <seealso cref="IComponent" /> within.</param>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
                /// <paramref name="list" />
                /// <para>-or-</para>
                /// <para><paramref name="name" /> was already used by another item.</para></exception>
                /// <exception cref="ArgumentNullException"><paramref name="item"/>, ><paramref name="container"/> or <paramref name="list"/> was <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="list"/> was not attached to the
                /// <paramref name="container">.</exception>
                internal static void SetAt(int index, IComponent item, string name, AttachableContainer container, ComponentList list)
                {
                    if (index < 0)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    object containerSyncRoot = ValidateItemAndList(item, container, list, true, out object listSyncRoot);
                    Monitor.Enter(containerSyncRoot);
                    try
                    {
                        Monitor.Enter(listSyncRoot);
                        try
                        {
                            if (index >= list._sites.Count)
                                throw new ArgumentOutOfRangeException(nameof(index));
                            SiteBase siteBase = list._sites[index];
                            Site oldSite = GetSite(siteBase.Component, container, list, out int currentIndex);
                            Site currentSite = GetSite(item, container, list, out currentIndex);
                            if (oldSite is null)
                            {
                                if (null != name)
                                    container.ValidateName(container.GetNames(item), name);
                                if (currentIndex > index)
                                    list._sites.RemoveAt(currentIndex);
                                else if (currentIndex < index && currentIndex > -1)
                                {
                                    list._sites.RemoveAt(currentIndex);
                                    index--;
                                }
                                else
                                {
                                    new Site(item, container, name).Add(list, index);
                                    return;
                                }
                                new Site(item, container, name).Add(list, (index < list._sites.Count) ? index : -1);
                            }
                            else if (currentSite == null)
                            {
                                if (null != name)
                                    container.ValidateName(container.GetNames(oldSite.Component), name);
                                if (currentIndex > -1)
                                {
                                    list._sites.RemoveAt(currentIndex);
                                    if (currentIndex < index)
                                        index--;
                                }
                                if (ReferenceEquals(oldSite.Container, container))
                                    oldSite.Component.Site = null;
                                list._sites[index] = new Site(item, container, name).Add();
                            }
                            else
                            {
                                if (ReferenceEquals(oldSite, currentSite))
                                    currentSite.Name = name;
                                else if (ReferenceEquals(oldSite.Component, item))
                                {
                                    currentSite = oldSite;
                                    currentSite.Name = name;
                                    if (currentIndex > index)
                                        list._sites.RemoveAt(currentIndex);
                                    else if (currentIndex < index && currentIndex > -1)
                                    {
                                        list._sites.RemoveAt(currentIndex);
                                        index--;
                                    }
                                    list._sites[index] = currentSite;
                                }
                                else
                                {
                                    if (null != name)
                                        container.ValidateName(container.GetNames(oldSite.Component, item), name);
                                    if (ReferenceEquals(oldSite.Component.Site, oldSite))
                                        oldSite.Component.Site = null;
                                    list._sites[index] = currentSite;
                                    currentSite.Name = name;
                                }
                                if (!ReferenceEquals(item, currentSite))
                                    item.Site = currentSite;
                            }
                        }
                        finally { Monitor.Exit(listSyncRoot); }
                    }
                    finally { Monitor.Exit(containerSyncRoot); }
                }
            }
        }

        private partial class PlaceHolderContainer
        {
            private sealed class Site : SiteBase
            {
                public new PlaceHolderContainer Container => (PlaceHolderContainer)base.Container;

                internal Site(IComponent component, PlaceHolderContainer container, string name) : base(component, container, name) { }

                internal Site(IComponent component, PlaceHolderContainer container) : base(component, container) { }

                public override bool IsOrphaned() => !Container._target._sites.Any(s => ReferenceEquals(s, this));
            }
        }
    }
}
