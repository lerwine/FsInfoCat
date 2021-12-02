namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpStatus : ITimedAsyncOpInfo, IAsyncOpStatus
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpStatus<T> : ITimedAsyncOpInfo<T>, IAsyncOpStatus<T>, ITimedAsyncOpStatus
    {
    }
}
