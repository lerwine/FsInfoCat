using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public partial class Observable<TNotice> : IObservable<TNotice>, IDisposable
    {
        private readonly object _syncRoot = new();
        private Registration _first;
        private Registration _last;
        private bool _isDisposed;

        public bool IsSubscribed(IObserver<TNotice> observer) => observer is not null && Registration.IsSubscribed(this, observer);

        public IDisposable Subscribe(IObserver<TNotice> observer) => Registration.Subscribe(this, observer ?? throw new ArgumentNullException(nameof(observer)));

        protected virtual void OnSubscribed(IObserver<TNotice> observer) { }

        protected virtual void RaiseNext(TNotice value) => RaiseNext(Registration.GetObservers(this), value);

        private void RaiseNext(Queue<IObserver<TNotice>> queue, TNotice value)
        {
            if (queue.TryDequeue(out IObserver<TNotice> observer))
                try { observer.OnNext(value); }
                finally { RaiseNext(queue, value); }
        }

        protected virtual void RaiseError(Exception error) => RaiseError(Registration.GetObservers(this), error ?? throw new ArgumentNullException(nameof(error)));

        private void RaiseError(Queue<IObserver<TNotice>> queue, Exception exception)
        {
            if (queue.TryDequeue(out IObserver<TNotice> observer))
                try { observer.OnError(exception); }
                finally { RaiseError(queue, exception); }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                    OnDisposing();
            }
        }

        protected virtual void OnDisposing() => Registration.Dispose(this);

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
