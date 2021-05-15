using FsInfoCat.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FsInfoCat.Internal
{
    class Suspendable : ISuspendable
    {
        private static readonly IThreadLockService _threadLockService = Extensions.GetThreadLockService();
        private Suspension _latest;

        public event EventHandler BeginSuspension;

        public event EventHandler EndSuspension;

        public event PropertyChangedEventHandler PropertyChanged;

        public object SyncRoot { get; } = new object();

        public bool IsSuspended => !(_latest is null);

        public Suspendable() { }

        public void AssertNotSuspended()
        {
            if (IsSuspended)
                throw new InvalidOperationException();
        }

        public void AssertNotSuspended(Action action)
        {
            using (var threadLock = _threadLockService.GetThreadLock(this))
            {
                if (IsSuspended)
                    throw new InvalidOperationException();
                action();
            }
        }

        public TResult AssertNotSuspended<TResult>(Func<TResult> function)
        {
            using (var threadLock = _threadLockService.GetThreadLock(this))
            {
                if (IsSuspended)
                    throw new InvalidOperationException();
                return function();
            }
        }

        public bool IfNotSuspended(Action ifNotSuspended)
        {
            using (var threadLock = _threadLockService.GetThreadLock(this))
            {
                if (IsSuspended)
                    return false;
                ifNotSuspended();
            }
            return true;
        }

        public bool IfSuspended(Action ifSuspended)
        {
            using (var threadLock = _threadLockService.GetThreadLock(this))
            {
                if (IsSuspended)
                    ifSuspended();
                else
                    return false;
            }
            return true;
        }

        public void IfNotSuspended(Action ifNotSuspended, Action ifSuspended)
        {
            using (var threadLock = _threadLockService.GetThreadLock(this))
            {
                if (IsSuspended)
                    ifSuspended();
                else
                    ifNotSuspended();
            }
        }

        public TResult IfNotSuspended<TResult>(Func<TResult> ifNotSuspended, Func<TResult> ifSuspended)
        {
            using (var threadLock = _threadLockService.GetThreadLock(this))
            {
                if (IsSuspended)
                    return ifSuspended();
                return ifNotSuspended();
            }
        }

        public TResult IfNotSuspended<TResult>(Func<TResult> ifNotSuspended, TResult ifSuspended)
        {
            using (var threadLock = _threadLockService.GetThreadLock(this))
            {
                if (IsSuspended)
                    return ifSuspended;
                return ifNotSuspended();
            }
        }

        public TResult IfNotSuspended<TResult>(TResult ifNotSuspended, Func<TResult> ifSuspended)
        {
            using (var threadLock = _threadLockService.GetThreadLock(this))
            {
                if (IsSuspended)
                    return ifSuspended();
                return ifNotSuspended;
            }
        }

        protected void OnBeginSuspension() => BeginSuspension?.Invoke(this, EventArgs.Empty);

        protected void OnEndSuspension() => EndSuspension?.Invoke(this, EventArgs.Empty);

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        public ISuspension Suspend() => Suspend(false);

        public ISuspension Suspend(bool noThreadLock) => Suspension.Create(this, noThreadLock ? null : _threadLockService.GetThreadLock(this));

        private class Suspension : ISuspension
        {
            private bool _isDisposed;
            private readonly IDisposable _threadLock;
            private readonly Suspendable _suspendable;
            private Suspension _previous;
            private Suspension _next;

            private Suspension(Suspendable suspendable, IDisposable threadLock)
            {
                _threadLock = threadLock;
                _suspendable = suspendable;
            }
            internal static Suspension Create(Suspendable suspendable, IDisposable threadLock)
            {
                Suspension result = new Suspension(suspendable ?? throw new ArgumentNullException(nameof(suspendable)), threadLock);
                try
                {
                    bool enteredSuspension;
                    using (threadLock = _threadLockService.GetThreadLock(suspendable))
                    {
                        enteredSuspension = (result._previous = suspendable._latest) is null;
                        if (enteredSuspension)
                        {
                            result.ConcurrencyToken = new object();
                            suspendable._latest = result;
                        }
                        else
                        {
                            result.ConcurrencyToken = suspendable._latest.ConcurrencyToken;
                            suspendable._latest = result._previous._next = result;
                        }
                    }
                    if (enteredSuspension)
                        try { suspendable.RaisePropertyChanged(nameof(IsSuspended)); }
                        finally { suspendable.OnBeginSuspension(); }
                }
                catch
                {
                    result.Dispose();
                    throw;
                }
                return result;
            }

            public object ConcurrencyToken { get; private set; }

            public void Dispose()
            {
                bool exitedSuspension;
                Thread.BeginCriticalRegion();
                if (_isDisposed)
                    exitedSuspension = false;
                else
                    exitedSuspension = _isDisposed = true;
                Thread.EndCriticalRegion();
                if (!exitedSuspension)
                    return;
                using (IDisposable threadLock = _threadLock ?? _threadLockService.GetThreadLock(_suspendable))
                {
                    if (_next is null)
                    {
                        exitedSuspension = (_suspendable._latest = _previous) is null;
                        if (!exitedSuspension)
                            _previous._next = null;
                    }
                    else
                    {
                        exitedSuspension = false;
                        if (!((_next._previous = _previous) is null))
                            _previous._next = _next;
                    }
                }
                if (!(_threadLock is null))
                    _threadLock.Dispose();
                if (exitedSuspension)
                    try { _suspendable.RaisePropertyChanged(nameof(IsSuspended)); }
                    finally { _suspendable.OnEndSuspension(); }
                GC.SuppressFinalize(this);
            }
        }
    }
}
