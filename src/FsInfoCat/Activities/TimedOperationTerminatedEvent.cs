using System;

namespace FsInfoCat.Activities
{
    public record TimedOperationTerminatedEvent : OperationTerminatedEvent, ITimedActivityCompletedEvent
    {
        public DateTime Started { get; init; }

        public TimeSpan Duration { get; init; }
    }

    public record TimedOperationTerminatedEvent<TState> : TimedOperationTerminatedEvent, ITimedActivityCompletedEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
}
