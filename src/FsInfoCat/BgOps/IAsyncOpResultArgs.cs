namespace FsInfoCat.BgOps
{
    public interface IAsyncOpResultArgs<TResult> : IAsyncOpEventArgs
    {
        TResult Result { get; }
    }


    public interface IAsyncOpResultArgs<TState, TResult> : IAsyncOpResultArgs<TResult>, IAsyncOpEventArgs<TState>
    {
    }
}
