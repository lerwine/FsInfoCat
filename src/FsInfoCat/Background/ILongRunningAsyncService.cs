using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService, instead")]
    public interface ILongRunningAsyncService : IAsyncResult, IHostedService, IDisposable
    {
        /// <summary>
        /// Gets the start date/time of the crawl job.
        /// </summary>
        /// <value>
        /// The date/time that <see cref="IHostedService.StartAsync(CancellationToken)"/> was invoked.
        /// </value>
        DateTime Started { get; }

        /// <summary>
        /// Gets the background job execution duration.
        /// </summary>
        /// <value>
        /// The amount of time that the background job has been in operation.
        /// </value>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// Gets the status of the background job.
        /// </summary>
        /// <value>
        /// The status of the background job.
        /// </value>
        AsyncJobStatus JobStatus { get; }

        /// <summary>
        /// Gets the latest task.
        /// </summary>
        /// <value>The latest task that was created.</value>
        Task Task { get; }
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService, instead")]
    public interface ILongRunningAsyncService<TResult> : ILongRunningAsyncService
    {
        /// <summary>
        /// Gets the latest task.
        /// </summary>
        /// <value>The latest task that was created.</value>
        new Task<TResult> Task { get; }
    }

    [Obsolete("Use FsInfoCat.AsyncOps.IBackgroundProgressFactory, instead")]
    public interface IBackgroundTaskQueue
    {
        Task EnqueueAsync(Func<CancellationToken, Task> doWorkAsync);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
