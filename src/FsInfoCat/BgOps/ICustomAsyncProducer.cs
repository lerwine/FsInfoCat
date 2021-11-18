namespace FsInfoCat.BgOps
{
    public interface ICustomAsyncProducer<TEvent, TResult> : IAsyncFunc<TResult>, ICustomAsyncOperation<TEvent>
        where TEvent : IAsyncOpEventArgs
    {
    }

    public interface ICustomAsyncProducer<TState, TEvent, TResult> : ICustomAsyncProducer<TEvent, TResult>, IAsyncFunc<TState, TResult>, ICustomAsyncOperation<TState, TEvent>
        where TEvent : IAsyncOpEventArgs<TState>
    {
    }
}
