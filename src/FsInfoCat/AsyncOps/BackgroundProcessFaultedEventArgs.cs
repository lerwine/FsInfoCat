using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProcessFaultedEventArgs : BackgroundProcessCompletedEventArgs, IBackgroundOperationFaultedEvent
    {
        ErrorCode IBackgroundOperationErrorEvent.Code => Code.ToErrorCode() ?? ErrorCode.Unexpected;

        public BackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundOperation operation, [DisallowNull] Exception exception, ErrorCode errorCode, string statusDescription = null)
            : base(operation, errorCode.ToMessageCode(MessageCode.UnexpectedError), exception ?? throw new ArgumentNullException(nameof(exception)), false, statusDescription) { }

        public BackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundOperation operation, [DisallowNull] Exception exception, string statusDescription = null)
            : base(operation, null, exception ?? throw new ArgumentNullException(nameof(exception)), false, statusDescription) { }
    }

    public class BackgroundProcessFaultedEventArgs<TState> : BackgroundProcessFaultedEventArgs, IBackgroundOperationFaultedEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundOperation<TState> operation, [DisallowNull] Exception exception, ErrorCode errorCode, string statusDescription = null)
            : base(operation, exception, errorCode, statusDescription) => AsyncState = operation.AsyncState;

        public BackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundOperation<TState> operation, [DisallowNull] Exception exception, string statusDescription = null)
            : base(operation, exception, statusDescription) => AsyncState = operation.AsyncState;
    }
}
