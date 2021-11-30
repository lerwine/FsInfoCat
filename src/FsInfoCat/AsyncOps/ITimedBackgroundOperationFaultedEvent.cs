namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundOperationFaultedEvent : IBackgroundOperationFaultedEvent, ITimedBackgroundOperationCompletedEvent, ITimedBackgroundOperationErrorEvent
    {
    }

    public interface ITimedBackgroundOperationFaultedEvent<TState> : ITimedBackgroundOperationFaultedEvent, IBackgroundOperationFaultedEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>, ITimedBackgroundOperationErrorEvent<TState>
    {
    }
}
