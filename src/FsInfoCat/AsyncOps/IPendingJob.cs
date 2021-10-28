using System;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("use FSIOQueueService")]
    public interface IPendingJob : ICancellableJob
    {
        IJobResult Job { get; }

        void CancelAfter(int millisecondsDelay);

        void CancelAfter(TimeSpan delay);
    }
    [Obsolete("use FSIOQueueService")]

    public interface IPendingJob<TResult> : IJobResult<TResult>, IPendingJob
    {
        new IJobResult<TResult> Job { get; }
    }
}
