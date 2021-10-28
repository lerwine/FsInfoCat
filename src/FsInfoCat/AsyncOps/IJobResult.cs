using System;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("use FSIOQueueService")]
    public interface IJobResult : IAsyncResult
    {
        DateTime Started { get; }

        TimeSpan Elapsed { get; }

        AsyncJobStatus Status { get; }

        bool IsCancellationRequested { get; }

        Task GetTask();
    }

    [Obsolete("use FSIOQueueService")]
    public interface IJobResult<TResult> : IJobResult
    {
        TResult Result { get; }

        new Task<TResult> GetTask();
    }

    [Obsolete("use FSIOQueueService")]
    public interface ICancellableJob : IJobResult
    {
        void Cancel();

        void Cancel(bool throwOnFirstException);
    }
}
