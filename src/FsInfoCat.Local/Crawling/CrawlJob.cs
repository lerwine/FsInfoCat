using FsInfoCat.Background;
using FsInfoCat.Local.Background;
using FsInfoCat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public partial class CrawlJob : ICrawlJob
    {
        private readonly CrawlWorker _worker;

        public Guid ConcurrencyId { get; } = Guid.NewGuid();

        internal IQueuedBgOperation<CrawlTerminationReason> JobResult { get; }

        public ICurrentItem CurrentItem => _worker.CurrentItem;

        public Task<CrawlTerminationReason> Task { get; }

        Task IQueuedBgOperation.Task => Task;

        public AsyncJobStatus Status => JobResult.Status;

        public DateTime Started => JobResult.Started;

        public TimeSpan Elapsed => JobResult.Elapsed;

        object IAsyncResult.AsyncState => JobResult.AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;

        bool IAsyncResult.IsCompleted => ((IAsyncResult)Task).IsCompleted;

        internal CrawlJob([DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] ILocalCrawlConfiguration crawlConfiguration, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService, DateTime? stopAt,
            [DisallowNull] Action<CrawlJob> onStarted)
        {
            _worker = new(crawlConfiguration, crawlMessageBus, fileSystemDetailService, ConcurrencyId, stopAt);
            JobResult = fsIOQueueService.Enqueue(cancellationToken => DoWorkAsync(onStarted.Invoke, cancellationToken));
            Task = JobResult.Task.ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                    return CrawlTerminationReason.Aborted;
                return task.Result;
            });
        }

        private async Task<CrawlTerminationReason> DoWorkAsync([DisallowNull] Action<CrawlJob> onStarted, CancellationToken cancellationToken)
        {
            onStarted(this);
            bool? timeoutReached = await _worker.DoWorkAsync(cancellationToken);
            return timeoutReached.HasValue ? (timeoutReached.Value ? CrawlTerminationReason.TimeLimitReached : CrawlTerminationReason.ItemLimitReached) : CrawlTerminationReason.Completed;
        }

        public void Cancel() => JobResult.Cancel();

        public void CancelAfter(int millisecondsDelay) => JobResult.CancelAfter(millisecondsDelay);

        public void CancelAfter(TimeSpan delay) => JobResult.CancelAfter(delay);
    }
}
