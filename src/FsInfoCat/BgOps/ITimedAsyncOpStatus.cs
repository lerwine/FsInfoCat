namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncOpStatus : ITimedAsyncOpInfo, IAsyncOpStatus
    {
    }

    public interface ITimedAsyncOpStatus<T> : ITimedAsyncOpInfo<T>, IAsyncOpStatus<T>, ITimedAsyncOpStatus
    {
    }
}
