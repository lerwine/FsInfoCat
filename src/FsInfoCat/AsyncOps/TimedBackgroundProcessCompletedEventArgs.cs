using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class TimedBackgroundProcessCompletedEventArgs : TimedBackgroundProcessStateEventArgs, ITimedBackgroundOperationCompletedEvent
    {
        public Exception Error { get; }

        public bool RanToCompletion { get; }

        public TimedBackgroundProcessCompletedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation operation, MessageCode? messageCode,
            Exception exception, bool ranToCompletion) : base(source, operation, messageCode)
        {
            Error = exception;
            RanToCompletion = ranToCompletion;
        }
    }

    public class TimedBackgroundProcessCompletedEventArgs<TState> : TimedBackgroundProcessCompletedEventArgs, ITimedBackgroundOperationCompletedEvent<TState>
    {
        public TState AsyncState { get; }

        public TimedBackgroundProcessCompletedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation<TState> operation, MessageCode? messageCode,
            Exception exception, bool ranToCompletion) : base(source, operation, messageCode, exception, ranToCompletion)
        {
            AsyncState = operation.AsyncState;
        }
    }
}
