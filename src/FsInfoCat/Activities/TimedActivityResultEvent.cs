namespace FsInfoCat.Activities
{
    internal record TimedActivityResultEvent<TResult> : TimedActivityCompletedEvent, ITimedActivityResultEvent<TResult>
    {
        public TResult Result { get; init; }
    }

    internal record TimedActivityResultEvent<TState, TResult> : TimedActivityResultEvent<TResult>, ITimedActivityResultEvent<TState, TResult>
    {
        public TState AsyncState { get; init; }
    }
}
