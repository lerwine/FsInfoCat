using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class TimedBackgroundProcessResultEventArgs<TResult> : TimedBackgroundProcessCompletedEventArgs, ITimedBackgroundOperationResultEvent<TResult>
    {
        public TResult Result { get; }

        public TimedBackgroundProcessResultEventArgs([DisallowNull] ITimedBackgroundFunc<TResult> operation, MessageCode? messageCode,
            Exception exception, TResult result, string statusDescription = null) : base(operation, messageCode, exception, true, statusDescription)
        {
            Result = result;
        }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
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
