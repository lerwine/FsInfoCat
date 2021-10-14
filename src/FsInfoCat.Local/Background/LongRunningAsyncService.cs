using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public abstract class LongRunningAsyncService : BackgroundService, ILongRunningAsyncService
    {
        public DateTime Started { get; private set; }

        public TimeSpan Elapsed { get; private set; }

        public AsyncJobStatus JobStatus { get; private set; }

        public Task Task { get; private set; }

        object IAsyncResult.AsyncState => ((IAsyncResult)(Task ?? throw new InvalidOperationException())).AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => ((IAsyncResult)(Task ?? throw new InvalidOperationException())).AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => ((IAsyncResult)Task)?.CompletedSynchronously ?? false;

        bool IAsyncResult.IsCompleted => ((IAsyncResult)Task)?.IsCompleted ?? false;
    }
}
