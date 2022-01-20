namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpProgress : IAsyncOpProgress, ITimedAsyncOpInfo
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpProgress<T> : ITimedAsyncOpProgress, IAsyncOpProgress<T>, ITimedAsyncOpInfo<T>
    {
    }
}
