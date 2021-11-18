namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncOpEventArgs : ITimedAsyncOpStatus, IAsyncOpEventArgs
    {
    }

    public interface ITimedAsyncOpEventArgs<T> : ITimedAsyncOpEventArgs, ITimedAsyncOpStatus<T>, IAsyncOpEventArgs<T>
    {
    }
}
