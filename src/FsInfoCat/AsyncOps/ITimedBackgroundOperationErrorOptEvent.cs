namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes a timed asynchronous operation progress event.
    /// </summary>
    /// <seealso cref="IBackgroundOperationErrorOptEvent" />
    /// <seealso cref="ITimedBackgroundProgressEvent" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundOperationErrorOptEvent : IBackgroundOperationErrorOptEvent, ITimedBackgroundProgressEvent
    {
    }

    /// <summary>
    /// Describes a timed asynchronous operation progress event.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="ITimedBackgroundOperationErrorOptEvent" />
    /// <seealso cref="IBackgroundOperationErrorOptEvent{TState}" />
    /// <seealso cref="ITimedBackgroundProgressEvent{TState}" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundOperationErrorOptEvent<TState> : IBackgroundOperationErrorOptEvent<TState>, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationErrorOptEvent
    {
    }
}
