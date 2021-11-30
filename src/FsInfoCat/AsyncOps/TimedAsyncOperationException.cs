using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FsInfoCat.AsyncOps
{
    [Serializable]
    public class TimedAsyncOperationException : AsyncOperationException, ITimedBackgroundOperationErrorEvent
    {
        public TimeSpan Duration { get; }

        public TimedAsyncOperationException() { }

        public TimedAsyncOperationException([DisallowNull] ITimedBackgroundProgressInfo progressInfo, ErrorCode code, [DisallowNull] string statusMessage) : base(progressInfo, code, statusMessage)
        {
            Duration = progressInfo.Duration;
        }

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
