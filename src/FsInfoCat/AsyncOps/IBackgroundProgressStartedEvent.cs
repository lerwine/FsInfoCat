namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents a progress event for the start of an asynchronous operation.
    /// </summary>
    /// <remarks>This pushes <see cref="IBackgroundProgressEvent{TState}" /> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundProgressEvent" />
    /// <seealso cref="ICancellableOperation" />
    public interface IBackgroundProgressStartedEvent : IBackgroundProgressEvent, ICancellableOperation
    {
    }

    /// <summary>
    /// Represents a progress event for the start of an asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="IBackgroundProgressEvent" /> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundProgressStartedEvent" />
    /// <seealso cref="IBackgroundProgressEvent{TState}" />
    /// <seealso cref="ICancellableOperation{TState}" />
    public interface IBackgroundProgressStartedEvent<TState> : IBackgroundProgressEvent<TState>, ICancellableOperation<TState>, IBackgroundProgressEvent, ICancellableOperation
    {
    }
}
