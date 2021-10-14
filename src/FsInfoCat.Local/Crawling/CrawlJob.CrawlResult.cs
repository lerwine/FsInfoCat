using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public partial class CrawlJob
    {
        record CrawlResult : ICrawlResult
        {
            public DateTime Started { get; init; }

            public TimeSpan Duration { get; init; }

            public CrawlTerminationReason TerminationReason { get; init; }

            public Task WorkerTask { get; init; }

            object IAsyncResult.AsyncState => ((IAsyncResult)WorkerTask).AsyncState;

            WaitHandle IAsyncResult.AsyncWaitHandle => ((IAsyncResult)WorkerTask).AsyncWaitHandle;

            bool IAsyncResult.CompletedSynchronously => ((IAsyncResult)WorkerTask).CompletedSynchronously;

            bool IAsyncResult.IsCompleted => ((IAsyncResult)WorkerTask).IsCompleted;
        }
    }
}
