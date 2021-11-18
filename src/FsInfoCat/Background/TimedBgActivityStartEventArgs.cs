using System;

namespace FsInfoCat.Background
{
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
