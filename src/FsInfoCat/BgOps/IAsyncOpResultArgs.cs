namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncOpResultArgs<TResult> : IAsyncOpEventArgs
    {
        TResult Result { get; }
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncOpResultArgs<TState, TResult> : IAsyncOpResultArgs<TResult>, IAsyncOpEventArgs<TState>
    {
    }
}

