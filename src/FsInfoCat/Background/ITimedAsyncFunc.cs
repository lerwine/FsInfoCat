using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public interface ITimedAsyncFunc<TResult> : IAsyncFunc<TResult>, ITimedAsyncAction
    {
        new Task<TResult> Task { get; }
    }

    public interface ITimedAsyncFunc<TState, TResult> : ITimedAsyncFunc<TResult>, ITimedAsyncAction<TState>, IAsyncFunc<TResult>, ITimedAsyncAction
    {
    }
}
