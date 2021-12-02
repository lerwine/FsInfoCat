using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncFunc<TResult> : IAsyncFunc<TResult>, ITimedAsyncAction
    {
        new Task<TResult> Task { get; }
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncFunc<TState, TResult> : ITimedAsyncFunc<TResult>, ITimedAsyncAction<TState>, IAsyncFunc<TResult>, ITimedAsyncAction
    {
    }
}
