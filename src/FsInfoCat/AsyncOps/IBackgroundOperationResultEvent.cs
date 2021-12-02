namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents the final progress event for the completion of an asynchronous operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundOperationCompletedEvent" />
    public interface IBackgroundOperationResultEvent<TResult> : IBackgroundOperationCompletedEvent
    {
        /// <summary>
        /// Gets the result value produced by the background operation.
        /// </summary>
        /// <value>The result value produced by the background operation.</value>
        /// <exception cref="System.OperationCanceledException">The asynchronous operation was canceled.</exception>
        /// <exception cref="System.Exception">The asynchronous operation terminated with an exception.</exception>
        TResult Result { get; }
    }

    /// <summary>
    /// Represents the final progress event for the completion of an asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundOperationResultEvent{TResult}" />
    /// <seealso cref="IBackgroundOperationCompletedEvent{TState}" />
    public interface IBackgroundOperationResultEvent<TState, TResult> : IBackgroundOperationCompletedEvent<TState>, IBackgroundOperationResultEvent<TResult>
    {
    }
}
