using System;
using System.ComponentModel;
using System.Threading;

namespace FsInfoCat.Internal
{
    public class SuspensionProvider : ISuspensionProvider
    {
        private object _token;
        private Suspension _latest;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler BeginSuspension;

        public event EventHandler EndSuspension;

        public bool IsSuspended { get; private set; }

        /// <summary>
        /// Gets the synchronization object used by this suspension provider for thread-safe operations.
        /// </summary>
        /// <value>
        /// The synchronization object used by this suspension provider for thread-safe operations.
        /// </value>
        protected object SyncRoot { get; } = new object();

        public bool IfNotSuspended(Action action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            Monitor.Enter(SyncRoot);
            try
            {
                if (IsSuspended)
                    return false;
                action();
            }
            finally { Monitor.Exit(SyncRoot); }
            return true;
        }

        public TResult NotSuspendedElse<TResult>(Func<TResult> ifSuspendedFalse, Func<TResult> ifSuspendedTrue)
        {
            if (ifSuspendedFalse is null)
                throw new ArgumentNullException(nameof(ifSuspendedFalse));
            if (ifSuspendedTrue is null)
                throw new ArgumentNullException(nameof(ifSuspendedTrue));
            Monitor.Enter(SyncRoot);
            try
            {
                if (IsSuspended)
                    return ifSuspendedTrue();
                return ifSuspendedFalse();
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public TResult NotSuspendedElse<TResult>(Func<TResult> ifSuspendedFalse, TResult ifSuspendedTrue)
        {
            if (ifSuspendedFalse is null)
                throw new ArgumentNullException(nameof(ifSuspendedFalse));
            Monitor.Enter(SyncRoot);
            try
            {
                if (IsSuspended)
                    return ifSuspendedTrue;
                return ifSuspendedFalse();
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public TResult NotSuspendedElse<TResult>(TResult ifSuspendedFalse, Func<TResult> ifSuspendedTrue)
        {
            if (ifSuspendedTrue is null)
                throw new ArgumentNullException(nameof(ifSuspendedTrue));
            Monitor.Enter(SyncRoot);
            try
            {
                if (IsSuspended)
                    return ifSuspendedTrue();
            }
            finally { Monitor.Exit(SyncRoot); }
            return ifSuspendedFalse;
        }

        public bool TryIfNotSuspended<TResult>(Func<TResult> function, out TResult result)
        {
            if (function is null)
                throw new ArgumentNullException(nameof(function));

            Monitor.Enter(SyncRoot);
            try
            {
                if (IsSuspended)
                {
                    result = default;
                    return false;
                }
                result = function();
            }
            finally { Monitor.Exit(SyncRoot); }
            return true;
        }

        /// <summary>
        /// Called when <see cref="IsSuspended"/> changes to <see langword="true"/> as the first <see cref="ISuspension_obsolete"/> is instantiated.
        /// </summary>
        protected virtual void OnBeginSuspension() { }

        /// <summary>
        /// Called when <see cref="IsSuspended"/> changes to <see langword="false"/> as the last <see cref="ISuspension_obsolete"/> is disposed.
        /// </summary>
        protected virtual void OnEndSuspension() { }

        /// <summary>
        /// Called when the <see cref="PropertyChanged"/> event is raised.
        /// </summary>
        /// <param name="args">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that chagned.</param>
        protected void RaisePropertyChanged(string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        private void RaiseSuspensionLifted(object token)
        {
            bool wasSuspended;
            Monitor.Enter(SyncRoot);
            try
            {
                if (!ReferenceEquals(_token, token))
                    return;
                wasSuspended = IsSuspended;
                IsSuspended = false;
            }
            finally { Monitor.Exit(SyncRoot); }

            if (wasSuspended)
                try
                {
                    if (ReferenceEquals(_token, token))
                        OnEndSuspension();
                }
                finally
                {
                    try
                    {
                        if (ReferenceEquals(_token, token))
                            EndSuspension?.Invoke(this, EventArgs.Empty);
                    }
                    finally
                    {
                        if (ReferenceEquals(_token, token))
                            RaisePropertyChanged(nameof(IsSuspended));
                    }
                }
        }

        public ISuspension_obsolete Suspend()
        {
            Suspension suspension;
            object token;
            bool wasSuspended;
            Monitor.Enter(SyncRoot);
            try
            {
                wasSuspended = IsSuspended;
                token = (suspension = new Suspension(this)).Token;
                IsSuspended = true;
            }
            finally { Monitor.Exit(SyncRoot); }

            if (wasSuspended)
                return suspension;

            try
            {
                if (ReferenceEquals(_token, token))
                    OnBeginSuspension();
            }
            finally
            {
                try
                {
                    if (ReferenceEquals(_token, token))
                        BeginSuspension?.Invoke(this, EventArgs.Empty);
                }
                finally
                {
                    if (ReferenceEquals(_token, token))
                        RaisePropertyChanged(nameof(IsSuspended));
                }
            }
            return suspension;
        }

        public void VerifyNotSuspended()
        {
            if (IsSuspended)
                throw new InvalidOperationException("Object is in a suspended state.");
        }

        private class Suspension : ISuspension_obsolete
        {
            private SuspensionProvider _suspensionManager;
            private Suspension _previous;
            private Suspension _next;

            public object Token { get; }

            public bool IsDisposed { get; private set; }

            internal Suspension(SuspensionProvider suspensionManager)
            {
                _suspensionManager = suspensionManager;
                Monitor.Enter(suspensionManager.SyncRoot);
                try
                {
                    if ((_previous = suspensionManager._latest) is null)
                        suspensionManager._token = Token = new object();
                    else
                    {
                        Token = _previous.Token;
                        _previous._next = this;
                    }
                    suspensionManager._latest = this;
                }
                finally { Monitor.Exit(suspensionManager.SyncRoot); }
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!IsDisposed)
                {
                    if (disposing)
                    {
                        Monitor.Enter(_suspensionManager.SyncRoot);
                        try
                        {
                            if (_next is null)
                            {
                                if (!((_suspensionManager._latest = _previous) is null))
                                {
                                    _previous._next = null;
                                    IsDisposed = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (!((_next._previous = _previous) is null))
                                    _previous._next = _next;
                                IsDisposed = true;
                                return;
                            }

                        }
                        finally { Monitor.Exit(_suspensionManager.SyncRoot); }
                        _suspensionManager.RaiseSuspensionLifted(Token);
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    IsDisposed = true;
                }
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            // ~Suspension()
            // {
            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //     Dispose(disposing: false);
            // }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
