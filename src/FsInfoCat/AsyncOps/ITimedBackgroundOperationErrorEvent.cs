namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes a timed asynchronous operation error event.
    /// </summary>
    /// <seealso cref="IBackgroundOperationErrorEvent" />
    /// <seealso cref="ITimedBackgroundOperationErrorOptEvent" />
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
    public interface ITimedBackgroundOperationErrorEvent<TState> : ITimedBackgroundOperationErrorEvent, IBackgroundOperationErrorEvent<TState>, ITimedBackgroundOperationErrorOptEvent<TState>
    {
    }
}
