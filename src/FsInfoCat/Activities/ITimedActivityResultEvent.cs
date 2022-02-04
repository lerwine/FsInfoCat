namespace FsInfoCat.Activities
{
    public interface ITimedActivityResultEvent<TResult> : IActivityResultEvent<TResult>, ITimedActivityCompletedEvent { }

    public interface ITimedActivityResultEvent<TState, TResult> : IActivityResultEvent<TState, TResult>, ITimedActivityCompletedEvent<TState>, ITimedActivityResultEvent<TResult> { }
}
