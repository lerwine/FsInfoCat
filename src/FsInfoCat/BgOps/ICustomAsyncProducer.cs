namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ICustomAsyncProducer<TEvent, TResult> : IAsyncFunc<TResult>, ICustomAsyncOperation<TEvent>
        where TEvent : IAsyncOpEventArgs
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ICustomAsyncProducer<TState, TEvent, TResult> : ICustomAsyncProducer<TEvent, TResult>, IAsyncFunc<TState, TResult>, ICustomAsyncOperation<TState, TEvent>
        where TEvent : IAsyncOpEventArgs<TState>
    {
    }
}
