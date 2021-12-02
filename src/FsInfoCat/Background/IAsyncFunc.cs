using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncFunc<TResult> : IAsyncAction
    {
        new Task<TResult> Task { get; }
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncFunc<TState, TResult> : IAsyncFunc<TResult>, IAsyncAction<TState>, IAsyncAction
    {
    }
}
