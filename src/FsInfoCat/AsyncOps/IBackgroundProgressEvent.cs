namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes an asynchronous operation progress event.
    /// </summary>
    /// <seealso cref="IBackgroundProgressInfo" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundProgressEvent : IBackgroundProgressInfo
    {
        /// <summary>
        /// Gets the message code associated with the progress event.
        /// </summary>
        /// <value>The message code for the progress event or <see langword="null"/> if there is no message code.</value>
        MessageCode? Code { get; }
    }

    /// <summary>
    /// Describes an asynchronous operation progress event.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundProgressEvent" />
    /// <seealso cref="IBackgroundProgressInfo{TState}" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundProgressEvent<TState> : IBackgroundProgressEvent, IBackgroundProgressInfo<TState>
    {
    }
}
