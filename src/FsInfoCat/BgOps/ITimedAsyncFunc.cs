namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncFunc<TResult> : IAsyncFunc<TResult>, ITimedAsyncAction
    {
    }

    public interface ITimedAsyncFunc<TState, TResult> : ITimedAsyncFunc<TResult>, IAsyncFunc<TState, TResult>, ITimedAsyncAction<TState>
    {
    }
}
