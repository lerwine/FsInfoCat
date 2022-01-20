namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncFunc<TResult> : IAsyncFunc<TResult>, ITimedAsyncAction
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncFunc<TState, TResult> : ITimedAsyncFunc<TResult>, IAsyncFunc<TState, TResult>, ITimedAsyncAction<TState>
    {
    }
}
