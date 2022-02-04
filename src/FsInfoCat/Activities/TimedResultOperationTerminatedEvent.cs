namespace FsInfoCat.Activities
{
    internal record TimedResultOperationTerminatedEvent<TResult> : TimedOperationTerminatedEvent, ITimedActivityResultEvent<TResult>
    {
        public TResult Result { get; init; }
    }

    internal record TimedResultOperationTerminatedEvent<TState, TResult> : TimedResultOperationTerminatedEvent<TResult>, ITimedActivityResultEvent<TState, TResult>
    {
        public TState AsyncState { get; init; }
    }
}
