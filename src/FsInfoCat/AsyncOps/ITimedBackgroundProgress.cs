namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundProgress<TEvent> : IBackgroundProgress<TEvent>, ITimedBackgroundProgressInfo, IBackgroundProgressFactory
        where TEvent : ITimedBackgroundProgressEvent
    {
    }

    public interface ITimedBackgroundProgress<TState, TEvent> : ITimedBackgroundProgress<TEvent>, IBackgroundProgress<TState, TEvent>, ITimedBackgroundProgressInfo<TState>, IBackgroundProgressFactory
        where TEvent : ITimedBackgroundProgressEvent<TState>
    {
    }
}
