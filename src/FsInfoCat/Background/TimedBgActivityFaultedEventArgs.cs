using FsInfoCat.AsyncOps;
using System;

namespace FsInfoCat.Background
{
    public class TimedBgActivityFaultedEventArgs<TState> : BgActivityFaultedEventArgs<TState>
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }

        public TimedBgActivityFaultedEventArgs(TState asyncState, ActivityCode activity, string statusMessage, string currentOperation, Exception exception,
            DateTime started, TimeSpan duration) : base(asyncState, activity, statusMessage, currentOperation, exception)
        {
            Started = started;
            Duration = duration;
        }
    }
}
