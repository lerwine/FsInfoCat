using System;

namespace FsInfoCat.Activities
{
    public record TimedOperationEvent : OperationEvent, ITimedOperationEvent
    {
        public DateTime Started { get; init; }

        public TimeSpan Duration { get; init; }
    }

    public record TimedOperationEvent<TState> : TimedOperationEvent, ITimedOperationEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
}
