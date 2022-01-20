namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents the final progress event for an asynchronous operation that terminated due to an unhandled exception.
    /// </summary>
    /// <seealso cref="IBackgroundOperationCompletedEvent" />
    /// <seealso cref="IBackgroundOperationErrorEvent" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundOperationFaultedEvent : IBackgroundOperationCompletedEvent, IBackgroundOperationErrorEvent
    {
    }

    /// <summary>
    /// Represents the final progress event for an asynchronous operation that terminated due to an unhandled exception.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundOperationFaultedEvent" />
    /// <seealso cref="IBackgroundOperationCompletedEvent{TState}" />
    /// <seealso cref="IBackgroundOperationErrorEvent{TState}" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundOperationFaultedEvent<TState> : IBackgroundOperationCompletedEvent<TState>, IBackgroundOperationErrorEvent<TState>, IBackgroundOperationFaultedEvent
    {
    }
}
