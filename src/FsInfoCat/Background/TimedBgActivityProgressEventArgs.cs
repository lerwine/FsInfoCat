using System;

namespace FsInfoCat.Background
{
    public class TimedBgActivityProgressEventArgs<TState> : BgActivityProgressEventArgs<TState>, ITimedBgStatusEventArgs<TState>
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }

        public TimedBgActivityProgressEventArgs(TState asyncState, ActivityCode activity, string statusMessage, string currentOperation, Exception exception,
            DateTime started, TimeSpan duration) : base(asyncState, activity, statusMessage, currentOperation, exception)
        {
            Started = started;
            Duration = duration;
        }
    }
}
