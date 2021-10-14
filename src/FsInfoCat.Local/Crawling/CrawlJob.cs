using FsInfoCat.Local.Background;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public partial class CrawlJob : BackgroundService, ICrawlJob
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

        Task ILongRunningAsyncService.Task => Task;

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
    }
}
