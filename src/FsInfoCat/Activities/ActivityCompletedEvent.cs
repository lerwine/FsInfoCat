namespace FsInfoCat.Activities
{
    // TODO: Document ActivityCompletedEvent classes
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public record ActivityCompletedEvent : ActivityEvent, IActivityCompletedEvent
    {
        public ActivityStatus StatusValue { get; init; }
    }

    public record ActivityCompletedEvent<TState> : ActivityCompletedEvent, IActivityCompletedEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
