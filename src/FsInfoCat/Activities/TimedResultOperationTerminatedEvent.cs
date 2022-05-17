namespace FsInfoCat.Activities
{
    // TODO: Document TimedResultOperationTerminatedEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    internal record TimedResultOperationTerminatedEvent<TResult> : TimedOperationTerminatedEvent, ITimedActivityResultEvent<TResult>
    {
        public TResult Result { get; init; }
    }

    internal record TimedResultOperationTerminatedEvent<TState, TResult> : TimedResultOperationTerminatedEvent<TResult>, ITimedActivityResultEvent<TState, TResult>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
