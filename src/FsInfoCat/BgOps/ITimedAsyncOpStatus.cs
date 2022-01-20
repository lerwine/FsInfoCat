namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpStatus : ITimedAsyncOpInfo, IAsyncOpStatus
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpStatus<T> : ITimedAsyncOpInfo<T>, IAsyncOpStatus<T>, ITimedAsyncOpStatus
    {
    }
}
