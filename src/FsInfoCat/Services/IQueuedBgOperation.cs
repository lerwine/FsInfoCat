using System;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    public interface IQueuedBgOperation : IAsyncResult
    {
        AsyncJobStatus Status { get; }

        Task Task { get; }

        DateTime Started { get; }

        TimeSpan Elapsed { get; }

        void Cancel();

        void Cancel(bool throwOnFirstException);

        void CancelAfter(int millisecondsDelay);

        void CancelAfter(TimeSpan delay);
    }

    public interface IQueuedBgOperation<TResult> : IQueuedBgOperation
    {
        new Task<TResult> Task { get; }
    }
}
