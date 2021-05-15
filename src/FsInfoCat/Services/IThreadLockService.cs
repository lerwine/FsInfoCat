using System;
using System.Collections;

namespace FsInfoCat.Services
{
    public interface IThreadLockService
    {
        IDisposable GetThreadLock(ISuspendable suspendable);
        IDisposable GetThreadLock(ICollection collection);
        bool TryGetThreadLock(ISuspendable suspendable, TimeSpan timeout, out IDisposable result);
        bool TryGetThreadLock(ICollection collection, TimeSpan timeout, out IDisposable result);
        bool TryGetThreadLock(ISuspendable suspendable, int millisecondsTimeout, out IDisposable result);
        bool TryGetThreadLock(ICollection collection, int millisecondsTimeout, out IDisposable result);
        bool TryGetThreadLock(ISuspendable suspendable, out IDisposable result);
        bool TryGetThreadLock(ICollection collection, out IDisposable result);
    }
}
