using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an activity operation event.
    /// </summary>
    /// <seealso cref="ActivityEvent" />
    /// <seealso cref="IOperationEvent" />
    public class OperationEvent : ActivityEvent, IOperationEvent
    {
        /// <summary>
        /// Gets the current operation descriptoin.
        /// </summary>
        /// <value>The description of the current operation for the activity.</value>
        public string CurrentOperation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public OperationEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, string currentOperation) : base(activityInfo, statusValue) => CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public OperationEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, statusValue, error) => CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public OperationEvent([DisallowNull] IOperationInfo operationInfo, ActivityState statusValue) : base(operationInfo, statusValue) => CurrentOperation = operationInfo.CurrentOperation;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public OperationEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) => CurrentOperation = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        public OperationEvent([DisallowNull] IOperationInfo operationInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(operationInfo, statusValue, error) => CurrentOperation = operationInfo.CurrentOperation;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        public OperationEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error) => CurrentOperation = "";
    }

    /// <summary>
    /// Represents an operation event for an activity that has an associated user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
    /// <seealso cref="OperationEvent" />
    /// <seealso cref="IOperationEvent{TState}" />
    public class OperationEvent<TState> : OperationEvent, IOperationEvent<TState>
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public OperationEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, string currentOperation) : base(activityInfo, statusValue, currentOperation) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        /// <param name="currentOperation">The description of the current operation for the activity.</param>
        public OperationEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error, string currentOperation) : base(activityInfo, statusValue, error, currentOperation) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public OperationEvent([DisallowNull] IOperationInfo<TState> operationInfo, ActivityState statusValue) : base(operationInfo, statusValue) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public OperationEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        public OperationEvent([DisallowNull] IOperationInfo<TState> operationInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(operationInfo, statusValue, error) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        public OperationEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error) => AsyncState = activityInfo.AsyncState;
    }
}
