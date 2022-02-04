using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes an activity event.
    /// </summary>
    /// <seealso cref="IActivityInfo" />
    public interface IActivityEvent : IActivityInfo
    {
        /// <summary>
        /// Gets the exception (if any) associated with the event.
        /// </summary>
        /// <value>The <see cref="Exception" /> associated with the event or <see langword="null" /> if there is none.</value>
        Exception Exception { get; }

        /// <summary>
        /// Gets the status message level.
        /// </summary>
        /// <value>The message level value for the associated <see cref="IActivityInfo.StatusMessage"/>.</value>
        StatusMessageLevel MessageLevel { get; }
    }

    /// <summary>
    /// Describes an event for an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IActivityEvent" />
    public interface IActivityEvent<TState> : IActivityInfo<TState>, IActivityEvent { }
}
