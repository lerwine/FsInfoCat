namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpEventArgs : ITimedAsyncOpStatus, IAsyncOpEventArgs
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpEventArgs<T> : ITimedAsyncOpEventArgs, ITimedAsyncOpStatus<T>, IAsyncOpEventArgs<T>
    {
    }
}
