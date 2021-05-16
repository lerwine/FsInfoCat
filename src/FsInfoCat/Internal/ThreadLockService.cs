using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace FsInfoCat.Internal
{
    class ThreadLockService : IThreadLockService
    {
        public IDisposable GetThreadLock(ISuspendable suspendable) => ExclusiveLock.Enter((suspendable ?? throw new ArgumentNullException(nameof(suspendable))).SyncRoot);

        public IDisposable GetThreadLock(ICollection collection) => ExclusiveLock.Enter((collection ?? throw new ArgumentNullException(nameof(collection))).SyncRoot);

        public bool TryGetThreadLock(ISuspendable suspendable, TimeSpan timeout, out IDisposable result) => ExclusiveLock.TryEnter((suspendable ?? throw new ArgumentNullException(nameof(suspendable))).SyncRoot, timeout, out result);

        public bool TryGetThreadLock(ICollection collection, TimeSpan timeout, out IDisposable result) => ExclusiveLock.TryEnter((collection ?? throw new ArgumentNullException(nameof(collection))).SyncRoot, timeout, out result);

        public bool TryGetThreadLock(ISuspendable suspendable, int millisecondsTimeout, out IDisposable result) => ExclusiveLock.TryEnter((suspendable ?? throw new ArgumentNullException(nameof(suspendable))).SyncRoot,
            millisecondsTimeout, out result);

        public bool TryGetThreadLock(ICollection collection, int millisecondsTimeout, out IDisposable result) => ExclusiveLock.TryEnter((collection ?? throw new ArgumentNullException(nameof(collection))).SyncRoot, millisecondsTimeout, out result);

        public bool TryGetThreadLock(ISuspendable suspendable, out IDisposable result) => ExclusiveLock.TryEnter((suspendable ?? throw new ArgumentNullException(nameof(suspendable))).SyncRoot, out result);

        public bool TryGetThreadLock(ICollection collection, out IDisposable result) => ExclusiveLock.TryEnter((collection ?? throw new ArgumentNullException(nameof(collection))).SyncRoot, out result);

        class ExclusiveLock : IDisposable
        {
            private readonly object _syncRoot;
            private bool _isDisposed;

            private ExclusiveLock(object syncRoot) { _syncRoot = syncRoot; }

            internal static IDisposable Enter(object obj)
            {
                Monitor.Enter(obj);
                return new ExclusiveLock(obj);
            }

            internal static bool TryEnter(object obj, TimeSpan timeout, out IDisposable result)
            {
                if (Monitor.TryEnter(obj, timeout))
                {
                    result = new ExclusiveLock(obj);
                    return true;
                }
                result = null;
                return false;
            }

            internal static bool TryEnter(object obj, int millisecondsTimeout, out IDisposable result)
            {
                if (Monitor.TryEnter(obj, millisecondsTimeout))
                {
                    result = new ExclusiveLock(obj);
                    return true;
                }
                result = null;
                return false;
            }

            internal static bool TryEnter(object obj, out IDisposable result)
            {
                if (Monitor.TryEnter(obj))
                {
                    result = new ExclusiveLock(obj);
                    return true;
                }
                result = null;
                return false;
            }

            public void Dispose()
            {
                Thread.BeginCriticalRegion();
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    Monitor.Exit(_syncRoot);
                }
                Thread.EndCriticalRegion();
                GC.SuppressFinalize(this);
            }
        }
    }
}
