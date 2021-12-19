using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed activity operation event.
    /// </summary>
    /// <seealso cref="OperationEvent" />
    /// <seealso cref="ITimedOperationEvent" />
    public class TimedOperationEvent : OperationEvent, ITimedOperationEvent
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
        /// Initializes a new instance of the <see cref="TimedOperationEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedOperationEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, string currentOperation) : base(activityInfo, statusValue, currentOperation)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedOperationEvent([DisallowNull] ITimedActivityInfo activityInfo, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, error, currentOperation)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent"/> class.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        public TimedOperationEvent([DisallowNull] ITimedOperationInfo operationInfo, ActivityState statusValue) : base(operationInfo, statusValue) => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        public TimedOperationEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent"/> class.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedOperationEvent([DisallowNull] ITimedOperationInfo operationInfo, [DisallowNull] Exception error) : base(operationInfo, error) => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedOperationEvent([DisallowNull] ITimedActivityInfo activityInfo, [DisallowNull] Exception error) : base(activityInfo, error) => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);
    }

    /// <summary>
    /// Represents an operation event for a timed activity that has an associated user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
    /// <seealso cref="TimedOperationEvent" />
    /// <seealso cref="ITimedOperationEvent{TState}" />
    public class TimedOperationEvent<TState> : TimedOperationEvent, ITimedOperationEvent<TState>
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedOperationEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue, string currentOperation) : base(activityInfo, statusValue, currentOperation)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedOperationEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, error, currentOperation)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        public TimedOperationEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, ActivityState statusValue) : base(operationInfo, statusValue) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        public TimedOperationEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedOperationEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, [DisallowNull] Exception error) : base(operationInfo, error) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedOperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedOperationEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, [DisallowNull] Exception error) : base(activityInfo, error) => AsyncState = activityInfo.AsyncState;
    }
}
