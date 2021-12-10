using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProcessCompletedEventArgs : BackgroundProcessStateEventArgs, IBackgroundOperationCompletedEvent
    {
        public Exception Error { get; }

        public bool RanToCompletion { get; }

        public BackgroundProcessCompletedEventArgs([DisallowNull] IBackgroundOperation operation, MessageCode? messageCode,
            Exception exception, bool ranToCompletion, string statusDescription = null) : base(operation, messageCode, statusDescription)
        {
            Error = exception;
            RanToCompletion = ranToCompletion;
        }
    }

    public class BackgroundProcessCompletedEventArgs<TState> : BackgroundProcessCompletedEventArgs, IBackgroundOperationCompletedEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProcessCompletedEventArgs([DisallowNull] IBackgroundOperation<TState> operation, MessageCode? messageCode,
            Exception exception, bool ranToCompletion, string statusDescription = null) : base(operation, messageCode, exception, ranToCompletion, statusDescription)
        {
            AsyncState = operation.AsyncState;
        }
    }
}
