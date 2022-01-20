namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpResultArgs<TResult> : ITimedAsyncOpEventArgs, IAsyncOpResultArgs<TResult>
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpResultArgs<TState, TResult> : ITimedAsyncOpResultArgs<TResult>, ITimedAsyncOpEventArgs<TState>, IAsyncOpResultArgs<TState, TResult>
    {
    }
}

