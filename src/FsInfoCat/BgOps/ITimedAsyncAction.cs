namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncAction : IAsyncAction, ITimedAsyncOpStatus
    {
    }

    public interface ITimedAsyncAction<T> : ITimedAsyncAction, IAsyncAction<T>, ITimedAsyncOpStatus<T>
    {
    }
}
