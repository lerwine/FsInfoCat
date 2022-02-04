namespace FsInfoCat.Activities
{
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
}
