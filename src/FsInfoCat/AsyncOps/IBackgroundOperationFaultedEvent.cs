namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundOperationFaultedEvent : IBackgroundOperationCompletedEvent, IBackgroundOperationErrorEvent
    {
    }

    public interface IBackgroundOperationFaultedEvent<TState> : IBackgroundOperationFaultedEvent, IBackgroundOperationCompletedEvent<TState>, IBackgroundOperationErrorEvent<TState>
    {
    }
}
