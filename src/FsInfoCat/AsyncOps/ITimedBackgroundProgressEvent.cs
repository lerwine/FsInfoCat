namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes a timed asynchronous operation progress event.
    /// </summary>
    /// <seealso cref="IBackgroundProgressEvent" />
    /// <seealso cref="ITimedBackgroundProgressInfo" />
    public interface ITimedBackgroundProgressEvent : IBackgroundProgressEvent, ITimedBackgroundProgressInfo
    {
    }

    /// <summary>
    /// Describes a timed asynchronous operation progress event.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="ITimedBackgroundProgressEvent" />
    /// <seealso cref="IBackgroundProgressEvent{TState}" />
    /// <seealso cref="ITimedBackgroundProgressInfo{TState}" />
    public interface ITimedBackgroundProgressEvent<TState> : ITimedBackgroundProgressEvent, IBackgroundProgressEvent<TState>, ITimedBackgroundProgressInfo<TState>
    {
    }
}
