using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.DeferredDelegation
{
    partial class DeferredDelegationService
    {
        partial class DelegateDeference<TTarget> : IDelegateDeference<TTarget> where TTarget : class
        {
            private bool _isDisposed;
            private readonly DeferenceCollection _owner;
            private readonly ILogger<DelegateDeference<TTarget>> _logger;

            internal event EventHandler Disposed;

            private DelegateDeference(DeferenceCollection owner)
            {
                _owner = owner;
                using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
                _logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<DelegateDeference<TTarget>>>();
            }

            public int DelegateQueueCount => _owner.DelegateQueueCount;

            public TTarget Target => _owner.Target;

            object IDelegateQueueing.Target => _owner.Target;

            public object SyncRoot => _owner.SyncRoot;

            public void DeferDelegate([DisallowNull] Delegate @delegate, params object[] args)
            {
                if (@delegate is null)
                    throw new ArgumentNullException(nameof(@delegate));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({delegate}, {args})", nameof(DeferDelegate), @delegate, args))
                    _owner.DeferDelegate(@delegate, args ?? Array.Empty<object>());
            }

            public void DeferDelegateWithErrorHandler([DisallowNull] Delegate @delegate, [DisallowNull] DeferredDelegateErrorHandler onError, params object[] args)
            {
                if (@delegate is null)
                    throw new ArgumentNullException(nameof(@delegate));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({delegate}, {onError}, {args})", nameof(DeferDelegate), @delegate, onError, args))
                    try { _owner.DeferDelegateWithErrorHandler(@delegate, onError, args ?? Array.Empty<object>()); }
                    catch (Exception exception) { onError(exception, args); }
            }

            public void DeferAction([DisallowNull] Action action, DeferredActionErrorHandler onError = null)
            {
                if (action is null)
                    throw new ArgumentNullException(nameof(action));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({action}, {onError})", nameof(DeferAction), action, onError))
                    _owner.DeferAction(action);
            }

            public void DeferAction<TArg>(TArg arg, [DisallowNull] Action<TArg> action, DeferredActionErrorHandler<TArg> onError = null)
            {
                if (action is null)
                    throw new ArgumentNullException(nameof(action));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({arg}, {action}, {onError})", nameof(DeferAction), arg, action, onError))
                    _owner.DeferAction(arg, action);
            }

            public void DeferAction<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, [DisallowNull] Action<TArg1, TArg2> action, DeferredActionErrorHandler<TArg1, TArg2> onError = null)
            {
                if (action is null)
                    throw new ArgumentNullException(nameof(action));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({arg1}, {arg2}, {action}, {onError})", nameof(DeferAction), arg1, arg2, action, onError))
                    _owner.DeferAction(arg1, arg2, action);
            }

            public void DeferAction<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Action<TArg1, TArg2, TArg3> action,
                DeferredActionErrorHandler<TArg1, TArg2, TArg3> onError = null)
            {
                if (action is null)
                    throw new ArgumentNullException(nameof(action));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({arg1}, {arg2}, {arg3}, {action}, {onError})", nameof(DeferAction), arg1, arg2, arg3, action, onError))
                    _owner.DeferAction(arg1, arg2, arg3, action);
            }

            public void DeferPropertyChangedEvent([DisallowNull] INotifyPropertyChanged sender, [DisallowNull] PropertyChangedEventArgs eventArgs, [DisallowNull] PropertyChangedEventHandler eventHandler,
                DeferredEventErrorHandler<PropertyChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventArgs is null)
                    throw new ArgumentNullException(nameof(eventArgs));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {eventArgs}, {eventHandler}, {onError})", nameof(DeferPropertyChangedEvent), sender, eventArgs, eventHandler, onError))
                    _owner.DeferPropertyChangedEvent(sender, eventArgs, eventHandler, onError);
            }

            public void DeferPropertyChangedEvent([DisallowNull] INotifyPropertyChanged sender, [DisallowNull] string propertyName, [DisallowNull] PropertyChangedEventHandler eventHandler,
                DeferredEventErrorHandler<PropertyChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (propertyName is null)
                    throw new ArgumentNullException(nameof(propertyName));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {propertyName}, {eventHandler}, {onError})", nameof(DeferPropertyChangedEvent), sender, propertyName, eventHandler, onError))
                    _owner.DeferPropertyChangedEvent(sender, new PropertyChangedEventArgs(propertyName), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, [DisallowNull] NotifyCollectionChangedEventArgs eventArgs,
                [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventArgs is null)
                    throw new ArgumentNullException(nameof(eventArgs));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {eventArgs}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent), sender, eventArgs, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, eventArgs, eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent), sender, action, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] IList changedItems,
                [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {changedItems}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent), sender, action,
                    changedItems, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, changedItems), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object changedItem,
                [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {changedItem}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent), sender, action,
                    changedItem, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, changedItem), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [DisallowNull] IList newItems, [DisallowNull] IList oldItems,
                [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (newItems is null)
                    throw new ArgumentNullException(nameof(newItems));
                if (oldItems is null)
                    throw new ArgumentNullException(nameof(oldItems));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {newItems}, {oldItems}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent), sender, action,
                    newItems, oldItems, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, newItems, oldItems), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] IList changedItems, int startingIndex,
                [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {changedItems}, {startingIndex}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent), sender,
                    action, changedItems, startingIndex, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, changedItems, startingIndex), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object changedItem, int index,
                [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {changedItem}, {index}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent), sender, action,
                    changedItem, index, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, changedItem, index), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object newItem,
                [AllowNull] object oldItem, [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {newItem}, {oldItem}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent), sender, action,
                    newItem, oldItem, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, newItem, oldItem), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [DisallowNull] IList newItems, [DisallowNull] IList oldItems,
                int startingIndex, [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (newItems is null)
                    throw new ArgumentNullException(nameof(newItems));
                if (oldItems is null)
                    throw new ArgumentNullException(nameof(oldItems));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {newItems}, {oldItems}, {startingIndex}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent),
                    sender, action, newItems, oldItems, startingIndex, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, newItems, oldItems, startingIndex), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] IList changedItems, int index, int oldIndex,
                [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {changedItems}, {index}, {oldIndex}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent),
                    sender, action, changedItems, index, oldIndex, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, changedItems, index, oldIndex), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object changedItem, int index, int oldIndex, [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {changedItem}, {index}, {oldIndex}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent),
                    sender, action, changedItem, index, oldIndex, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, changedItem, index, oldIndex), eventHandler, onError);
            }

            public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object newItem, [AllowNull] object oldItem,
                int index, [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {action}, {newItem}, {oldItem}, {index}, {eventHandler}, {onError})", nameof(DeferCollectionChangedEvent),
                    sender, action, newItem, oldItem, index, eventHandler, onError))
                    _owner.DeferCollectionChangedEvent(sender, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index), eventHandler, onError);
            }

            public void DeferUnhandledExceptionEvent([DisallowNull] object sender, [DisallowNull] UnhandledExceptionEventArgs eventArgs, [DisallowNull] UnhandledExceptionEventHandler eventHandler)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventArgs is null)
                    throw new ArgumentNullException(nameof(eventArgs));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {eventArgs}, {eventHandler})", nameof(DeferEvent), sender, eventArgs, eventHandler))
                    _owner.DeferUnhandledExceptionEvent(sender, eventArgs, eventHandler);
            }

            public void DeferUnhandledExceptionEvent([DisallowNull] object sender, [DisallowNull] Exception exception, [DisallowNull] UnhandledExceptionEventHandler eventHandler, bool isTerminating = false)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (exception is null)
                    throw new ArgumentNullException(nameof(exception));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {exception}, {eventHandler}, {isTerminating})", nameof(DeferEvent), sender, exception, eventHandler, isTerminating))
                    _owner.DeferUnhandledExceptionEvent(sender, new UnhandledExceptionEventArgs(exception, isTerminating), eventHandler);
            }

            public void DeferEvent<TEventArgs>([DisallowNull] object sender, [DisallowNull] TEventArgs eventArgs, [DisallowNull] EventHandler<TEventArgs> eventHandler,
                DeferredEventErrorHandler<TEventArgs> onError = null) where TEventArgs : EventArgs
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventArgs is null)
                    throw new ArgumentNullException(nameof(eventArgs));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {eventArgs}, {eventHandler}, {onError})", nameof(DeferEvent), sender, eventArgs, eventHandler, onError))
                    _owner.DeferEvent(sender, eventArgs, eventHandler, onError);
            }

            public void DeferEvent([DisallowNull] object sender, [DisallowNull] EventArgs eventArgs, [DisallowNull] EventHandler eventHandler, DeferredEventErrorHandler<EventArgs> onError = null)
            {
                if (sender is null)
                    throw new ArgumentNullException(nameof(sender));
                if (eventArgs is null)
                    throw new ArgumentNullException(nameof(eventArgs));
                if (eventHandler is null)
                    throw new ArgumentNullException(nameof(eventHandler));
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}({sender}, {eventArgs}, {eventHandler}, {onError})", nameof(DeferEvent), sender, eventArgs, eventHandler, onError))
                    _owner.DeferEvent(sender, eventArgs, eventHandler, onError);
            }

            public void DeferEvent([DisallowNull] object sender, [DisallowNull] EventHandler eventHandler, DeferredEventErrorHandler<EventArgs> onError = null) =>
                DeferEvent(sender, EventArgs.Empty, eventHandler, onError);

            public void DequeueDelegates()
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                using (_logger.BeginScope("{MethodName}()", nameof(DequeueDelegates)))
                    _owner.DequeueDelegates();
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    using (_logger.BeginScope("{MethodName}()", nameof(Dispose))) { }
                    _isDisposed = true;
                    Disposed?.Invoke(this, EventArgs.Empty);
                }
                GC.SuppressFinalize(this);
            }
        }
    }
}
