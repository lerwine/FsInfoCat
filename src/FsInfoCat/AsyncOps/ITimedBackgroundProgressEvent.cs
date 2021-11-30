namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundProgressEvent : IBackgroundProgressEvent, ITimedBackgroundProgressInfo
    {
    }

    public interface ITimedBackgroundProgressEvent<TState> : ITimedBackgroundProgressEvent, IBackgroundProgressEvent<TState>, ITimedBackgroundProgressInfo<TState>
    {
    }
}
