namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncProducer<TResult> : ITimedAsyncOperation, ICustomAsyncProducer<ITimedAsyncOpEventArgs, TResult>, IAsyncProducer<TResult>
    {
    }

    public interface ITimedAsyncProducer<TState, TResult> : ITimedAsyncProducer<TResult>, ITimedAsyncOperation<TState>, ICustomAsyncProducer<TState, ITimedAsyncOpEventArgs<TState>, TResult>, IAsyncProducer<TState, TResult>
    {
    }
}
