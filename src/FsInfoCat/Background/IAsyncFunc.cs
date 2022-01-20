using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncFunc<TResult> : IAsyncAction
    {
        new Task<TResult> Task { get; }
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncFunc<TState, TResult> : IAsyncFunc<TResult>, IAsyncAction<TState>, IAsyncAction
    {
    }
}
