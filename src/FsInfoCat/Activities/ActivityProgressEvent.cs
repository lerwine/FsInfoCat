using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an activity progress event.
    /// </summary>
    /// <seealso cref="OperationEvent" />
    /// <seealso cref="IActivityProgressEvent" />
    public class ActivityProgressEvent : OperationEvent, IActivityProgressEvent
    {
        /// <summary>
        /// Gets the percent complete.
        /// </summary>
        /// <value>The percent complete.</value>
        public byte? PercentComplete { get; }

        private static byte AssertValidPercentComplete(byte percentComplete)
        {
            if (percentComplete > 100)
                throw new ArgumentOutOfRangeException(nameof(percentComplete));
            return percentComplete;
        }

        private static byte AssertValidPercentComplete(byte percentComplete, ActivityState statusValue)
        {
            if (percentComplete > 100)
                throw new ArgumentOutOfRangeException(nameof(percentComplete));
            if (statusValue == ActivityState.RanToCompletion && percentComplete < 100)
                throw new ArgumentException($"{nameof(percentComplete)} must be 100 when {nameof(statusValue)} is {ActivityState.RanToCompletion}", nameof(percentComplete));
            return percentComplete;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, string currentOperation, byte percentComplete) : base(activityInfo, statusValue, currentOperation)
            => PercentComplete = AssertValidPercentComplete(percentComplete, statusValue);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation, byte percentComplete) : base(activityInfo, statusValue, error, currentOperation)
            => PercentComplete = AssertValidPercentComplete(percentComplete);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, [DisallowNull] Exception error, string currentOperation, byte percentComplete) : base(activityInfo, ActivityState.Faulted, error, currentOperation)
            => PercentComplete = AssertValidPercentComplete(percentComplete);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, string currentOperation) : base(activityInfo, statusValue, currentOperation) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, statusValue, error, currentOperation) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, ActivityState.Faulted, error, currentOperation) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IOperationInfo operationInfo, ActivityState statusValue, byte? percentComplete) : base(operationInfo, statusValue)
        {
            if (percentComplete.HasValue)
                PercentComplete = AssertValidPercentComplete(percentComplete.Value, statusValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, byte percentComplete) : base(activityInfo, statusValue)
            => PercentComplete = AssertValidPercentComplete(percentComplete, statusValue);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IOperationInfo operationInfo, ActivityState statusValue, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, statusValue, error)
            => PercentComplete = AssertValidPercentComplete(percentComplete);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IOperationInfo operationInfo, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, ActivityState.Faulted, error)
            => PercentComplete = AssertValidPercentComplete(percentComplete);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error, byte percentComplete) : base(activityInfo, statusValue, error)
            => PercentComplete = AssertValidPercentComplete(percentComplete);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, [DisallowNull] Exception error, byte percentComplete) : base(activityInfo, ActivityState.Faulted, error)
            => PercentComplete = AssertValidPercentComplete(percentComplete);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IOperationInfo operationInfo, ActivityState statusValue) : base(operationInfo, statusValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public ActivityProgressEvent([DisallowNull] IOperationInfo operationInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(operationInfo, statusValue, error) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public ActivityProgressEvent([DisallowNull] IOperationInfo operationInfo, [DisallowNull] Exception error) : base(operationInfo, ActivityState.Faulted, error) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo activityInfo, [DisallowNull] Exception error) : base(activityInfo, ActivityState.Faulted, error) { }
    }

    /// <summary>
    /// Represents a progress event for an activity that has an associated user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
    /// <seealso cref="ActivityProgressEvent" />
    /// <seealso cref="IActivityProgressEvent{TState}" />
    public class ActivityProgressEvent<TState> : ActivityProgressEvent, IActivityProgressEvent<TState>
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, string currentOperation, byte percentComplete) : base(activityInfo, statusValue, currentOperation, percentComplete)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation, byte percentComplete)
            : base(activityInfo, statusValue, error, currentOperation, percentComplete) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, [DisallowNull] Exception error, string currentOperation, byte percentComplete) : base(activityInfo, ActivityState.Faulted, error, currentOperation, percentComplete)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, string currentOperation) : base(activityInfo, statusValue, currentOperation)
            => AsyncState = activityInfo.AsyncState;

        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, statusValue, error, currentOperation)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, ActivityState.Faulted, error, currentOperation)
            => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IOperationInfo<TState> operationInfo, ActivityState statusValue, byte percentComplete) : base(operationInfo, statusValue, (byte?)percentComplete)
            => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, byte percentComplete) : base(activityInfo, statusValue, percentComplete) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="operationInfo">The activity operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IOperationInfo<TState> operationInfo, ActivityState statusValue, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, statusValue, error, percentComplete) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IOperationInfo<TState> operationInfo, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, ActivityState.Faulted, error, percentComplete) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error, byte percentComplete) : base(activityInfo, statusValue, error, percentComplete) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, [DisallowNull] Exception error, byte percentComplete) : base(activityInfo, ActivityState.Faulted, error, percentComplete) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IOperationInfo<TState> operationInfo, ActivityState statusValue) : base(operationInfo, statusValue) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event (not faulted).
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public ActivityProgressEvent([DisallowNull] IOperationInfo<TState> operationInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(operationInfo, statusValue, error) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public ActivityProgressEvent([DisallowNull] IOperationInfo<TState> operationInfo, [DisallowNull] Exception error) : base(operationInfo, ActivityState.Faulted, error) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent an activity event with an associated exception.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityProgressEvent{TState}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        public ActivityProgressEvent([DisallowNull] IActivityInfo<TState> activityInfo, [DisallowNull] Exception error) : base(activityInfo, ActivityState.Faulted, error) => AsyncState = activityInfo.AsyncState;
    }
}
