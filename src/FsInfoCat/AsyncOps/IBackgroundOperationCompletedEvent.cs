namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents the final progress event for the completion of an asynchronous operation.
    /// </summary>
    /// <seealso cref="IBackgroundProgressEvent" />
    /// <seealso cref="IBackgroundOperationErrorOptEvent" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundOperationCompletedEvent : IBackgroundProgressEvent, IBackgroundOperationErrorOptEvent
    {
        /// <summary>
        /// Gets a value indicating whether the asynchronous operation ran to completion.
        /// </summary>
        /// <value><see langword="true"/> if the associated background <see cref="System.Threading.Tasks.Task"/> ran to completion; otherwise, <see langword="false"/>.</value>
        bool RanToCompletion { get; }
    }

    /// <summary>
    /// Represents the final progress event for the completion of an asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundOperationCompletedEvent" />
    /// <seealso cref="IBackgroundProgressEvent{TState}" />
    /// <seealso cref="IBackgroundOperationErrorOptEvent{TState}" />
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundOperationCompletedEvent<TState> : IBackgroundProgressEvent<TState>, IBackgroundOperationErrorOptEvent<TState>, IBackgroundOperationCompletedEvent
    {
    }
}
