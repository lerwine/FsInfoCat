namespace FsInfoCat.Activities
{
    internal record ActivityResultEvent<TResult> : ActivityCompletedEvent, IActivityResultEvent<TResult>
    {
        public TResult Result { get; init; }
    }

    internal record ActivityResultEvent<TState, TResult> : ActivityResultEvent<TResult>, IActivityResultEvent<TState, TResult>
    {
        public TState AsyncState { get; init; }
    }
}
