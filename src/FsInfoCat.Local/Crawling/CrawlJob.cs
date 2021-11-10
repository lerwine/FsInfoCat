using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public partial class CrawlJob : ICrawlJob, IProgress<IAsyncOperationInfo>
    {
        private readonly CrawlWorker _worker;
        private readonly ICrawlMessageBus _crawlMessageBus;

        public Guid ConcurrencyId { get; } = Guid.NewGuid();

        internal IQueuedBgProducer<CrawlTerminationReason> JobResult { get; }

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

        public string CurrentOperation => _worker.CurrentItem?.GetFullName();

        public object AsyncState => ((IAsyncOperationInfo)JobResult).AsyncState;

        internal CrawlJob([DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] ILocalCrawlConfiguration crawlConfiguration, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService, DateTime? stopAt,
            [DisallowNull] Action<CrawlJob> onStarted, IAsyncOperationInfo parentOperation = null)
        {
            _crawlMessageBus = crawlMessageBus ?? throw new ArgumentNullException(nameof(crawlMessageBus));
            _worker = new(crawlConfiguration, fileSystemDetailService, ConcurrencyId, stopAt);
            ParentOperation = parentOperation;
            JobResult = fsIOQueueService.Enqueue(ActivityCode.CrawlingFileSystem, this, updateProgress => DoWorkAsync(onStarted.Invoke, updateProgress));
            Task = JobResult.Task.ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                    return CrawlTerminationReason.Aborted;
                return task.Result;
            });
        }

        private async Task<CrawlTerminationReason> DoWorkAsync([DisallowNull] Action<CrawlJob> onStarted, [DisallowNull] IStatusReportable updateProgress)
        {
            onStarted(this);
            bool? timeoutReached = await _worker.DoWorkAsync(updateProgress);
            return timeoutReached.HasValue ? (timeoutReached.Value ? CrawlTerminationReason.TimeLimitReached : CrawlTerminationReason.ItemLimitReached) : CrawlTerminationReason.Completed;
        }

        public void Cancel() => JobResult.Cancel();

        public void CancelAfter(int millisecondsDelay) => JobResult.CancelAfter(millisecondsDelay);

        public void CancelAfter(TimeSpan delay) => JobResult.CancelAfter(delay);

        void IProgress<IAsyncOperationInfo>.Report(IAsyncOperationInfo value)
        {
            MessageCode? statusDescription = value.StatusDescription;
            if (statusDescription.HasValue)
            {
                if (CurrentItem is ICurrentDirectory directory)
                    _crawlMessageBus.Report(new DirectoryCrawlEventArgs(this, directory, statusDescription.Value, value.CurrentOperation));
                else if (CurrentItem is ICurrentFile file)
                    _crawlMessageBus.Report(new FileCrawlEventArgs(this, file, statusDescription.Value, value.CurrentOperation));
                else
                    _crawlMessageBus.ReportOtherActivity(new CrawlActivityEventArgs(this, statusDescription.Value, value.CurrentOperation));
            }
            else if (CurrentItem is ICurrentDirectory directory)
                _crawlMessageBus.Report(new DirectoryCrawlEventArgs(this, directory, StatusDescription, directory.GetFullName()));
            else if (CurrentItem is ICurrentFile file)
                _crawlMessageBus.Report(new FileCrawlEventArgs(this, file, StatusDescription, file.GetFullName()));
            else
                _crawlMessageBus.ReportOtherActivity(new CrawlActivityEventArgs(this, StatusDescription, CurrentOperation));
        }
    }
}
