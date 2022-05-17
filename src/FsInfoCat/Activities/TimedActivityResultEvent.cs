namespace FsInfoCat.Activities
{
    // TODO: Document TimedActivityResultEvent records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    internal record TimedActivityResultEvent<TResult> : TimedActivityCompletedEvent, ITimedActivityResultEvent<TResult>
    {
        public TResult Result { get; init; }
    }

    internal record TimedActivityResultEvent<TState, TResult> : TimedActivityResultEvent<TResult>, ITimedActivityResultEvent<TState, TResult>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
