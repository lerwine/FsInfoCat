using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public abstract partial class ComponentList : IEnumerable, ICollection, IList
    {
        private readonly object _syncRoot = new object();
        private ContainerBase _container;
        private readonly Collection<ContainerBase.SiteBase> _sites = new Collection<ContainerBase.SiteBase>();

        protected IComponent this[int index] { get => _sites[index].Component; set => SetAt(index, value); }

        protected IComponent this[string name]
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    int index = IndexOf(name);
                    return (index < 0) ? null : _sites[index].Component;
                } finally { Monitor.Exit(_syncRoot); }
            }
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    int index = IndexOf(name);
                    if (index < 0)
                        Add(value, name);
                    else
                        SetAt(index, value, name);
                } finally { Monitor.Exit(_syncRoot); }
            }
        }

        object IList.this[int index] { get => this[index]; set => this[index] = (IComponent)value; }

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        public int Count => _sites.Count;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => _syncRoot;

        protected ComponentList(AttachableContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Attach(this);
        }

        protected ComponentList(IEnumerable<IComponent> items) : this()
        {
            if (null != items)
            {
                foreach (IComponent i in items)
                {
                    if (null != i)
                        Add(i);
                }
            }
        }

        protected ComponentList()
        {
            _container = new PlaceHolderContainer(this);
        }

        protected virtual int Add(IComponent value) => _container.AddListItem(value, this);

        int IList.Add(object value) => Add((IComponent)value);

        protected virtual int Add(IComponent value, string name) => _container.AddListItem(value, name, this);

        public void Clear() => _container.ClearList(this);

#warning Need to implement Contains(IComponent)
        protected bool Contains(IComponent component) => throw new NotImplementedException();
        bool IList.Contains(object value) => Contains(value as IComponent);

        public bool Contains(string name) => IndexOf(name) > -1;

#warning Need to implement ICollection.CopyTo(Array, int)
        void ICollection.CopyTo(Array array, int index) => throw new NotImplementedException();

        protected IEnumerable<IComponent> GetComponents() => _sites.Select(s => s.Component);

        protected IEnumerable<string> GetNames() => _sites.Select(s => s.Name).Where(n => null != n);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetComponents()).GetEnumerator();

        protected int IndexOf(IComponent component)
        {
            if (null == component)
                return -1;
            Monitor.Enter(_syncRoot);
            try
            {
                for (int i = 0; i < _sites.Count; i++)
                {
                    if (ReferenceEquals(_sites[i].Component, component))
                        return i;
                }
            } finally { Monitor.Exit(_syncRoot); }
            return -1;
        }

        int IList.IndexOf(object value) => IndexOf(value as IComponent);

        public int IndexOf(string name)
        {
            if (null == name)
                return -1;
            IEqualityComparer<string> nameComparer = _container.NameComparer;
            Monitor.Enter(_syncRoot);
            try
            {
                for (int i = 0; i < _sites.Count; i++)
                {
                    string n = _sites[i].Name;
                    if (null != n && nameComparer.Equals(n, name))
                        return i;
                }
            } finally { Monitor.Exit(_syncRoot); }
            return -1;
        }
        protected virtual void Insert(int index, IComponent value) => _container.InsertListItem(index, value, this);
        void IList.Insert(int index, object value) => Insert(index, (IComponent)value);

        protected virtual void Insert(int index, IComponent value, string name) => _container.InsertListItem(index, value, name, this);

        protected bool Remove(IComponent component) => _container.RemoveListItem(component, this);
        void IList.Remove(object value) => Remove(value as IComponent);

        public bool Remove(string name)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                int index = IndexOf(name);
                if (index >= -1)
                {
                    RemoveAt(index);
                    return true;
                }
            } finally { Monitor.Exit(_syncRoot); }
            return false;
        }

        public void RemoveAt(int index) => _container.RemoveListItemAt(index, this);

        protected virtual void SetAt(int index, IComponent value) => _container.SetListItemAt(index, value, this);

        protected virtual void SetAt(int index, IComponent value, string name) => _container.SetListItemAt(index, value, name, this);

        public override bool Equals(object obj) => null != obj && ReferenceEquals(obj, this);

        public override int GetHashCode() => _sites.GetHashCode();
    }

    public class ComponentList<TComponent> : ComponentList, ICollection<TComponent>, IEnumerable<TComponent>, IEnumerable, IList<TComponent>, IReadOnlyCollection<TComponent>, IReadOnlyList<TComponent>
        where TComponent : IComponent
    {
        public new TComponent this[int index] { get => (TComponent)base[index]; set => base.SetAt(index, value); }

        public new TComponent this[string name] { get => (TComponent)base[name]; set => base[name] = value; }

        TComponent IReadOnlyList<TComponent>.this[int index] => this[index];

        bool ICollection<TComponent>.IsReadOnly => false;

        public ComponentList(AttachableContainer container) : base(container) { }

        public ComponentList(IEnumerable<TComponent> items) : base(items.Cast<IComponent>()) { }

        public ComponentList() : base() { }

        public void Add(TComponent item) => base.Add(item);
        protected override int Add(IComponent value) => base.Add((TComponent)value);

        public void Add(TComponent value, string name) => base.Add(value, name);
        protected override int Add(IComponent value, string name) => base.Add((TComponent)value, name);

        public bool Contains(TComponent item) => base.Contains(item);

        public void CopyTo(TComponent[] array, int arrayIndex) => GetComponents().Cast<TComponent>().ToList().CopyTo(array, arrayIndex);

        public IEnumerator<TComponent> GetEnumerator() => GetComponents().Cast<TComponent>().GetEnumerator();

        public int IndexOf(TComponent item) => base.IndexOf(item);

        public void Insert(int index, TComponent item) => base.Insert(index, item);
        protected override void Insert(int index, IComponent value) => base.Insert(index, (TComponent)value);

        public void Insert(int index, TComponent value, string name) => base.Insert(index, value, name);
        protected override void Insert(int index, IComponent value, string name) => base.Insert(index, (TComponent)value, name);

        public bool Remove(TComponent item) => base.Remove(item);

        protected override void SetAt(int index, IComponent value) => base.SetAt(index, (TComponent)value);

        protected override void SetAt(int index, IComponent value, string name) => base.SetAt(index, (TComponent)value, name);
    }
}
