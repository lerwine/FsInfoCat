using System;

namespace FsInfoCat.Services
{
    public class BackgroundProcessFaultedEventArgs : BackgroundProcessCompletedEventArgs, IBackgroundOperationFaultedEvent
    {
        MessageCode IBackgroundOperationErrorEvent.Code => Code ?? MessageCode.UnexpectedError;

        public BackgroundProcessFaultedEventArgs(IBackgroundOperation operation, Exception exception, ErrorCode errorCode)
            : base(operation, errorCode.ToMessageCode(), exception) { }

        public BackgroundProcessFaultedEventArgs(IBackgroundOperation operation, Exception exception)
            : base(operation, null, exception) { }
    }

    public class BackgroundProcessFaultedEventArgs<TState> : BackgroundProcessFaultedEventArgs, IBackgroundOperationFaultedEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProcessFaultedEventArgs(IBackgroundOperation<TState> operation, Exception exception, ErrorCode errorCode)
            : base(operation, exception, errorCode) => AsyncState = operation.AsyncState;

        public BackgroundProcessFaultedEventArgs(IBackgroundOperation<TState> operation, Exception exception)
            : base(operation, exception) => AsyncState = operation.AsyncState;
    }
}
