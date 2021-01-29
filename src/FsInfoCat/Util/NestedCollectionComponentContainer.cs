using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public partial class NestedCollectionComponentContainer<TOwner, TItem> : INestedContainer, IServiceProvider
        where TOwner : IComponent
        where TItem : IComponent
    {
        private readonly object _syncRoot = new object();
        private Site _first = null;
        private Site _last = null;
        private int _componentCount = 0;
        private ComponentCollection _components = null;
        private bool _isCaseSensitive;
        private Site.NestedCollection _items = null;
        private bool _isDisposed = false;

        public IComponent Owner { get; private set; }

        ComponentCollection IContainer.Components
        {
            get
            {
                ComponentCollection components = _components;
                if (null == components)
                    _components = components = new ComponentCollection(GetComponents().ToArray());
                return components;
            }
        }

        public bool IsCaseSensitive
        {
            get => _isCaseSensitive;
            protected set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    if (_isCaseSensitive == value)
                        return;
                    if (value)
                        NameComparer = StringComparer.InvariantCulture;
                    else
                    {
                        StringComparer nameComparer = StringComparer.InvariantCultureIgnoreCase;
                        if (null != _first && !ReferenceEquals(_first, _last) && Site.GetAllSites(this).Where(s => null != s.Name).GroupBy(s => s.Name, nameComparer).Any(g => g.Count() > 1))
                            throw new InvalidOperationException("Switching to case insensitivity would result in duplicate names");
                        NameComparer = nameComparer;
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public StringComparer NameComparer { get; private set; }

        public Collection<TItem> Items
        {
            get => _items;
            set => Site.NestedCollection.SetItems(value, this);
        }

        public NestedCollectionComponentContainer(TOwner owner, bool isCaseSensitive)
        {
            if (null == owner)
                throw new ArgumentNullException(nameof(owner));
            _isCaseSensitive = isCaseSensitive;
            NameComparer = (isCaseSensitive) ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase;
            Owner = owner;
            owner.Disposed += OnOwnerDisposed;
        }

        private void OnOwnerDisposed(object sender, EventArgs e)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                Owner.Disposed -= OnOwnerDisposed;
                Owner = null;
            }
            finally { Monitor.Exit(_syncRoot); }
            Dispose();
        }

        public void Add(IComponent component) => Site.Add(component, this, component.GetComponentName());

        public void Add(IComponent component, string name) => Site.Add(component, this, name);

        public IEnumerable<IComponent> GetComponents() => Site.GetAllSites(this).Select(s => s.Component);

        public IEnumerable<string> GetNames() => Site.GetAllSites(this).Select(s => s.Name).Where(n => null != n);

        public object GetService(Type serviceType) => (serviceType == typeof(IContainer)) ? this : null;

        public void Remove(IComponent component)
        {
            Site site;
            if (null != component && null != (site = component.Site as Site) && ReferenceEquals(site.Container, this))
                site.Remove();
        }

        protected virtual void Dispose(bool disposing)
        {
            IComponent owner;
            Monitor.Enter(_syncRoot);
            try
            {
                if (_isDisposed)
                    return;
                _isDisposed = true;
                if (!disposing)
                    return;
                if (null == (owner = Owner))
                    return;
                owner.Disposed -= OnOwnerDisposed;
                while (null != _last)
                    _last.Remove();
            }
            finally { Monitor.Exit(_syncRoot); }
            owner.Dispose();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
