using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class PropertyChangeNotifiedCollection<TComponent, TWrapper> : IList<TComponent>, ICollection<TComponent>,
        IEnumerable<TComponent>, IEnumerable, IList, ICollection, IReadOnlyList<TComponent>, IReadOnlyCollection<TComponent>,
        INotifyCollectionChanged, INotifyPropertyChanged
        where TComponent : class
        where TWrapper : INotifyComponentPropertyChangeRelay<TComponent>
    {
        private ObservableCollection<INotifyComponentPropertyChangeRelay<TComponent>> _backingCollection;

        public TComponent this[int index] { get => ((IList<TComponent>)_backingCollection)[index]; set => ((IList<TComponent>)_backingCollection)[index] = value; }
        object IList.this[int index] { get => ((IList)_backingCollection)[index]; set => ((IList)_backingCollection)[index] = value; }

        TComponent IReadOnlyList<TComponent>.this[int index] => ((IReadOnlyList<TComponent>)_backingCollection)[index];

        public int Count => ((ICollection<TComponent>)_backingCollection).Count;

        bool ICollection<TComponent>.IsReadOnly => throw new NotImplementedException();

        bool IList.IsReadOnly => throw new NotImplementedException();

        bool IList.IsFixedSize => false;

        protected object SyncRoot { get; } = new object();

        object ICollection.SyncRoot => SyncRoot;

        bool ICollection.IsSynchronized => true;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Add(TComponent item)
        {
            //_backingCollection.Add(item);
        }

        int IList.Add(object value)
        {
            return ((IList)_backingCollection).Add(value);
        }

        public void Clear()
        {
            ((ICollection<TComponent>)_backingCollection).Clear();
        }

        public bool Contains(TComponent item)
        {
            //return _backingCollection.Contains(item);
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            return ((IList)_backingCollection).Contains(value);
        }

        public void CopyTo(TComponent[] array, int arrayIndex)
        {
            //_backingCollection.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            //_backingCollection.CopyTo(array, index);
        }

        public IEnumerator<TComponent> GetEnumerator()
        {
            //return _backingCollection.GetEnumerator();
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_backingCollection).GetEnumerator();
        }

        public int IndexOf(TComponent item)
        {
            //return _backingCollection.IndexOf(item);
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, TComponent item)
        {
            //_backingCollection.Insert(index, item);
        }

        void IList.Insert(int index, object value)
        {
            ((IList)_backingCollection).Insert(index, value);
        }

        public bool Remove(TComponent item)
        {
            //return _backingCollection.Remove(item);
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            //((IList)_backingCollection).Remove(value);
        }

        public void RemoveAt(int index)
        {
            ((IList<TComponent>)_backingCollection).RemoveAt(index);
        }
    }
}
