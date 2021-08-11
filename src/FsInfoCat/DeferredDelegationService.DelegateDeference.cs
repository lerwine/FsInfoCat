using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat
{
    partial class DeferredDelegationService
    {
        partial class DelegateDeference<TTarget> : IDelegateDeference<TTarget> where TTarget : class
        {
            private bool _isDisposed;
            private readonly DeferenceCollection _owner;

            private DelegateDeference(DeferenceCollection owner)
            {
                _owner = owner;
            }

            public int EventQueueCount => _owner.EventQueueCount;

            public TTarget Target => _owner.Target;

            object IDelegateDeference.Target => _owner.Target;

            public object SyncRoot => _owner.SyncRoot;

            public void ReleaseEvents()
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                _owner.ReleaseEvents();
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    ReleaseEvents();
                    _isDisposed = true;
                }
                GC.SuppressFinalize(this);
            }

            public void AddDelegate([DisallowNull] Delegate @delegate, params object[] args)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                _owner.AddDelegate(@delegate, args);
            }

            public void AddAction([DisallowNull] Action action)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                _owner.AddAction(action);
            }

            public void AddAction<TArg>(TArg arg, [DisallowNull] Action<TArg> action)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                _owner.AddAction(arg, action);
            }

            public void AddAction<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, [DisallowNull] Action<TArg1, TArg2> action)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                _owner.AddAction(arg1, arg2, action);
            }

            public void AddAction<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Action<TArg1, TArg2, TArg3> action)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(DelegateDeference<TTarget>));
                _owner.AddAction(arg1, arg2, arg3, action);
            }

            internal static IDelegateDeference<TTarget> Enter([DisallowNull] DeferenceCollection collection)
            {
                if (collection is null)
                    throw new ArgumentNullException(nameof(collection));
                Monitor.Enter(collection.SyncRoot);
                return new DelegateDeference<TTarget>(collection);
            }

            internal static IDelegateDeference<TTarget> Enter([DisallowNull] DeferenceCollection collection, ref bool lockTaken)
            {
                if (collection is null)
                    throw new ArgumentNullException(nameof(collection));
                Monitor.Enter(collection.SyncRoot, ref lockTaken);
                return new DelegateDeference<TTarget>(collection);
            }

            internal static bool TryEnter([DisallowNull] DeferenceCollection collection, out IDelegateDeference<TTarget> deference)
            {
                if (collection is null)
                    throw new ArgumentNullException(nameof(collection));
                throw new NotImplementedException();
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
                            deference = new DelegateDeference<TTarget>(collection);
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
                    deference = new DelegateDeference<TTarget>(collection);
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
                            deference = new DelegateDeference<TTarget>(collection);
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
                    deference = new DelegateDeference<TTarget>(collection);
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
                            deference = new DelegateDeference<TTarget>(collection);
                            return true;
                        }
                    }
                }
                finally { Thread.EndCriticalRegion(); }
                deference = null;
                return false;
            }
        }
    }
}
