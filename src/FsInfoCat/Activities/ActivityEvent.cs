using System;

namespace FsInfoCat.Activities
{
    //TODO: Document ActivityEvent classes
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public record ActivityEvent : IActivityEvent
    {
        public Exception Exception { get; init; }

        public Guid ActivityId { get; init; }

        public Guid? ParentActivityId { get; init; }

        public string ShortDescription { get; init; }

        public string StatusMessage { get; init; }

        public Model.StatusMessageLevel MessageLevel { get; init; }
    }

    public record ActivityEvent<TState> : ActivityEvent, IActivityEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
