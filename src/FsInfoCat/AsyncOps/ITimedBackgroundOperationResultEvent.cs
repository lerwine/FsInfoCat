namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundOperationResultEvent<TResult> : IBackgroundOperationResultEvent<TResult>, ITimedBackgroundOperationCompletedEvent
    {
    }

    public interface ITimedBackgroundOperationResultEvent<TState, TResult> : ITimedBackgroundOperationResultEvent<TResult>, IBackgroundOperationResultEvent<TState, TResult>, ITimedBackgroundOperationCompletedEvent<TState>
    {
    }
}
