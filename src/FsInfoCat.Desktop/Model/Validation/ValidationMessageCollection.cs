using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class ValidationMessageCollection : IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable, IList, ICollection, IReadOnlyList<string>, IReadOnlyCollection<string>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly ObservableCollection<ValidationResult> _backingList;

        public string this[int index] => _backingList[index].ErrorMessage;

        public int Count => _backingList.Count;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public ValidationMessageCollection(ObservableCollection<ValidationResult> backingList)
        {
            (_backingList = backingList ?? throw new ArgumentNullException(nameof(backingList))).CollectionChanged += BackingList_CollectionChanged;
            ((INotifyPropertyChanged)backingList).PropertyChanged += BackingList_PropertyChanged;
        }

        private void BackingList_PropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        private void BackingList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IList newItems = (e.NewItems is null) ? null : e.NewItems.Cast<object>().Select(v => (v is ValidationResult r) ? r.ErrorMessage : null).ToArray();
            IList oldItems = (e.OldItems is null) ? null : e.OldItems.Cast<object>().Select(v => (v is ValidationResult r) ? r.ErrorMessage : null).ToArray();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 0)
                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems);
                    else
                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, newItems, e.NewStartingIndex, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 0)
                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems);
                    else
                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewStartingIndex < 0)
                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems);
                    else
                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
            CollectionChanged?.Invoke(this, e);
        }

        private IEnumerable<string> GetMessages() => _backingList.Select(v => v.ErrorMessage);

        public bool Contains(string item) => GetMessages().Contains(item);

        public void CopyTo(string[] array, int arrayIndex) => GetMessages().ToList().CopyTo(array, arrayIndex);

        public IEnumerator<string> GetEnumerator() => GetMessages().GetEnumerator();

        public int IndexOf(string item)
        {
            int index = -1;
            if (item is null)
                foreach (string message in GetMessages())
                {
                    ++index;
                    if (message is null)
                        return index;
                }
            else
                foreach (string message in GetMessages())
                {
                    ++index;
                    if (item.Equals(message))
                        return index;
                }
            return -1;
        }

        #region Explicit Members

        string IList<string>.this[int index] { get => _backingList[index].ErrorMessage; set => throw new NotSupportedException(); }

        object IList.this[int index] { get => _backingList[index].ErrorMessage; set => throw new NotSupportedException(); }

        bool ICollection<string>.IsReadOnly => true;

        bool IList.IsReadOnly => true;

        bool IList.IsFixedSize => false;

        object ICollection.SyncRoot => ((ICollection)_backingList).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)_backingList).IsSynchronized;

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetMessages()).GetEnumerator();

        void IList<string>.Insert(int index, string item) => throw new NotSupportedException();

        void IList<string>.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection<string>.Add(string item) => throw new NotSupportedException();

        void ICollection<string>.Clear() => throw new NotSupportedException();

        bool ICollection<string>.Remove(string item) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        bool IList.Contains(object value) => (value is null) ? Contains(null) : (value is string s && Contains(s));

        void IList.Clear() => throw new NotSupportedException();

        int IList.IndexOf(object value) => (value is null) ? IndexOf(null) : ((value is string s) ? IndexOf(s) : -1);

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index) => GetMessages().ToArray().CopyTo(array, index);

        #endregion
    }
}
