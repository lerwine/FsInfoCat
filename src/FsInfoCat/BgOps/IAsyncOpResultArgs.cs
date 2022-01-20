namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncOpResultArgs<TResult> : IAsyncOpEventArgs
    {
        TResult Result { get; }
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncOpResultArgs<TState, TResult> : IAsyncOpResultArgs<TResult>, IAsyncOpEventArgs<TState>
    {
    }
}

