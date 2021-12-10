using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class TimedBackgroundProcessFaultedEventArgs : TimedBackgroundProcessCompletedEventArgs, ITimedBackgroundOperationFaultedEvent
    {
        ErrorCode IBackgroundOperationErrorEvent.Code => Code.ToErrorCode() ?? ErrorCode.Unexpected;

        public TimedBackgroundProcessFaultedEventArgs([DisallowNull] ITimedBackgroundOperation operation, [DisallowNull] Exception exception, ErrorCode errorCode, string statusDescription = null)
            : base(operation, errorCode.ToMessageCode(MessageCode.UnexpectedError), exception ?? throw new ArgumentNullException(nameof(exception)), false, statusDescription) { }

        public TimedBackgroundProcessFaultedEventArgs([DisallowNull] ITimedBackgroundOperation operation, [DisallowNull] Exception exception, string statusDescription = null)
            : base(operation, null, exception ?? throw new ArgumentNullException(nameof(exception)), false, statusDescription) { }
    }

    public class TimedBackgroundProcessFaultedEventArgs<TState> : TimedBackgroundProcessFaultedEventArgs, ITimedBackgroundOperationFaultedEvent<TState>
    {
        public TState AsyncState { get; }

        public TimedBackgroundProcessFaultedEventArgs([DisallowNull] ITimedBackgroundOperation<TState> operation, [DisallowNull] Exception exception, ErrorCode errorCode, string statusDescription = null)
            : base(operation, exception, errorCode, statusDescription) => AsyncState = operation.AsyncState;

        public TimedBackgroundProcessFaultedEventArgs([DisallowNull] ITimedBackgroundOperation<TState> operation, [DisallowNull] Exception exception, string statusDescription = null)
            : base(operation, exception, statusDescription) => AsyncState = operation.AsyncState;
    }
}
