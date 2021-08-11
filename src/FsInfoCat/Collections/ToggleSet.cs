using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Collections
{
    public class ToggleSet<T> : INotifyPropertyChanged, ISynchronizable
    {
        private readonly object _syncRoot = new();
        private Node _firstInSet;
        private Node _lastInSet;
        private int _count = 0;
        private readonly ICollection<Node> _trueAccessor;
        private readonly ICollection<Node> _falseAccessor;
        private readonly ICollection<Node> _indeterminateAccessor;

        public event PropertyChangedEventHandler PropertyChanged;

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
            _comparer = comparer ?? EqualityComparer<T>.Default;
            True = StateSet.Create(_comparer, _syncRoot, out ICollection<Node> accessor);
            _trueAccessor = accessor;
            False = StateSet.Create(_comparer, _syncRoot, out accessor);
            _falseAccessor = accessor;
            Indeterminate = StateSet.Create(_comparer, _syncRoot, out accessor);
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
            Action[] actions;
            using (MonitorEntered<ToggleSet<T>> monitorEntered = MonitorEntered.TakeSynchronizableLock(this))
                actions = Node.Add(this, value, state).ToArray();
            foreach (Action a in actions)
                a();
        }

        public bool Set(T value, bool? state)
        {
            Action[] actions;
            using (MonitorEntered<ToggleSet<T>> monitorEntered = MonitorEntered.TakeSynchronizableLock(this))
                actions = Node.Set(this, value, state).ToArray();
            if (actions.Length == 0)
                return false;
            foreach (Action a in actions)
                a();
            return true;
        }

        public bool Remove(T value)
        {
            Action[] actions;
            using (MonitorEntered<ToggleSet<T>> monitorEntered = MonitorEntered.TakeSynchronizableLock(this))
                actions = Node.Remove(this, value).ToArray();
            if (actions.Length == 0)
                return false;
            foreach (Action a in actions)
                a();
            return true;
        }

        public void Clear() => Node.Clear(this);

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new(propertyName));

        public class Node : INotifyPropertyChanged
        {
            private ToggleSet<T> _owner;
            private bool? _state;

            private Node(ToggleSet<T> owner, T value, bool? state)
            {
                _owner = owner;
                Value = value;
            }

            internal Node PreviousInSet { get; private set; }

            internal Node NextInSet { get; private set; }

            internal Node PreviousOfState { get; private set; }

            internal Node NextOfState { get; private set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public T Value { get; }

            public bool? State
            {
                get => _state;
                set
                {
                    if (!MonitorEntered.TakeSynchronizableLockIfNotNull(_owner, out MonitorEntered<ToggleSet<T>> monitorEntered))
                    {
                        _state = value;
                        RaisePropertyChanged(nameof(State));
                        return;
                    }
                    using (monitorEntered)
                    {
                        if (value.HasValue)
                        {
                            if (_state.HasValue)
                            {
                                if (value.Value)
                                {
                                    monitorEntered.Target._trueAccessor.Add(this);
                                    monitorEntered.Target._falseAccessor.Remove(this);
                                    // False to True
                                }
                                else
                                {
                                    // True to False
                                }
                            }
                            else
                            {
                                if (value.Value)
                                {
                                    // Indeterminate to True
                                }
                                else
                                {
                                    // Indeterminate to False
                                }
                            }
                        }
                        else if (_state.HasValue)
                        {
                            if (_state.Value)
                            {
                                // True to Indeterminate
                            }
                            else
                            {
                                // False to Indeterminate
                            }
                        }
                    }
                }
            }

            //private IEnumerable<Action> ChangeState(bool? value)
            //{
            //    if (value.HasValue)
            //    {
            //        if (_state.HasValue)
            //        {
            //            if (value.Value == _state.Value)
            //                yield break;
            //            int oldIndex = GetIndexOfState();
            //            if (value.Value)
            //            {
            //                _owner._falseAccessor.Remove(this);
            //                int newIndex = _owner._trueAccessor.Count;
            //                _owner._trueAccessor.Add(this);
            //                yield return () =>
            //                {
            //                    _owner.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
            //                    _owner.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
            //                    _owner.False.RaiseCountChanged();
            //                    _owner.True.RaiseCountChanged();
            //                };
            //                if (_owner.False.Count == 0)
            //                {
            //                    _owner.AnyFalse = false;
            //                    if (_owner.AllFalse)
            //                    {
            //                        _owner.AllFalse = false;
            //                        _owner.AnyTrue = true;
            //                        _owner.AllTrue = true;
            //                        yield return () =>
            //                        {
            //                            _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AllFalse));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AllTrue));
            //                        };
            //                    }
            //                    else
            //                    {
            //                        yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                        if (_owner.AnyTrue)
            //                        {
            //                            if (!(_owner.AnyIndeterminate || _owner.AllTrue))
            //                            {
            //                                _owner.AllTrue = true;
            //                                yield return () => _owner.RaisePropertyChanged(nameof(_owner.AllTrue));
            //                            }
            //                        }
            //                        else
            //                        {
            //                            _owner.AnyTrue = true;
            //                            if (_owner.AnyIndeterminate)
            //                                yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                            else
            //                                yield return () =>
            //                                {
            //                                    _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                                    _owner.RaisePropertyChanged(nameof(_owner.AllTrue));
            //                                };
            //                        }
            //                    }
            //                }
            //                else if (!_owner.AnyTrue)
            //                {
            //                    _owner.AnyTrue = true;
            //                    yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                }
            //            }
            //            else
            //            {
            //                _owner._trueAccessor.Remove(this);
            //                int newIndex = _owner._falseAccessor.Count;
            //                _owner._falseAccessor.Add(this);
            //                yield return () =>
            //                {
            //                    _owner.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
            //                    _owner.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
            //                    _owner.True.RaiseCountChanged();
            //                    _owner.False.RaiseCountChanged();
            //                };
            //                if (_owner.True.Count == 0)
            //                {
            //                    _owner.AnyTrue = false;
            //                    if (_owner.AllTrue)
            //                    {
            //                        _owner.AllTrue = false;
            //                        _owner.AnyFalse = true;
            //                        _owner.AllFalse = true;
            //                        yield return () =>
            //                        {
            //                            _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AllTrue));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AllFalse));
            //                        };
            //                    }
            //                    else
            //                    {
            //                        yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                        if (_owner.AnyFalse)
            //                        {
            //                            if (!(_owner.AnyIndeterminate || _owner.AllFalse))
            //                            {
            //                                _owner.AllFalse = true;
            //                                yield return () => _owner.RaisePropertyChanged(nameof(_owner.AllFalse));
            //                            }
            //                        }
            //                        else
            //                        {
            //                            _owner.AnyFalse = true;
            //                            if (_owner.AnyIndeterminate)
            //                                yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                            else
            //                                yield return () =>
            //                                {
            //                                    _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                                    _owner.RaisePropertyChanged(nameof(_owner.AllFalse));
            //                                };
            //                        }
            //                    }
            //                }
            //                else if (!_owner.AnyFalse)
            //                {
            //                    _owner.AnyFalse = true;
            //                    yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                }
            //            }
            //        }
            //        else
            //        {
            //            int oldIndex = GetIndexOfState();
            //            _owner._indeterminateAccessor.Remove(this);
            //            if (value.Value)
            //            {
            //                int newIndex = _owner._trueAccessor.Count;
            //                _owner._trueAccessor.Add(this);
            //                yield return () =>
            //                {
            //                    _owner.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
            //                    _owner.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
            //                    _owner.Indeterminate.RaiseCountChanged();
            //                    _owner.True.RaiseCountChanged();
            //                };
            //                if (_owner.Indeterminate.Count == 0)
            //                {
            //                    _owner.AnyIndeterminate = false;
            //                    if (_owner.AllIndeterminate)
            //                    {
            //                        _owner.AllIndeterminate = false;
            //                        _owner.AnyTrue = true;
            //                        _owner.AllTrue = true;
            //                        yield return () =>
            //                        {
            //                            _owner.RaisePropertyChanged(nameof(_owner.AnyIndeterminate));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AllIndeterminate));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AllTrue));
            //                        };
            //                    }
            //                    else
            //                    {
            //                        yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyIndeterminate));
            //                        if (_owner.AnyTrue)
            //                        {
            //                            if (!(_owner.AnyFalse || _owner.AllTrue))
            //                            {
            //                                _owner.AllTrue = true;
            //                                yield return () => _owner.RaisePropertyChanged(nameof(_owner.AllTrue));
            //                            }
            //                        }
            //                        else
            //                        {
            //                            _owner.AnyTrue = true;
            //                            if (_owner.AnyFalse)
            //                                yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                            else
            //                                yield return () =>
            //                                {
            //                                    _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                                    _owner.RaisePropertyChanged(nameof(_owner.AllTrue));
            //                                };
            //                        }
            //                    }
            //                }
            //                else if (!_owner.AnyTrue)
            //                {
            //                    _owner.AnyTrue = true;
            //                    yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyTrue));
            //                }
            //            }
            //            else
            //            {
            //                int newIndex = _owner._falseAccessor.Count;
            //                _owner._falseAccessor.Add(this);
            //                yield return () =>
            //                {
            //                    _owner.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
            //                    _owner.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
            //                    _owner.Indeterminate.RaiseCountChanged();
            //                    _owner.False.RaiseCountChanged();
            //                };
            //                if (_owner.Indeterminate.Count == 0)
            //                {
            //                    _owner.AnyIndeterminate = false;
            //                    if (_owner.AllIndeterminate)
            //                    {
            //                        _owner.AllIndeterminate = false;
            //                        _owner.AnyFalse = true;
            //                        _owner.AllFalse = true;
            //                        yield return () =>
            //                        {
            //                            _owner.RaisePropertyChanged(nameof(_owner.AnyIndeterminate));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AllIndeterminate));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                            _owner.RaisePropertyChanged(nameof(_owner.AllFalse));
            //                        };
            //                    }
            //                    else
            //                    {
            //                        yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyIndeterminate));
            //                        if (_owner.AnyFalse)
            //                        {
            //                            if (!(_owner.AnyTrue || _owner.AllFalse))
            //                            {
            //                                _owner.AllFalse = true;
            //                                yield return () => _owner.RaisePropertyChanged(nameof(_owner.AllFalse));
            //                            }
            //                        }
            //                        else
            //                        {
            //                            _owner.AnyFalse = true;
            //                            if (_owner.AnyTrue)
            //                                yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                            else
            //                                yield return () =>
            //                                {
            //                                    _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                                    _owner.RaisePropertyChanged(nameof(_owner.AllFalse));
            //                                };
            //                        }
            //                    }
            //                }
            //                else if (!_owner.AnyFalse)
            //                {
            //                    _owner.AnyFalse = true;
            //                    yield return () => _owner.RaisePropertyChanged(nameof(_owner.AnyFalse));
            //                }
            //            }
            //        }
            //    }
            //    else if (_state.HasValue)
            //    {
            //        int oldIndex = GetIndexOfState();
            //        int newIndex = _owner._indeterminateAccessor.Count;
            //        _owner._indeterminateAccessor.Add(this);
            //        yield return () =>
            //        {
            //            _owner.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
            //            _owner.Indeterminate.RaiseCountChanged();
            //        };
            //        // TODO: Emit actions
            //        if (_state.Value)
            //        {
            //            _owner._trueAccessor.Remove(this);
            //            yield return () =>
            //            {
            //                _owner.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
            //                _owner.True.RaiseCountChanged();
            //            };
            //        }
            //        else
            //        {
            //            _owner._falseAccessor.Remove(this);
            //            yield return () =>
            //            {
            //                _owner.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
            //                _owner.False.RaiseCountChanged();
            //            };
            //        }

            //    }
            //    RaisePropertyChanged(nameof(State));
            //}

            private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new(propertyName));

            private int GetIndexInSet()
            {
                if (_owner is null)
                    return -1;
                if (NextInSet is null)
                    return _owner._count - 1;
                int index = 0;
                for (Node n = PreviousInSet; n is not null; n = n.PreviousInSet)
                    index++;
                return index;
            }

            private int GetIndexOfState()
            {
                if (_owner is null)
                    return -1;
                if (NextOfState is null)
                    return (State.HasValue ? (State.Value ? _owner.True : _owner.False) : _owner.Indeterminate).Count - 1;
                int index = 0;
                for (Node n = PreviousOfState; n is not null; n = n.PreviousOfState)
                    index++;
                return index;
            }
            
            internal static IEnumerable<Action> Add(ToggleSet<T> target, T value, bool? state)
            {
                if (target._lastInSet is null)
                {
                    target.IsEmpty = false;
                    Node node = new(target, value, state);
                    target._firstInSet = target._lastInSet = node;
                    target._count = 1;
                    yield return () =>
                    {
                        target.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                        target.All.RaiseCountChanged();
                        target.RaisePropertyChanged(nameof(target.IsEmpty));
                    };
                    if (state.HasValue)
                    {
                        if (state.Value)
                        {
                            target._trueAccessor.Add(node);
                            target.AnyTrue = target.AllTrue = true;
                            yield return () =>
                            {
                                target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                                target.True.RaiseCountChanged();
                                target.RaisePropertyChanged(nameof(target.AnyTrue));
                                target.RaisePropertyChanged(nameof(target.AllTrue));
                            };
                        }
                        else
                        {
                            target._falseAccessor.Add(node);
                            target.AnyFalse = target.AllFalse = true;
                            yield return () =>
                            {
                                target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                                target.False.RaiseCountChanged();
                                target.RaisePropertyChanged(nameof(target.AnyFalse));
                                target.RaisePropertyChanged(nameof(target.AllFalse));
                            };
                        }
                    }
                    else
                    {
                        target._indeterminateAccessor.Add(node);
                        target.AnyIndeterminate = target.AllIndeterminate = true;
                        yield return () =>
                        {
                            target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                            target.Indeterminate.RaiseCountChanged();
                            target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                            target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                        };
                    }
                }
                else
                {
                    if (target.FindNode(value) is not null)
                        throw new InvalidOperationException();
                    Node node = new(target, value, state);
                    target._lastInSet = (node.PreviousInSet = target._lastInSet).NextInSet = node;
                    int indexInSet = target._count++;
                    yield return () =>
                    {
                        target.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexInSet));
                        target.All.RaiseCountChanged();
                    };
                    if (state.HasValue)
                    {
                        if (state.Value)
                        {
                            int indexOfState = target._trueAccessor.Count;
                            target._trueAccessor.Add(node);
                            yield return () =>
                            {
                                target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                                target.True.RaiseCountChanged();
                            };
                            if (!target.AnyTrue)
                            {
                                target.AnyTrue = true;
                                yield return () => target.RaisePropertyChanged(nameof(target.AnyTrue));
                            }
                            if (target.AllFalse)
                            {
                                target.AllFalse = false;
                                yield return () => target.RaisePropertyChanged(nameof(target.AllFalse));
                            }
                            if (target.AllIndeterminate)
                            {
                                target.AllIndeterminate = false;
                                yield return () => target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                            }
                        }
                        else
                        {
                            int indexOfState = target._falseAccessor.Count;
                            target._falseAccessor.Add(node);
                            yield return () =>
                            {
                                target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                                target.False.RaiseCountChanged();
                                target.RaisePropertyChanged(nameof(target.AnyFalse));
                            };
                            if (!target.AnyFalse)
                            {
                                target.AnyFalse = true;
                                yield return () => target.RaisePropertyChanged(nameof(target.AnyFalse));
                            }
                            if (target.AllTrue)
                            {
                                target.AllTrue = false;
                                yield return () => target.RaisePropertyChanged(nameof(target.AllTrue));
                            }
                            else if (target.AllIndeterminate)
                            {
                                target.AllIndeterminate = false;
                                yield return () => target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                            }
                        }
                    }
                    else
                    {
                        int indexOfState = target._indeterminateAccessor.Count;
                        target._indeterminateAccessor.Add(node);
                        yield return () =>
                        {
                            target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                            target.Indeterminate.RaiseCountChanged();
                        };
                        if (!target.AnyIndeterminate)
                        {
                            target.AnyIndeterminate = true;
                            yield return () => target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                        }
                        if (target.AllTrue)
                        {
                            target.AllTrue = false;
                            yield return () => target.RaisePropertyChanged(nameof(target.AllTrue));
                        }
                        else if (target.AllFalse)
                        {
                            target.AllFalse = false;
                            yield return () => target.RaisePropertyChanged(nameof(target.AllFalse));
                        }
                    }
                }
            }

            internal static void Clear(ToggleSet<T> toggleSet)
            {
                T[] removedTrue, removedFalse, removedIndeterminate;
                Node[] removedAll;
                using (MonitorEntered<ToggleSet<T>> monitorEntered = MonitorEntered.TakeSynchronizableLock(toggleSet))
                {
                    if ((removedAll = GetNodesInSet(toggleSet._firstInSet).ToArray()).Length == 0)
                        return;
                    removedTrue = toggleSet.True.ToArray();
                    removedFalse = toggleSet.False.ToArray();
                    removedIndeterminate = toggleSet.Indeterminate.ToArray();
                    toggleSet._trueAccessor.Clear();
                    toggleSet._falseAccessor.Clear();
                    toggleSet._indeterminateAccessor.Clear();
                    foreach (Node node in removedAll)
                    {
                        node.PreviousInSet = node.NextInSet = null;
                        node._owner = null;
                    }
                    toggleSet._firstInSet = toggleSet._lastInSet = null;
                    toggleSet._count = 0;
                }

                if (removedTrue.Length > 0)
                {
                    toggleSet.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedTrue, 0));
                    toggleSet.True.RaiseCountChanged();
                }
                if (removedFalse.Length > 0)
                {
                    toggleSet.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedFalse, 0));
                    toggleSet.False.RaiseCountChanged();
                }
                if (removedIndeterminate.Length > 0)
                {
                    toggleSet.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedIndeterminate, 0));
                    toggleSet.Indeterminate.RaiseCountChanged();
                }
                toggleSet.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedAll.Select(n => n.Value).ToArray(), 0));
                toggleSet.All.RaiseCountChanged();
            }

            internal static IEnumerable<Action> Remove(ToggleSet<T> toggleSet, T value)
            {
                int indexOfState;
                if (toggleSet.TryFindNode(value, out Node node, out int indexInSet))
                {
                    yield return () =>
                    {
                        toggleSet.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, indexInSet));
                        toggleSet.All.RaiseCountChanged();
                    };
                    if (node.NextInSet is null)
                    {
                        if ((toggleSet._lastInSet = node.PreviousInSet) is null)
                        {
                            toggleSet._firstInSet = null;
                            toggleSet._count = indexOfState = 0;
                            toggleSet.IsEmpty = true;
                            yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.IsEmpty));
                        }
                        else
                        {
                            node.PreviousInSet = node.PreviousInSet.NextInSet = null;
                            toggleSet._count--;
                            indexOfState = node.GetIndexOfState();
                        }
                    }
                    else
                    {
                        indexOfState = node.GetIndexOfState();
                        toggleSet._count--;
                        if ((node.NextInSet.PreviousInSet = node.PreviousInSet) is null)
                            node.NextInSet = (toggleSet._firstInSet = node.NextInSet).PreviousInSet = null;
                        else
                        {
                            node.PreviousInSet.NextInSet = node.NextInSet;
                            node.NextInSet = node.PreviousInSet = null;
                        }
                    }
                    if (node._state.HasValue)
                    {
                        if (node._state.Value)
                        {
                            toggleSet._trueAccessor.Remove(node);
                            yield return () =>
                            {
                                toggleSet.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, indexOfState));
                                toggleSet.True.RaiseCountChanged();
                            };
                            if (toggleSet.True.Count == 0)
                            {
                                toggleSet.AnyTrue = false;
                                if (toggleSet.AllTrue)
                                {
                                    toggleSet.AllTrue = false;
                                    yield return () =>
                                    {
                                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AllTrue));
                                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyTrue));
                                    };
                                }
                                else
                                {
                                    yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyTrue));
                                    if (toggleSet.AnyFalse)
                                    {
                                        if (!toggleSet.AnyIndeterminate)
                                        {
                                            toggleSet.AllFalse = true;
                                            yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllFalse));
                                        }
                                    }
                                    else
                                    {
                                        toggleSet.AllIndeterminate = true;
                                        yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllIndeterminate));
                                    }
                                }
                            }
                        }
                        else
                        {
                            toggleSet._falseAccessor.Remove(node);
                            yield return () =>
                            {
                                toggleSet.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, indexOfState));
                                toggleSet.False.RaiseCountChanged();
                            };
                            if (toggleSet.False.Count == 0)
                            {
                                toggleSet.AnyFalse = false;
                                if (toggleSet.AllFalse)
                                {
                                    toggleSet.AllFalse = false;
                                    yield return () =>
                                    {
                                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AllFalse));
                                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyFalse));
                                    };
                                }
                                else
                                {
                                    yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyFalse));
                                    if (toggleSet.AnyTrue)
                                    {
                                        if (!toggleSet.AnyIndeterminate)
                                        {
                                            toggleSet.AllTrue = true;
                                            yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllTrue));
                                        }
                                    }
                                    else
                                    {
                                        toggleSet.AllIndeterminate = true;
                                        yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllIndeterminate));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        toggleSet._indeterminateAccessor.Remove(node);
                        yield return () =>
                        {
                            toggleSet.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, indexOfState));
                            toggleSet.Indeterminate.RaiseCountChanged();
                        };
                        if (toggleSet.Indeterminate.Count == 0)
                        {
                            toggleSet.AnyIndeterminate = false;
                            if (toggleSet.AllIndeterminate)
                            {
                                toggleSet.AllFalse = false;
                                yield return () =>
                                {
                                    toggleSet.RaisePropertyChanged(nameof(toggleSet.AllIndeterminate));
                                    toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyIndeterminate));
                                };
                            }
                            else
                            {
                                yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyIndeterminate));
                                if (toggleSet.AnyTrue)
                                {
                                    if (!toggleSet.AnyFalse)
                                    {
                                        toggleSet.AllTrue = true;
                                        yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllTrue));
                                    }
                                }
                                else
                                {
                                    toggleSet.AllFalse = true;
                                    yield return () => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllFalse));
                                }
                            }
                        }
                    }
                }
            }

            internal static IEnumerable<Action> Set(ToggleSet<T> target, T value, bool? state)
            {
                using (MonitorEntered<ToggleSet<T>> monitorEntered = MonitorEntered.TakeSynchronizableLock(target))
                {
                    if (target._lastInSet is null)
                    {
                        Node node = new(target, value, state);
                        target._firstInSet = target._lastInSet = node;
                        target._count = 1;
                        target.IsEmpty = false;
                        yield return () =>
                        {
                            target.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                            target.All.RaiseCountChanged();
                            target.RaisePropertyChanged(nameof(target.IsEmpty));
                        };
                        if (state.HasValue)
                        {
                            if (state.Value)
                            {
                                target._trueAccessor.Add(node);
                                target.AnyTrue = target.AllTrue = true;
                                yield return () =>
                                {
                                    target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                                    target.True.RaiseCountChanged();
                                    target.RaisePropertyChanged(nameof(target.AnyTrue));
                                    target.RaisePropertyChanged(nameof(target.AllTrue));
                                };
                            }
                            else
                            {
                                target._falseAccessor.Add(node);
                                target.AnyFalse = target.AllFalse = true;
                                yield return () =>
                                {
                                    target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                                    target.False.RaiseCountChanged();
                                    target.RaisePropertyChanged(nameof(target.AnyFalse));
                                    target.RaisePropertyChanged(nameof(target.AllFalse));
                                };
                            }
                        }
                        else
                        {
                            target._indeterminateAccessor.Add(node);
                            target.AnyIndeterminate = target.AllIndeterminate = true;
                            yield return () =>
                            {
                                target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                                target.Indeterminate.RaiseCountChanged();
                                target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                                target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                            };
                        }
                    }
                    else
                    {
                        Node node = target.FindNode(value);
                        if (node is null)
                        {
                            node = new Node(target, value, state);
                            target._lastInSet = (node.PreviousInSet = target._lastInSet).NextInSet = node;
                            int indexInSet = target._count++;
                            yield return () =>
                            {
                                target.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexInSet));
                                target.All.RaiseCountChanged();
                            };
                            if (state.HasValue)
                            {
                                if (state.Value)
                                {
                                    int indexOfState = target._trueAccessor.Count;
                                    target._trueAccessor.Add(node);
                                    yield return () =>
                                    {
                                        target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                                        target.True.RaiseCountChanged();
                                    };
                                    if (!target.AnyTrue)
                                    {
                                        target.AnyTrue = true;
                                        yield return () => target.RaisePropertyChanged(nameof(target.AnyTrue));
                                    }
                                    if (target.AllFalse)
                                    {
                                        target.AllFalse = false;
                                        yield return () => target.RaisePropertyChanged(nameof(target.AllFalse));
                                    }
                                    else if (target.AllIndeterminate)
                                    {
                                        target.AllIndeterminate = false;
                                        yield return () => target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                                    }
                                }
                                else
                                {
                                    int indexOfState = target._falseAccessor.Count;
                                    target._falseAccessor.Add(node);
                                    yield return () =>
                                    {
                                        target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                                        target.False.RaiseCountChanged();
                                    };
                                    if (!target.AnyFalse)
                                    {
                                        target.AnyFalse = true;
                                        yield return () => target.RaisePropertyChanged(nameof(target.AnyFalse));
                                    }
                                    if (target.AllTrue)
                                    {
                                        target.AllTrue = false;
                                        yield return () => target.RaisePropertyChanged(nameof(target.AllTrue));
                                    }
                                    else if (target.AllIndeterminate)
                                    {
                                        target.AllIndeterminate = false;
                                        yield return () => target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                                    }
                                }
                            }
                            else
                            {
                                int indexOfState = target._indeterminateAccessor.Count;
                                target._indeterminateAccessor.Add(node);
                                yield return () =>
                                {
                                    target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                                    target.Indeterminate.RaiseCountChanged();
                                };
                                if (!target.AnyIndeterminate)
                                {
                                    target.AnyIndeterminate = true;
                                    yield return () => target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                                }
                                if (target.AllTrue)
                                {
                                    target.AllTrue = false;
                                    yield return () => target.RaisePropertyChanged(nameof(target.AllTrue));
                                }
                                else if (target.AllFalse)
                                {
                                    target.AllFalse = false;
                                    yield return () => target.RaisePropertyChanged(nameof(target.AllFalse));
                                }
                            }
                        }
                        else
                        {
                            if (node.State.HasValue ? !(state.HasValue && state.Value == node.State.Value) : state.HasValue)
                            {
                                // TODO: Finis implementation
                            }
                        }
                    }
                }
                // TODO: Finis implementation
            }

            internal class AccessorBase
            {
                protected AccessorBase(StateSet target)
                {
                    Target = target;
                }

                protected StateSet Target { get; }

                protected void InsertOfState(Node item, Node previous)
                {
                    if ((item.NextOfState = (item.PreviousOfState = previous).NextOfState) is not null)
                        previous.NextOfState = item.NextOfState.PreviousOfState = item;
                    else
                        previous.NextOfState = item;
                }
                protected void RemoveOfState(Node node)
                {
                    if (node.PreviousOfState is null)
                    {
                        if (node.NextOfState != null)
                            node.NextOfState = node.NextOfState.PreviousOfState = null;
                    }
                    else
                    {
                        if ((node.PreviousOfState.NextOfState = node.NextOfState) is not null)
                        {
                            node.NextOfState.PreviousOfState = node.PreviousOfState;
                            node.NextOfState = null;
                        }
                        node.PreviousOfState = null;
                    }
                }
                protected void ClearOfState([DisallowNull] Node node)
                {
                    while (node.PreviousOfState is not null)
                        node = node.PreviousOfState;
                    Node nextNode = node.NextOfState;
                    while (nextNode is not null)
                    {
                        node.NextOfState = nextNode.PreviousOfState = null;
                        nextNode = (node = nextNode).NextOfState;
                    }
                }
            }
        }

        public class StateSet : IReadOnlySet<T>, ICollection, INotifyCollectionChanged
        {
            private readonly object _syncRoot;
            private readonly IEqualityComparer<T> _comparer;
            internal Node First { get; private set; }
            internal Node Last { get; private set; }

            public event NotifyCollectionChangedEventHandler CollectionChanged;
            public event PropertyChangedEventHandler PropertyChanged;

            public int Count { get; private set; } = 0;

            bool ICollection.IsSynchronized => true;

            object ICollection.SyncRoot => _syncRoot;

            private StateSet(IEqualityComparer<T> comparer, object syncRoot)
            {
                _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
                _syncRoot = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));
            }

            internal static StateSet Create(IEqualityComparer<T> comparer, object syncRoot, out ICollection<Node> accessor)
            {
                StateSet result = new(comparer, syncRoot);
                accessor = new Accessor(result);
                return result;
            }

            class Accessor : Node.AccessorBase, ICollection<Node>
            {
                private readonly StateSet _target;

                internal Accessor(StateSet target) : base(target) { }

                public int Count => _target.Count;

                bool ICollection<Node>.IsReadOnly => false;

                public void Add(Node item)
                {
                    Node previous = _target.Last;
                    if (previous is null)
                    {
                        _target.First = _target.Last = item;
                        _target.Count = 1;
                    }
                    else
                    {
                        _target.Count++;
                        InsertOfState(item, previous);
                        _target.Last = item;
                    }
                }

                public void Clear()
                {
                    if (_target.First is not null)
                        ClearOfState(_target.First);
                    _target.First = _target.Last = null;
                    _target.Count = 0;
                }

                public bool Contains(Node item) => GetNodesOfState(_target.First).Contains(item);

                public void CopyTo(Node[] array, int arrayIndex) => GetNodesOfState(_target.First).ToList().CopyTo(array, arrayIndex);

                public IEnumerator<Node> GetEnumerator() => GetNodesOfState(_target.First).GetEnumerator();

                public bool Remove(Node item)
                {
                    if (item is null || _target.First is null)
                        return false;
                    if (item.NextOfState is null)
                    {
                        if (!ReferenceEquals(item, _target.Last))
                            return false;
                        if ((_target.Last = item.PreviousOfState) is null)
                        {
                            _target.First = null;
                            _target.Count = 0;
                            return true;
                        }
                        RemoveOfState(item);
                    }
                    else if (item.PreviousOfState is null)
                    {
                        if (!ReferenceEquals(item, _target.First))
                            return false;
                        _target.First = item.NextOfState;
                    }
                    else
                    {
                        Node f = item;
                        while (f.PreviousOfState is not null)
                            f = f.PreviousOfState;
                        if (!ReferenceEquals(f, _target.First))
                            return false;
                    }
                    RemoveOfState(item);
                    return true;
                }

                IEnumerator IEnumerable.GetEnumerator() => GetNodesOfState(_target.First).ToArray().GetEnumerator();
            }

            public bool Contains(T item) => GetNodesInSet(First).Select(n => n.Value).Contains(item, _comparer);

            public void CopyTo(Array array, int index) => GetNodesInSet(First).Select(n => n.Value).ToArray().CopyTo(array, index);

            public IEnumerator<T> GetEnumerator() => GetNodesInSet(First).Select(n => n.Value).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetNodesInSet(First).Select(n => n.Value)).GetEnumerator();

            public bool IsProperSubsetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using MonitorEntered<StateSet> monitorEntered = MonitorEntered.TakeSynchronizedLock(this);
                if (Count == 0)
                    return other.Any();
                IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                using IEnumerator<T> x = values.GetEnumerator();
                using IEnumerator<T> y = other.GetEnumerator();
                bool isProper = false;
                while (x.MoveNext())
                {
                    if (!y.MoveNext() || !other.Contains(x.Current, _comparer))
                        return false;
                    if (!values.Contains(y.Current, _comparer))
                        isProper = true;
                }
                if (isProper)
                    return true;
                while (y.MoveNext())
                {
                    if (!values.Contains(y.Current, _comparer))
                        return true;
                }
                return false;
            }

            public bool IsProperSupersetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using MonitorEntered<StateSet> monitorEntered = MonitorEntered.TakeSynchronizedLock(this);
                if (Count == 0)
                    return false;
                IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                using IEnumerator<T> x = values.GetEnumerator();
                using IEnumerator<T> y = other.GetEnumerator();
                bool isProper = false;
                while (y.MoveNext())
                {
                    if (!x.MoveNext() || !values.Contains(y.Current, _comparer))
                        return false;
                    if (!other.Contains(x.Current, _comparer))
                        isProper = true;
                }
                if (isProper)
                    return true;
                while (x.MoveNext())
                {
                    if (!other.Contains(x.Current, _comparer))
                        return true;
                }
                return false;
            }

            public bool IsSubsetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using MonitorEntered<StateSet> monitorEntered = MonitorEntered.TakeSynchronizedLock(this);
                if (Count == 0)
                    return true;
                IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                using IEnumerator<T> x = values.GetEnumerator();
                using IEnumerator<T> y = other.GetEnumerator();
                bool isProper = false;
                while (x.MoveNext())
                {
                    if (!y.MoveNext() || !other.Contains(x.Current, _comparer))
                        return false;
                    if (!values.Contains(y.Current, _comparer))
                        isProper = true;
                }
                if (isProper || !y.MoveNext())
                    return true;
                do
                {
                    if (!values.Contains(y.Current, _comparer))
                        return true;
                } while (y.MoveNext());
                return false;
            }

            public bool IsSupersetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using MonitorEntered<StateSet> monitorEntered = MonitorEntered.TakeSynchronizedLock(this);
                if (Count == 0)
                    return !other.Any();
                IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                using IEnumerator<T> x = values.GetEnumerator();
                using IEnumerator<T> y = other.GetEnumerator();
                bool isProper = false;
                while (y.MoveNext())
                {
                    if (!x.MoveNext() || !values.Contains(y.Current, _comparer))
                        return false;
                    if (!other.Contains(x.Current, _comparer))
                        isProper = true;
                }
                if (isProper || !x.MoveNext())
                    return true;
                do
                {
                    if (!other.Contains(x.Current, _comparer))
                        return true;
                } while (x.MoveNext());
                return false;
            }

            public bool Overlaps(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using MonitorEntered<StateSet> monitorEntered = MonitorEntered.TakeSynchronizedLock(this);
                if (Count == 0)
                    return false;
                IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                return other.Any(o => values.Contains(o, _comparer));
            }

            public bool SetEquals(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                using MonitorEntered<StateSet> monitorEntered = MonitorEntered.TakeSynchronizedLock(this);
                if (Count == 0)
                    return !other.Any();
                IEnumerable<T> values = GetNodesInSet(First).Select(n => n.Value);
                using IEnumerator<T> x = values.GetEnumerator();
                using IEnumerator<T> y = other.GetEnumerator();
                while (x.MoveNext())
                {
                    if (!(y.MoveNext() && values.Contains(y.Current, _comparer) && other.Contains(x.Current, _comparer)))
                        return false;
                }
                return !y.MoveNext();
            }

            internal void RaiseCountChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));

            internal void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args) => CollectionChanged?.Invoke(this, args);
        }

        public class AllItems : IReadOnlySet<T>, ICollection, INotifyCollectionChanged
        {
            private readonly ToggleSet<T> _owner;

            public event NotifyCollectionChangedEventHandler CollectionChanged;
            public event PropertyChangedEventHandler PropertyChanged;

            public int Count => _owner._count;

            bool ICollection.IsSynchronized => true;

            object ICollection.SyncRoot => _owner._syncRoot;

            internal AllItems(ToggleSet<T> owner) { _owner = owner ?? throw new ArgumentNullException(nameof(owner)); }

            public bool Contains(T item) => GetNodesInSet(_owner._firstInSet).Select(n => n.Value).Contains(item, _owner._comparer);

            public void CopyTo(Array array, int index) => GetNodesInSet(_owner._firstInSet).Select(n => n.Value).ToArray().CopyTo(array, index);

            public IEnumerator<T> GetEnumerator() => GetNodesInSet(_owner._firstInSet).Select(n => n.Value).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetNodesInSet(_owner._firstInSet).Select(n => n.Value)).GetEnumerator();

            public bool IsProperSubsetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return other.Any();
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (x.MoveNext())
                    {
                        if (!y.MoveNext() || !other.Contains(x.Current, _owner._comparer))
                            return false;
                        if (!values.Contains(y.Current, _owner._comparer))
                            isProper = true;
                    }
                    if (isProper)
                        return true;
                    while (y.MoveNext())
                    {
                        if (!values.Contains(y.Current, _owner._comparer))
                            return true;
                    }
                }
                finally { Monitor.Exit(_owner._syncRoot); }
                return false;
            }

            public bool IsProperSupersetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return false;
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (y.MoveNext())
                    {
                        if (!x.MoveNext() || !values.Contains(y.Current, _owner._comparer))
                            return false;
                        if (!other.Contains(x.Current, _owner._comparer))
                            isProper = true;
                    }
                    if (isProper)
                        return true;
                    while (x.MoveNext())
                    {
                        if (!other.Contains(x.Current, _owner._comparer))
                            return true;
                    }
                }
                finally { Monitor.Exit(_owner._syncRoot); }
                return false;
            }

            public bool IsSubsetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return true;
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (x.MoveNext())
                    {
                        if (!y.MoveNext() || !other.Contains(x.Current, _owner._comparer))
                            return false;
                        if (!values.Contains(y.Current, _owner._comparer))
                            isProper = true;
                    }
                    if (isProper || !y.MoveNext())
                        return true;
                    do
                    {
                        if (!values.Contains(y.Current, _owner._comparer))
                            return true;
                    } while (y.MoveNext());
                }
                finally { Monitor.Exit(_owner._syncRoot); }
                return false;
            }

            public bool IsSupersetOf(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return !other.Any();
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    bool isProper = false;
                    while (y.MoveNext())
                    {
                        if (!x.MoveNext() || !values.Contains(y.Current, _owner._comparer))
                            return false;
                        if (!other.Contains(x.Current, _owner._comparer))
                            isProper = true;
                    }
                    if (isProper || !x.MoveNext())
                        return true;
                    do
                    {
                        if (!other.Contains(x.Current, _owner._comparer))
                            return true;
                    } while (x.MoveNext());
                }
                finally { Monitor.Exit(_owner._syncRoot); }
                return false;
            }

            public bool Overlaps(IEnumerable<T> other)
            {
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (other is null || _owner._count == 0)
                        return false;
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    return _owner._count > 0 && other.Any(o => values.Contains(o, _owner._comparer));
                }
                finally { Monitor.Exit(_owner._syncRoot); }
            }

            public bool SetEquals(IEnumerable<T> other)
            {
                if (other is null)
                    return false;
                Monitor.Enter(_owner._syncRoot);
                try
                {
                    if (_owner._count == 0)
                        return !other.Any();
                    IEnumerable<T> values = GetNodesInSet(_owner._firstInSet).Select(n => n.Value);
                    using IEnumerator<T> x = values.GetEnumerator();
                    using IEnumerator<T> y = other.GetEnumerator();
                    while (x.MoveNext())
                    {
                        if (!(y.MoveNext() && values.Contains(y.Current, _owner._comparer) && other.Contains(x.Current, _owner._comparer)))
                            return false;
                    }
                    return !y.MoveNext();
                }
                finally { Monitor.Exit(_owner._syncRoot); }
            }

            internal void RaiseCountChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));

            internal void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args) => CollectionChanged?.Invoke(this, args);
        }
    }
}
