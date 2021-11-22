using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Services
{
    public class BackgroundProcessCompletedEventArgs : BackgroundProcessStateEventArgs, IBackgroundOperationCompletedEvent
    {
        public Exception Error { get; }

        public BackgroundProcessCompletedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation operation, MessageCode? messageCode, Exception exception)
            : base(source, operation, messageCode)
        {
            Error = exception;
        }
    }

    public sealed class BackgroundProcessCompletedEventArgs<TState> : BackgroundProcessCompletedEventArgs, IBackgroundOperationCompletedEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProcessCompletedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation<TState> operation, MessageCode? messageCode, Exception exception)
            : base(source, operation, messageCode, exception)
        {
            AsyncState = operation.AsyncState;
        }
    }
}
