using System;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a background operation.
    /// </summary>
    [Obsolete("Use FsInfoCat.AsyncOps.IBackgroundOperation, instead")]
    public interface IBgOperation : IAsyncOperationInfo, IAsyncResult
    {
        /// <summary>
        /// Gets the <see cref="Task"/> for the background operation.
        /// </summary>
        /// <remarks>If <see cref="Task.Status"/> is <see cref="TaskStatus.Running"/>, this does not necessarily indicate that the background operation is in progress. Refer to <see cref="Status"/> to get the status of the background operation.</remarks>
        Task Task { get; }

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
    /// Represents a background operation.
    /// </summary>
    /// <typeparam name="T">The type of the asynchronous state value.</typeparam>
    [Obsolete("Use FsInfoCat.AsyncOps.IBackgroundOperation<T>, instead")]
    public interface IBgOperation<T> : IBgOperation, IAsyncOperationInfo<T> { }
}
