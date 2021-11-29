using System;

namespace FsInfoCat.AsyncOps
{
    public interface ICancellableOperation : IBackgroundProgressInfo, IObservable<IBackgroundProgressEvent>
    {
        bool IsCancellationRequested { get; }

        void Cancel();

        void Cancel(bool throwOnFirstException);

        void CancelAfter(int millisecondsDelay);

        void CancelAfter(TimeSpan delay);
    }

    public interface ICancellableOperation<TState> : ICancellableOperation, IBackgroundProgressInfo<TState>, IObservable<IBackgroundProgressEvent<TState>>
    {
    }
}
