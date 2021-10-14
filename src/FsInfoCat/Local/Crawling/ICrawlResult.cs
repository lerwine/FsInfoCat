using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    /// <summary>
    /// Describes the result of a file system crawl job.
    /// </summary>
    public interface ICrawlResult : IAsyncResult
    {
        /// <summary>
        /// Gets the start date/time of the crawl job.
        /// </summary>
        /// <value>
        /// The date/time that <see cref="IHostedService.StartAsync(System.Threading.CancellationToken)"/> was invoked.
        /// </value>
        DateTime Started { get; }

        /// <summary>
        /// Gets the crawl job execution duration.
        /// </summary>
        /// <value>
        /// The amount of time from <see cref="IHostedService.StartAsync(System.Threading.CancellationToken)"/> was invoked until the job terminated.
        /// </value>
        TimeSpan Duration { get; }

        /// <summary>
        /// Indicates why the file system crawl was terminated.
        /// </summary>
        CrawlTerminationReason TerminationReason { get; }


        /// <summary>
        /// Gets the completed worker task.
        /// </summary>
        Task WorkerTask { get; }
    }
}
