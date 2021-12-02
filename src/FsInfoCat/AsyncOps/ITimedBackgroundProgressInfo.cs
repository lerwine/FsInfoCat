using System;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes a timed asynchronous operation.
    /// </summary>
    /// <seealso cref="IBackgroundProgressInfo" />
    public interface ITimedBackgroundProgressInfo : IBackgroundProgressInfo
    {
        /// <summary>
        /// Gets the duration of the background operation.
        /// </summary>
        /// <value>The amount of time the background operation has been running.</value>
        TimeSpan Duration { get; }
    }

    /// <summary>
    /// Describes a timed asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="ITimedBackgroundProgressInfo" />
    /// <seealso cref="IBackgroundProgressInfo{TState}" />
    public interface ITimedBackgroundProgressInfo<TState> : ITimedBackgroundProgressInfo, IBackgroundProgressInfo<TState>
    {
    }
}
