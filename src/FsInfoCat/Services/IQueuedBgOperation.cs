using System;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    /// <summary>
    /// Represents a background operation enqueued by the <see cref="IFSIOQueueService"/>.
    /// </summary>
    public interface IQueuedBgOperation : IAsyncResult
    {
        /// <summary>
        /// Gets the status of the background operation.
        /// </summary>
        AsyncJobStatus Status { get; }

        /// <summary>
        /// Gets the <see cref="Task"/> for the background operation.
        /// </summary>
        /// <remarks>If <see cref="Task.Status"/> is <see cref="TaskStatus.Running"/>, this does not necessarily indicate that the background operation is in progress. Refer to <see cref="Status"/> to get the status of the background operation.</remarks>
        Task Task { get; }

        /// <summary>
        /// Gets the date and time when the background operation was started.
        /// If the background operation is still enqueued, this will be the date and time that the background operation was enqueued.
        /// </summary>
        DateTime Started { get; }

        /// <summary>
        /// Gets the amount of time that the background operation has been running.
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// Cancels the background operation;
        /// </summary>
        void Cancel();

        /// <summary>
        /// Cancels the background operation after a specified number of milliseconds.
        /// </summary>
        /// <param name="millisecondsDelay">Number of miilliseconds to wait before cancelling the background operation.</param>
        void CancelAfter(int millisecondsDelay);

        /// <summary>
        /// Cancels the background operation after a specified duration.
        /// </summary>
        /// <param name="delay">Duration of delay before cancelling the background operation.</param>
        void CancelAfter(TimeSpan delay);
    }

    /// <summary>
    /// Represents a background operation that produces a result value, enqueued by the <see cref="IFSIOQueueService"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
    public interface IQueuedBgOperation<TResult> : IQueuedBgOperation
    {
        /// <summary>
        /// Gets the <see cref="Task{TResult}"/> for the background operation.
        /// </summary>
        /// <remarks>If <see cref="Task.Status"/> is <see cref="TaskStatus.Running"/>, this does not necessarily indicate that the background operation is in progress. Refer to <see cref="Status"/> to get the status of the background operation.</remarks>
        new Task<TResult> Task { get; }
    }
}
