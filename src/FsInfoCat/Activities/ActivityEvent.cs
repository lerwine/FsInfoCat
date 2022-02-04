using System;

namespace FsInfoCat.Activities
{
    public record ActivityEvent : IActivityEvent
    {
        public Exception Exception { get; init; }

        public Guid ActivityId { get; init; }

        public Guid? ParentActivityId { get; init; }

        public string ShortDescription { get; init; }

        public string StatusMessage { get; init; }

        public StatusMessageLevel MessageLevel { get; init; }
    }

    public record ActivityEvent<TState> : ActivityEvent, IActivityEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
}
