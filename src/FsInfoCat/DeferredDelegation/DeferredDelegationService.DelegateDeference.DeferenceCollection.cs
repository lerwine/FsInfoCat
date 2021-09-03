using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat.DeferredDelegation
{
    partial class DeferredDelegationService
    {
        internal sealed partial class DelegateDeference<TTarget> where TTarget : class
        {
            internal sealed class DeferenceCollection : IDelegateQueueing<TTarget>, IDeferenceCollection
            {
                private bool _isDisposed;
                private readonly DeferredDelegationService _service;
                private readonly LinkedList<WeakReference<DelegateDeference<TTarget>>> _deferences = new();
                private readonly ConcurrentQueue<(Delegate Delegate, Delegate ErrorHandler, object[] Args)> _queue = new();
                private readonly ILogger<DelegateDeference<TTarget>> _logger;

                private DeferenceCollection(DeferredDelegationService service, TTarget target, object syncRoot)
                {
                    _service = service;
                    Target = target;
                    SyncRoot = syncRoot;
                    using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                    _logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<DelegateDeference<TTarget>>>();
                }

                public event UnhandledExceptionEventHandler UnhandledException;

                public object SyncRoot { get; }

                public TTarget Target { get; }

                object IDelegateQueueing.Target => Target;

                public int DelegateQueueCount => _queue.Count;

                public bool Verify()
                {
                    if (_isDisposed)
                        return false;
                    while (_deferences.First is not null)
                    {
                        if (_deferences.First.Value.TryGetTarget(out _))
                            return true;
                        _deferences.Remove(_deferences.First);
                    }
                    _service._deferenceCollections.Remove(this);
                    Dispose();
                    return false;
                }

                private void EnqueueDelegateInvocation([DisallowNull] Delegate @delegate, [AllowNull] Delegate onError, params object[] args)
                {
                    if (@delegate is null)
                        throw new ArgumentNullException(nameof(@delegate));
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(DeferenceCollection));
                    _queue.Enqueue((@delegate, onError, args ?? Array.Empty<object>()));
                }

                public void DeferDelegate([DisallowNull] Delegate @delegate, params object[] args) => EnqueueDelegateInvocation(@delegate, null, args);

                public void DeferDelegateWithErrorHandler([DisallowNull] Delegate @delegate, [DisallowNull] DeferredDelegateErrorHandler onError, params object[] args) =>
                    EnqueueDelegateInvocation(@delegate, onError, args);

                public void DeferAction(Action action, DeferredActionErrorHandler onError = null) => EnqueueDelegateInvocation(action, onError);

                public void DeferAction<TArg>(TArg arg, Action<TArg> action, DeferredActionErrorHandler<TArg> onError = null) => EnqueueDelegateInvocation(action, onError, arg);

                public void DeferAction<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Action<TArg1, TArg2> action, DeferredActionErrorHandler<TArg1, TArg2> onError = null) =>
                    EnqueueDelegateInvocation(action, onError, arg1, arg2);

                public void DeferAction<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Action<TArg1, TArg2, TArg3> action, DeferredActionErrorHandler<TArg1, TArg2, TArg3> onError = null) =>
                    EnqueueDelegateInvocation(action, onError, arg1, arg2, arg3);

                public void DeferEvent<TEventArgs>([DisallowNull] object sender, [DisallowNull] TEventArgs eventArgs, [DisallowNull] EventHandler<TEventArgs> eventHandler,
                    DeferredEventErrorHandler<TEventArgs> onError = null) where TEventArgs : EventArgs => EnqueueDelegateInvocation(eventHandler, onError, sender, eventArgs);

                public void DeferEvent([DisallowNull] object sender, [DisallowNull] EventArgs eventArgs, [DisallowNull] EventHandler eventHandler, DeferredEventErrorHandler<EventArgs> onError = null) =>
                    EnqueueDelegateInvocation(eventHandler, onError, sender, eventArgs);

                public void DeferPropertyChangedEvent([DisallowNull] INotifyPropertyChanged sender, [DisallowNull] PropertyChangedEventArgs eventArgs, [DisallowNull] PropertyChangedEventHandler eventHandler, DeferredEventErrorHandler<PropertyChangedEventArgs> onError = null)
                {
                    // TODO: Implement DeferPropertyChangedEvent(INotifyPropertyChanged, PropertyChangedEventArg, PropertyChangedEventHandler, DeferredEventErrorHandler<PropertyChangedEventArgs>)
                    throw new NotImplementedException();
                }

                public void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, [DisallowNull] NotifyCollectionChangedEventArgs eventArgs, [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null)
                {
                    // TODO: Implement DeferPropertyChangedEvent(INotifyPropertyChanged, NotifyCollectionChangedEventArgs, NotifyCollectionChangedEventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs>)
                    throw new NotImplementedException();
                }

                public void DeferUnhandledExceptionEvent([DisallowNull] object sender, [DisallowNull] UnhandledExceptionEventArgs eventArgs, [DisallowNull] UnhandledExceptionEventHandler eventHandler)
                {
                    // TODO: Implement DeferUnhandledExceptionEvent(object, UnhandledExceptionEventArgs, UnhandledExceptionEventHandler)
                    throw new NotImplementedException();
                }

                public void DeferEvent([DisallowNull] object sender, [DisallowNull] EventHandler eventHandler, DeferredEventErrorHandler<EventArgs> onError = null)
                {
                    // TODO: Implement DeferEvent([DisallowNull] object, EventHandler, DeferredEventErrorHandler<EventArgs>)
                    throw new NotImplementedException();
                }

                private static DelegateDeference<TTarget> AddFirstDeference([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot)
                {
                    using IDisposable scope = service._logger.BeginScope("{MethodName}({service}, {target}, {syncRoot})", nameof(AddFirstDeference), service, target, syncRoot);
                    Monitor.Enter(service._deferenceCollections);
                    try
                    {
                        DeferenceCollection collection = new(service, target, syncRoot);
                        service._deferenceCollections.AddLast(collection);
                        return collection.AddDeference();
                    }
                    finally { Monitor.Exit(service._deferenceCollections); }
                }

                private DelegateDeference<TTarget> AddDeference()
                {
                    using IDisposable scope = _logger.BeginScope("{MethodName}()", nameof(AddDeference));
                    Monitor.Enter(_deferences);
                    try
                    {
                        DelegateDeference<TTarget> deference = new(this);
                        deference.Disposed += Deference_Disposed;
                        _deferences.AddLast(new WeakReference<DelegateDeference<TTarget>>(deference));
                        return deference;
                    }
                    finally { Monitor.Exit(_deferences); }
                }

                private void Deference_Disposed(object sender, EventArgs e)
                {
                    if (sender is DelegateDeference<TTarget> target)
                    {
                        using IDisposable scope = _logger.BeginScope("{MethodName}({sender}, {e})", nameof(Deference_Disposed), sender, e);
                        Monitor.Enter(_deferences);
                        try
                        {
                            // Start with first node and look for matching instance
                            LinkedListNode<WeakReference<DelegateDeference<TTarget>>> node = _deferences.First;
                            while (node is not null)
                            {
                                if (node.Value.TryGetTarget(out DelegateDeference<TTarget> item))
                                {
                                    if (ReferenceEquals(item, target))
                                    {
                                        // Get next node and remove the node with the matching value.
                                        LinkedListNode<WeakReference<DelegateDeference<TTarget>>> next = node.Next;
                                        _deferences.Remove(node);
                                        node = next;
                                        break;
                                    }
                                    node = node.Next;
                                }
                                else
                                {
                                    // Get next node and remove the node that no longe reference anything.
                                    LinkedListNode<WeakReference<DelegateDeference<TTarget>>> next = node.Next;
                                    _deferences.Remove(node);
                                    node = next;
                                }
                            }
                            // Remove nodes which no longer reference anything until we find one that does.
                            while (node is not null)
                            {
                                if (node.Value.TryGetTarget(out _))
                                    break;
                                LinkedListNode<WeakReference<DelegateDeference<TTarget>>> next = node.Next;
                                _deferences.Remove(node);
                                node = next;
                            }
                            // Self-dispose if no nodes are left
                            if (_deferences.Count == 0)
                                Dispose();
                        }
                        finally { Monitor.Exit(_deferences); }
                    }
                }

                internal static IDelegateDeference<TTarget> Enter([DisallowNull] DeferenceCollection collection)
                {
                    if (collection is null)
                        throw new ArgumentNullException(nameof(collection));
                    Monitor.Enter(collection.SyncRoot);
                    return collection.AddDeference();
                }

                internal static IDelegateDeference<TTarget> Enter([DisallowNull] DeferenceCollection collection, ref bool lockTaken)
                {
                    if (collection is null)
                        throw new ArgumentNullException(nameof(collection));
                    Monitor.Enter(collection.SyncRoot, ref lockTaken);
                    return collection.AddDeference();
                }

                internal static bool TryEnter([DisallowNull] DeferenceCollection collection, out IDelegateDeference<TTarget> deference)
                {
                    if (collection is null)
                        throw new ArgumentNullException(nameof(collection));
                    if (Monitor.TryEnter(collection.Target))
                    {
                        deference = collection.AddDeference();
                        return true;
                    }
                    deference = null;
                    return false;
                }

                internal static bool TryEnter([DisallowNull] DeferenceCollection collection, ref bool lockTaken, out IDelegateDeference<TTarget> deference)
                {
                    if (collection is null)
                        throw new ArgumentNullException(nameof(collection));
                    Thread.BeginCriticalRegion();
                    try
                    {
                        if (!lockTaken)
                        {
                            try { Monitor.TryEnter(collection.Target, ref lockTaken); }
                            catch
                            {
                                if (!lockTaken)
                                    throw;
                            }
                            if (lockTaken)
                            {
                                deference = collection.AddDeference();
                                return true;
                            }
                        }
                    }
                    finally { Thread.EndCriticalRegion(); }
                    deference = null;
                    return false;
                }

                internal static bool TryEnter([DisallowNull] DeferenceCollection collection, TimeSpan timeout, out IDelegateDeference<TTarget> deference)
                {
                    if (collection is null)
                        throw new ArgumentNullException(nameof(collection));
                    if (Monitor.TryEnter(collection.Target, timeout))
                    {
                        deference = collection.AddDeference();
                        return true;
                    }
                    deference = null;
                    return false;
                }

                internal static bool TryEnter([DisallowNull] DeferenceCollection collection, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<TTarget> deference)
                {
                    if (collection is null)
                        throw new ArgumentNullException(nameof(collection));
                    Thread.BeginCriticalRegion();
                    try
                    {
                        if (!lockTaken)
                        {
                            try { Monitor.TryEnter(collection.Target, timeout, ref lockTaken); }
                            catch
                            {
                                if (!lockTaken)
                                    throw;
                            }
                            if (lockTaken)
                            {
                                deference = collection.AddDeference();
                                return true;
                            }
                        }
                    }
                    finally { Thread.EndCriticalRegion(); }
                    deference = null;
                    return false;
                }

                internal static bool TryEnter([DisallowNull] DeferenceCollection collection, int millisecondsTimeout, out IDelegateDeference<TTarget> deference)
                {
                    if (collection is null)
                        throw new ArgumentNullException(nameof(collection));
                    if (Monitor.TryEnter(collection.Target, millisecondsTimeout))
                    {
                        deference = collection.AddDeference();
                        return true;
                    }
                    deference = null;
                    return false;
                }

                internal static bool TryEnter([DisallowNull] DeferenceCollection collection, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<TTarget> deference)
                {
                    if (collection is null)
                        throw new ArgumentNullException(nameof(collection));
                    Thread.BeginCriticalRegion();
                    try
                    {
                        if (!lockTaken)
                        {
                            try { Monitor.TryEnter(collection.Target, millisecondsTimeout, ref lockTaken); }
                            catch
                            {
                                if (!lockTaken)
                                    throw;
                            }
                            if (lockTaken)
                            {
                                deference = collection.AddDeference();
                                return true;
                            }
                        }
                    }
                    finally { Thread.EndCriticalRegion(); }
                    deference = null;
                    return false;
                }

                internal static IDelegateDeference<TTarget> EnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot)
                {
                    if (service is null)
                        throw new ArgumentNullException(nameof(service));
                    if (target is null)
                        throw new ArgumentNullException(nameof(target));
                    if (syncRoot is null)
                        throw new ArgumentNullException(nameof(syncRoot));
                    Monitor.Enter(syncRoot);
                    return AddFirstDeference(service, target, syncRoot);
                }

                internal static IDelegateDeference<TTarget> EnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, ref bool lockTaken)
                {
                    if (service is null)
                        throw new ArgumentNullException(nameof(service));
                    if (target is null)
                        throw new ArgumentNullException(nameof(target));
                    if (syncRoot is null)
                        throw new ArgumentNullException(nameof(syncRoot));
                    Monitor.Enter(syncRoot, ref lockTaken);
                    return AddFirstDeference(service, target, syncRoot);
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, out IDelegateDeference<TTarget> deference)
                {
                    if (service is null)
                        throw new ArgumentNullException(nameof(service));
                    if (target is null)
                        throw new ArgumentNullException(nameof(target));
                    if (syncRoot is null)
                        throw new ArgumentNullException(nameof(syncRoot));
                    if (Monitor.TryEnter(syncRoot))
                    {
                        deference = AddFirstDeference(service, target, syncRoot);
                        return true;
                    }
                    deference = null;
                    return false;
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, ref bool lockTaken, out IDelegateDeference<TTarget> deference)
                {
                    Thread.BeginCriticalRegion();
                    try
                    {
                        if (!lockTaken)
                        {
                            try { Monitor.TryEnter(syncRoot, ref lockTaken); }
                            catch
                            {
                                if (!lockTaken)
                                    throw;
                            }
                            if (lockTaken)
                            {
                                deference = AddFirstDeference(service, target, syncRoot);
                                return true;
                            }
                        }
                    }
                    finally { Thread.EndCriticalRegion(); }
                    deference = null;
                    return false;
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, TimeSpan timeout, out IDelegateDeference<TTarget> deference)
                {
                    if (service is null)
                        throw new ArgumentNullException(nameof(service));
                    if (target is null)
                        throw new ArgumentNullException(nameof(target));
                    if (syncRoot is null)
                        throw new ArgumentNullException(nameof(syncRoot));
                    if (Monitor.TryEnter(syncRoot, timeout))
                    {
                        deference = AddFirstDeference(service, target, syncRoot);
                        return true;
                    }
                    deference = null;
                    return false;
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, TimeSpan timeout, ref bool lockTaken,
                    out IDelegateDeference<TTarget> deference)
                {
                    Thread.BeginCriticalRegion();
                    try
                    {
                        if (!lockTaken)
                        {
                            try { Monitor.TryEnter(syncRoot, timeout, ref lockTaken); }
                            catch
                            {
                                if (!lockTaken)
                                    throw;
                            }
                            if (lockTaken)
                            {
                                deference = AddFirstDeference(service, target, syncRoot);
                                return true;
                            }
                        }
                    }
                    finally { Thread.EndCriticalRegion(); }
                    deference = null;
                    return false;
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, int millisecondsTimeout,
                    out IDelegateDeference<TTarget> deference)
                {
                    if (service is null)
                        throw new ArgumentNullException(nameof(service));
                    if (target is null)
                        throw new ArgumentNullException(nameof(target));
                    if (syncRoot is null)
                        throw new ArgumentNullException(nameof(syncRoot));
                    if (Monitor.TryEnter(syncRoot, millisecondsTimeout))
                    {
                        deference = AddFirstDeference(service, target, syncRoot);
                        return true;
                    }
                    deference = null;
                    return false;
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, int millisecondsTimeout, ref bool lockTaken,
                    out IDelegateDeference<TTarget> deference)
                {
                    Thread.BeginCriticalRegion();
                    try
                    {
                        if (!lockTaken)
                        {
                            try { Monitor.TryEnter(syncRoot, millisecondsTimeout, ref lockTaken); }
                            catch
                            {
                                if (!lockTaken)
                                    throw;
                            }
                            if (lockTaken)
                            {
                                deference = AddFirstDeference(service, target, syncRoot);
                                return true;
                            }
                        }
                    }
                    finally { Thread.EndCriticalRegion(); }
                    deference = null;
                    return false;
                }

                private void InvokeDeferred((Delegate Delegate, Delegate ErrorHandler, object[] Args) deferredInvocation)
                {
                    using IDisposable scope = _logger.BeginScope("{MethodName}({Delegate}, {ErrorHandler}, {Args})", nameof(InvokeDeferred), deferredInvocation.Delegate, deferredInvocation.ErrorHandler, deferredInvocation.Args);
                    try { deferredInvocation.Delegate.DynamicInvoke(deferredInvocation.Args); }
                    catch (Exception exception)
                    {
                        if (deferredInvocation.ErrorHandler is null)
                            RaiseUnhandledExceptionEvent(exception);
                        else
                            try { deferredInvocation.ErrorHandler.DynamicInvoke(new object[] { exception }.Concat(deferredInvocation.Args).ToArray()); }
                            catch (Exception error) { RaiseUnhandledExceptionEvent(error); }
                    }
                }

                private void RaiseUnhandledExceptionEvent(Exception exception)
                {
                    _logger.LogError(exception, "Unhandled error");
                    UnhandledException?.Invoke(this, new UnhandledExceptionEventArgs(exception, false));
                }

                public void DequeueDelegates()
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(DeferenceCollection));
                    while (_queue.TryDequeue(out (Delegate Delegate, Delegate ErrorHandler, object[] Args) deferredInvocation))
                        InvokeDeferred(deferredInvocation);
                }

                public void Dispose()
                {
                    if (!_isDisposed)
                    {
                        using IDisposable scope = _logger.BeginScope("{MethodName}()", nameof(Dispose));
                        _isDisposed = true;
                        if (Monitor.IsEntered(SyncRoot))
                            Monitor.Exit(SyncRoot);
                        try
                        {
                            Monitor.Enter(_service._deferenceCollections);
                            try { _service._deferenceCollections.Remove(this); }
                            finally { Monitor.Exit(_service._deferenceCollections); }
                        }
                        finally
                        {
                            while (_queue.TryDequeue(out (Delegate Delegate, Delegate ErrorHandler, object[] Args) deferredInvocation))
                                InvokeDeferred(deferredInvocation);
                        }
                    }
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}
