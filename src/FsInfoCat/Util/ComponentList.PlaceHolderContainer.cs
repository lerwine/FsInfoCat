using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public abstract partial class ComponentList
    {
        public static Component PlaceHolderOwner { get; } = new Component();

        public static bool IsPlaceHolderContainer(IContainer container) => container is PlaceHolderContainer;

        private sealed partial class PlaceHolderContainer : ContainerBase
        {
            private readonly ComponentList _target;

            protected override object SyncRoot => _target._syncRoot;

            public override IEqualityComparer<string> NameComparer { get; }

            internal PlaceHolderContainer(ComponentList target) : this(target, null) { }

            internal PlaceHolderContainer(ComponentList target, IEqualityComparer<string> nameComparer) : base(PlaceHolderOwner)
            {
                AttachableContainer oldContainer = target._container as AttachableContainer;
                if (oldContainer is null)
                {
                    if (null != target._container)
                        throw new ArgumentOutOfRangeException("List is already detached");
                    NameComparer = nameComparer ?? throw new ArgumentNullException(nameof(nameComparer));
                }
                else
                    NameComparer = nameComparer ?? oldContainer.NameComparer;
                _target = target ?? throw new ArgumentNullException(nameof(target));
                target._container = this;
                if (null == oldContainer)
                    return;
                var items = target._sites.Select(s => new
                {
                    Component = s.Component,
                    Name = s.Name
                }).ToArray();
                target._sites.Clear();
                foreach (var a in items)
                    oldContainer.Remove(a.Component);
                foreach (var item in items)
                {
                    Site site = new Site(item.Component, this, item.Name);
                    target._sites.Add(site);
                    item.Component.Site = site;
                }
            }

            private IComponent ValidateItemAndList(IComponent item, ComponentList componentList)
            {
                if (item is null)
                    throw new ArgumentNullException(nameof(item));
                ValidateList(componentList);
                return item;
            }

            private void ValidateList(ComponentList componentList)
            {
                if (componentList is null)
                    throw new ArgumentNullException(nameof(componentList));

                if (!ReferenceEquals(componentList, _target))
                    throw new InvalidOperationException();
            }

            public override void Add(IComponent component) => throw new NotSupportedException();

            public override void Add(IComponent component, string name) => throw new NotSupportedException();

            protected override int Add(IComponent item, ComponentList componentList) => Add(item, item.GetComponentName(), componentList);

            protected override int Add(IComponent item, string name, ComponentList componentList)
            {
                ValidateItemAndList(item, componentList);

                Monitor.Enter(_target._syncRoot);
                try
                {
                    int index = _target.IndexOf(item);
                    if (index >= 0)
                    {
                        SiteBase site = SetName(index, name);
                        if (!ReferenceEquals(site, item.Site))
                            item.Site = site;
                        return index;
                    }
                    return AddNew(-1, item, name);
                }
                finally { Monitor.Exit(_target._syncRoot); }
            }

            private int AddNew(int index, IComponent item, string name)
            {
                Monitor.Enter(_target._syncRoot);
                try
                {
                    if (_target.Contains(name))
                        throw new ArgumentOutOfRangeException(nameof(name), ERROR_MESSAGE_ITEM_WITH_NAME_EXISTS);
                    Site site = new Site(item, this, name);
                    if (index < 0)
                    {
                        IContainer oldContainer = item.GetContainer();
                        if (null != oldContainer && !ReferenceEquals(oldContainer, this))
                            oldContainer.Remove(item);
                        index = _target._sites.Count;
                        _target._sites.Add(site);
                    }
                    else if (index < _target._sites.Count)
                    {
                        IContainer oldContainer = item.GetContainer();
                        if (null != oldContainer && !ReferenceEquals(oldContainer, this))
                            oldContainer.Remove(item);
                        _target._sites.Insert(index, site);
                    }
                    else
                        throw new ArgumentOutOfRangeException(nameof(index));
                    item.Site = site;
                    return index;
                }
                finally { Monitor.Exit(_target._syncRoot); }
            }

            [Obsolete]
            internal static void Attach(ComponentList list)
            {
                ContainerBase oldContainer = list._container;
                if (oldContainer is PlaceHolderContainer)
                    return;
                var items = list._sites.Select(s => new
                {
                    Component = s.Component,
                    Name = s.Name
                }).ToArray();
                list._sites.Clear();
                foreach (var a in items)
                    oldContainer.Remove(a.Component);
                list._container = null;
                PlaceHolderContainer placeHolderContainer = new PlaceHolderContainer(list, oldContainer.NameComparer);
                list._container = placeHolderContainer;
                foreach (var item in items)
                {
                    Site site = new Site(item.Component, placeHolderContainer, item.Name);
                    list._sites.Add(site);
                    item.Component.Site = site;
                }
            }

            protected override void Clear(ComponentList componentList)
            {
                ValidateList(componentList);

                Monitor.Enter(_target._syncRoot);
                try
                {
                    IEnumerable<SiteBase> removedSites = _target._sites.ToArray();
                    _target._sites.Clear();
                    using (IEnumerator<SiteBase> enumerator = removedSites.GetEnumerator())
                        SetSitesNull(enumerator);
                }
                finally { Monitor.Exit(_target._syncRoot); }
            }

            private void SetSitesNull(IEnumerator<SiteBase> enumerator)
            {
                if (!enumerator.MoveNext())
                    return;
                try
                {
                    SiteBase site = enumerator.Current;
                    if (ReferenceEquals(site, site.Component.Site))
                        site.Component.Site = null;
                }
                finally { SetSitesNull(enumerator); }
            }

#warning Need to implement GetComponents()
            protected override IEnumerable<IComponent> GetComponents() => throw new NotImplementedException();

            protected override void Insert(int index, IComponent item, string name, ComponentList componentList)
            {
                ValidateItemAndList(item, componentList);

                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));

                Monitor.Enter(_target._syncRoot);
                try
                {
                    if (index > _target._sites.Count)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    int currentIndex = _target.IndexOf(item);
                    if (currentIndex > -1)
                    {
                        SiteBase currentSite = SetName(currentIndex, name);
                        if (index > currentIndex + 1)
                        {
                            _target._sites.RemoveAt(currentIndex);
                            _target._sites.Insert(index - 1, currentSite);
                        }
                        else if (index < currentIndex)
                        {
                            _target._sites.RemoveAt(currentIndex);
                            _target._sites.Insert(index, currentSite);
                        }
                        if (!ReferenceEquals(item.Site, currentSite))
                            item.Site = currentSite;
                    }
                    else
                        AddNew((index == _target._sites.Count) ? -1 : index, item, name);
                }
                finally { Monitor.Exit(_target._syncRoot); }
            }

            protected override void Insert(int index, IComponent item, ComponentList componentList) => Insert(index, item, item.GetComponentName(), componentList);

            public override void Remove(IComponent component)
            {
                if (component is null)
                    return;

                Monitor.Enter(_target._syncRoot);
                try
                {
                    int index = _target.IndexOf(component);
                    if (index > -1)
                        RemoveAt(index);
                }
                finally { Monitor.Exit(_target._syncRoot); }
            }

            protected override bool Remove(IComponent item, ComponentList componentList)
            {
                ValidateList(componentList);

                if (item is null)
                    return false;

                Monitor.Enter(_target._syncRoot);
                try
                {
                    int index = _target.IndexOf(item);
                    if (index < 0)
                        return false;
                    RemoveAt(index);
                }
                finally { Monitor.Exit(_target._syncRoot); }
                return true;
            }

            protected override void RemoveAt(int index, ComponentList componentList)
            {
                ValidateList(componentList);
                RemoveAt(index);
            }

            private void RemoveAt(int index)
            {
                SiteBase siteBase = _target._sites[index];
                _target.RemoveAt(index);
                siteBase.Component.Site = null;
            }

            protected override void SetAt(int index, IComponent item, ComponentList componentList) => SetAt(index, item, item.GetComponentName(), componentList);

            protected override void SetAt(int index, IComponent item, string name, ComponentList componentList)
            {
                ValidateItemAndList(item, componentList);

                int itemIndex = _target.IndexOf(item);
                int nameIndex = _target.IndexOf(name);
                SiteBase newSite;
                if (itemIndex == index)
                    newSite = SetName(index, name);
                else
                {
                    IComponent oldComponent = _target._sites[index].Component;
                    if (itemIndex < 0)
                    {
                        if (nameIndex > -1 && nameIndex != index)
                            throw new ArgumentOutOfRangeException(nameof(name), ERROR_MESSAGE_ITEM_WITH_NAME_EXISTS);
                        newSite = new Site(item, this, name);
                        oldComponent.Site = null;
                        (_target._sites[index] = newSite).Name = name;
                        item.Site = newSite;
                        return;
                    }
                    if (nameIndex > -1 && nameIndex != itemIndex && nameIndex != index)
                        throw new ArgumentOutOfRangeException(nameof(name), ERROR_MESSAGE_ITEM_WITH_NAME_EXISTS);
                    newSite = _target._sites[itemIndex];
                    oldComponent.Site = null;
                    _target._sites.RemoveAt(itemIndex);
                    (_target._sites[(itemIndex < index) ? index - 1 : index] = newSite).Name = name;
                }
                if (!ReferenceEquals(newSite, item.Site))
                    item.Site = newSite;
            }

            private SiteBase SetName(int index, string name)
            {
                SiteBase site = _target._sites[index];
                site.Name = name;
                return site;
            }

            protected override void OnDisposing()
            {
                Monitor.Enter(_target._syncRoot);
                try
                {
                    IEnumerable<SiteBase> toDispose = _target._sites.ToArray();
                    _target._sites.Clear();
                    using (IEnumerator<SiteBase> enumerator = toDispose.GetEnumerator())
                        DisposeComponents(enumerator);
                }
                finally { Monitor.Exit(_target._syncRoot); }
            }

            private void DisposeComponents(IEnumerator<SiteBase> enumerator)
            {
                if (!enumerator.MoveNext())
                    return;
                try
                {
                    SiteBase site = enumerator.Current;
                    using (IComponent component = site.Component)
                    {
                        if (ReferenceEquals(site, component.Site))
                            component.Site = null;
                    }
                }
                finally { DisposeComponents(enumerator); }
            }
        }
    }
}
