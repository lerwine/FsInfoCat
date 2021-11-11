using FsInfoCat.Collections;
using FsInfoCat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public class CrawlQueue : ICrawlQueue
    {
        private readonly ILogger<CrawlQueue> _logger;
        private readonly IFSIOQueueService _fsIOQueueService;
        private readonly WeakReferenceSet<IProgress<ICrawlJob>> _crawlProgressListeners = new();
        private readonly ICrawlMessageBus _crawlMessageBus;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly List<CrawlJob> _enqueued = new();
        private readonly WeakReferenceSet<IProgress<bool>> _activeStateChangedEventListeners = new();

        public CrawlJob ActiveJob { get; private set; }

        ICrawlJob ICrawlQueue.ActiveJob => ActiveJob;

        public CrawlQueue([DisallowNull] ILogger<CrawlQueue> logger, [DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService)
        {
            _logger = logger;
            _fsIOQueueService = fsIOQueueService;
            _crawlMessageBus = crawlMessageBus;
            _fileSystemDetailService = fileSystemDetailService;
            _logger.LogDebug($"{nameof(ICrawlQueue)} Service instantiated");
        }

        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(CrawlQueue).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<ICrawlQueue, CrawlQueue>();
        }

        public void AddActiveStateChangedEventListener([DisallowNull] IProgress<bool> listener) => _activeStateChangedEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public bool RemoveActiveStateChangedEventListener(IProgress<bool> listener) => _activeStateChangedEventListeners.Remove(listener);

        private ICrawlJob Enqueue(DateTime? stopAt, [DisallowNull] ILocalCrawlConfiguration crawlConfiguration)
        {
            CrawlJob crawlJob;
            Monitor.Enter(_enqueued);
            try
            {
                crawlJob = new(_fsIOQueueService, crawlConfiguration, _crawlMessageBus, _fileSystemDetailService, stopAt, OnJobStarted);
                _enqueued.Add(crawlJob);
            }
            finally { Monitor.Exit(_enqueued); }
            crawlJob.JobResult.Task.ContinueWith(task => OnJobCompleted(task, crawlJob));
            return crawlJob;
        }

        public ICrawlJob Enqueue([DisallowNull] ILocalCrawlConfiguration crawlConfiguration) => Enqueue(null, crawlConfiguration);

        public ICrawlJob Enqueue([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime stopAt) => Enqueue(stopAt, crawlConfiguration);

        private void OnJobStarted([DisallowNull] CrawlJob crawlJob)
        {
            bool isFirstJob;
            Monitor.Enter(_enqueued);
            try
            {
                isFirstJob = ActiveJob is null;
                try { _enqueued.Remove(crawlJob); }
                finally { ActiveJob = crawlJob; }
            }
            finally { Monitor.Exit(_enqueued); }
            _crawlMessageBus.Report(new CrawlJobStartEventArgs(crawlJob, isFirstJob));
            if (isFirstJob)
                _activeStateChangedEventListeners.RaiseProgressChangedAsync(true, CancellationToken.None);
        }

        private void OnJobCompleted(Task<CrawlTerminationReason> task, [DisallowNull] CrawlJob crawlJob)
        {
            bool isLastJob;
            Monitor.Enter(_enqueued);
            try
            {
                if (ActiveJob is not null && ReferenceEquals(ActiveJob, crawlJob))
                {
                    isLastJob = _enqueued.Count == 0;
                    if (isLastJob)
                        ActiveJob = null;
                }
                else
                    isLastJob = false;
            }
            finally { Monitor.Exit(_enqueued); }
            CrawlJobEndEventArgs eventArgs;
            if (task.IsFaulted)
            {
                //Exception exception = (task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception;
                // TODO: if (exception is AsyncOperationFailureException failureException)
                eventArgs = new CrawlJobEndEventArgs(crawlJob, isLastJob, CrawlTerminationReason.Aborted);
            }
            else
                eventArgs = (task.IsCanceled ? CrawlTerminationReason.Aborted : task.Result) switch
                {
                    CrawlTerminationReason.ItemLimitReached => new CrawlJobEndEventArgs(crawlJob, isLastJob, CrawlTerminationReason.ItemLimitReached),
                    CrawlTerminationReason.TimeLimitReached => new CrawlJobEndEventArgs(crawlJob, isLastJob, CrawlTerminationReason.TimeLimitReached),
                    CrawlTerminationReason.Completed => new CrawlJobEndEventArgs(crawlJob, isLastJob, CrawlTerminationReason.Completed),
                    _ => new CrawlJobEndEventArgs(crawlJob, isLastJob, CrawlTerminationReason.Aborted),
                };
            _crawlMessageBus.Report(eventArgs);
            if (isLastJob)
                _activeStateChangedEventListeners.RaiseProgressChangedAsync(false, CancellationToken.None);
        }

        public bool IsActive(ICrawlJob crawlJob) => ActiveJob is not null && ReferenceEquals(ActiveJob, crawlJob);

        public bool IsEnqueued(ICrawlJob crawlJob)
        {
            Monitor.Enter(_enqueued);
            try { return ActiveJob is not null && !ReferenceEquals(ActiveJob, crawlJob) && _enqueued.Any(e => ReferenceEquals(e, crawlJob)); }
            finally { Monitor.Exit(_enqueued); }
        }

        public bool IsEnqueuedOrActive(ICrawlJob crawlJob)
        {
            Monitor.Enter(_enqueued);
            try { return ActiveJob is not null && (ReferenceEquals(ActiveJob, crawlJob) && _enqueued.Any(e => ReferenceEquals(e, crawlJob))); }
            finally { Monitor.Exit(_enqueued); }
        }

        public async Task CancelAllCrawlsAsync()
        {
            _logger.LogDebug("{Method}()", nameof(CancelAllCrawlsAsync));
            Monitor.Enter(_enqueued);
            try
            {
                if (ActiveJob is null)
                    return;
                await Task.WhenAll(Enumerable.Repeat(ActiveJob, 1).Concat(_enqueued).ToArray().Select(j =>
                {
                    j.Cancel();
                    return j.Task;
                }));
            }
            finally { Monitor.Exit(_enqueued); }
        }
    }
}
