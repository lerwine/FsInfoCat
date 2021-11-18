namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncOperation : ITimedAsyncOpStatus, ICustomAsyncOperation<ITimedAsyncOpEventArgs>, IAsyncOperation
    {
    }
    public interface ITimedAsyncOperation<T> : ITimedAsyncOperation, ITimedAsyncOpStatus<T>, ICustomAsyncOperation<T, ITimedAsyncOpEventArgs<T>>, IAsyncOperation<T>
    {
    }
}
