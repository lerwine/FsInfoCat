using System;

namespace FsInfoCat.AsyncOps
{
    public interface IPendingJob : ICancellableJob
    {
        IJobResult Job { get; }

        void CancelAfter(int millisecondsDelay);

        void CancelAfter(TimeSpan delay);
    }

    public interface IPendingJob<TResult> : IJobResult<TResult>, IPendingJob
    {
        new IJobResult<TResult> Job { get; }
    }
}
