namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncFunc<TResult> : IAsyncFunc<TResult>, ITimedAsyncAction
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncFunc<TState, TResult> : ITimedAsyncFunc<TResult>, IAsyncFunc<TState, TResult>, ITimedAsyncAction<TState>
    {
    }
}
