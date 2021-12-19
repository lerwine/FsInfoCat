using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about the state of a timed activity.
    /// </summary>
    /// <seealso cref="IActivityInfo" />
    public interface ITimedActivityInfo : IActivityInfo
    {
        /// <summary>
        /// Gets the activity start time.
        /// </summary>
        /// <value>A <see cref="DateTime"/> indicating when the activity was started. If StatusValue is WaitingToRun, this will be the date and time when the activity was created.</value>
        DateTime Started { get; }

        /// <summary>
        /// Gets the activity execution time.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> that indicates the amount of time that the activity had been running.</value>
        TimeSpan Duration { get; }
    }

    /// <summary>
    /// Contains information about the state of a timed activity that is associated with a user-specified value.
    /// </summary>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="ITimedActivityInfo" />
    public interface ITimedActivityInfo<TState> : IActivityInfo<TState>, ITimedActivityInfo { }
}
