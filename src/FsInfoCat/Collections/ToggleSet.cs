using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using FsInfoCat.DeferredDelegation;

namespace FsInfoCat.Collections
{
    // TODO: Document ToggleSet class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial class ToggleSet<T> : INotifyPropertyChanged, ISynchronizable
    {
        private readonly object _syncRoot = new();
        private Node _firstInSet;
        private Node _lastInSet;
        private int _count = 0;
        private readonly ICollection<Node> _trueAccessor;
        private readonly ICollection<Node> _falseAccessor;
        private readonly ICollection<Node> _indeterminateAccessor;

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IDeferredDelegationService _deferredDelegation;
        private readonly IEqualityComparer<T> _comparer;

        object ISynchronizable.SyncRoot => _syncRoot;

        public bool IsEmpty { get; private set; }

        public AllItems All { get; }

        public bool AnyTrue { get; private set; }

        public bool AllTrue { get; private set; }

        public StateSet True { get; }

        public bool AnyFalse { get; private set; }

        public bool AllFalse { get; private set; }

        public StateSet False { get; }

        public bool AnyIndeterminate { get; private set; }

        public bool AllIndeterminate { get; private set; }

        public StateSet Indeterminate { get; }

        public ToggleSet(IEqualityComparer<T> comparer = null)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            _deferredDelegation = serviceScope.ServiceProvider.GetRequiredService<IDeferredDelegationService>();
            _comparer = comparer ?? EqualityComparer<T>.Default;
            True = StateSet.Create(_deferredDelegation, _comparer, _syncRoot, out ICollection<Node> accessor);
            _trueAccessor = accessor;
            False = StateSet.Create(_deferredDelegation, _comparer, _syncRoot, out accessor);
            _falseAccessor = accessor;
            Indeterminate = StateSet.Create(_deferredDelegation, _comparer, _syncRoot, out accessor);
            _indeterminateAccessor = accessor;
            All = new(this);
        }

        private static IEnumerable<Node> GetNodesInSet(Node node)
        {
            while (node is not null)
            {
                node = node.NextInSet;
                yield return node;
            }
        }

        private static IEnumerable<Node> GetNodesOfState(Node node)
        {
            while (node is not null)
            {
                node = node.NextOfState;
                yield return node;
            }
        }

        private Node FindNode(T value) => GetNodesInSet(_firstInSet).FirstOrDefault(n => _comparer.Equals(n.Value, value));

        private bool TryFindNode(T value, out Node node, out int index)
        {
            index = 0;
            foreach (Node n in GetNodesInSet(_firstInSet))
            {
                if (_comparer.Equals(n.Value, value))
                {
                    node = n;
                    return true;
                }
                index++;
            }
            node = null;
            return false;
        }

        public void Add(T value, bool? state)
        {
            IDelegateDeference<ToggleSet<T>> delegateDeference = _deferredDelegation.EnterSynchronizable(this);
            Node.Add(this, value, state, delegateDeference);
        }

        public bool Set(T value, bool? state)
        {
            IDelegateDeference<ToggleSet<T>> delegateDeference = _deferredDelegation.EnterSynchronizable(this);
            return Node.Set(this, value, state, delegateDeference);
        }

        public bool Remove(T value)
        {
            IDelegateDeference<ToggleSet<T>> delegateDeference = _deferredDelegation.EnterSynchronizable(this);
            return Node.Remove(this, value, delegateDeference);
        }

        public void Clear() => Node.Clear(this);

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new(propertyName));
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
