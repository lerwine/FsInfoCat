using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class TimedBackgroundProgressEventArgs : BackgroundProgressEventArgs, ITimedBackgroundProgressEvent
    {
        public TimeSpan Duration { get; }

        public TimedBackgroundProgressEventArgs([DisallowNull] ITimedBackgroundProgressInfo progress) : base(progress)
        {
            Duration = progress.Duration;
        }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class TimedBackgroundProgressEventArgs<TState> : TimedBackgroundProgressEventArgs, ITimedBackgroundProgressEvent<TState>
    {
        public TState AsyncState { get; }

        public TimedBackgroundProgressEventArgs([DisallowNull] ITimedBackgroundProgressInfo<TState> progress) : base(progress)
        {
            AsyncState = progress.AsyncState;
        }
    }
}
