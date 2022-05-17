using System;

namespace FsInfoCat.Activities
{
    // TODO: Document TimedOperationTerminatedEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public record TimedOperationTerminatedEvent : OperationTerminatedEvent, ITimedActivityCompletedEvent
    {
        public DateTime Started { get; init; }

        public TimeSpan Duration { get; init; }
    }

    public record TimedOperationTerminatedEvent<TState> : TimedOperationTerminatedEvent, ITimedActivityCompletedEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
