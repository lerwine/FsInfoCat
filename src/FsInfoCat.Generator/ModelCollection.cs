using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.CodeAnalysis;

namespace FsInfoCat.Generator
{
    public class ModelCollection : IList<ModelCollection.Component>, IContainer
    {
        private readonly object _syncRoot = new object();
        private bool _isDisposed;
        private Component.Site[] _sites;
        private ComponentCollection _components;

        ComponentCollection IContainer.Components
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_components == null)
                    {
                        IComponent[] result = new IComponent[Count];
                        for (int i = 0; i < Count; i++) result[i] = _sites[i].Component;
                        _components = new ComponentCollection(result);
                    }
                    return _components;
                } finally { Monitor.Exit(_syncRoot); }
            }
        }

        public int Count { get; private set; }

        bool ICollection<Component>.IsReadOnly => false;

        private IModelParent _owner;

        public IModelParent Owner
        {
            get => _owner;
            internal set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_owner != null) throw new InvalidOperationException();
                    _owner = value;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public Component this[int index]
        {
            get
            {
                if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
                Monitor.Enter(_syncRoot);
                try
                {
                    if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
                    return _sites?[index].Component;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        Component IList<Component>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        internal ModelCollection(IModelParent owner) => Owner = owner ?? throw new ArgumentNullException(nameof(owner));

        public void Add(Component component) => Component.Site.Add(this, component);

        void IContainer.Add(IComponent component, string name)
        {
            if (((Component)component).GetName() != name) throw new InvalidOperationException();
            Component.Site.Add(this, (Component)component);
        }

        void IContainer.Add(IComponent component) => Component.Site.Add(this, (Component)component);

        public void Clear() => Component.Site.Clear(this);

        public bool Contains(Component item) => Component.Site.Contains(this, item);

        public void CopyTo(Component[] array, int arrayIndex)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                ((_sites is null || Count == 0) ? Enumerable.Empty<Component>() : _sites.Select(s => s.Component)).ToList().CopyTo(array, arrayIndex);
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public IEnumerator<Component> GetEnumerator()
        {
            Monitor.Enter(_syncRoot);
            try
            {
                return ((_sites is null || Count == 0) ? Enumerable.Empty<Component>() : _sites.Select(s => s.Component)).GetEnumerator();
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Monitor.Enter(_syncRoot);
            try
            {
                return ((_sites is null || Count == 0) ? Enumerable.Empty<Component>() : _sites.Select(s => s.Component)).ToArray().GetEnumerator();
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public int IndexOf(Component item) => Component.Site.IndexOf(this, item);

        void IList<Component>.Insert(int index, Component item) => throw new NotSupportedException();

        public bool Remove(Component component) => Component.Site.Remove(this, component);

        void IContainer.Remove(IComponent component)
        {
            if (component is Component c) Component.Site.Remove(this, c);
        }

        public void RemoveAt(int index) => Component.Site.RemoveAt(this, index);

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing) Component.Site.OnDisposing(this);
                _isDisposed = true;
            }
        }

        ~ModelCollection()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public abstract class Component : IComponent, IModelDefinition
        {
            private readonly object _syncRoot = new object();
            private event EventHandler Disposed;
            private bool _isDisposed;
            private ISite _site;

            ISite IComponent.Site { get => _site; set => Site.Set(this, value); }

            event EventHandler IComponent.Disposed { add => Disposed += value; remove => Disposed -= value; }

            protected internal IModelParent Parent => (_site is Site site) ? site.Container._owner : null;

            public CommentDocumentation DocumentationComments { get; set; }

            protected internal abstract string GetName();

            ~Component()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: false);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_isDisposed)
                {
                    if (disposing) Component.Site.OnDisposing(this);
                    _isDisposed = true;
                }
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            public ModelData GetRoot() => (_site is Site site) ? site.Container._owner?.GetRoot() : null;

            internal class Site : ISite
            {
                private Site(ModelCollection container, Component component)
                {
                    Container = container;
                    Component = component;
                }

                public string Name => Component.GetName();

                string ISite.Name
                {
                    get => Name;
                    set { if (Component.GetName() != value) throw new InvalidOperationException(); }
                }

                public Component Component { get; }

                IComponent ISite.Component => Component;

                public ModelCollection Container { get; }

                IContainer ISite.Container => Container;

                bool ISite.DesignMode => false;

                public object GetService(Type serviceType) => (serviceType == typeof(ISite)) ? (object)this : (serviceType == typeof(IContainer)) ? Container : null;

                internal static void Add(ModelCollection modelCollection, Component component)
                {
                    if (component is null) throw new ArgumentNullException(nameof(component));
                    Monitor.Enter(component._syncRoot);
                    try
                    {
                        if (component._site != null)
                        {
                            if (component._site is Site site && ReferenceEquals(site, modelCollection)) return;
                            component._site.Container?.Remove(component);
                        }
                        Monitor.Enter(modelCollection._syncRoot);
                        try
                        {
                            modelCollection.Count++;
                            if (modelCollection._sites is null)
                                modelCollection._sites = new Site[16];
                            else if (modelCollection.Count == modelCollection._sites.Length)
                            {
                                Site[] newSites = new Site[modelCollection.Count + 16];
                                Array.Copy(modelCollection._sites, newSites, modelCollection.Count);
                                modelCollection._sites = newSites;
                            }
                            Site site = new Site(modelCollection, component);
                            component._site = site;
                            modelCollection._sites[modelCollection.Count - 1] = site;
                            modelCollection._components = null;
                        }
                        finally { Monitor.Exit(modelCollection._syncRoot); }
                    }
                    finally { Monitor.Exit(component._syncRoot); }
                }

                internal static void Clear(ModelCollection modelCollection)
                {
                    Monitor.Enter(modelCollection._syncRoot);
                    try
                    {
                        Site[] sites = modelCollection._sites;
                        modelCollection._sites = null;
                        if (sites != null && modelCollection.Count > 0)
                            foreach (Site s in sites.Take(modelCollection.Count))
                            {
                                Monitor.Enter(s.Component._syncRoot);
                                try { s.Component._site = null; }
                                finally { Monitor.Exit(s.Component._syncRoot); }
                            }
                        modelCollection._components = null;
                    }
                    finally { Monitor.Exit(modelCollection._syncRoot); }
                }

                internal static bool Contains(ModelCollection modelCollection, Component item)
                {
                    if (item is null) return false;
                    Monitor.Enter(item._syncRoot);
                    try
                    {
                        return item._site is Site site && ReferenceEquals(site.Container, modelCollection);
                    }
                    finally { Monitor.Exit(item._syncRoot); }
                }

                internal static int IndexOf(ModelCollection modelCollection, Component item)
                {
                    if (item is null) return -1;
                    Monitor.Enter(item._syncRoot);
                    try
                    {
                        if (item._site is Site site && ReferenceEquals(site.Container, modelCollection))
                        {
                            Monitor.Enter(modelCollection._syncRoot);
                            try
                            {
                                for (int i = 0; i < modelCollection.Count; i++)
                                    if (ReferenceEquals(modelCollection._sites[i].Component, item)) return i;
                            }
                            finally { Monitor.Exit(modelCollection._syncRoot); }
                        }
                    }
                    finally { Monitor.Exit(item._syncRoot); }
                    return -1;
                }

                private static void OnRemove(ModelCollection modelCollection, Component component, int index)
                {
                    component._site = null;
                    modelCollection.Count--;
                    switch (modelCollection.Count)
                    {
                        case 0:
                            modelCollection._sites[0] = null;
                            break;
                        case 1:
                            if (index == 0) modelCollection._sites[0] = modelCollection._sites[1];
                            modelCollection._sites[1] = null;
                            break;
                        default:
                            if (index < modelCollection.Count) Array.Copy(modelCollection._sites, index + 1, modelCollection._sites, index, modelCollection.Count - index);
                            modelCollection._sites[modelCollection.Count] = null;
                            break;
                    }
                    modelCollection._components = null;
                }

                internal static void RemoveAt(ModelCollection modelCollection, int index)
                {
                    if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
                    Monitor.Enter(modelCollection._syncRoot);
                    try
                    {
                        if (index >= modelCollection.Count) throw new ArgumentOutOfRangeException(nameof(index));
                        Component component = modelCollection[index];
                        Monitor.Enter(modelCollection._syncRoot);
                        try { OnRemove(modelCollection, component, index); }
                        finally { Monitor.Exit(modelCollection._syncRoot); }
                    }
                    finally { Monitor.Exit(modelCollection._syncRoot); }
                }

                internal static bool Remove(ModelCollection modelCollection, Component component)
                {
                    if (component is null) return false;
                    Monitor.Enter(component._syncRoot);
                    try
                    {
                        if (component._site is Site site && ReferenceEquals(site.Container, modelCollection))
                        {
                            Monitor.Enter(modelCollection._syncRoot);
                            try
                            {
                                for (int i = 0; i < modelCollection.Count; i++)
                                    if (ReferenceEquals(modelCollection._sites[i].Component, component))
                                    {
                                        OnRemove(modelCollection, component, i);
                                        return true;
                                    }
                            }
                            finally { Monitor.Exit(modelCollection._syncRoot); }
                        }
                    }
                    finally { Monitor.Exit(component._syncRoot); }
                    return false;
                }

                internal static void OnDisposing(Component component)
                {
                    Monitor.Enter(component._syncRoot);
                    try { if (component._site != null) component._site.Container?.Remove(component); }
                    finally { Monitor.Exit(component._syncRoot); }
                }

                internal static void OnDisposing(ModelCollection modelCollection)
                {
                    Monitor.Enter(modelCollection._syncRoot);
                    try
                    {
                        Site[] sites = modelCollection._sites;
                        modelCollection._sites = null;
                        if (sites != null)
                        {
                            while (modelCollection.Count > 0)
                            {
                                modelCollection.Count--;
                                Site s = sites[modelCollection.Count];
                                Monitor.Enter(s.Component._syncRoot);
                                try { s.Component._site = null; }
                                finally { Monitor.Exit(s.Component._syncRoot); }
                                s.Component.Dispose();
                            }
                        }
                        modelCollection._components = null;
                    }
                    finally { Monitor.Exit(modelCollection._syncRoot); }
                }

                internal static void Set(Component component, ISite value)
                {
                    Monitor.Enter(component._syncRoot);
                    try
                    {
                        if (value != null && value is Site newSite)
                        {
                            if (component._site != null && ReferenceEquals(component._site, newSite)) return;
                            throw new InvalidOperationException();
                        }
                        component._site?.Container?.Remove(component);
                        component._site = value;
                    }
                    finally { Monitor.Exit(component._syncRoot); }
                }
            }
        }
    }
}
