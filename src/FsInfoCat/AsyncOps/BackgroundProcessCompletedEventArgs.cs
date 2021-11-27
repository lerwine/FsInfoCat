using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProcessCompletedEventArgs : BackgroundProcessStateEventArgs, IBackgroundOperationCompletedEvent
    {
        public Exception Error { get; }

        public bool RanToCompletion { get; }

        public BackgroundProcessCompletedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation operation, MessageCode? messageCode, Exception exception, bool ranToCompletion)
            : base(source, operation, messageCode)
        {
            Error=exception;
            RanToCompletion=ranToCompletion;
        }
    }

    public sealed class BackgroundProcessCompletedEventArgs<TState> : BackgroundProcessCompletedEventArgs, IBackgroundOperationCompletedEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProcessCompletedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation<TState> operation, MessageCode? messageCode, Exception exception, bool ranToCompletion)
            : base(source, operation, messageCode, exception, ranToCompletion)
        {
            AsyncState=operation.AsyncState;
        }
    }
}
