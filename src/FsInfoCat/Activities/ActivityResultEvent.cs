using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an activity result event.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="ActivityProgressEvent" />
    /// <seealso cref="IActivityResultEvent{TResult}" />
    public class ActivityResultEvent<TResult> : ActivityProgressEvent, IActivityResultEvent<TResult>
    {
        /// <summary>
        /// Gets the result value.
        /// </summary>
        /// <value>The result value produced by the activity or the <c>default</c> value if <see cref="ActivityEvent.StatusValue">StatusValue</see> is not <see cref="ActivityState.RanToCompletion"/>.</value>
        public TResult Result { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TResult}"/> to represent a canceled activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo operationInfo, byte percentComplete) : base(operationInfo, ActivityState.Canceled, (byte?)percentComplete) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TResult}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo operationInfo, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, error, percentComplete) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TResult}"/> to represent a canceled activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo operationInfo) : base(operationInfo, ActivityState.Canceled) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TResult}"/> to represent a successful activity completion event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="result">The result value produced by the activity.</param>
        /// <param name="indicateCompletionPercentage"><see langword="true"/> to specify 100% completion value; otherwisse, <see langword="false"/> to set <see cref="ActivityProgressEvent.PercentComplete"/> to <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo operationInfo, TResult result, bool indicateCompletionPercentage) : base(operationInfo, ActivityState.RanToCompletion, indicateCompletionPercentage ? 100 : null) => Result = result;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TResult}"/> to represent a successful activity completion event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="result">The result value produced by the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo operationInfo, TResult result) : base(operationInfo, ActivityState.RanToCompletion) => Result = result;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TResult}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo operationInfo, [DisallowNull] Exception error) : base(operationInfo, error) { }
    }

    /// <summary>
    /// Represents a result event for an activity that has an associated user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="ActivityResultEvent{TResult}" />
    /// <seealso cref="IActivityResultEvent{TState, TResult}" />
    public class ActivityResultEvent<TState, TResult> : ActivityResultEvent<TResult>, IActivityResultEvent<TState, TResult>
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TState, TResult}"/> to represent a canceled activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo<TState> operationInfo, byte percentComplete) : base(operationInfo, percentComplete) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TState, TResult}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo<TState> operationInfo, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, error, percentComplete) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TState, TResult}"/> to represent a canceled activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo<TState> operationInfo) : base(operationInfo) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TState, TResult}"/> to represent a successful activity completion event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="result">The result value produced by the activity.</param>
        /// <param name="indicateCompletionPercentage"><see langword="true"/> to specify 100% completion value; otherwisse, <see langword="false"/> to set <see cref="ActivityProgressEvent.PercentComplete"/> to <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo<TState> operationInfo, TResult result, bool indicateCompletionPercentage) : base(operationInfo, result, indicateCompletionPercentage) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TState, TResult}"/> to represent a successful activity completion event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="result">The result value produced by the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo<TState> operationInfo, TResult result) : base(operationInfo, result) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResultEvent{TState, TResult}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public ActivityResultEvent([DisallowNull] IOperationInfo<TState> operationInfo, [DisallowNull] Exception error) : base(operationInfo, error) => AsyncState = operationInfo.AsyncState;
    }
}
