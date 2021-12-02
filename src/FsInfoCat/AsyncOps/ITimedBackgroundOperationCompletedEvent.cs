namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents the final progress event for the completion of a timed asynchronous operation.
    /// </summary>
    /// <seealso cref="IBackgroundOperationCompletedEvent" />
    /// <seealso cref="ITimedBackgroundProgressEvent" />
    /// <seealso cref="ITimedBackgroundOperationErrorOptEvent" />
    public interface ITimedBackgroundOperationCompletedEvent : IBackgroundOperationCompletedEvent, ITimedBackgroundProgressEvent, ITimedBackgroundOperationErrorOptEvent
    {
    }

    /// <summary>
    /// Represents the final progress event for the completion of a timed asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="ITimedBackgroundOperationCompletedEvent" />
    /// <seealso cref="IBackgroundOperationCompletedEvent{TState}" />
    /// <seealso cref="ITimedBackgroundProgressEvent{TState}" />
    /// <seealso cref="ITimedBackgroundOperationErrorOptEvent{TState}" />
    public interface ITimedBackgroundOperationCompletedEvent<TState> : ITimedBackgroundOperationCompletedEvent, IBackgroundOperationCompletedEvent<TState>, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationErrorOptEvent<TState>
    {
    }
}
