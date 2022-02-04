namespace FsInfoCat.Activities
{
    public record OperationTerminatedEvent : OperationEvent, IActivityCompletedEvent { }

    public record OperationTerminatedEvent<TState> : OperationEvent<TState>, IActivityCompletedEvent<TState> { }
}
