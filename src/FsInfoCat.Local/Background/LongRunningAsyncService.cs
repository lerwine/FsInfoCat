using FsInfoCat.Background;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public abstract class LongRunningAsyncService<TTask> : BackgroundService, ILongRunningAsyncService
        where TTask : Task
    {
        private Stopwatch _stopwatch;
        private TTask _task;

        protected CancellationToken StoppingToken { get; private set; }

        protected ILogger Logger { get; }

        public DateTime Started { get; private set; }

        public TimeSpan Elapsed => _stopwatch?.Elapsed ?? TimeSpan.Zero;

        public AsyncJobStatus JobStatus { get; private set; }

        public TTask Task => _task ?? throw new InvalidOperationException();

        Task ILongRunningAsyncService.Task => Task;

        object IAsyncResult.AsyncState => ((IAsyncResult)(Task ?? throw new InvalidOperationException())).AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => ((IAsyncResult)(Task ?? throw new InvalidOperationException())).AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => ((IAsyncResult)Task)?.CompletedSynchronously ?? false;

        bool IAsyncResult.IsCompleted => ((IAsyncResult)Task)?.IsCompleted ?? false;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected abstract TTask ExecuteAsync();

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            JobStatus = AsyncJobStatus.Running;
            Stopwatch stopwatch = new();
            stopwatch.Start();
            _stopwatch = stopwatch;
            StoppingToken = stoppingToken;
            TTask workerTask = ExecuteAsync();
            _task = workerTask;
            workerTask.ContinueWith(task =>
            {
                stopwatch.Stop();
                if (task.IsCanceled)
                    JobStatus = AsyncJobStatus.Canceled;
                else if (task.IsFaulted)
                    JobStatus = AsyncJobStatus.Faulted;
                else
                    JobStatus = AsyncJobStatus.Succeeded;
            }, TaskContinuationOptions.NotOnFaulted);
            return workerTask;
        }

        protected LongRunningAsyncService(ILogger logger) { Logger = logger; }
    }
}
