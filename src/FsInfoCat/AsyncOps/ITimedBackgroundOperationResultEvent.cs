namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents the final progress event for the completion of a timed asynchronous operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundOperationResultEvent{TResult}" />
    /// <seealso cref="ITimedBackgroundOperationCompletedEvent" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundOperationResultEvent<TResult> : IBackgroundOperationResultEvent<TResult>, ITimedBackgroundOperationCompletedEvent
    {
    }

    /// <summary>
    /// Represents the final progress event for the completion of a timed asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the asynchronous operation.</typeparam>
    /// <seealso cref="ITimedBackgroundOperationResultEvent{TResult}" />
    /// <seealso cref="IBackgroundOperationResultEvent{TState, TResult}" />
    /// <seealso cref="ITimedBackgroundOperationCompletedEvent{TState}" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundOperationResultEvent<TState, TResult> : IBackgroundOperationResultEvent<TState, TResult>, ITimedBackgroundOperationCompletedEvent<TState>, ITimedBackgroundOperationResultEvent<TResult>, IBackgroundOperationResultEvent<TResult>, ITimedBackgroundOperationCompletedEvent
    {
    }
}
