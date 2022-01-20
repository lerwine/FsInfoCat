namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpEventArgs : ITimedAsyncOpStatus, IAsyncOpEventArgs
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpEventArgs<T> : ITimedAsyncOpEventArgs, ITimedAsyncOpStatus<T>, IAsyncOpEventArgs<T>
    {
    }
}
