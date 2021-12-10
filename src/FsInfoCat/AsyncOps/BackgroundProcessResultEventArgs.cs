using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProcessResultEventArgs<TResult> : BackgroundProcessCompletedEventArgs, IBackgroundOperationResultEvent<TResult>
    {
        public TResult Result { get; }

        public BackgroundProcessResultEventArgs([DisallowNull] IBackgroundFunc<TResult> operation, MessageCode? messageCode,
            Exception exception, TResult result, string statusDescription = null) : base(operation, messageCode, exception, true, statusDescription)
        {
            Result = result;
        }
    }

    public class BackgroundProcessResultEventArgs<TState, TResult> : BackgroundProcessCompletedEventArgs<TState>, IBackgroundOperationResultEvent<TState, TResult>
    {
        public TResult Result { get; }

        public BackgroundProcessResultEventArgs([DisallowNull] IBackgroundFunc<TState, TResult> operation, MessageCode? messageCode,
            Exception exception, TResult result, string statusDescription = null) : base(operation, messageCode, exception, true, statusDescription)
        {
            Result = result;
        }
    }
}
