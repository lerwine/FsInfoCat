using System;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedBgActivityObject : IBgActivityObject
    {
        DateTime Started { get; }

        TimeSpan Duration { get; }
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedBgActivityObject<TState> : ITimedBgActivityObject, IBgActivityObject<TState>
    {
    }
}
