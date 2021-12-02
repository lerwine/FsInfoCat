using System;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes an asynchronous operation that can be canceled.
    /// </summary>
    /// <remarks>This pushes <see cref="IBackgroundProgressEvent"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundProgressInfo" />
    /// <seealso cref="IBackgroundProgressEvent" />
    /// <seealso cref="IObservable{IBackgroundProgressEvent}" />
    public interface ICancellableOperation : IBackgroundProgressInfo, IObservable<IBackgroundProgressEvent>
    {
        /// <summary>
        /// Gets a value indicating whether cancellation has been requested for this asynchronous operation.
        /// </summary>
        /// <value><see langword="true"/> if cancellation has been requested for this asynchronous operation; otherwise, <see langword="false"/>.</value>
        bool IsCancellationRequested { get; }

        /// <summary>
        /// Communicates a request to cancel this asynchronous operation.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Communicates a request to cancel this asynchronous operation, specifying whether remaining callbacks and cancelable operations should be processed if an exception occurs.
        /// </summary>
        /// <param name="throwOnFirstException"><see langword="true"/> if exceptions should immediately propagate; otherwise, <see langword="false"/>.</param>
        void Cancel(bool throwOnFirstException);

        /// <summary>
        /// Schedules a cancellation request for this asynchronous operation after the specified number of milliseconds.
        /// </summary>
        /// <param name="millisecondsDelay">The number of milliseconds to wait before cancelling this asynchronous operation.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsDelay"/> is less than zero.</exception>
        void CancelAfter(int millisecondsDelay);

        /// <summary>
        /// Schedules a cancellation request for this asynchronous operation after the specified time span
        /// </summary>
        /// <param name="delay">The time span to wait before cancelling this asynchronous operation.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="delay"/> is less than zero or greater than <see cref="int.MaxValue"/>.</exception>
        void CancelAfter(TimeSpan delay);
    }

    /// <summary>
    /// Describes an asynchronous operation that can be canceled.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="IBackgroundProgressEvent{TState}"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="ICancellableOperation" />
    /// <seealso cref="IBackgroundProgressInfo{TState}" />
    /// <seealso cref="IBackgroundProgressEvent{TState}" />
    /// <seealso cref="IObservable{IBackgroundProgressEvent{TState}}" />
    public interface ICancellableOperation<TState> : ICancellableOperation, IBackgroundProgressInfo<TState>, IObservable<IBackgroundProgressEvent<TState>>
    {
    }
}
