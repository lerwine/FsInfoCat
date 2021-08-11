using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat
{
    partial class DeferredDelegationService
    {
        internal sealed partial class DelegateDeference<TTarget> where TTarget : class
        {
            internal sealed class DeferenceCollection : IDeferenceCollection
            {
                private bool _isDisposed;
                private readonly DeferredDelegationService _service;
                private readonly LinkedList<WeakReference<DelegateDeference<TTarget>>> _deferences = new();
                private readonly ConcurrentQueue<(Delegate Event, object[] Args)> _queue = new();

                private DeferenceCollection(DeferredDelegationService service, TTarget target, object syncRoot)
                {
                    _service = service;
                    Target = target;
                    SyncRoot = syncRoot;
                }

                public object SyncRoot { get; }

                public TTarget Target { get; }

                object IDelegateDeference.Target => Target;

                public int EventQueueCount => _queue.Count;

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
                    _service._deferences.Remove(this);
                    Dispose();
                    return false;
                }

                public void AddAction(Action action) => AddDelegate(action);

                public void AddAction<TArg>(TArg arg, Action<TArg> action) => AddDelegate(action, arg);

                public void AddAction<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Action<TArg1, TArg2> action) => AddDelegate(action, arg1, arg2);

                public void AddAction<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Action<TArg1, TArg2, TArg3> action) => AddDelegate(action, arg1, arg2, arg3);

                public void AddDelegate(Delegate @delegate, params object[] args)
                {
                    if (@delegate is null)
                        throw new ArgumentNullException(nameof(@delegate));
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(DeferenceCollection));
                    _queue.Enqueue((delegate, args ?? Array.Empty<object>()));
                }

                public void ReleaseEvents()
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(DeferenceCollection));
                    while (_queue.TryDequeue(out (Delegate Event, object[] Args) d))
                        d.Event.DynamicInvoke(d.Args);
                }

                public void Dispose()
                {
                    if (!_isDisposed)
                    {
                        _isDisposed = true;
                        while (_queue.TryDequeue(out (Delegate Event, object[] Args) d))
                            d.Event.DynamicInvoke(d.Args);
                    }
                    GC.SuppressFinalize(this);
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
                    DeferenceCollection collection = new(service, target, syncRoot);
                    service._deferences.AddLast(collection);
                    return new DelegateDeference<TTarget>(collection);
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
                    DeferenceCollection collection = new(service, target, syncRoot);
                    service._deferences.AddLast(collection);
                    return new DelegateDeference<TTarget>(collection);
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
                        DeferenceCollection collection = new(service, target, syncRoot);
                        service._deferences.AddLast(collection);
                        deference = new DelegateDeference<TTarget>(collection);
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
                                DeferenceCollection collection = new(service, target, syncRoot);
                                service._deferences.AddLast(collection);
                                deference = new DelegateDeference<TTarget>(collection);
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
                        DeferenceCollection collection = new(service, target, syncRoot);
                        service._deferences.AddLast(collection);
                        deference = new DelegateDeference<TTarget>(collection);
                        return true;
                    }
                    deference = null;
                    return false;
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<TTarget> deference)
                {
                    throw new NotImplementedException();
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, int millisecondsTimeout, out IDelegateDeference<TTarget> deference)
                {
                    throw new NotImplementedException();
                }

                internal static bool TryEnterNew([DisallowNull] DeferredDelegationService service, [DisallowNull] TTarget target, [DisallowNull] object syncRoot, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<TTarget> deference)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
