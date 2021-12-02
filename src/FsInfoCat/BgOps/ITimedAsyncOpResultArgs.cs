namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpResultArgs<TResult> : ITimedAsyncOpEventArgs, IAsyncOpResultArgs<TResult>
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpResultArgs<TState, TResult> : ITimedAsyncOpResultArgs<TResult>, ITimedAsyncOpEventArgs<TState>, IAsyncOpResultArgs<TState, TResult>
    {
    }
}

