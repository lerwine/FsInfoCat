using System;
using System.Threading.Tasks;

namespace FsInfoCat
{
    [Obsolete("Use IHostedService")]
    public interface IAsyncJob : IAsyncResult
    {
        /// <summary>
        /// Gets the title of the background job.
        /// </summary>
        /// <value>
        /// The title of the background job.
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets the current background job status message.
        /// </summary>
        /// <value>
        /// The current background job status message.
        /// </value>
        string Message { get; }

        /// <summary>
        /// Gets the status message level value.
        /// </summary>
        /// <value>
        /// The status message level value.
        /// </value>
        StatusMessageLevel MessageLevel { get; }

        /// <summary>
        /// Gets the background job concurrency identifier.
        /// </summary>
        /// <value>
        /// The unique identifier that distinguishes the asynchronous job.
        /// </value>
        Guid ConcurrencyId { get; }

        /// <summary>
        /// Gets the status of the background job.
        /// </summary>
        /// <value>
        /// The status of the background job.
        /// </value>
        AsyncJobStatus JobStatus { get; }

        /// <summary>
        /// Gets the task that is executing the background job.
        /// </summary>
        /// <value>
        /// The task that is executing the background job.
        /// </value>
        Task Task { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is or was being canceled.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this background job is or was being canceled; otherwise, <see langword="false"/>.
        /// </value>
        bool IsCancellationRequested { get; }

        /// <summary>
        /// Gets the background job execution duration.
        /// </summary>
        /// <value>
        /// The amount of time that the background job has been in operation.
        /// </value>
        TimeSpan Duration { get; }

        /// <summary>
        /// Cancels the current background job.
        /// </summary>
        /// <param name="throwOnFirstException"><see langword="true"/> if exceptions should immediately propagate; otherwise, <see langword="false"/>.</param>
        void Cancel(bool throwOnFirstException);

        /// <summary>
        /// Cancels the current background job.
        /// </summary>
        void Cancel();
    }

    [Obsolete("Use IHostedService")]
    public interface IAsyncJob<TResult> : IAsyncJob
    {
        /// <summary>
        /// Gets the task that is executing the background job.
        /// </summary>
        /// <value>
        /// The task that is executing the background job and returns the result value.
        /// </value>
        new Task<TResult> Task { get; }
    }
}
