namespace FsInfoCat.Activities
{
    public record ActivityCompletedEvent : ActivityEvent, IActivityCompletedEvent
    {
        public ActivityStatus StatusValue { get; init; }
    }

    public record ActivityCompletedEvent<TState> : ActivityCompletedEvent, IActivityCompletedEvent<TState>
    {
        public TState AsyncState { get; init; }
    }
}
