using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public interface ILongRunningAsyncService : IAsyncResult, IHostedService, IDisposable
    {
        /// <summary>
        /// Gets the start date/time of the crawl job.
        /// </summary>
        /// <value>
        /// The date/time that <see cref="IHostedService.StartAsync(System.Threading.CancellationToken)"/> was invoked.
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

        Task Task { get; }
    }

    public interface ILongRunningAsyncService<TResult> : ILongRunningAsyncService
    {
        new Task<TResult> Task { get; }
    }
}
