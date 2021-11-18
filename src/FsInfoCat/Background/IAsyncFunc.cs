using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public interface IAsyncFunc<TResult> : IAsyncAction
    {
        new Task<TResult> Task { get; }
    }

    public interface IAsyncFunc<TState, TResult> : IAsyncFunc<TResult>, IAsyncAction<TState>, IAsyncAction
    {
    }
}
