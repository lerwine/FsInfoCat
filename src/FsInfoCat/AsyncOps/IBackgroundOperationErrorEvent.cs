namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes an asynchronous operation error event.
    /// </summary>
    /// <seealso cref="IBackgroundOperationErrorOptEvent" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundOperationErrorEvent : IBackgroundOperationErrorOptEvent
    {
        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>The error code to associate with the error.</value>
        new ErrorCode Code { get; }
    }

    /// <summary>
    /// Describes an asynchronous operation error event.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundOperationErrorEvent" />
    /// <seealso cref="IBackgroundOperationErrorOptEvent{TState}" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundOperationErrorEvent<TState> : IBackgroundOperationErrorOptEvent<TState>, IBackgroundOperationErrorEvent
    {
    }
}
