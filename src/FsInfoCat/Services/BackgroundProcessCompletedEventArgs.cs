using System;

namespace FsInfoCat.Services
{
    public class BackgroundProcessCompletedEventArgs : BackgroundProcessStateEventArgs, IBackgroundOperationCompletedEvent
    {
        public Exception Error { get; }

        public BackgroundProcessCompletedEventArgs(IBackgroundOperation operation, MessageCode? messageCode, Exception exception)
            : base(operation, messageCode)
        {
            Error = exception;
        }
    }

    public sealed class BackgroundProcessCompletedEventArgs<TState> : BackgroundProcessCompletedEventArgs, IBackgroundOperationCompletedEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProcessCompletedEventArgs(IBackgroundOperation<TState> operation, MessageCode? messageCode, Exception exception)
            : base(operation, messageCode, exception)
        {
            AsyncState = operation.AsyncState;
        }
    }
}
