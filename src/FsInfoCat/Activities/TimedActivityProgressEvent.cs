using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed activity progress event.
    /// </summary>
    /// <seealso cref="ActivityProgressEvent" />
    /// <seealso cref="ITimedActivityProgressEvent" />
    public class TimedActivityProgressEvent : ActivityProgressEvent, ITimedActivityProgressEvent
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
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, string currentOperation, byte percentComplete) : base(activityInfo, statusValue, currentOperation,
            percentComplete) => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation, byte percentComplete) : base(activityInfo, statusValue, error, currentOperation, percentComplete)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, [DisallowNull] Exception error, string currentOperation, byte percentComplete) : base(activityInfo, ActivityState.Faulted, error, currentOperation, percentComplete)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, string currentOperation) : base(activityInfo, statusValue, currentOperation)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, statusValue, error, currentOperation)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, ActivityState.Faulted, error, currentOperation)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo operationInfo, ActivityState statusValue, byte percentComplete) : base(operationInfo, statusValue, (byte?)percentComplete)
            => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, byte percentComplete) : base(activityInfo, statusValue, percentComplete)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo operationInfo, ActivityState statusValue, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, statusValue, error, percentComplete)
            => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);
        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo operationInfo, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, ActivityState.Faulted, error, percentComplete)
            => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error, byte percentComplete) : base(activityInfo, statusValue, error, percentComplete)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, [DisallowNull] Exception error, byte percentComplete) : base(activityInfo, ActivityState.Faulted, error, percentComplete)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo operationInfo, ActivityState statusValue) : base(operationInfo, statusValue)
            => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue) : base(activityInfo, statusValue)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo operationInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(operationInfo, statusValue, error)
            => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo operationInfo, [DisallowNull] Exception error) : base(operationInfo, ActivityState.Faulted, error)
            => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo activityInfo, [DisallowNull] Exception error) : base(activityInfo, ActivityState.Faulted, error)
            => (Started, Duration) = (activityInfo.Started, activityInfo.Duration);
    }

    /// <summary>
    /// Represents a progress event for a timed activity that has an associated user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
    /// <seealso cref="TimedActivityProgressEvent" />
    /// <seealso cref="ITimedActivityProgressEvent{TState}" />
    public class TimedActivityProgressEvent<TState> : TimedActivityProgressEvent, ITimedActivityProgressEvent<TState>
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue, string currentOperation, byte percentComplete) : base(activityInfo, statusValue, currentOperation,
            percentComplete) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, [DisallowNull] Exception error, ActivityState statusValue, string currentOperation, byte percentComplete) : base(activityInfo, statusValue, error, currentOperation, percentComplete)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, [DisallowNull] Exception error, string currentOperation, byte percentComplete) : base(activityInfo, ActivityState.Faulted, error, currentOperation, percentComplete)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue, string currentOperation) : base(activityInfo, statusValue, currentOperation)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, statusValue, error, currentOperation)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, ActivityState.Faulted, error, currentOperation)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, ActivityState statusValue, byte percentComplete) : base(operationInfo, statusValue, percentComplete)
            => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue, byte percentComplete) : base(activityInfo, statusValue, percentComplete)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="operationInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, ActivityState statusValue, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, statusValue, error, percentComplete)
            => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, ActivityState.Faulted, error, percentComplete)
            => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error, byte percentComplete) : base(activityInfo, statusValue, error, percentComplete)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, [DisallowNull] Exception error, byte percentComplete) : base(activityInfo, ActivityState.Faulted, error, percentComplete)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, ActivityState statusValue) : base(operationInfo, statusValue)
            => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue) : base(activityInfo, statusValue)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="operationInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(operationInfo, statusValue, error)
            => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, [DisallowNull] Exception error) : base(operationInfo, ActivityState.Faulted, error)
            => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public TimedActivityProgressEvent([DisallowNull] ITimedActivityInfo<TState> activityInfo, [DisallowNull] Exception error) : base(activityInfo, ActivityState.Faulted, error)
            => AsyncState = activityInfo.AsyncState;
    }
}
