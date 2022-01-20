using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class TimedBackgroundProcessCompletedEventArgs : TimedBackgroundProcessStateEventArgs, ITimedBackgroundOperationCompletedEvent
    {
        public Exception Error { get; }

        public bool RanToCompletion { get; }

        public TimedBackgroundProcessCompletedEventArgs([DisallowNull] ITimedBackgroundOperation operation, MessageCode? messageCode,
            Exception exception, bool ranToCompletion, string statusDescription = null) : base(operation, messageCode, statusDescription)
        {
            Error = exception;
            RanToCompletion = ranToCompletion;
        }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class TimedBackgroundProcessCompletedEventArgs<TState> : TimedBackgroundProcessCompletedEventArgs, ITimedBackgroundOperationCompletedEvent<TState>
    {
        public TState AsyncState { get; }

        public TimedBackgroundProcessCompletedEventArgs([DisallowNull] ITimedBackgroundOperation<TState> operation, MessageCode? messageCode,
            Exception exception, bool ranToCompletion, string statusDescription = null) : base(operation, messageCode, exception, ranToCompletion, statusDescription)
        {
            AsyncState = operation.AsyncState;
        }
    }
}
