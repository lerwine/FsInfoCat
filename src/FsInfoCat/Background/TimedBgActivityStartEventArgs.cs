using FsInfoCat.AsyncOps;
using System;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class TimedBgActivityStartEventArgs<TState> : BgActivityStartEventArgs<TState>, ITimedBgStatusEventArgs<TState>
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }

        public TimedBgActivityStartEventArgs(TState asyncState, ActivityCode activity, string statusMessage, string currentOperation, DateTime started,
            TimeSpan duration) : base(asyncState, activity, statusMessage, currentOperation)
        {
            Started = started;
            Duration = duration;
        }
    }
}
