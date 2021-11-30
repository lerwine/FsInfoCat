using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class TimedBackgroundProcessFaultedEventArgs : TimedBackgroundProcessCompletedEventArgs, ITimedBackgroundOperationFaultedEvent
    {
        MessageCode IBackgroundOperationErrorEvent.Code => Code ?? MessageCode.UnexpectedError;

        public TimedBackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation operation, [DisallowNull] Exception exception, ErrorCode errorCode)
            : base(source, operation, errorCode.ToMessageCode(), exception ?? throw new ArgumentNullException(nameof(exception)), false) { }

        public TimedBackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation operation, [DisallowNull] Exception exception)
            : base(source, operation, null, exception ?? throw new ArgumentNullException(nameof(exception)), false) { }
    }

    public class TimedBackgroundProcessFaultedEventArgs<TState> : TimedBackgroundProcessFaultedEventArgs, ITimedBackgroundOperationFaultedEvent<TState>
    {
        public TState AsyncState { get; }

        public TimedBackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation<TState> operation, [DisallowNull] Exception exception, ErrorCode errorCode)
            : base(source, operation, exception, errorCode) => AsyncState = operation.AsyncState;

        public TimedBackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation<TState> operation, [DisallowNull] Exception exception)
            : base(source, operation, exception) => AsyncState = operation.AsyncState;
    }
}
