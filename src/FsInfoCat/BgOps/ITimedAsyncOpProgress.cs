namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncOpProgress : IAsyncOpProgress, ITimedAsyncOpInfo
    {
    }

    public interface ITimedAsyncOpProgress<T> : ITimedAsyncOpProgress, IAsyncOpProgress<T>, ITimedAsyncOpInfo<T>
    {
    }
}
