using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class TimedBackgroundProgressErrorEventArgs : BackgroundProgressErrorEventArgs, ITimedBackgroundOperationErrorEvent
    {
        public TimeSpan Duration { get; }

        public TimedBackgroundProgressErrorEventArgs([DisallowNull] ITimedBackgroundProgressInfo progress, [DisallowNull] Exception error, ErrorCode code)
            : base(progress, error, code)
        {
            Duration = progress.Duration;
        }

        public TimedBackgroundProgressErrorEventArgs([DisallowNull] ITimedBackgroundProgressInfo progress, [DisallowNull] Exception error)
            : this(progress, error, error is AsyncOperationException failureException ? failureException.Code : ErrorCode.Unexpected) { }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class TimedBackgroundProgressErrorEventArgs<TState> : TimedBackgroundProgressErrorEventArgs, ITimedBackgroundOperationErrorEvent<TState>
    {
        public TState AsyncState { get; }

        public TimedBackgroundProgressErrorEventArgs([DisallowNull] ITimedBackgroundProgressInfo<TState> progress, [DisallowNull] Exception error, ErrorCode code) : base(progress, error, code) => AsyncState = progress.AsyncState;

        public TimedBackgroundProgressErrorEventArgs([DisallowNull] ITimedBackgroundProgressInfo<TState> progress, [DisallowNull] Exception error) : base(progress, error) => AsyncState = progress.AsyncState;
    }
}
