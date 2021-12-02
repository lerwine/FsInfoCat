using System;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes an asynchronous operation progress event.
    /// </summary>
    /// <seealso cref="IBackgroundProgressEvent" />
    public interface IBackgroundOperationErrorOptEvent : IBackgroundProgressEvent
    {
        /// <summary>
        /// Gets the exception for the asynchronous operation event.
        /// </summary>
        /// <value>The exception for the asynchronous operation event or <see langword="null"/> if there is no exception.</value>
        Exception Error { get; }
    }

    /// <summary>
    /// Describes an asynchronous operation progress event.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundOperationErrorOptEvent" />
    /// <seealso cref="IBackgroundProgressEvent{TState}" />
    public interface IBackgroundOperationErrorOptEvent<TState> : IBackgroundProgressEvent<TState>, IBackgroundOperationErrorOptEvent
    {
    }
}
