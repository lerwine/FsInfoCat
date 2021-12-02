namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents the final progress event for a timed asynchronous operation that terminated due to an unhandled exception.
    /// </summary>
    /// <seealso cref="IBackgroundOperationFaultedEvent" />
    /// <seealso cref="ITimedBackgroundOperationCompletedEvent" />
    /// <seealso cref="ITimedBackgroundOperationErrorEvent" />
    public interface ITimedBackgroundOperationFaultedEvent : IBackgroundOperationFaultedEvent, ITimedBackgroundOperationCompletedEvent, ITimedBackgroundOperationErrorEvent
    {
    }

    /// <summary>
    /// Represents the final progress event for a timed asynchronous operation that terminated due to an unhandled exception.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="ITimedBackgroundOperationFaultedEvent" />
    /// <seealso cref="IBackgroundOperationFaultedEvent{TState}" />
    /// <seealso cref="ITimedBackgroundOperationCompletedEvent{TState}" />
    /// <seealso cref="ITimedBackgroundOperationErrorEvent{TState}" />
    public interface ITimedBackgroundOperationFaultedEvent<TState> : IBackgroundOperationFaultedEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>, ITimedBackgroundOperationErrorEvent<TState>, ITimedBackgroundOperationFaultedEvent
    {
    }
}
