namespace FsInfoCat.Services
{
    public interface IBackgroundOperationResultEvent<TResult> : IBackgroundOperationCompletedEvent
    {
        TResult Result { get; }
    }

    public interface IBackgroundOperationResultEvent<TState, TResult> : IBackgroundOperationResultEvent<TResult>, IBackgroundOperationCompleteEvent<TState>
    {
    }
}
