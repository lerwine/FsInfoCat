using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace FsInfoCat.ComponentSupport
{
    public sealed class ValidationResultCollection : ICollection<ValidationResult>, IEnumerable<ValidationResult>, IEnumerable, IList<ValidationResult>, ICollection,
        IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly object _syncRoot = new object();
        private IEventSuspensionManager _eventSuspension = Services.GetSuspendableService().NewEventSuspensionManager();
        private Node _first;
        private Node _last;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasErrors { get; private set; }

        public ValidationResult this[int index]
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try { throw new NotImplementedException(); }
                finally { Monitor.Exit(_syncRoot); }
            }
            set
            {
                Monitor.Enter(_syncRoot);
                try { throw new NotImplementedException(); }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count { get; private set; }

        bool ICollection<ValidationResult>.IsReadOnly => throw new NotImplementedException();

        bool IList.IsReadOnly => throw new NotImplementedException();

        bool IList.IsFixedSize => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        public ValidationResultCollection()
        {
            _eventSuspension.CollectionChanged += EventSuspension_CollectionChanged;
        }

        private void EventSuspension_CollectionChanged(object sender, NotifyCollectionChangedEventArgs<IEventItem> e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (IEventItem item in _eventSuspension.DequeueAll().ToArray())
                {
                    if (item.Args is PropertyChangedEventArgs propertyChangedEventArgs)
                        PropertyChanged?.Invoke(this, propertyChangedEventArgs);
                    else if (item.Args is NotifyCollectionChangedEventArgs collectionChangedEventArgs)
                        CollectionChanged?.Invoke(this, collectionChangedEventArgs);
                }
        }

        public int Add(ValidationResult item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            Node existing;
            int index;
            using (ISuspension eventSuspension = _eventSuspension.Suspend(true))
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if ((existing = _last) is null || (existing = FindBestMatch(_first, item, out int oldIndex)) is null || !ReferenceEquals(existing.Value, item))
                    {
                        index = Count;
                        InsertNode(null, new Node(item));
                        _eventSuspension.Enqueue(new NotifyCollectionChangedEventItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index)));
                        _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(Count))));
                        if (index == 0)
                            _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(HasErrors))));
                    }
                    else
                    {
                        index = Count = 1;
                        if (!ReferenceEquals(_last.Value, item))
                        {
                            RemoveNode(existing);
                            InsertNode(null, existing);
                            _eventSuspension.Enqueue(new NotifyCollectionChangedEventItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, index, oldIndex)));
                        }
                        existing = null;
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
                if (!(existing is null))
                {
                    index--;
                    Remove(existing);
                }
            }
            return index;
        }

        internal void SetSingle(ValidationResult validationResult)
        {
            throw new NotImplementedException();
        }

        int IList.Add(object value) => Add((ValidationResult)value);

        void ICollection<ValidationResult>.Add(ValidationResult item) => Add(item);

        public void Clear()
        {
            using (ISuspension eventSuspension = _eventSuspension.Suspend(true))
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (!(_first is null))
                    {
                        _eventSuspension.Enqueue(new NotifyCollectionChangedEventItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, GetItems().ToArray(), 0)));
                        _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(Count))));
                        _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(HasErrors))));
                        for (Node node = _first.Next; !(node is null); node = node.Next)
                            node.Previous = node.Previous.Next = null;
                        _first = _last = null;
                        Count = 0;
                        HasErrors = false;
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public bool Contains(ValidationResult item)
        {
            Monitor.Enter(_syncRoot);
            try { throw new NotImplementedException(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        bool IList.Contains(object value) => value is ValidationResult item && Contains(item);

        public void CopyTo(ValidationResult[] array, int arrayIndex) => GetItems().ToList().CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => GetItems().ToArray().CopyTo(array, index);

        private static Node FindBestMatch(Node startNode, ValidationResult item, out int offset)
        {
            offset = -1;
            if (startNode is null)
                return null;
            offset = 0;
            if (ReferenceEquals(startNode.Value, item))
                return startNode;
            string errorMessage = GetNormalized(item, out string[] memberNames);
            Node node;
            if (IsEqualTo(startNode.Value, errorMessage, memberNames))
            {
                if ((node = FindSame(startNode.Next, item, out offset)) is null)
                {
                    offset = 0;
                    return startNode;
                }
                offset++;
                return node;
            }
            for (node = startNode.Next; !(node is null); node = node.Next)
            {
                offset++;
                if (ReferenceEquals(node.Value, item))
                    return node;
                if (IsEqualTo(node.Value, errorMessage, memberNames))
                {
                    if ((startNode = FindSame(node.Next, item, out int index)) is null)
                        return node;
                    offset += index;
                    return startNode;
                }
            }
            offset = -1;
            return null;
        }

        private static Node FindSame(Node startNode, ValidationResult item, out int offset)
        {
            offset = -1;
            for (Node node = startNode; !(node is null); node = node.Next)
            {
                offset++;
                if (ReferenceEquals(node.Value, item))
                    return node;
            }
            offset = -1;
            return null;
        }

        public IEnumerator<ValidationResult> GetEnumerator() => GetItems().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetItems()).GetEnumerator();

        private IEnumerable<ValidationResult> GetItems()
        {
            for (Node node = _first; !(node is null); node = node.Next)
                yield return node.Value;
        }

        private Node GetNodeAt(int index)
        {
            if (index < 0 || index >= Count)
                return null;
            if (index == 0)
                return _first;
            Node result;
            if (index > Count >> 1)
            {
                result = _last;
                while (++index < Count)
                    result = result.Previous;
            }
            else
            {
                result = _first.Next;
                while (--index > 0)
                    result = result.Next;
            }
            return result;
        }

        private static string GetNormalized(ValidationResult item, out string[] memberNames)
        {
            memberNames = (item.MemberNames is null) ? Array.Empty<string>() : item.MemberNames.Select(s => s ?? "").ToArray();
            return item.ErrorMessage ?? "";
        }

        private static bool IsEqualTo(ValidationResult item, string message, string[] memberNames) => GetNormalized(item, out string[] m).Equals(message) && m.SequenceEqual(memberNames);

        public int IndexOf(ValidationResult item)
        {
            if (item is null)
                return -1;
            Monitor.Enter(_syncRoot);
            try
            {
                if (!(FindBestMatch(_first, item, out int index) is null))
                    return index;
            }
            finally { Monitor.Exit(_syncRoot); }
            return -1;
        }

        int IList.IndexOf(object value) => (value is ValidationResult item) ? IndexOf(item) : -1;

        public void Insert(int index, ValidationResult item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            Node existing;
            using (ISuspension eventSuspension = _eventSuspension.Suspend(true))
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    Node insertBefore = GetNodeAt(index);
                    if (insertBefore is null)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    existing = FindBestMatch(_first, item, out int oldIndex);
                    if (existing is null || !ReferenceEquals(existing.Value, item))
                    {
                        InsertNode(insertBefore, new Node(item));
                        _eventSuspension.Enqueue(new NotifyCollectionChangedEventItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index)));
                        _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(Count))));
                    }
                    else
                    {
                        if (oldIndex != index)
                        {
                            RemoveNode(existing);
                            InsertNode(insertBefore, existing);
                            _eventSuspension.Enqueue(new NotifyCollectionChangedEventItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, oldIndex)));
                        }
                        existing = null;
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
                if (!(existing is null))
                    Remove(existing);
            }
        }

        internal void SetItems(Collection<ValidationResult> validationResults)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value) => Insert(index, (ValidationResult)value);

        private bool InsertNode(Node referenceNode, Node newNode)
        {
            if ((newNode.Next = referenceNode) is null)
            {
                if ((newNode.Previous = _last) is null)
                {
                    _first = _last = newNode;
                    Count = 1;
                    return true;
                }
                else
                    _last = _last.Next = newNode;
            }
            else if ((newNode.Previous = referenceNode.Previous) is null)
                _first = referenceNode.Previous = newNode;
            else
                newNode.Previous.Next = referenceNode.Previous = newNode;
            Count++;
            return false;
        }

        private void Remove(Node node)
        {
            using (ISuspension eventSuspension = _eventSuspension.Suspend(true))
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (!(node.Previous is null && node.Next is null) || (ReferenceEquals(node, _first) && ReferenceEquals(node, _last)))
                    {
                        int index = 0;
                        for (Node n = node.Previous; !(n is null); n = n.Previous)
                            index++;
                        RemoveNode(node);
                        _eventSuspension.Enqueue(new NotifyCollectionChangedEventItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, node.Value, index)));
                        _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(Count))));
                        if (_first is null)
                            _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(HasErrors))));
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public bool Remove(ValidationResult item)
        {
            if (item is null)
                return false;
            using (ISuspension eventSuspension = _eventSuspension.Suspend(true))
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    Node node = FindBestMatch(_first, item, out int index);
                    if (node is null)
                        return false;
                    RemoveNode(node);
                    _eventSuspension.Enqueue(new NotifyCollectionChangedEventItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, node.Value, index)));
                    _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(Count))));
                    if (_first is null)
                        _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(HasErrors))));
                }
                finally { Monitor.Exit(_syncRoot); }
            }
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            using (ISuspension eventSuspension = _eventSuspension.Suspend(true))
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    Node node = GetNodeAt(index);
                    if (node is null)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    RemoveNode(node);
                    _eventSuspension.Enqueue(new NotifyCollectionChangedEventItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, node.Value, index)));
                    _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(Count))));
                    if (_first is null)
                        _eventSuspension.Enqueue(new PropertyChangedEventItem(new PropertyChangedEventArgs(nameof(HasErrors))));
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        void IList.Remove(object value)
        {
            if (value is ValidationResult item)
                Remove(item);
        }

        private bool RemoveNode(Node node)
        {
            if (node.Previous is null)
            {
                if (node.Next is null)
                {
                    if (!(ReferenceEquals(node, _first) && ReferenceEquals(node, _last)))
                        return false;
                    _first = _last = null;
                    Count = 0;
                    return true;
                }
                node.Next = (_first = node.Next).Previous = null;
            }
            else if ((node.Previous.Next = node.Next) is null)
                node.Previous = (_last = node.Previous).Next = null;
            else
            {
                node.Next.Previous = node.Previous;
                node.Next = node.Previous = null;
            }
            Count--;
            return false;
        }

        private class Node
        {
            internal Node Previous { get; set; }
            internal Node Next { get; set; }
            internal ValidationResult Value { get; }
            internal Node(ValidationResult value) { Value = value; }
        }

        public class NotifyCollectionChangedEventItem : IEventItem<NotifyCollectionChangedEventArgs>
        {
            public NotifyCollectionChangedEventArgs Args { get; }

            public string EventName { get; } = nameof(INotifyCollectionChanged.CollectionChanged);

            EventArgs IEventItem.Args => Args;


            public NotifyCollectionChangedEventItem(NotifyCollectionChangedEventArgs args)
            {
                Args = args ?? throw new ArgumentNullException(nameof(args));
            }

            private bool IsValidArgItems(IList items)
            {
                if (items is null || items.Count == 0)
                    return true;
                return items.OfType<ValidationResult>().Count().Equals(items.Count);
            }

            public bool Equals(IEventItem other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                if (!(other.Args is NotifyCollectionChangedEventArgs args && EventName.Equals(other.EventName)))
                    return false;
                if (ReferenceEquals(args, Args))
                    return true;
                if (args.Action != Args.Action || args.OldStartingIndex != Args.OldStartingIndex || args.NewStartingIndex != Args.NewStartingIndex)
                    return false;
                if (args.OldItems is null || args.OldItems.Count == 0)
                {
                    if (!(Args.OldItems is null || Args.OldItems.Count == 0))
                        return false;
                }
                else
                {
                    if (Args.OldItems is null || !ReferenceEquals(args.OldItems, args.OldItems))
                        return false;
                }
                if (args.NewItems is null || args.NewItems.Count == 0)
                    return Args.NewItems is null || Args.NewItems.Count == 0;
                return ReferenceEquals(args.NewItems, Args.NewItems);
            }
        }

        public class PropertyChangedEventItem : IEventItem<PropertyChangedEventArgs>
        {
            public PropertyChangedEventArgs Args { get; }

            EventArgs IEventItem.Args => Args;

            public string EventName { get; } = nameof(INotifyPropertyChanged.PropertyChanged);

            public PropertyChangedEventItem(PropertyChangedEventArgs args)
            {
                if (string.IsNullOrWhiteSpace((args ?? throw new ArgumentNullException(nameof(args))).PropertyName))
                    throw new ArgumentOutOfRangeException(nameof(args), "Property name cannot be null or whitespace.");
                Args = args;
            }

            public bool Equals(IEventItem other) => !(other is null) && (ReferenceEquals(this, other) ||
                (other.Args is PropertyChangedEventArgs args && EventName.Equals(other.EventName) && Args.PropertyName.Equals(args.PropertyName)));
        }
    }
}
