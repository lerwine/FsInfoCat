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

        internal IQueuedBgOperation<CrawlTerminationReason> JobResult { get; }

        public ICurrentItem CurrentItem => _worker.CurrentItem;

        public Task<CrawlTerminationReason> Task => JobResult.Task;

        Task IQueuedBgOperation.Task => Task;

        public AsyncJobStatus Status => JobResult.Status;

        public DateTime Started => JobResult.Started;

        public TimeSpan Elapsed => JobResult.Elapsed;

        object IAsyncResult.AsyncState => JobResult.AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => JobResult.AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => JobResult.CompletedSynchronously;

        bool IAsyncResult.IsCompleted => JobResult.IsCompleted;

        internal CrawlJob([DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] ILocalCrawlConfiguration crawlConfiguration, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService, DateTime? stopAt, Action<CrawlJob> onStarted)
        {
            _worker = new(crawlConfiguration, crawlMessageBus, fileSystemDetailService, stopAt);
            fsIOQueueService.Enqueue(cancellationToken => DoWorkAsync(onStarted.Invoke, cancellationToken).ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                    return CrawlTerminationReason.Aborted;
                bool? itemLimitReached = task.Result;
                if (itemLimitReached.HasValue)
                    return itemLimitReached.Value ? CrawlTerminationReason.ItemLimitReached : CrawlTerminationReason.TimeLimitReached;
                return CrawlTerminationReason.Completed;
            }));
        }

        private async Task<bool?> DoWorkAsync([DisallowNull] Action<CrawlJob> onStarted, CancellationToken cancellationToken)
        {
            onStarted?.Invoke(this);
            return await _worker.DoWorkAsync(cancellationToken);
        }

        public void Cancel() => JobResult.Cancel();

        public void CancelAfter(int millisecondsDelay) => JobResult.CancelAfter(millisecondsDelay);

        public void CancelAfter(TimeSpan delay) => JobResult.CancelAfter(delay);
    }
}
