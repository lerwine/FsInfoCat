namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundOperationCompletedEvent : IBackgroundOperationCompletedEvent, ITimedBackgroundProgressEvent, ITimedBackgroundOperationErrorOptEvent
    {
    }

    public interface ITimedBackgroundOperationCompletedEvent<TState> : ITimedBackgroundOperationCompletedEvent, IBackgroundOperationCompletedEvent<TState>, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationErrorOptEvent<TState>
    {
    }
}
