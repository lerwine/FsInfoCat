using FsInfoCat.AsyncOps;
using System;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public class TimedBgActivityCompletedEventArgs<TState> : BgActivityCompletedEventArgs<TState>, ITimedBgStatusEventArgs<TState>
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }

        public TimedBgActivityCompletedEventArgs(TState asyncState, ActivityCode activity, string statusMessage,
            DateTime started, TimeSpan duration)
            : base(asyncState, activity, statusMessage)
        {
            Started = started;
            Duration = duration;
        }
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public class TimedBgActivityCompletedEventArgs<TState, TResult> : BgActivityCompletedEventArgs<TState, TResult>, ITimedBgStatusEventArgs<TState>
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }

        public TimedBgActivityCompletedEventArgs(TState asyncState, ActivityCode activity, string statusMessage, TResult result, DateTime started, TimeSpan duration)
            : base(asyncState, activity, statusMessage, result)
        {
            Started = started;
            Duration = duration;
        }
    }
}
