namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpProgress : IAsyncOpProgress, ITimedAsyncOpInfo
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpProgress<T> : ITimedAsyncOpProgress, IAsyncOpProgress<T>, ITimedAsyncOpInfo<T>
    {
    }
}
