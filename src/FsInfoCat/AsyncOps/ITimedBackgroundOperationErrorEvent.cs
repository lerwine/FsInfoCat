namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes a timed asynchronous operation error event.
    /// </summary>
    /// <seealso cref="IBackgroundOperationErrorEvent" />
    /// <seealso cref="ITimedBackgroundOperationErrorOptEvent" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundOperationErrorEvent : IBackgroundOperationErrorEvent, ITimedBackgroundOperationErrorOptEvent
    {
    }

    /// <summary>
    /// Describes a timed asynchronous operation error event.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="ITimedBackgroundOperationErrorEvent" />
    /// <seealso cref="IBackgroundOperationErrorEvent{TState}" />
    /// <seealso cref="ITimedBackgroundOperationErrorOptEvent{TState}" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundOperationErrorEvent<TState> : IBackgroundOperationErrorEvent<TState>, ITimedBackgroundOperationErrorOptEvent<TState>, ITimedBackgroundOperationErrorEvent
    {
    }
}
