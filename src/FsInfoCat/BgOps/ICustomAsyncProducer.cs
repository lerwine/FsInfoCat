namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ICustomAsyncProducer<TEvent, TResult> : IAsyncFunc<TResult>, ICustomAsyncOperation<TEvent>
        where TEvent : IAsyncOpEventArgs
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ICustomAsyncProducer<TState, TEvent, TResult> : ICustomAsyncProducer<TEvent, TResult>, IAsyncFunc<TState, TResult>, ICustomAsyncOperation<TState, TEvent>
        where TEvent : IAsyncOpEventArgs<TState>
    {
    }
}
