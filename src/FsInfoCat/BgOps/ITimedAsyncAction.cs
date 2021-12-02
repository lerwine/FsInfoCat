namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncAction : IAsyncAction, ITimedAsyncOpStatus
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncAction<T> : ITimedAsyncAction, IAsyncAction<T>, ITimedAsyncOpStatus<T>
    {
    }
}
