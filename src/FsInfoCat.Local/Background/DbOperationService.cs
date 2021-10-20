using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    [Obsolete("Use FsInfoCat.AsyncOps.JobQueue for long running background tasks")]
    public class DbOperationService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<DbOperationService> _logger;
        private readonly CancellationTokenRegistration _registration;

        private readonly Channel<IWorkItem> _queue = Channel.CreateUnbounded<IWorkItem>();

        private void OnApplicationStopping() => _queue.Writer.TryComplete();

        [ServiceBuilderHandler(Priority = 100)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DbOperationService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<DbOperationService>();
        }

        public DbOperationService(IServiceProvider services, IHostApplicationLifetime applicationLifetime, ILogger<DbOperationService> logger)
        {
            _logger = logger;
            _logger.LogDebug($"{nameof(DbOperationService)} Service constructor invoked");
            _services = services;
            _registration = applicationLifetime.ApplicationStopping.Register(OnApplicationStopping);
            _logger.LogDebug($"{nameof(DbOperationService)} Service instantiated");
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using CancellationTokenRegistration registration = stoppingToken.Register(() => _queue.Writer.TryComplete());
            while (await _queue.Reader.WaitToReadAsync())
            {
                if (_queue.Reader.TryRead(out IWorkItem workItem))
                    await workItem.RunAsync(_services, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _queue.Writer.TryComplete();
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            try { _registration.Dispose(); }
            finally { base.Dispose(); }
        }

        public async Task<WorkItem<TResult>> EnqueueAsync<TResult>(Func<LocalDbContext, CancellationToken, Task<TResult>> doWorkAsync, IProgress<WorkItem<TResult>> onProgress = null)
        {
            WorkItem<TResult> workItem = new(doWorkAsync, onProgress);
            await _queue.Writer.WriteAsync(workItem);
            return workItem;
        }

        public async Task<TResult> RunAsync<TResult>(Func<LocalDbContext, CancellationToken, Task<TResult>> doWorkAsync, IProgress<WorkItem<TResult>> onProgress = null)
        {
            WorkItem<TResult> workItem = await EnqueueAsync(doWorkAsync, onProgress);
            return await workItem.Task;
        }

        interface IWorkItem
        {
            Task RunAsync(IServiceProvider services, CancellationToken stoppingToken);
        }

        public class WorkItem<TResult> : IAsyncResult, IWorkItem, IDisposable
        {
            private readonly SemaphoreSlim _semaphore = new(0);
            private readonly CancellationTokenSource _tokenSource = new();
            private readonly Func<LocalDbContext, CancellationToken, Task<TResult>> _doWorkAsync;
            private CancellationTokenRegistration? _cancellationRegistration;
            private readonly IProgress<WorkItem<TResult>> _onProgress;
            private IServiceScope _serviceScope;
            private readonly Stopwatch _stopwatch = new();
            private readonly Task _completion;

            public DateTime Started { get; private set; }

            public AsyncJobStatus JobStatus { get; private set; }

            public TimeSpan Elapsed => _stopwatch.Elapsed;

            public TResult Result => Task.Result;

            public Task<TResult> Task { get; }

            public LocalDbContext DbContext { get; private set; }

            public object AsyncState => ((IAsyncResult)Task).AsyncState;

            public WaitHandle AsyncWaitHandle => ((IAsyncResult)_completion).AsyncWaitHandle;

            public bool CompletedSynchronously => ((IAsyncResult)_completion).CompletedSynchronously && ((IAsyncResult)Task).CompletedSynchronously;

            public bool IsCompleted => ((IAsyncResult)_completion).IsCompleted;

            private async Task<TResult> DoWork(CancellationToken cancellationToken)
            {
                await _semaphore.WaitAsync();
                if (JobStatus == AsyncJobStatus.WaitingToRun)
                {
                    JobStatus = AsyncJobStatus.Running;
                    _onProgress?.Report(this);
                }
                return await _doWorkAsync(DbContext, cancellationToken);
            }

            internal WorkItem(Func<LocalDbContext, CancellationToken, Task<TResult>> doWorkAsync, IProgress<WorkItem<TResult>> onProgress)
            {
                _doWorkAsync = doWorkAsync;
                _onProgress = onProgress;
                Task = DoWork(_tokenSource.Token);
                _completion = Task.ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        JobStatus = AsyncJobStatus.Canceled;
                    else if (task.IsFaulted)
                        JobStatus = AsyncJobStatus.Faulted;
                    else
                        JobStatus = AsyncJobStatus.Succeeded;
                    _onProgress?.Report(this);
                });
            }

            public void Dispose()
            {
                switch (JobStatus)
                {
                    case AsyncJobStatus.WaitingToRun:
                    case AsyncJobStatus.Running:
                        if (!_tokenSource.IsCancellationRequested)
                        {
                            JobStatus = AsyncJobStatus.Cancelling;
                            _tokenSource.Cancel(true);
                        }
                        break;
                }
                _cancellationRegistration?.Dispose();
                DbContext?.Dispose();
                _serviceScope?.Dispose();
                ((IDisposable)_semaphore).Dispose();
                _tokenSource.Dispose();
            }

            async Task IWorkItem.RunAsync(IServiceProvider services, CancellationToken stoppingToken) 
            {
                try
                {
                    _cancellationRegistration = stoppingToken.Register(() => _tokenSource.Cancel(true));
                    stoppingToken.ThrowIfCancellationRequested();
                    _serviceScope = services.CreateScope();
                    DbContext = _serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                }
                finally { _semaphore.Release(); }
                await Task;
            }
        }
    }
}
