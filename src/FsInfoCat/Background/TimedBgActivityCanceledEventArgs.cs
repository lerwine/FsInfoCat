using FsInfoCat.AsyncOps;
using System;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public class TimedBgActivityCanceledEventArgs<TState> : BgActivityCanceledEventArgs<TState>, ITimedBgStatusEventArgs<TState>
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }

        public TimedBgActivityCanceledEventArgs(TState asyncState, ActivityCode activity, string statusMessage, string currentOperation, DateTime started, TimeSpan duration)
            : base(asyncState, activity, statusMessage, currentOperation)
        {
            Started = started;
            Duration = duration;
        }
    }
}
