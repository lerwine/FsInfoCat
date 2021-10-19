using FsInfoCat.Collections;
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
        private readonly Queue<ICrawlJob> _enqueued = new();
        private readonly WeakReferenceSet<IProgress<bool>> _activeStateChangedEventListeners = new();

        public ICrawlJob ActiveJob { get; private set; }

        public CrawlQueue(ILogger<CrawlQueue> logger)
        {
            _logger = logger;
            _logger.LogDebug($"{nameof(ICrawlQueue)} Service instantiated");
        }

        [ServiceBuilderHandler]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(CrawlQueue).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<ICrawlQueue, CrawlQueue>();
        }

        public void AddActiveStateChangedEventListener([DisallowNull] IProgress<bool> listener) => _activeStateChangedEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public bool RemoveActiveStateChangedEventListener(IProgress<bool> listener) => _activeStateChangedEventListeners.Remove(listener);

        public async Task<bool> TryEnqueueAsync(ICrawlJob crawlJob, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({crawlJob}, {cancellationToken})", nameof(TryEnqueueAsync), crawlJob, cancellationToken);
            Monitor.Enter(_enqueued);
            try
            {
                if (crawlJob.JobStatus != AsyncJobStatus.WaitingToRun)
                {
                    _logger.LogWarning("Attempted to start CrawlJob with status of {JobStatus}", crawlJob.JobStatus);
                    return false;
                }
                if (ActiveJob is null)
                {
                    ActiveJob = crawlJob;
                    try
                    {
                        _logger.LogDebug("Starting first job");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        crawlJob.StartAsync(cancellationToken).ContinueWith(task => OnJobCompleted(crawlJob, cancellationToken));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "Failed to start job");
                        ActiveJob = null;
                        return false;
                    }
                }
                else if (ReferenceEquals(ActiveJob, crawlJob) || _enqueued.Any(j => ReferenceEquals(j, crawlJob)))
                {
                    _logger.LogWarning("Job already enqueued");
                    return false;
                }
                else
                {
                    _logger.LogDebug("Enqueueing job");
                    _enqueued.Enqueue(crawlJob);
                    return true;
                }
            }
            finally { Monitor.Exit(_enqueued); }
            _logger.LogDebug("Raising ActiveStateChangedEvent as true");
            await _activeStateChangedEventListeners.RaiseProgressChangedAsync(true, cancellationToken);
            return true;
        }

        private async Task OnJobCompleted(ICrawlJob crawlJob, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({crawlJob}, {cancellationToken})", nameof(OnJobCompleted), crawlJob, cancellationToken);
            bool isFinalJob;
            Monitor.Enter(_enqueued);
            try
            {
                if (ActiveJob is not null && ReferenceEquals(ActiveJob, crawlJob))
                {
                    if (_enqueued.TryDequeue(out crawlJob))
                    {
                        ActiveJob = crawlJob;
                        try
                        {
                            _logger.LogDebug("Starting next job");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            crawlJob.StartAsync(cancellationToken).ContinueWith(task => OnJobCompleted(crawlJob, cancellationToken));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            return;
                        }
                        catch (Exception exception)
                        {
                            _logger.LogError(exception, "Failed to start next job");
                            isFinalJob = false;
                        }
                    }
                    else
                    {
                        isFinalJob = true;
                        ActiveJob = null;
                    }
                }
                else
                    return;
            }
            finally { Monitor.Exit(_enqueued); }
            if (isFinalJob)
            {
                _logger.LogDebug("Raising ActiveStateChangedEvent as false");
                await _activeStateChangedEventListeners.RaiseProgressChangedAsync(false, cancellationToken);
            }
            else
                await OnJobCompleted(crawlJob, cancellationToken);
        }

        public async Task<bool> TryDequeueAsync(ICrawlJob crawlJob, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({crawlJob}, {cancellationToken})", nameof(TryDequeueAsync), crawlJob, cancellationToken);
            if (crawlJob is null)
                return false;
            Monitor.Enter(_enqueued);
            try
            {
                if (ActiveJob is null)
                    return false;
                if (ReferenceEquals(ActiveJob, crawlJob))
                {
                    _logger.LogDebug("Stopping active job");
                    await crawlJob.StopAsync(new CancellationToken(true));
                }
                else if (_enqueued.TryPeek(out ICrawlJob job) && ReferenceEquals(job, crawlJob))
                {
                    _logger.LogDebug("Stopping pending job");
                    await crawlJob.StopAsync(new CancellationToken(true));
                }
                else
                    return false;
            }
            finally { Monitor.Exit(_enqueued); }
            return true;
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
                CancellationToken token = new(true);
                await Task.WhenAll(Enumerable.Repeat(ActiveJob, 1).Concat(_enqueued).Select(j => j.StopAsync(token)));
            }
            finally { Monitor.Exit(_enqueued); }
        }
    }
}
