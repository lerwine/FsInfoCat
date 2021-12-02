namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOperation : ITimedAsyncOpStatus, ICustomAsyncOperation<ITimedAsyncOpEventArgs>, IAsyncOperation
    {
    }
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOperation<T> : ITimedAsyncOperation, ITimedAsyncOpStatus<T>, ICustomAsyncOperation<T, ITimedAsyncOpEventArgs<T>>, IAsyncOperation<T>
    {
    }
}
