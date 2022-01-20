namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncProducer<TResult> : ITimedAsyncOperation, ICustomAsyncProducer<ITimedAsyncOpEventArgs, TResult>, IAsyncProducer<TResult>
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncProducer<TState, TResult> : ITimedAsyncProducer<TResult>, ITimedAsyncOperation<TState>, ICustomAsyncProducer<TState, ITimedAsyncOpEventArgs<TState>, TResult>, IAsyncProducer<TState, TResult>
    {
    }
}
