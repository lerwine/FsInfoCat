namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundOperationErrorOptEvent : IBackgroundOperationErrorOptEvent, ITimedBackgroundProgressEvent
    {
    }

    public interface ITimedBackgroundOperationErrorOptEvent<TState> : ITimedBackgroundOperationErrorOptEvent, IBackgroundOperationErrorOptEvent<TState>, ITimedBackgroundProgressEvent<TState>
    {
    }
}
