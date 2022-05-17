using System;

namespace FsInfoCat.Activities
{
    // TODO: Document TimedActivityCompletedEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public record TimedActivityCompletedEvent : ActivityCompletedEvent, ITimedActivityCompletedEvent
    {
        public DateTime Started { get; init; }

        public TimeSpan Duration { get; init; }
    }

    public record TimedActivityCompletedEvent<TState> : TimedActivityCompletedEvent, ITimedActivityCompletedEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
