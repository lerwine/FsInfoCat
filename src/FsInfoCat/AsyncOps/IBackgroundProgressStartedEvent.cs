namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundProgressStartedEvent : IBackgroundProgressEvent, ICancellableOperation
    {
    }

    public interface IBackgroundProgressStartedEvent<TState> : IBackgroundProgressStartedEvent, IBackgroundProgressEvent<TState>, ICancellableOperation<TState>
    {
    }
}
