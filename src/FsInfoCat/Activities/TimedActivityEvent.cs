using System;

namespace FsInfoCat.Activities
{
    // TODO: Document TimedActivityEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public record TimedActivityEvent : ActivityEvent, ITimedActivityEvent
    {
        public DateTime Started { get; init; }

        public TimeSpan Duration { get; init; }
    }

    public record TimedActivityEvent<TState> : TimedActivityEvent, ITimedActivityEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
