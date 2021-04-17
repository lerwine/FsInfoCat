namespace FsInfoCat.Desktop
{
    public interface IAsyncResult<TResult> : IAsyncOperation
    {
        TResult Result { get; }
    }

    public interface IAsyncResult<TState, TResult> : IAsyncResult<TResult>, IAsyncOperation<TState>
    {
    }
}
