using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an exception that is specific to an operation within an activity.
    /// </summary>
    /// <seealso cref="Exception" />
    /// <seealso cref="IActivityEvent" />
    [Serializable]
    // TODO: Finish documentation for ActivityException
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ActivityException : Exception, IActivityEvent
    {
        /// <summary>
        /// Gets the error code associated with the exception.
        /// </summary>
        /// <value>The code.</value>
        public ErrorCode Code { get; }

        /// <summary>
        /// Gets the operation that was being conducted when the exception occurred.
        /// </summary>
        /// <value>The <see cref="IOperationInfo"/> that describes the operation that was being conducted when the exception occurred.</value>
        public IOperationInfo Operation { get; }

        Exception IActivityEvent.Exception => InnerException ?? this;

        Guid IActivityInfo.ActivityId => Operation?.ActivityId ?? Guid.Empty;

        Guid? IActivityInfo.ParentActivityId => Operation?.ParentActivityId;

        string IActivityInfo.ShortDescription => Operation?.ShortDescription;

        string IActivityInfo.StatusMessage => Operation?.StatusMessage;

        StatusMessageLevel IActivityEvent.MessageLevel => StatusMessageLevel.Error;

        public override string ToString()
        {
            if (Operation is null)
            {
                if (string.IsNullOrWhiteSpace(Message))
                    return "";
                return $"{Message}\nCode: {Code}";
            }
            if (string.IsNullOrWhiteSpace(Message))
            {
                if (string.IsNullOrWhiteSpace(Operation.CurrentOperation))
                    return $"Unexpected error (Code={Code}).\nActivity: {Operation.ShortDescription};\nStatus: {Operation.StatusMessage}";
                return $"Unexpected error (Code={Code}).\nActivity: {Operation.ShortDescription};\nOperation: {Operation.CurrentOperation};\nStatus: {Operation.StatusMessage}";
            }
            if (string.IsNullOrWhiteSpace(Operation.CurrentOperation))
                return $"{Message}\nActivity: {Operation.ShortDescription};\nStatus: {Operation.StatusMessage} (Code={Code})";
            return $"{Message}\nActivity: {Operation.ShortDescription};\nOperation: {Operation.CurrentOperation};\nStatus: {Operation.StatusMessage} (Code={Code})";
        }

        public ActivityException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public ActivityException([DisallowNull] IOperationInfo operation, string message) : this(operation, message, ErrorCode.Unexpected) => Operation = operation ?? throw new ArgumentNullException(nameof(operation));

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public ActivityException([DisallowNull] IOperationInfo operation, string message, Exception inner) : this(operation, message, ErrorCode.Unexpected, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="code">The error code.</param>
        /// <exception cref="ArgumentNullException">operation</exception>
        public ActivityException([DisallowNull] IOperationInfo operation, string message, ErrorCode code) : base(message) => (Operation, Code) = (operation ?? throw new ArgumentNullException(nameof(operation)), code);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="code">The error code.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public ActivityException([DisallowNull] IOperationInfo operation, string message, ErrorCode code, Exception inner) : base(message, inner) => (Operation, Code) = (operation ?? throw new ArgumentNullException(nameof(operation)), code);

        protected ActivityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Represents an exception that is specific to an operation within an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="ActivityException" />
    /// <seealso cref="IActivityEvent{TState}" />
    [Serializable]
    public class ActivityException<TState> : ActivityException, IActivityEvent<TState>
    {
        /// <summary>
        /// Gets the operation that was being conducted when the exception occurred.
        /// </summary>
        /// <value>The <see cref="IOperationInfo"/> that describes the operation that was being conducted when the exception occurred.</value>
        public new IOperationInfo<TState> Operation => (IOperationInfo<TState>)base.Operation;

        TState IActivityInfo<TState>.AsyncState => (Operation is null) ? default : Operation.AsyncState;

        public ActivityException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException{TState}"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public ActivityException([DisallowNull] IOperationInfo<TState> operation, string message) : base(operation, message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException{TState}"/> class.
        /// </summary>
        /// <param name="operation">The  operation that was being conducted when the exception occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
        public ActivityException([DisallowNull] IOperationInfo<TState> operation, string message, Exception inner) : base(operation, message, inner) { }

        protected ActivityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
