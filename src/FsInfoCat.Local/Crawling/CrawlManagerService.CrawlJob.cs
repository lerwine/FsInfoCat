using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FsInfoCat.Local.Crawling
{
    public class CrawlQueue : ICrawlQueue
    {
        private readonly ILogger<CrawlQueue> _logger;
        private readonly Queue<ICrawlJob> _enqueued = new();
        private readonly WeakReferenceSet<IProgress<bool>> _activeStateChangedEventListeners = new();

        public ICrawlJob ActiveJob { get; private set; }

        public CrawlQueue(ILogger<CrawlQueue> logger) => _logger = logger;

        [ServiceBuilderHandler()]
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<ICrawlQueue, CrawlQueue>();
        }

        public void AddActiveStateChangedEventListener([DisallowNull] IProgress<bool> listener) => _activeStateChangedEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public bool RemoveActiveStateChangedEventListener(IProgress<bool> listener) => _activeStateChangedEventListeners.Remove(listener);

        public bool TryEnqueue(ICrawlJob crawlJob)
        {
            Monitor.Enter(_enqueued);
            try
            {
                if (crawlJob.JobStatus != AsyncJobStatus.WaitingToRun)
                    return false;
                if (ActiveJob is null)
                {
                    ActiveJob = crawlJob;
                    try { crawlJob.StartAsync(CancellationToken.None).ContinueWith(task => OnJobCompleted(crawlJob)); }
                     catch
                    {
                        ActiveJob = null;
                        return false;
                    }
                }
                else if (ReferenceEquals(ActiveJob, crawlJob) || _enqueued.Any(j => ReferenceEquals(j, crawlJob)))
                    return false;
                else
                {
                    _enqueued.Enqueue(crawlJob);
                    return true;
                }
            }
            finally { Monitor.Exit(_enqueued); }
            // TODO: Raise active state changed
            return true;
        }

        private void OnJobCompleted(ICrawlJob crawlJob)
        {
            Monitor.Enter(_enqueued);
            try
            {
                if (ActiveJob is not null && ReferenceEquals(ActiveJob, crawlJob))
                {
                    if (_enqueued.TryDequeue(out crawlJob))
                    {
                        ActiveJob = crawlJob;
                        try { crawlJob.StartAsync(CancellationToken.None).ContinueWith(task => OnJobCompleted(crawlJob)); }
                        catch { OnJobCompleted(crawlJob); }
                        return;
                    }
                }
                else
                    return;
                ActiveJob = null;
            }
            finally { Monitor.Exit(_enqueued); }
            // TODO: Raise active state changed
        }

        public bool TryDequeue(ICrawlJob crawlJob, bool throwIfActive = false)
        {
            Monitor.Enter(_enqueued);
            try
            {

            }
            finally { Monitor.Exit(_enqueued); }
            throw new NotImplementedException();
        }

        public bool IsActive(ICrawlJob crawlJob)
        {
            throw new NotImplementedException();
        }

        public bool IsEnqueued(ICrawlJob crawlJob)
        {
            throw new NotImplementedException();
        }

        public bool IsEnqueuedOrActive(ICrawlJob crawlJob)
        {
            throw new NotImplementedException();
        }

        public Task CancelAllCrawlsAsync()
        {
            Monitor.Enter(_enqueued);
            try
            {

            }
            finally { Monitor.Exit(_enqueued); }
            throw new NotImplementedException();
        }

        public CrawlQueue(ILogger<CrawlQueue> logger) => _logger = logger;
    }
    public class CrawlMessageBus : ICrawlMessageBus
    {
        private readonly ILogger<CrawlMessageBus> _logger;
        private readonly WeakReferenceSet<IProgress<ICrawlActivityEventArgs>> _crawlActivityEventListeners = new();
        private readonly WeakReferenceSet<IProgress<ICrawlErrorEventArgs>> _crawlErrorEventListeners = new();
        private readonly WeakReferenceSet<IProgress<ICrawlManagerEventArgs>> _crawlManagerEventListeners = new();
        private readonly WeakReferenceSet<IProgress<FileCrawlEventArgs>> _anyFileCrawlEventListeners = new();
        private readonly WeakReferenceSet<IProgress<FileCrawlEventArgs>> _fileCrawlEventListeners = new();
        private readonly WeakReferenceSet<IProgress<ICrawlManagerFsItemEventArgs>> _anyFsItemEventListeners = new();
        private readonly WeakReferenceSet<IProgress<ICrawlManagerFsItemEventArgs>> _fsItemEventListeners = new();
        private readonly WeakReferenceSet<IProgress<DirectoryCrawlEventArgs>> _anyDirectoryEventListeners = new();
        private readonly WeakReferenceSet<IProgress<DirectoryCrawlEventArgs>> _directoryEventListeners = new();

        [ServiceBuilderHandler()]
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<ICrawlMessageBus, CrawlMessageBus>();
        }

        public CrawlMessageBus(ILogger<CrawlMessageBus> logger) => _logger = logger;

        public void AddCrawlActivityEventListener([DisallowNull] IProgress<ICrawlActivityEventArgs> listener) => _crawlActivityEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlErrorEventListener([DisallowNull] IProgress<ICrawlErrorEventArgs> listener) => _crawlErrorEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlManagerEventListener([DisallowNull] IProgress<ICrawlManagerEventArgs> listener) => _crawlManagerEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddFileCrawlEventListener([DisallowNull] IProgress<FileCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyFileCrawlEventListeners : _fileCrawlEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddFileSystemItemEventListener([DisallowNull] IProgress<ICrawlManagerFsItemEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyFsItemEventListeners : _fsItemEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddSubdirectoryCrawlEventListener([DisallowNull] IProgress<DirectoryCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyDirectoryEventListeners : _directoryEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public bool RemoveCrawlActivityEventListener(IProgress<ICrawlActivityEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCrawlErrorEventListener(IProgress<ICrawlErrorEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCrawlManagerEventListener(IProgress<ICrawlManagerEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileCrawlEventListener(IProgress<FileCrawlEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileSystemItemEventListener(IProgress<ICrawlManagerFsItemEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveSubdirectoryCrawlEventListener(IProgress<DirectoryCrawlEventArgs> listener)
        {
            throw new NotImplementedException();
        }

    }
    public class CrawlJob : BackgroundService, ICrawlJob
    {
        private readonly ILogger<CrawlJob> _logger;
        private readonly ICrawlMessageBus _crawlMessageBus;
        private readonly ICrawlQueue _crawlQueue;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly LocalDbContext _localDbContext;
        private Stopwatch _stopwatch;
        private CancellationToken _stoppingToken;
        private Task<ICrawlResult> _task;
        private CrawlTerminationReason _terminationReason;

        public DateTime? StopAt { get; }

        public long? TTL { get; }

        public DateTime Started { get; private set; }

        public TimeSpan Elapsed => _stopwatch?.Elapsed ?? TimeSpan.Zero;

        public AsyncJobStatus JobStatus { get; private set; }

        public ICurrentItem CurrentItem { get; private set; }

        public Task<ICrawlResult> Task => _task ?? throw new InvalidOperationException();

        object IAsyncResult.AsyncState => ((IAsyncResult)Task).AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => ((IAsyncResult)_task)?.CompletedSynchronously ?? false;

        bool IAsyncResult.IsCompleted => ((IAsyncResult)_task)?.CompletedSynchronously ?? false;

        public CrawlJob(ILogger<CrawlJob> logger, ICrawlMessageBus crawlMessageBus, ICrawlQueue crawlQueue, IFileSystemDetailService fileSystemDetailService, LocalDbContext localDbContext) =>
            (_logger, _crawlMessageBus, _crawlQueue, _fileSystemDetailService, _localDbContext) = (logger, crawlMessageBus, crawlQueue, fileSystemDetailService, localDbContext);

        [ServiceBuilderHandler()]
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddHostedService<ICrawlJob>(provider => new CrawlJob(provider.GetRequiredService<ILogger<CrawlJob>>(), provider.GetRequiredService<ICrawlMessageBus>(), provider.GetRequiredService<ICrawlQueue>(),
                provider.GetRequiredService<IFileSystemDetailService>(), provider.GetRequiredService<LocalDbContext>()));
        }

        private async Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Do not invoke this method directly. Use <see cref="ICrawlQueue.TryEnqueue(ICrawlJob)"/> instead.
        /// </summary>
        /// <param name="cancellationToken">Indicates if/when the process has been aborted.</param>
        /// <returns>A <see cref="Task"/> for the long-running operation.</returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (_crawlQueue.ActiveJob is null || !ReferenceEquals(this, _crawlQueue.ActiveJob))
                throw new InvalidOperationException();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            JobStatus = AsyncJobStatus.Running;
            Stopwatch stopwatch = new();
            stopwatch.Start();
            _stopwatch = stopwatch;
            _stoppingToken = stoppingToken;
            Task workerTask = ExecuteAsync();
            _task = workerTask.ContinueWith<ICrawlResult>(task =>
            {
                stopwatch.Stop();
                if (task.IsCanceled)
                    JobStatus = AsyncJobStatus.Canceled;
                else if (task.IsFaulted)
                    JobStatus = AsyncJobStatus.Faulted;
                else
                {
                    JobStatus = AsyncJobStatus.Succeeded;
                    return new CrawlResult
                    {
                        Started = Started,
                        Duration = stopwatch.Elapsed,
                        WorkerTask = task,
                        TerminationReason = _terminationReason
                    };
                }
                _terminationReason = CrawlTerminationReason.Aborted;
                return new CrawlResult
                {
                    Started = Started,
                    Duration = stopwatch.Elapsed,
                    WorkerTask = task,
                    TerminationReason = _terminationReason
                };
            }, TaskContinuationOptions.NotOnFaulted); ;
            return workerTask;
        }

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
    public partial class CrawlManagerService
    {
        class CrawlJob : ICrawlJob
        {
            private readonly CrawlManagerService _service;
            private readonly ILocalCrawlConfiguration _crawlConfiguration;
            private readonly CancellationTokenSource _tokenSource;
            private bool? _isTimedOut;

            public DateTime? StopAt { get; }
            public long? TTL { get; }

            private async Task<DirectoryCrawlContext> GetRootContext(LocalDbContext dbContext, Subdirectory subdirectory, IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken)
            {
                EntityEntry<Subdirectory> entry = dbContext.Entry(subdirectory);
                Subdirectory parent = await entry.GetRelatedReferenceAsync(d => d.Parent, cancellationToken);
                if (parent is null)
                {
                    Volume volume = await entry.GetRelatedReferenceAsync(d => d.Volume, cancellationToken);
                    if (volume is null)
                        throw new InvalidOperationException("Subdirectory has no parent or volume");
                    ILogicalDiskInfo[] logicalDiskInfos = await fileSystemDetailService.GetLogicalDisksAsync(cancellationToken);
                    ILogicalDiskInfo matchingDiskInfo = logicalDiskInfos.FirstOrDefault(d => d.TryGetVolumeIdentifier(out VolumeIdentifier vid) && vid.Equals(volume.Identifier));
                    if (matchingDiskInfo is not null)
                        return new DirectoryCrawlContext()
                        {
                            DbContext = dbContext,
                            EventArgs = new(new DirectoryInfo(matchingDiskInfo.Name), subdirectory, "", ConcurrencyId),
                            ItemNumber = 0,
                            Token = cancellationToken,
                            Depth = 0
                        };
                }
                else
                {
                    DirectoryCrawlContext p = await GetRootContext(dbContext, parent, fileSystemDetailService, cancellationToken);
                    if (p is not null)
                        return new DirectoryCrawlContext()
                        {
                            DbContext = dbContext,
                            EventArgs = new(new DirectoryInfo(Path.Combine(p.EventArgs.GetFullName(), subdirectory.Name)), subdirectory, "", p.EventArgs),
                            ItemNumber = 0,
                            Token = cancellationToken,
                            Depth = 0
                        };
                }
                return null;
            }

            private static Task CrawlAsync(DirectoryCrawlContext crawlContext)
            {
                throw new NotImplementedException();
            }

            internal async Task CrawlAsync()
            {
                using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Subdirectory subdirectory = await dbContext.Subdirectories.FindAsync(new object[] { _crawlConfiguration.RootId }, _tokenSource.Token);
                if (subdirectory is null)
                    throw new Exception("Could not find crawl root subdirectory record");
                DirectoryCrawlContext rootContext = await GetRootContext(dbContext, subdirectory, serviceScope.ServiceProvider.GetRequiredService<IFileSystemDetailService>(), _tokenSource.Token);
                TimeSpan? ttl = TTL.HasValue ? TimeSpan.FromSeconds(TTL.Value) : null;
                if (StopAt.HasValue)
                {
                    TimeSpan t = StopAt.Value.Subtract(DateTime.Now);
                    if (!(ttl.HasValue && ttl.Value < t))
                        ttl = t;
                }
                if (ttl.HasValue)
                {
                    if (ttl.Value > TimeSpan.Zero)
                    {
                        using Timer timer = new(o =>
                        {
                            if (!_isTimedOut.HasValue)
                                _isTimedOut = true;
                            Cancel(true);
                        }, null, ttl.Value, Timeout.InfiniteTimeSpan);
                        await CrawlAsync(rootContext);
                        return;
                    }
                    _isTimedOut = true;
                    Cancel(true);
                }
                await CrawlAsync(rootContext);
            }

            internal CrawlJob([DisallowNull] CrawlManagerService service, [DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime? stopAt, CancellationTokenSource tokenSource)
            {
                _service = service;
                _crawlConfiguration = crawlConfiguration;
                _tokenSource = tokenSource;
                StopAt = stopAt;
                TTL = _crawlConfiguration.TTL;
            }

            public DateTime Started => throw new NotImplementedException();

            public ICurrentItem CurrentItem { get; private set; }

            public string Title => throw new NotImplementedException();

            public string Message => throw new NotImplementedException();

            public StatusMessageLevel MessageLevel => throw new NotImplementedException();

            public Guid ConcurrencyId => throw new NotImplementedException();

            public AsyncJobStatus JobStatus => throw new NotImplementedException();

            public Task Task => throw new NotImplementedException();

            public bool IsCancellationRequested => throw new NotImplementedException();

            public TimeSpan Elapsed => throw new NotImplementedException();

            public object AsyncState => throw new NotImplementedException();

            public WaitHandle AsyncWaitHandle => throw new NotImplementedException();

            public bool CompletedSynchronously => throw new NotImplementedException();

            public bool IsCompleted => throw new NotImplementedException();

            public void Cancel(bool throwOnFirstException)
            {
                throw new NotImplementedException();
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }
        }
    }
}
