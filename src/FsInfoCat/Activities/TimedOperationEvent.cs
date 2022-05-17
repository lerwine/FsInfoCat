using System;

namespace FsInfoCat.Activities
{
    // TODO: Document TimedOperationEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public record TimedOperationEvent : OperationEvent, ITimedOperationEvent
    {
        public DateTime Started { get; init; }

        public TimeSpan Duration { get; init; }
    }

    public record TimedOperationEvent<TState> : TimedOperationEvent, ITimedOperationEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
