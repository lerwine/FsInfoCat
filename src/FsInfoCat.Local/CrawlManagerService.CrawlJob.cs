using FsInfoCat.Local.Crawling;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlManagerService
    {
        [Obsolete("Use Crawling.CrawlManagerService")]
        internal class CrawlJob : ICrawlJob
        {
            private readonly CancellationTokenSource _tokenSource = new();
            private readonly CrawlWorker _worker;
            private readonly System.Diagnostics.Stopwatch _stopWatch = new();

            internal CrawlManagerService Service { get; }

            internal ILocalCrawlConfiguration CrawlConfiguration { get; }

            public ICurrentItem CurrentItem => _worker.CurrentItem;

            public string Message => _worker.Message;

            public StatusMessageLevel MessageLevel => _worker.MessageLevel;

            public Guid ConcurrencyId { get; private set; }

            public AsyncJobStatus JobStatus { get; private set; }

            public Task Task { get; }

            public bool IsCancellationRequested => _tokenSource.IsCancellationRequested;

            public DateTime Started { get; private set; }

            public TimeSpan Elapsed => _stopWatch.Elapsed;

            public object AsyncState => Task.AsyncState;

            public WaitHandle AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

            public bool CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;

            public bool IsCompleted => Task.IsCompleted;

            string IAsyncJob.Title => CrawlConfiguration.DisplayName;

            internal CrawlJob(CrawlManagerService service, ILocalCrawlConfiguration crawlConfiguration, DateTime? stopAt)
            {
                Service = service;
                CrawlConfiguration = crawlConfiguration;
                _worker = new(this, stopAt, _tokenSource.Token);
                lock (service._pendingJobs)
                {
                    if (service._activeJob is null)
                    {
                        service._activeJob = this;
                        Task = StartWorkerAsync(true).ContinueWith(OnJobCompleted);
                    }
                    else
                        Task = (service._pendingJobs.AddLast(this).Previous?.Value ?? service._activeJob).Task.ContinueWith(t => StartWorkerAsync(false)).ContinueWith(OnJobCompleted);
                }
            }

            private async Task StartWorkerAsync(bool isFirstJob)
            {
                Started = DateTime.Now;
                if (JobStatus != AsyncJobStatus.Cancelling)
                {
                    _stopWatch.Start();
                    JobStatus = AsyncJobStatus.Running;
                }
                lock (Service._pendingJobs)
                {
                    if (!isFirstJob)
                        Service._pendingJobs.Remove(this);
                    Service._activeJob = this;
                }
                Service.RaiseCrawlJobStart(new("message", StatusMessageLevel.Information, CrawlConfiguration, isFirstJob, ConcurrencyId));
                await _worker.RunAsync();
            }

            private void OnJobCompleted(Task task)
            {
                _stopWatch.Stop();
                lock (Service._pendingJobs)
                {
                    bool isLastJob = Service._pendingJobs.First is null;
                    if (isLastJob)
                        Service._activeJob = null;
                    if (task.IsCanceled)
                    {
                        JobStatus = AsyncJobStatus.Canceled;
                        Service.RaiseCrawlJobEnd(new("Crawl job canceled.", StatusMessageLevel.Warning, CrawlConfiguration, isLastJob, ConcurrencyId));
                    }
                    else if (task.IsFaulted)
                    {
                        JobStatus = AsyncJobStatus.Faulted;
                        Service.RaiseCrawlJobEnd(new("Crawl job failed. See event logs for details.", StatusMessageLevel.Error, CrawlConfiguration, isLastJob, ConcurrencyId));
                    }
                    else
                    {
                        JobStatus = AsyncJobStatus.Succeeded;
                        Service.RaiseCrawlJobEnd(new(_worker.Message, _worker.MessageLevel, CrawlConfiguration, isLastJob, ConcurrencyId));
                    }
                }
            }

            public void Cancel(bool throwOnFirstException)
            {
                switch (JobStatus)
                {
                    case AsyncJobStatus.WaitingToRun:
                    case AsyncJobStatus.Running:
                        JobStatus = AsyncJobStatus.Cancelling;
                        break;
                }
                if (!_tokenSource.IsCancellationRequested)
                    _tokenSource.Cancel(throwOnFirstException);
            }

            public void Cancel()
            {
                switch (JobStatus)
                {
                    case AsyncJobStatus.WaitingToRun:
                    case AsyncJobStatus.Running:
                        JobStatus = AsyncJobStatus.Cancelling;
                        break;
                }
                if (!_tokenSource.IsCancellationRequested)
                    _tokenSource.Cancel();
            }
        }
    }
}
