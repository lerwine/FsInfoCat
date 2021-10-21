using FsInfoCat.AsyncOps;
using FsInfoCat.Background;
using FsInfoCat.Local.Background;
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

        internal IJobResult<CrawlTerminationReason> JobResult { get; }

        public DateTime Started => JobResult.Started;

        public ICurrentItem CurrentItem => _worker.CurrentItem;

        object IAsyncResult.AsyncState => JobResult.AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => JobResult.AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => JobResult.CompletedSynchronously;

        bool IAsyncResult.IsCompleted => JobResult.IsCompleted;

        public CrawlTerminationReason Result => JobResult.Result;

        public TimeSpan Elapsed => JobResult.Elapsed;

        public AsyncJobStatus Status => JobResult.Status;

        public bool IsCancellationRequested => JobResult.IsCancellationRequested;

        public Task<CrawlTerminationReason> GetTask() => JobResult.GetTask();

        Task IJobResult.GetTask() => GetTask();

        internal CrawlJob([DisallowNull] JobQueue jobQueueService, [DisallowNull] ILocalCrawlConfiguration crawlConfiguration, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService, DateTime? stopAt, Action<CrawlJob> onStarted)
        {
            _worker = new(crawlConfiguration, crawlMessageBus, fileSystemDetailService, stopAt);
            JobResult = jobQueueService.Enqueue(context => DoWorkAsync(onStarted.Invoke, context.CancellationToken).ContinueWith(task =>
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

        public void Cancel() => (JobResult as ICancellableJob)?.Cancel();

        public void Cancel(bool throwOnFirstException) => (JobResult as ICancellableJob)?.Cancel(throwOnFirstException);
    }
}
