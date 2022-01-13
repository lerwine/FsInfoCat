using System;

namespace FsInfoCat.Activities
{
    public record OperationEvent : IOperationEvent
    {
        public Guid ActivityId { get; init; }

        public Guid? ParentActivityId { get; init; }

        public string ShortDescription { get; init; }

        public ActivityStatus StatusValue { get; init; }

        public string StatusMessage { get; init; }

        public string CurrentOperation { get; init; }

        public int PercentComplete { get; init; }

        public Exception Exception { get; init; }
    }

    public record OperationEvent<TState> : OperationEvent, IOperationEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
}
