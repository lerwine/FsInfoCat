namespace FsInfoCat.Activities
{
    // TODO: Document OperationEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public record OperationEvent : ActivityEvent, IOperationEvent
    {
        public ActivityStatus StatusValue { get; init; }

        public string CurrentOperation { get; init; }

        public int PercentComplete { get; init; }
    }

    public record OperationEvent<TState> : OperationEvent, IOperationEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
