using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents information about the state of a timed activity.
    /// </summary>
    /// <seealso cref="IActivityInfo" />
    public interface ITimedActivityInfo : IActivityInfo
    {
        /// <summary>
        /// Gets the start time.
        /// </summary>
        /// <value>The date and time when the activity was started started.</value>
        /// <remarks>If <see cref="IActivityStatusInfo.StatusValue"/> is <see cref="ActivityStatus.WaitingToRun"/>, this will be the date and time when this object was instantiated.</remarks>
        DateTime Started { get; }

        /// <summary>
        /// Gets the duration of the activity.
        /// </summary>
        /// <value>The duration of the activity.</value>
        TimeSpan Duration { get; }
    }

    /// <summary>
    /// Represents information about the state of a timed activity that is associated with a user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="ITimedActivityInfo" />
    public interface ITimedActivityInfo<TState> : IActivityInfo<TState>, ITimedActivityInfo { }
}
