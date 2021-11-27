using System;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundOperation : IBackgroundProgressInfo, IObservable<IBackgroundProgressEvent>, IDisposable
    {
        Task Task { get; }

        bool IsCancellationRequested { get; }

        void Cancel();

        void Cancel(bool throwOnFirstException);

        void CancelAfter(int millisecondsDelay);

        void CancelAfter(TimeSpan delay);
    }

    public interface IBackgroundOperation<TState> : IBackgroundOperation, IBackgroundProgressInfo<TState>, IObservable<IBackgroundProgressEvent<TState>>
    {
    }
}
