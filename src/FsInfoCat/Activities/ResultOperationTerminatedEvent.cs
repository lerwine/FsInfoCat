namespace FsInfoCat.Activities
{
    internal record ResultOperationTerminatedEvent<TResult> : OperationTerminatedEvent, IActivityResultEvent<TResult>
    {
        public TResult Result { get; init; }
    }

    internal record ResultOperationTerminatedEvent<TState, TResult> : ResultOperationTerminatedEvent<TResult>, IActivityResultEvent<TState, TResult>
    {
        public TState AsyncState { get; init; }
    }
}
