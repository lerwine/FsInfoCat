using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class TimedBackgroundProcessResultEventArgs<TResult> : TimedBackgroundProcessCompletedEventArgs, ITimedBackgroundOperationResultEvent<TResult>
    {
        public TResult Result { get; }

        public TimedBackgroundProcessResultEventArgs([DisallowNull] ITimedBackgroundFunc<TResult> operation, MessageCode? messageCode,
            Exception exception, TResult result, string statusDescription = null) : base(operation, messageCode, exception, true, statusDescription)
        {
            Result = result;
        }
    }

    public class TimedBackgroundProcessResultEventArgs<TState, TResult> : TimedBackgroundProcessCompletedEventArgs<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>
    {
        public TResult Result { get; }

        public TimedBackgroundProcessResultEventArgs([DisallowNull] ITimedBackgroundFunc<TState, TResult> operation, MessageCode? messageCode,
            Exception exception, TResult result, string statusDescription = null) : base(operation, messageCode, exception, true, statusDescription)
        {
            Result = result;
        }
    }
}
