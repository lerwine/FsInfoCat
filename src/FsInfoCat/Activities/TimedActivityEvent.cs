using System;

namespace FsInfoCat.Activities
{
    public record TimedActivityEvent : ActivityEvent, ITimedActivityEvent
    {
        public DateTime Started { get; init; }

        public TimeSpan Duration { get; init; }
    }

    public record TimedActivityEvent<TState> : TimedActivityEvent, ITimedActivityEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
}
