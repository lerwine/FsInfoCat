namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncOpResultArgs<TResult> : ITimedAsyncOpEventArgs, IAsyncOpResultArgs<TResult>
    {
    }

    public interface ITimedAsyncOpResultArgs<TState, TResult> : ITimedAsyncOpResultArgs<TResult>, ITimedAsyncOpEventArgs<TState>, IAsyncOpResultArgs<TState, TResult>
    {
    }
}

