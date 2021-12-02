namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Reports progress information for a timed asynchronous operation.
    /// </summary>
    /// <typeparam name="TEvent">The type of event that gets pushed to the corresponding <see cref="IBackgroundOperation"/>.</typeparam>
    /// <seealso cref="IBackgroundProgress{TEvent}" />
    /// <seealso cref="ITimedBackgroundProgressInfo" />
    /// <seealso cref="IBackgroundProgressFactory" />
    /// <seealso cref="ITimedBackgroundProgressEvent" />
    public interface ITimedBackgroundProgress<TEvent> : IBackgroundProgress<TEvent>, ITimedBackgroundProgressInfo, IBackgroundProgressFactory
        where TEvent : ITimedBackgroundProgressEvent
    {
    }

    /// <summary>
    /// Reports progress information for a timed asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <typeparam name="TEvent">The type of event that gets pushed to the corresponding <see cref="IBackgroundOperation{TState}"/>.</typeparam>
    /// <seealso cref="ITimedBackgroundProgress{TEvent}" />
    /// <seealso cref="IBackgroundProgress{TState, TEvent}" />
    /// <seealso cref="ITimedBackgroundProgressInfo{TState}" />
    /// <seealso cref="IBackgroundProgressFactory" />
    /// <seealso cref="ITimedBackgroundProgressEvent{TState}" />
    public interface ITimedBackgroundProgress<TState, TEvent> : ITimedBackgroundProgress<TEvent>, IBackgroundProgress<TState, TEvent>, ITimedBackgroundProgressInfo<TState>, IBackgroundProgressFactory
        where TEvent : ITimedBackgroundProgressEvent<TState>
    {
    }
}
