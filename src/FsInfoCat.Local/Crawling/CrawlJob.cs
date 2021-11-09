using FsInfoCat.Services;
using System;
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

        Task IBgOperation.Task => Task;

        public AsyncJobStatus Status => JobResult.Status;

        public DateTime Started => JobResult.Started;

        public TimeSpan Elapsed => JobResult.Elapsed;

        public MessageCode StatusDescription { get; private set; }

        public IAsyncOperationInfo ParentOperation { get; }

        object IAsyncResult.AsyncState => ((IAsyncOperationInfo)JobResult).AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;

        bool IAsyncResult.IsCompleted => ((IAsyncResult)Task).IsCompleted;

        ActivityCode IBgOperationEventArgs.Activity => ActivityCode.CrawlingFileSystem;

        ActivityCode? IAsyncOperationInfo.Activity => ActivityCode.CrawlingFileSystem;

        MessageCode? IAsyncOperationInfo.StatusDescription => throw new NotImplementedException();

        public string CurrentOperation => throw new NotImplementedException();

        public object AsyncState => throw new NotImplementedException();

        internal CrawlJob([DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] ILocalCrawlConfiguration crawlConfiguration, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService, DateTime? stopAt,
            [DisallowNull] Action<CrawlJob> onStarted, IAsyncOperationInfo parentOperation = null)
        {
            _worker = new(crawlConfiguration, crawlMessageBus, fileSystemDetailService, ConcurrencyId, stopAt);
            ParentOperation = parentOperation;
            JobResult = fsIOQueueService.Enqueue(ActivityCode.CrawlingFileSystem, updateProgress => DoWorkAsync(onStarted.Invoke, updateProgress));
            Task = JobResult.Task.ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                    return CrawlTerminationReason.Aborted;
                return task.Result;
            });
        }

        private async Task<CrawlTerminationReason> DoWorkAsync([DisallowNull] Action<CrawlJob> onStarted, [DisallowNull] IAsyncOperationProgress updateProgress)
        {
            onStarted(this);
            bool? timeoutReached = await _worker.DoWorkAsync(updateProgress);
            return timeoutReached.HasValue ? (timeoutReached.Value ? CrawlTerminationReason.TimeLimitReached : CrawlTerminationReason.ItemLimitReached) : CrawlTerminationReason.Completed;
        }

        public void Cancel() => JobResult.Cancel();

        public void CancelAfter(int millisecondsDelay) => JobResult.CancelAfter(millisecondsDelay);

        public void CancelAfter(TimeSpan delay) => JobResult.CancelAfter(delay);
    }
}
