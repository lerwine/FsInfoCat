using System;

namespace FsInfoCat
{
    // TODO: Document ObserverSubscriptionRelay class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public sealed class ObserverSubscriptionRelay<TNotice, TBase> : IObserver<TNotice>
        where TNotice : TBase
    {
        private readonly WeakReference<IObserver<TBase>> _target;
        private readonly Disposable _disposable;

        private ObserverSubscriptionRelay(IObservable<TNotice> observable, IObserver<TBase> observer)
        {
            _target = new(observer);
            _disposable = new(observable.Subscribe(this));
        }

        public static IDisposable Create(IObservable<TNotice> observable, IObserver<TBase> observer) => new ObserverSubscriptionRelay<TNotice, TBase>(observable, observer)._disposable;

        public void OnCompleted()
        {
            if (_target.TryGetTarget(out IObserver<TBase> observer))
                observer.OnCompleted();
            else
                _disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            if (_target.TryGetTarget(out IObserver<TBase> observer))
                observer.OnError(error);
            else
                _disposable.Dispose();
        }

        public void OnNext(TNotice value)
        {
            if (_target.TryGetTarget(out IObserver<TBase> observer))
                observer.OnNext(value);
            else
                _disposable.Dispose();
        }

        sealed class Disposable(IDisposable subscription) : IDisposable
        {
            private IDisposable _subscription = subscription;

            private void Dispose(bool disposing)
            {
                IDisposable subscription = _subscription;
                _subscription = null;
                if (disposing)
                    subscription?.Dispose();
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
