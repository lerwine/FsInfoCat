namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncAction : IAsyncAction, ITimedAsyncOpStatus
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncAction<T> : ITimedAsyncAction, IAsyncAction<T>, ITimedAsyncOpStatus<T>
    {
    }
}
