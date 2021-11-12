using FsInfoCat.DeferredDelegation;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat.Collections
{
    public partial class ToggleSet<T>
    {
        public class Node : INotifyPropertyChanged
        {
            private ToggleSet<T> _owner;
            private bool? _state;

            private Node(ToggleSet<T> owner, T value, bool? state)
            {
                _owner = owner;
                Value = value;
                _state = state;
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
                    if (!_owner._deferredDelegation.EnterSynchronizableIfNotNull(_owner, out IDelegateDeference<ToggleSet<T>> delegateDeference))
                    {
                        _state = value;
                        RaisePropertyChanged(nameof(State));
                        return;
                    }
                    using (delegateDeference)
                    {
                        ToggleSet<T> target = delegateDeference.Target;
                        if (value.HasValue)
                        {
                            if (_state.HasValue)
                            {
                                if (value.Value == _state.Value)
                                    return;
                                int oldIndex = GetIndexOfState();
                                if (value.Value)
                                {
                                    int newIndex = target._trueAccessor.Count;
                                    target._trueAccessor.Add(this);
                                    _ = target._falseAccessor.Remove(this);
                                    delegateDeference.DeferAction(() =>
                                    {
                                        target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
                                        target.True.RaiseCountChanged();
                                        target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
                                        target.False.RaiseCountChanged();
                                    });
                                    if (target.AllFalse)
                                    {
                                        target.AllFalse = target.AnyFalse = false;
                                        target.AllTrue = target.AnyTrue = true;
                                        delegateDeference.DeferAction(() =>
                                        {
                                            target.RaisePropertyChanged(nameof(target.AllFalse));
                                            target.RaisePropertyChanged(nameof(target.AnyFalse));
                                            target.RaisePropertyChanged(nameof(target.AllTrue));
                                            target.RaisePropertyChanged(nameof(target.AnyTrue));
                                        });
                                    }
                                    else
                                    {
                                        if (!target.AnyTrue)
                                        {
                                            target.AnyTrue = true;
                                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyTrue)));
                                        }
                                        if (target.False.Count == 0)
                                        {
                                            target.AnyFalse = false;
                                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyFalse)));
                                        }
                                    }
                                }
                                else
                                {
                                    int newIndex = target._falseAccessor.Count;
                                    target._falseAccessor.Add(this);
                                    _ = target._trueAccessor.Remove(this);
                                    delegateDeference.DeferAction(() =>
                                    {
                                        target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
                                        target.False.RaiseCountChanged();
                                        target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
                                        target.True.RaiseCountChanged();
                                    });
                                    if (target.AllTrue)
                                    {
                                        target.AllTrue = target.AnyTrue = false;
                                        target.AllFalse = target.AnyFalse = true;
                                        delegateDeference.DeferAction(() =>
                                        {
                                            target.RaisePropertyChanged(nameof(target.AllTrue));
                                            target.RaisePropertyChanged(nameof(target.AnyTrue));
                                            target.RaisePropertyChanged(nameof(target.AllFalse));
                                            target.RaisePropertyChanged(nameof(target.AnyFalse));
                                        });
                                    }
                                    else
                                    {
                                        if (!target.AnyFalse)
                                        {
                                            target.AnyFalse = true;
                                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyFalse)));
                                        }
                                        if (target.True.Count == 0)
                                        {
                                            target.AnyTrue = false;
                                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyTrue)));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int oldIndex = GetIndexOfState();
                                if (value.Value)
                                {
                                    int newIndex = target._trueAccessor.Count;
                                    target._trueAccessor.Add(this);
                                    _ = target._indeterminateAccessor.Remove(this);
                                    delegateDeference.DeferAction(() =>
                                    {
                                        target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
                                        target.True.RaiseCountChanged();
                                        target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
                                        target.Indeterminate.RaiseCountChanged();
                                    });
                                    if (target.AllIndeterminate)
                                    {
                                        target.AllIndeterminate = target.AnyIndeterminate = false;
                                        target.AllTrue = target.AnyTrue = true;
                                        delegateDeference.DeferAction(() =>
                                        {
                                            target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                                            target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                                            target.RaisePropertyChanged(nameof(target.AllTrue));
                                            target.RaisePropertyChanged(nameof(target.AnyTrue));
                                        });
                                    }
                                    else
                                    {
                                        if (!target.AnyTrue)
                                        {
                                            target.AnyTrue = true;
                                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyTrue)));
                                        }
                                        if (target.Indeterminate.Count == 0)
                                        {
                                            target.AnyIndeterminate = false;
                                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyIndeterminate)));
                                        }
                                    }
                                }
                                else
                                {
                                    int newIndex = target._falseAccessor.Count;
                                    target._falseAccessor.Add(this);
                                    _ = target._indeterminateAccessor.Remove(this);
                                    delegateDeference.DeferAction(() =>
                                    {
                                        target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
                                        target.False.RaiseCountChanged();
                                        target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
                                        target.Indeterminate.RaiseCountChanged();
                                    });
                                    if (target.AllIndeterminate)
                                    {
                                        target.AllIndeterminate = target.AnyIndeterminate = false;
                                        target.AllFalse = target.AnyFalse = true;
                                        delegateDeference.DeferAction(() =>
                                        {
                                            target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                                            target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                                            target.RaisePropertyChanged(nameof(target.AllFalse));
                                            target.RaisePropertyChanged(nameof(target.AnyFalse));
                                        });
                                    }
                                    else
                                    {
                                        if (!target.AnyFalse)
                                        {
                                            target.AnyFalse = true;
                                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyFalse)));
                                        }
                                        if (target.Indeterminate.Count == 0)
                                        {
                                            target.AnyIndeterminate = false;
                                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyIndeterminate)));
                                        }
                                    }
                                }
                            }
                            delegateDeference.DeferAction(() => RaisePropertyChanged(nameof(State)));
                        }
                        else if (_state.HasValue)
                        {
                            int oldIndex = GetIndexOfState();
                            int newIndex = target._indeterminateAccessor.Count;
                            if (_state.Value)
                            {
                                target._indeterminateAccessor.Add(this);
                                _ = target._trueAccessor.Remove(this);
                                delegateDeference.DeferAction(() =>
                                {
                                    target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
                                    target.Indeterminate.RaiseCountChanged();
                                    target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
                                    target.True.RaiseCountChanged();
                                });
                                if (target.AllTrue)
                                {
                                    target.AllTrue = target.AnyTrue = false;
                                    target.AllIndeterminate = target.AnyIndeterminate = true;
                                    delegateDeference.DeferAction(() =>
                                    {
                                        target.RaisePropertyChanged(nameof(target.AllTrue));
                                        target.RaisePropertyChanged(nameof(target.AnyTrue));
                                        target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                                        target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                                    });
                                }
                                else
                                {
                                    if (!target.AnyIndeterminate)
                                    {
                                        target.AnyIndeterminate = true;
                                        delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyIndeterminate)));
                                    }
                                    if (target.True.Count == 0)
                                    {
                                        target.AnyTrue = false;
                                        delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyTrue)));
                                    }
                                }
                            }
                            else
                            {
                                target._indeterminateAccessor.Add(this);
                                _ = target._falseAccessor.Remove(this);
                                delegateDeference.DeferAction(() =>
                                {
                                    target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value, newIndex));
                                    target.Indeterminate.RaiseCountChanged();
                                    target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Value, oldIndex));
                                    target.False.RaiseCountChanged();
                                });
                                if (target.AllFalse)
                                {
                                    target.AllFalse = target.AnyFalse = false;
                                    target.AllIndeterminate = target.AnyIndeterminate = true;
                                    delegateDeference.DeferAction(() =>
                                    {
                                        target.RaisePropertyChanged(nameof(target.AllFalse));
                                        target.RaisePropertyChanged(nameof(target.AnyFalse));
                                        target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                                        target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                                    });
                                }
                                else
                                {
                                    if (!target.AnyIndeterminate)
                                    {
                                        target.AnyIndeterminate = true;
                                        delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyIndeterminate)));
                                    }
                                    if (target.False.Count == 0)
                                    {
                                        target.AnyFalse = false;
                                        delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyFalse)));
                                    }
                                }
                            }
                            delegateDeference.DeferAction(() => RaisePropertyChanged(nameof(State)));
                        }
                    }
                }
            }

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

            internal static void Add(ToggleSet<T> target, T value, bool? state, IDelegateDeference<ToggleSet<T>> delegateDeference)
            {
                if (target._lastInSet is null)
                {
                    target.IsEmpty = false;
                    Node node = new(target, value, state);
                    target._firstInSet = target._lastInSet = node;
                    target._count = 1;
                    delegateDeference.DeferAction(() =>
                    {
                        target.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                        target.All.RaiseCountChanged();
                        target.RaisePropertyChanged(nameof(target.IsEmpty));
                    });
                    if (state.HasValue)
                    {
                        if (state.Value)
                        {
                            target._trueAccessor.Add(node);
                            target.AnyTrue = target.AllTrue = true;
                            delegateDeference.DeferAction(() =>
                            {
                                target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                                target.True.RaiseCountChanged();
                                target.RaisePropertyChanged(nameof(target.AnyTrue));
                                target.RaisePropertyChanged(nameof(target.AllTrue));
                            });
                        }
                        else
                        {
                            target._falseAccessor.Add(node);
                            target.AnyFalse = target.AllFalse = true;
                            delegateDeference.DeferAction(() =>
                            {
                                target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                                target.False.RaiseCountChanged();
                                target.RaisePropertyChanged(nameof(target.AnyFalse));
                                target.RaisePropertyChanged(nameof(target.AllFalse));
                            });
                        }
                    }
                    else
                    {
                        target._indeterminateAccessor.Add(node);
                        target.AnyIndeterminate = target.AllIndeterminate = true;
                        delegateDeference.DeferAction(() =>
                        {
                            target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                            target.Indeterminate.RaiseCountChanged();
                            target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                            target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                        });
                    }
                }
                else
                {
                    if (target.FindNode(value) is not null)
                        throw new InvalidOperationException();
                    Node node = new(target, value, state);
                    target._lastInSet = (node.PreviousInSet = target._lastInSet).NextInSet = node;
                    int indexInSet = target._count++;
                    delegateDeference.DeferAction(() =>
                    {
                        target.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexInSet));
                        target.All.RaiseCountChanged();
                    });
                    if (state.HasValue)
                    {
                        if (state.Value)
                        {
                            int indexOfState = target._trueAccessor.Count;
                            target._trueAccessor.Add(node);
                            delegateDeference.DeferAction(() =>
                            {
                                target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                                target.True.RaiseCountChanged();
                            });
                            if (!target.AnyTrue)
                            {
                                target.AnyTrue = true;
                                delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyTrue)));
                            }
                            if (target.AllFalse)
                            {
                                target.AllFalse = false;
                                delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllFalse)));
                            }
                            else if (target.AllIndeterminate)
                            {
                                target.AllIndeterminate = false;
                                delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllIndeterminate)));
                            }
                        }
                        else
                        {
                            int indexOfState = target._falseAccessor.Count;
                            target._falseAccessor.Add(node);
                            delegateDeference.DeferAction(() =>
                            {
                                target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                                target.False.RaiseCountChanged();
                                target.RaisePropertyChanged(nameof(target.AnyFalse));
                            });
                            if (!target.AnyFalse)
                            {
                                target.AnyFalse = true;
                                delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyFalse)));
                            }
                            if (target.AllTrue)
                            {
                                target.AllTrue = false;
                                delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllTrue)));
                            }
                            else if (target.AllIndeterminate)
                            {
                                target.AllIndeterminate = false;
                                delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllIndeterminate)));
                            }
                        }
                    }
                    else
                    {
                        int indexOfState = target._indeterminateAccessor.Count;
                        target._indeterminateAccessor.Add(node);
                        delegateDeference.DeferAction(() =>
                        {
                            target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                            target.Indeterminate.RaiseCountChanged();
                        });
                        if (!target.AnyIndeterminate)
                        {
                            target.AnyIndeterminate = true;
                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyIndeterminate)));
                        }
                        if (target.AllTrue)
                        {
                            target.AllTrue = false;
                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllTrue)));
                        }
                        else if (target.AllFalse)
                        {
                            target.AllFalse = false;
                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllFalse)));
                        }
                    }
                }
            }

            internal static void Clear(ToggleSet<T> toggleSet)
            {
                IDelegateDeference<ToggleSet<T>> delegateDeference = toggleSet._deferredDelegation.EnterSynchronizable(toggleSet);
                Node[] removedAll = GetNodesInSet(toggleSet._firstInSet).ToArray();
                if (removedAll.Length == 0)
                    return;
                T[] removedTrue = toggleSet.True.ToArray();
                T[] removedFalse = toggleSet.False.ToArray();
                T[] removedIndeterminate = toggleSet.Indeterminate.ToArray();
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
                if (removedTrue.Length > 0)
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedTrue, 0));
                        toggleSet.True.RaiseCountChanged();
                    });
                if (removedFalse.Length > 0)
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedFalse, 0));
                        toggleSet.False.RaiseCountChanged();
                    });
                if (removedIndeterminate.Length > 0)
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedIndeterminate, 0));
                        toggleSet.Indeterminate.RaiseCountChanged();
                    });
                delegateDeference.DeferAction(() =>
                {
                    toggleSet.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedAll.Select(n => n.Value).ToArray(), 0));
                    toggleSet.All.RaiseCountChanged();
                });
                if (toggleSet.AllTrue)
                {
                    toggleSet.AllTrue = toggleSet.AnyTrue = false;
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AllTrue));
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyTrue));
                    });
                }
                else if (toggleSet.AllFalse)
                {
                    toggleSet.AllFalse = toggleSet.AnyFalse = false;
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AllFalse));
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyFalse));
                    });
                }
                else if (toggleSet.AllIndeterminate)
                {
                    toggleSet.AllIndeterminate = toggleSet.AnyIndeterminate = false;
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AllIndeterminate));
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyIndeterminate));
                    });
                }
                else if (toggleSet.AnyTrue)
                {
                    toggleSet.AnyTrue = false;
                    if (toggleSet.AnyFalse)
                    {
                        toggleSet.AnyFalse = false;
                        if (toggleSet.AnyIndeterminate)
                        {
                            toggleSet.AnyIndeterminate = false;
                            delegateDeference.DeferAction(() =>
                            {
                                toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyTrue));
                                toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyFalse));
                                toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyIndeterminate));
                            });
                        }
                        else
                            delegateDeference.DeferAction(() =>
                            {
                                toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyTrue));
                                toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyFalse));
                            });
                    }
                    else
                    {
                        toggleSet.AnyIndeterminate = false;
                        delegateDeference.DeferAction(() =>
                        {
                            toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyTrue));
                            toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyIndeterminate));
                        });
                    }
                }
                else if (toggleSet.AnyFalse)
                {
                    toggleSet.AnyFalse = toggleSet.AnyIndeterminate = false;
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyFalse));
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyIndeterminate));
                    });
                }
            }

            internal static bool Remove(ToggleSet<T> toggleSet, T value, IDelegateDeference<ToggleSet<T>> delegateDeference)
            {
                if (!toggleSet.TryFindNode(value, out Node node, out int indexInSet))
                    return false;
                int indexOfState;
                delegateDeference.DeferAction(() =>
                {
                    toggleSet.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, indexInSet));
                    toggleSet.All.RaiseCountChanged();
                });
                if (node.NextInSet is null)
                {
                    if ((toggleSet._lastInSet = node.PreviousInSet) is null)
                    {
                        toggleSet._firstInSet = null;
                        toggleSet._count = indexOfState = 0;
                        toggleSet.IsEmpty = true;
                        delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.IsEmpty)));
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
                        _ = toggleSet._trueAccessor.Remove(node);
                        delegateDeference.DeferAction(() =>
                        {
                            toggleSet.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, indexOfState));
                            toggleSet.True.RaiseCountChanged();
                        });
                        if (toggleSet.True.Count > 0)
                            return true;
                        toggleSet.AnyTrue = false;
                        if (toggleSet.AllTrue)
                        {
                            toggleSet.AllTrue = false;
                            delegateDeference.DeferAction(() =>
                            {
                                toggleSet.RaisePropertyChanged(nameof(toggleSet.AllTrue));
                                toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyTrue));
                            });
                            return true;
                        }
                        delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyTrue)));
                        if (toggleSet.AnyFalse)
                        {
                            if (!toggleSet.AnyIndeterminate)
                            {
                                toggleSet.AllFalse = true;
                                delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllFalse)));
                            }
                        }
                        else
                        {
                            toggleSet.AllIndeterminate = true;
                            delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllIndeterminate)));
                        }
                        return true;
                    }
                    _ = toggleSet._falseAccessor.Remove(node);
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, indexOfState));
                        toggleSet.False.RaiseCountChanged();
                    });
                    if (toggleSet.False.Count > 0)
                        return true;
                    toggleSet.AnyFalse = false;
                    if (toggleSet.AllFalse)
                    {
                        toggleSet.AllFalse = false;
                        delegateDeference.DeferAction(() =>
                        {
                            toggleSet.RaisePropertyChanged(nameof(toggleSet.AllFalse));
                            toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyFalse));
                        });
                        return true;
                    }
                    delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyFalse)));
                    if (toggleSet.AnyTrue)
                    {
                        if (!toggleSet.AnyIndeterminate)
                        {
                            toggleSet.AllTrue = true;
                            delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllTrue)));
                        }
                    }
                    else
                    {
                        toggleSet.AllIndeterminate = true;
                        delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllIndeterminate)));
                    }
                    return true;
                }
                _ = toggleSet._indeterminateAccessor.Remove(node);
                delegateDeference.DeferAction(() =>
                {
                    toggleSet.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, indexOfState));
                    toggleSet.Indeterminate.RaiseCountChanged();
                });
                if (toggleSet.Indeterminate.Count > 0)
                    return true;
                toggleSet.AnyIndeterminate = false;
                if (toggleSet.AllIndeterminate)
                {
                    toggleSet.AllFalse = false;
                    delegateDeference.DeferAction(() =>
                    {
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AllIndeterminate));
                        toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyIndeterminate));
                    });
                    return true;
                }
                delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AnyIndeterminate)));
                if (toggleSet.AnyTrue)
                {
                    if (!toggleSet.AnyFalse)
                    {
                        toggleSet.AllTrue = true;
                        delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllTrue)));
                    }
                }
                else
                {
                    toggleSet.AllFalse = true;
                    delegateDeference.DeferAction(() => toggleSet.RaisePropertyChanged(nameof(toggleSet.AllFalse)));
                }
                return true;
            }

            internal static bool Set(ToggleSet<T> target, T value, bool? state, IDelegateDeference<ToggleSet<T>> delegateDeference)
            {
                Node node;
                if (target._lastInSet is null)
                {
                    target.IsEmpty = false;
                    node = new(target, value, state);
                    target._firstInSet = target._lastInSet = node;
                    target._count = 1;
                    delegateDeference.DeferAction(() =>
                    {
                        target.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                        target.All.RaiseCountChanged();
                        target.RaisePropertyChanged(nameof(target.IsEmpty));
                    });
                    if (state.HasValue)
                    {
                        if (state.Value)
                        {
                            target._trueAccessor.Add(node);
                            target.AnyTrue = target.AllTrue = true;
                            delegateDeference.DeferAction(() =>
                            {
                                target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                                target.True.RaiseCountChanged();
                                target.RaisePropertyChanged(nameof(target.AnyTrue));
                                target.RaisePropertyChanged(nameof(target.AllTrue));
                            });
                            return true;
                        }
                        target._falseAccessor.Add(node);
                        target.AnyFalse = target.AllFalse = true;
                        delegateDeference.DeferAction(() =>
                        {
                            target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                            target.False.RaiseCountChanged();
                            target.RaisePropertyChanged(nameof(target.AnyFalse));
                            target.RaisePropertyChanged(nameof(target.AllFalse));
                        });
                        return true;
                    }
                    target._indeterminateAccessor.Add(node);
                    target.AnyIndeterminate = target.AllIndeterminate = true;
                    delegateDeference.DeferAction(() =>
                    {
                        target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));
                        target.Indeterminate.RaiseCountChanged();
                        target.RaisePropertyChanged(nameof(target.AnyIndeterminate));
                        target.RaisePropertyChanged(nameof(target.AllIndeterminate));
                    });
                    return true;
                }
                node = target.FindNode(value);
                if (node is not null)
                {
                    if (node.State == state)
                        return false;
                    node.State = state;
                    return true;
                }
                node = new(target, value, state);
                target._lastInSet = (node.PreviousInSet = target._lastInSet).NextInSet = node;
                int indexInSet = target._count++;
                delegateDeference.DeferAction(() =>
                {
                    target.All.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexInSet));
                    target.All.RaiseCountChanged();
                });
                int indexOfState;
                if (state.HasValue)
                {
                    if (state.Value)
                    {
                        indexOfState = target._trueAccessor.Count;
                        target._trueAccessor.Add(node);
                        delegateDeference.DeferAction(() =>
                        {
                            target.True.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                            target.True.RaiseCountChanged();
                        });
                        if (!target.AnyTrue)
                        {
                            target.AnyTrue = true;
                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyTrue)));
                        }
                        if (target.AllFalse)
                        {
                            target.AllFalse = false;
                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllFalse)));
                        }
                        else if (target.AllIndeterminate)
                        {
                            target.AllIndeterminate = false;
                            delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllIndeterminate)));
                        }
                        return true;
                    }
                    indexOfState = target._falseAccessor.Count;
                    target._falseAccessor.Add(node);
                    delegateDeference.DeferAction(() =>
                    {
                        target.False.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                        target.False.RaiseCountChanged();
                        target.RaisePropertyChanged(nameof(target.AnyFalse));
                    });
                    if (!target.AnyFalse)
                    {
                        target.AnyFalse = true;
                        delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyFalse)));
                    }
                    if (target.AllTrue)
                    {
                        target.AllTrue = false;
                        delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllTrue)));
                    }
                    else if (target.AllIndeterminate)
                    {
                        target.AllIndeterminate = false;
                        delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllIndeterminate)));
                    }
                    return true;
                }
                indexOfState = target._indeterminateAccessor.Count;
                target._indeterminateAccessor.Add(node);
                delegateDeference.DeferAction(() =>
                {
                    target.Indeterminate.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, indexOfState));
                    target.Indeterminate.RaiseCountChanged();
                });
                if (!target.AnyIndeterminate)
                {
                    target.AnyIndeterminate = true;
                    delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AnyIndeterminate)));
                }
                if (target.AllTrue)
                {
                    target.AllTrue = false;
                    delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllTrue)));
                }
                else if (target.AllFalse)
                {
                    target.AllFalse = false;
                    delegateDeference.DeferAction(() => target.RaisePropertyChanged(nameof(target.AllFalse)));
                }
                return true;
            }

            internal class AccessorBase
            {
                protected AccessorBase(StateSet target)
                {
                    Target = target;
                }

                protected StateSet Target { get; }

                protected static void InsertOfState(Node item, Node previous) => previous.NextOfState = (item.NextOfState = (item.PreviousOfState = previous).NextOfState) is not null
                        ? (item.NextOfState.PreviousOfState = item)
                        : item;

                protected static void RemoveOfState(Node node)
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
                protected static void ClearOfState([DisallowNull] Node node)
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
    }
}
