namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOperation : ITimedAsyncOpStatus, ICustomAsyncOperation<ITimedAsyncOpEventArgs>, IAsyncOperation
    {
    }
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOperation<T> : ITimedAsyncOperation, ITimedAsyncOpStatus<T>, ICustomAsyncOperation<T, ITimedAsyncOpEventArgs<T>>, IAsyncOperation<T>
    {
    }
}
