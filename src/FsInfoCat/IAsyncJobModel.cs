using System;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IAsyncJobModel
    {
        Guid ConcurrencyId { get; }

        bool IsCompleted { get; }

        AsyncJobStatus Status { get; }

        Task Task { get; }

        bool IsCancellationRequested { get; }

        void Cancel(bool throwOnFirstException);

        void Cancel();
    }
    public interface IAsyncJobModel<TTask> : IAsyncJobModel
        where TTask : Task
    {
        new TTask Task { get; }
    }
}
