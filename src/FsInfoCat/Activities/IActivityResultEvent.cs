namespace FsInfoCat.Activities
{
    public interface IActivityResultEvent<TResult> : IActivityCompletedEvent
    {
        TResult Result { get; }
    }

    public interface IActivityResultEvent<TState, TResult> : IActivityCompletedEvent<TState>, IActivityResultEvent<TResult> { }
}
