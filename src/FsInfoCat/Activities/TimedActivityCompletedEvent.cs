using System;

namespace FsInfoCat.Activities
{
    public record TimedActivityCompletedEvent : ActivityCompletedEvent, ITimedActivityCompletedEvent
    {
        public DateTime Started { get; init; }

        public TimeSpan Duration { get; init; }
    }

    public record TimedActivityCompletedEvent<TState> : TimedActivityCompletedEvent, ITimedActivityCompletedEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
}
