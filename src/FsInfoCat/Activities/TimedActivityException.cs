using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an exception that is specific to a timed operation within an activity.
    /// </summary>
    /// <seealso cref="Exception" />
    /// <seealso cref="IFaultedActivityInfo" />
    [Serializable]
    public class TimedActivityException : ActivityException, ITimedActivityEvent
    {
        /// <summary>
        /// Gets the operation that was being conducted when the exception occurred.
        /// </summary>
        /// <value>The <see cref="IOperationInfo"/> that describes the operation that was being conducted when the exception occurred.</value>
        public new ITimedOperationInfo Operation => (ITimedOperationInfo)base.Operation;

        TimeSpan ITimedActivityInfo.Duration => Operation?.Duration ?? TimeSpan.Zero;

        DateTime ITimedActivityInfo.Started => Operation?.Started ?? DateTime.MaxValue;

        public TimedActivityException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityException"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public TimedActivityException([DisallowNull] ITimedOperationInfo operation, string message) : base(operation, message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityException"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public TimedActivityException([DisallowNull] ITimedOperationInfo operation, string message, Exception inner) : base(operation, message, inner) { }

        protected TimedActivityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Represents an exception that is specific to a timed operation within an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="ActivityException" />
    /// <seealso cref="IFaultedActivityInfo{TState}" />
    [Serializable]
    public class TimedActivityException<TState> : TimedActivityException, ITimedActivityEvent<TState>
    {
        /// <summary>
        /// Gets the operation that was being conducted when the exception occurred.
        /// </summary>
        /// <value>The <see cref="IOperationInfo"/> that describes the operation that was being conducted when the exception occurred.</value>
        public new ITimedOperationInfo<TState> Operation => (ITimedOperationInfo<TState>)base.Operation;

        TState IActivityInfo<TState>.AsyncState => (Operation is null) ? default : Operation.AsyncState;

        public TimedActivityException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityException{TState}"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public TimedActivityException([DisallowNull] ITimedOperationInfo<TState> operation, string message) : base(operation, message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedActivityException{TState}"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public TimedActivityException([DisallowNull] ITimedOperationInfo<TState> operation, string message, Exception inner) : base(operation, message, inner) { }

        protected TimedActivityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
