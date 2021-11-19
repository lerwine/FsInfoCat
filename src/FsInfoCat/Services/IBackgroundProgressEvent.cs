namespace FsInfoCat.Services
{
    public interface IBackgroundProgressEvent : IBackgroundProgressInfo
    {
        MessageCode? Code { get; }
    }

    public interface IBackgroundProgressEvent<TState> : IBackgroundProgressEvent, IBackgroundProgressInfo<TState>
    {
    }
}
