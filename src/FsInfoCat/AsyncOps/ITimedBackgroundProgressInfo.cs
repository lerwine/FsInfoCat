using System;

namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundProgressInfo : IBackgroundProgressInfo
    {
        TimeSpan Duration { get; }
    }

    public interface ITimedBackgroundProgressInfo<TState> : ITimedBackgroundProgressInfo, IBackgroundProgressInfo<TState>
    {
    }
}
