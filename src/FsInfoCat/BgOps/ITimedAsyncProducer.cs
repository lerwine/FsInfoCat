namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncProducer<TResult> : ITimedAsyncOperation, ICustomAsyncProducer<ITimedAsyncOpEventArgs, TResult>, IAsyncProducer<TResult>
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncProducer<TState, TResult> : ITimedAsyncProducer<TResult>, ITimedAsyncOperation<TState>, ICustomAsyncProducer<TState, ITimedAsyncOpEventArgs<TState>, TResult>, IAsyncProducer<TState, TResult>
    {
    }
}
