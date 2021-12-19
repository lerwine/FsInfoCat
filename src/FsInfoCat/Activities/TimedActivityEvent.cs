using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed activity event.
    /// </summary>
    /// <seealso cref="ActivityEvent" />
    /// <seealso cref="ITimedActivityEvent" />
    public class TimedActivityEvent : ActivityEvent, ITimedActivityEvent
    {
        /// <summary>
        /// Gets the activity start time.
        /// </summary>
        /// <value>A <see cref="DateTime"/> indicating when the activity was started. If StatusValue is WaitingToRun, this will be the date and time when the activity was created.</value>
        public DateTime Started { get; }

        /// <summary>
        /// Gets the activity execution time.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> that indicates the amount of time that the activity had been running.</value>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public TimedActivityEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        public TimedActivityEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error) => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);
    }

    /// <summary>
    /// Represents an event for a timed activity that has an associated user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
    /// <seealso cref="TimedActivityEvent" />
    /// <seealso cref="ITimedActivityEvent{TState}" />
    public class TimedActivityEvent<TState> : TimedActivityEvent, ITimedActivityEvent<TState>
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public TimedActivityEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        public TimedActivityEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error) => AsyncState = activityInfo.AsyncState;
    }
}
