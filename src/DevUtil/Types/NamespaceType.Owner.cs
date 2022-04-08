using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace DevUtil.Types
{
    public partial class NamespaceType
    {
        public class Owner : NotifyPropertyChange
        {
            private readonly Dictionary<string, Collection<NamespaceType>> _byName = new();
            private ObservableCollection<NamespaceType> _namespaces;

            protected object SyncRoot { get; } = new object();

            [XmlArrayItem(ElementName = RootElementName)]
            public ObservableCollection<NamespaceType> Namespaces
            {
                get => _namespaces;
                set
                {
                    ObservableCollection<NamespaceType> oldValue;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if ((oldValue = _namespaces) is null)
                        {
                            if (value is null)
                                return;
                            RaisePropertyChanging(oldValue, value);
                            _namespaces = value;
                            value.CollectionChanged += Namespaces_CollectionChanged;
                            using (IEnumerator<NamespaceType> newItems = value.GetEnumerator())
                            {
                                if (newItems.MoveNext())
                                    OnItemsAdded(newItems);
                            }
                        }
                        else if (value is null)
                        {
                            RaisePropertyChanging(oldValue, value);
                            _namespaces.CollectionChanged -= Namespaces_CollectionChanged;
                            _namespaces = value;
                            using (IEnumerator<NamespaceType> oldItems = oldValue.GetEnumerator())
                            {
                                if (oldItems.MoveNext())
                                    OnItemsRemoved(oldItems);
                            }
                        }
                        else
                        {
                            if (ReferenceEquals(_namespaces, value))
                                return;

                            RaisePropertyChanging(oldValue, value);
                            oldValue.CollectionChanged -= Namespaces_CollectionChanged;
                            _namespaces = value;
                            value.CollectionChanged += Namespaces_CollectionChanged;
                            using (IEnumerator<NamespaceType> oldItems = oldValue.GetEnumerator())
                            {
                                if (oldItems.MoveNext())
                                    OnItemsRemoved(oldItems);
                            }
                            using (IEnumerator<NamespaceType> newItems = value.GetEnumerator())
                            {
                                if (newItems.MoveNext())
                                    OnItemsAdded(newItems);
                            }
                        }
                        RaisePropertyChanged(oldValue, value);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                }
            }

            protected void Namespaces_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            using (IEnumerator<NamespaceType> newItems = e.NewItems.OfType<NamespaceType>().GetEnumerator())
                            {
                                if (newItems.MoveNext())
                                    OnItemsAdded(newItems);
                            }
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            using (IEnumerator<NamespaceType> oldItems = e.NewItems.OfType<NamespaceType>().GetEnumerator())
                            {
                                if (oldItems.MoveNext())
                                    OnItemsRemoved(oldItems);
                            }
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            // TODO: Implement NotifyCollectionChangedAction.Replace
                            throw new NotImplementedException();
                        case NotifyCollectionChangedAction.Reset:
                            // TODO: Implement NotifyCollectionChangedAction.Reset
                            throw new NotImplementedException();
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            private void OnItemsAdded(IEnumerator<NamespaceType> enumerator)
            {
                try
                {
                    NamespaceType ns = enumerator.Current;
                    Monitor.Enter(ns.SyncRoot);
                    try
                    {
                        if (ns._owner is null)
                        {
                            ns.PropertyChanged += Namespace_PropertyChanged;
                            ns._owner = this;
                        }
                        else if (!ReferenceEquals(ns._owner, this))
                        {
                            if (ns._owner._namespaces is null)
                                ns._owner = null;
                            else
                                ns._owner._namespaces.Remove(ns);
                            if (ns._owner is null)
                            {
                                ns.PropertyChanged += Namespace_PropertyChanged;
                                ns._owner = this;
                            }
                        }
                        string name = ns._name;
                        if (_byName.TryGetValue(name, out Collection<NamespaceType> collection))
                        {
                            if (!collection.Contains(ns))
                            {
                                collection.Add(ns);
                                // TODO: Add name duplication error if collection.Count == 2
                            }
                        }
                        else
                        {
                            collection = new();
                            collection.Add(ns);
                            _byName.Add(name, collection);
                        }
                    }
                    finally { Monitor.Exit(ns.SyncRoot); }
                }
                finally
                {
                    if (enumerator.MoveNext())
                        OnItemsAdded(enumerator);
                }
            }

            private void OnItemsRemoved(IEnumerator<NamespaceType> enumerator)
            {
                try
                {
                    NamespaceType ns = enumerator.Current;
                    Monitor.Enter(ns.SyncRoot);
                    try
                    {
                        if (ns._owner is not null && ReferenceEquals(ns._owner, this) && (ns._owner._namespaces is null || !ns._owner._namespaces.Contains(ns)))
                            ns.PropertyChanged -= Namespace_PropertyChanged;
                        string name = ns._name;
                        if (_byName.TryGetValue(name, out Collection<NamespaceType> collection))
                        {
                            if (collection.Remove(ns))
                            {
                                switch (collection.Count)
                                {
                                    case 0:
                                        _byName.Remove(name);
                                        break;
                                    case 1:
                                        // TODO: Clear name duplication error
                                        break;
                                }
                            }
                        }
                    }
                    finally { Monitor.Exit(ns.SyncRoot); }
                }
                finally
                {
                    if (enumerator.MoveNext())
                        OnItemsRemoved(enumerator);
                }
            }

            private void Namespace_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (!(sender is NamespaceType ns && e is PropertyChangedEventArgs<string> args) || e.PropertyName != nameof(Name))
                    return;
                Monitor.Enter(SyncRoot);
                try
                {
                    Monitor.Enter(ns.SyncRoot);
                    try
                    {
                        if (ns._owner is null || !ReferenceEquals(ns._owner, this))
                            ns.PropertyChanged -= Namespace_PropertyChanged;
                        else
                        {
                            if (_byName.TryGetValue(args.OldValue, out Collection<NamespaceType> collection))
                            {
                                if (collection.Remove(ns))
                                    switch (collection.Count)
                                    {
                                        case 0:
                                            _byName.Remove(args.OldValue);
                                            break;
                                        case 1:
                                            // TODO: Clear name duplication error
                                            break;
                                    }
                            }
                            if (_byName.TryGetValue(ns._name, out collection))
                            {
                                if (!collection.Contains(ns))
                                {
                                    collection.Add(ns);
                                    // TODO: Add name duplication error if collection.Count == 2
                                }
                            }
                            else
                            {
                                collection = new();
                                collection.Add(ns);
                                _byName.Add(ns._name, collection);
                            }
                        }
                    }
                    finally { Monitor.Exit(ns.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }
    }
}
