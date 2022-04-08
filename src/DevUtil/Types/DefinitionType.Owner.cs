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
    public abstract partial class DefinitionType
    {
        public class Owner : NotifyPropertyChange
        {
            private readonly Dictionary<string, Collection<DefinitionType>> _byName = new();
            private ObservableCollection<DefinitionType> _types;

            protected object SyncRoot { get; } = new object();


            [XmlArrayItem(ElementName = EntityType.RootElementName, Type = typeof(EntityType))]
            [XmlArrayItem(ElementName = EnumType.RootElementName, Type = typeof(EnumType))]
            [XmlArrayItem(ElementName = InterfaceType.RootElementName, Type = typeof(InterfaceType))]
            [XmlArrayItem(ElementName = StructType.RootElementName, Type = typeof(StructType))]
            [XmlArrayItem(ElementName = ClassType.RootElementName, Type = typeof(ClassType))]
            public ObservableCollection<DefinitionType> Types
            {
                get => _types;
                set
                {
                    ObservableCollection<DefinitionType> oldValue;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if ((oldValue = _types) is null)
                        {
                            if (value is null)
                                return;
                            RaisePropertyChanging(oldValue, value);
                            _types = value;
                            value.CollectionChanged += Namespaces_CollectionChanged;
                            using (IEnumerator<DefinitionType> newItems = value.GetEnumerator())
                            {
                                if (newItems.MoveNext())
                                    OnItemsAdded(newItems);
                            }
                        }
                        else if (value is null)
                        {
                            RaisePropertyChanging(oldValue, value);
                            _types.CollectionChanged -= Namespaces_CollectionChanged;
                            _types = value;
                            using (IEnumerator<DefinitionType> oldItems = oldValue.GetEnumerator())
                            {
                                if (oldItems.MoveNext())
                                    OnItemsRemoved(oldItems);
                            }
                        }
                        else
                        {
                            if (ReferenceEquals(_types, value))
                                return;

                            RaisePropertyChanging(oldValue, value);
                            oldValue.CollectionChanged -= Namespaces_CollectionChanged;
                            _types = value;
                            value.CollectionChanged += Namespaces_CollectionChanged;
                            using (IEnumerator<DefinitionType> oldItems = oldValue.GetEnumerator())
                            {
                                if (oldItems.MoveNext())
                                    OnItemsRemoved(oldItems);
                            }
                            using (IEnumerator<DefinitionType> newItems = value.GetEnumerator())
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
                            using (IEnumerator<DefinitionType> newItems = e.NewItems.OfType<DefinitionType>().GetEnumerator())
                            {
                                if (newItems.MoveNext())
                                    OnItemsAdded(newItems);
                            }
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            using (IEnumerator<DefinitionType> oldItems = e.NewItems.OfType<DefinitionType>().GetEnumerator())
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

            private void OnItemsAdded(IEnumerator<DefinitionType> enumerator)
            {
                try
                {
                    DefinitionType dt = enumerator.Current;
                    Monitor.Enter(dt.SyncRoot);
                    try
                    {
                        if (dt._owner is null)
                        {
                            dt.PropertyChanged += DefinitionType_PropertyChanged;
                            dt._owner = this;
                        }
                        else if (!ReferenceEquals(dt._owner, this))
                        {
                            if (dt._owner._types is null)
                                dt._owner = null;
                            else
                                dt._owner._types.Remove(dt);
                            if (dt._owner is null)
                            {
                                dt.PropertyChanged += DefinitionType_PropertyChanged;
                                dt._owner = this;
                            }
                        }
                        string name = dt._name;
                        if (_byName.TryGetValue(name, out Collection<DefinitionType> collection))
                        {
                            if (!collection.Contains(dt))
                            {
                                collection.Add(dt);
                                // TODO: Add name duplication error if collection.Count == 2
                            }
                        }
                        else
                        {
                            collection = new();
                            collection.Add(dt);
                            _byName.Add(name, collection);
                        }
                    }
                    finally { Monitor.Exit(dt.SyncRoot); }
                }
                finally
                {
                    if (enumerator.MoveNext())
                        OnItemsAdded(enumerator);
                }
            }

            private void OnItemsRemoved(IEnumerator<DefinitionType> enumerator)
            {
                try
                {
                    DefinitionType dt = enumerator.Current;
                    Monitor.Enter(dt.SyncRoot);
                    try
                    {
                        if (dt._owner is not null && ReferenceEquals(dt._owner, this) && (dt._owner._types is null || !dt._owner._types.Contains(dt)))
                            dt.PropertyChanged -= DefinitionType_PropertyChanged;
                        string name = dt._name;
                        if (_byName.TryGetValue(name, out Collection<DefinitionType> collection))
                        {
                            if (collection.Remove(dt))
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
                    finally { Monitor.Exit(dt.SyncRoot); }
                }
                finally
                {
                    if (enumerator.MoveNext())
                        OnItemsRemoved(enumerator);
                }
            }

            private void DefinitionType_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (!(sender is DefinitionType dt && e is PropertyChangedEventArgs<string> args) || e.PropertyName != nameof(Name))
                    return;
                Monitor.Enter(SyncRoot);
                try
                {
                    Monitor.Enter(dt.SyncRoot);
                    try
                    {
                        if (dt._owner is null || !ReferenceEquals(dt._owner, this))
                            dt.PropertyChanged -= DefinitionType_PropertyChanged;
                        else
                        {
                            if (_byName.TryGetValue(args.OldValue, out Collection<DefinitionType> collection))
                            {
                                if (collection.Remove(dt))
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
                            if (_byName.TryGetValue(dt._name, out collection))
                            {
                                if (!collection.Contains(dt))
                                {
                                    collection.Add(dt);
                                    // TODO: Add name duplication error if collection.Count == 2
                                }
                            }
                            else
                            {
                                collection = new();
                                collection.Add(dt);
                                _byName.Add(dt._name, collection);
                            }
                        }
                    }
                    finally { Monitor.Exit(dt.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }
    }
}
