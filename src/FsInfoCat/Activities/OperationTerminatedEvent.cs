namespace FsInfoCat.Activities
{
    // TODO: Document OperationTerminatedEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public record OperationTerminatedEvent : OperationEvent, IActivityCompletedEvent { }

    public record OperationTerminatedEvent<TState> : OperationEvent<TState>, IActivityCompletedEvent<TState> { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
