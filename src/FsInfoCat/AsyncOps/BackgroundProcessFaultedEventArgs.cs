using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProcessFaultedEventArgs : BackgroundProcessCompletedEventArgs, IBackgroundOperationFaultedEvent
    {
        MessageCode IBackgroundOperationErrorEvent.Code => Code ?? MessageCode.UnexpectedError;

        public BackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation operation, [DisallowNull] Exception exception, ErrorCode errorCode)
            : base(source, operation, errorCode.ToMessageCode(), exception ?? throw new ArgumentNullException(nameof(exception)), false) { }

        public BackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation operation, [DisallowNull] Exception exception)
            : base(source, operation, null, exception ?? throw new ArgumentNullException(nameof(exception)), false) { }
    }

    public class BackgroundProcessFaultedEventArgs<TState> : BackgroundProcessFaultedEventArgs, IBackgroundOperationFaultedEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation<TState> operation, [DisallowNull] Exception exception, ErrorCode errorCode)
            : base(source, operation, exception, errorCode) => AsyncState = operation.AsyncState;

        public BackgroundProcessFaultedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation<TState> operation, [DisallowNull] Exception exception)
            : base(source, operation, exception) => AsyncState = operation.AsyncState;
    }
}