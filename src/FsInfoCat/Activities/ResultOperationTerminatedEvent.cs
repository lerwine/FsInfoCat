namespace FsInfoCat.Activities
{
    // TODO: Document ResultOperationTerminatedEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    internal record ResultOperationTerminatedEvent<TResult> : OperationTerminatedEvent, IActivityResultEvent<TResult>
    {
        public TResult Result { get; init; }
    }

    internal record ResultOperationTerminatedEvent<TState, TResult> : ResultOperationTerminatedEvent<TResult>, IActivityResultEvent<TState, TResult>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
