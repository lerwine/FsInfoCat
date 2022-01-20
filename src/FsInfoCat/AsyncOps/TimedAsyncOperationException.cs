using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents an exception within the context of a timed asynchronous operation.
    /// </summary>
    /// <seealso cref="AsyncOperationException"/>
    /// <seealso cref="IBackgroundOperationErrorEvent"/>
    [Serializable]
    [Obsolete("Use FsInfoCat.Activities.ActivityException, instead.")]
    public class TimedAsyncOperationException : AsyncOperationException, ITimedBackgroundOperationErrorEvent
    {
        /// <summary>
        /// Gets the timestamp for the exception.
        /// </summary>
        /// <value>The duration of the asynchronous operation at the time the exception occurred.</value>
        public TimeSpan Duration { get; }

        public TimedAsyncOperationException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedAsyncOperationException"/> class.
        /// </summary>
        /// <param name="progressInfo">The progress information representing the current progress at the time the exception occurred.</param>
        /// <param name="code">The error code to associate with the exception.</param>
        /// <param name="statusMessage">The status message describing the exception.</param>
        /// <exception cref="ArgumentNullException"><paramref name="progressInfo"/> was null.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusMessage"/> was null or contained only whitespace.</exception>
        public TimedAsyncOperationException([DisallowNull] ITimedBackgroundProgressInfo progressInfo, ErrorCode code, [DisallowNull] string statusMessage) : base(progressInfo, code, statusMessage)
        {
            Duration = progressInfo.Duration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedAsyncOperationException"/> class.
        /// </summary>
        /// <param name="progressInfo">The progress information representing the current progress at the time the exception occurred.</param>
        /// <param name="code">The error code to associate with the exception.</param>
        /// <param name="statusMessage">The status message describing the exception.</param>
        /// <param name="inner">The actual exception that was thrown.</param>
        /// <exception cref="ArgumentNullException"><paramref name="progressInfo"/> was null.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusMessage"/> was null or contained only whitespace.</exception>
        public TimedAsyncOperationException([DisallowNull] ITimedBackgroundProgressInfo progressInfo, ErrorCode code, [DisallowNull] string statusMessage, Exception inner) : base(progressInfo, code, statusMessage, inner)
        {
            Duration = progressInfo.Duration;
        }

        protected TimedAsyncOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
#pragma warning disable CS8605 // Unboxing a possibly null value.
            Duration = (TimeSpan)info.GetValue(nameof(Duration), typeof(TimeSpan));
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Duration), Duration, typeof(TimeSpan?));
        }
    }
}
