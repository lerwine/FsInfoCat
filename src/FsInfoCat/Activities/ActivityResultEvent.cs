namespace FsInfoCat.Activities
{
    // TODO: Document ActivityResultEvent classes
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    internal record ActivityResultEvent<TResult> : ActivityCompletedEvent, IActivityResultEvent<TResult>
    {
        public TResult Result { get; init; }
    }

    internal record ActivityResultEvent<TState, TResult> : ActivityResultEvent<TResult>, IActivityResultEvent<TState, TResult>
    {
        public TState AsyncState { get; init; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
