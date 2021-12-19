using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed activity result event.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="ActivityResultEvent{TResult}" />
    /// <seealso cref="ITimedActivityResultEvent{TResult}" />
    public class TimedActivityResultEvent<TResult> : ActivityResultEvent<TResult>, ITimedActivityResultEvent<TResult>
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
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TResult}"/> to represent a canceled activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo operationInfo, byte percentComplete) : base(operationInfo, percentComplete) => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TResult}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> or <paramref name="error"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo operationInfo, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, error, percentComplete)
            => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TResult}"/> to represent a canceled activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo operationInfo) : base(operationInfo) => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TResult}"/> to represent a successful activity completion event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="result">The result value produced by the activity.</param>
        /// <param name="indicateCompletionPercentage"><see langword="true"/> to specify 100% completion value; otherwisse, <see langword="false"/> to set <see cref="ActivityProgressEvent.PercentComplete"/> to <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo operationInfo, TResult result, bool indicateCompletionPercentage) : base(operationInfo, result, indicateCompletionPercentage)
            => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TResult}"/> to represent a successful activity completion event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="result">The result value produced by the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo operationInfo, TResult result) : base(operationInfo, result) => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TResult}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> or <paramref name="error"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo operationInfo, [DisallowNull] Exception error) : base(operationInfo, error) => (Started, Duration) = (operationInfo.Started, operationInfo.Duration);
    }

    /// <summary>
    /// Represents a result event for a timed activity that has an associated user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="TimedActivityResultEvent{TResult}" />
    /// <seealso cref="ITimedActivityResultEvent{TState, TResult}" />
    public class TimedActivityResultEvent<TState, TResult> : TimedActivityResultEvent<TResult>, ITimedActivityResultEvent<TState, TResult>
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TState, TResult}"/> to represent a canceled activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, byte percentComplete) : base(operationInfo, percentComplete) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TState, TResult}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <param name="percentComplete">The completion percentage value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> or <paramref name="error"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, [DisallowNull] Exception error, byte percentComplete) : base(operationInfo, error, percentComplete)
            => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TState, TResult}"/> to represent a canceled activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo) : base(operationInfo) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TState, TResult}"/> to represent a successful activity completion event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="result">The result value produced by the activity.</param>
        /// <param name="indicateCompletionPercentage"><see langword="true"/> to specify 100% completion value; otherwisse, <see langword="false"/> to set <see cref="ActivityProgressEvent.PercentComplete"/> to <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, TResult result, bool indicateCompletionPercentage) : base(operationInfo, result, indicateCompletionPercentage) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TState, TResult}"/> to represent a successful activity completion event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="result">The result value produced by the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, TResult result) : base(operationInfo, result) => AsyncState = operationInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityResultEvent{TState, TResult}"/> to represent a faulted activity event.
        /// </summary>
        /// <param name="operationInfo">The source operation information.</param>
        /// <param name="error">The unhandled exception that terminated the event.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operationInfo"/> or <paramref name="error"/> is <see langword="null"/>.</exception>
        public TimedActivityResultEvent([DisallowNull] ITimedOperationInfo<TState> operationInfo, [DisallowNull] Exception error) : base(operationInfo, error) => AsyncState = operationInfo.AsyncState;
    }
}
