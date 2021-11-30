namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundOperationErrorEvent : IBackgroundOperationErrorEvent, ITimedBackgroundOperationErrorOptEvent
    {
    }

    public interface ITimedBackgroundOperationErrorEvent<TState> : ITimedBackgroundOperationErrorEvent, IBackgroundOperationErrorEvent<TState>, ITimedBackgroundOperationErrorOptEvent<TState>
    {
    }
}
