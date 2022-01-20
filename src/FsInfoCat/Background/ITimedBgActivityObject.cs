using System;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBgActivityObject : IBgActivityObject
    {
        DateTime Started { get; }

        TimeSpan Duration { get; }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBgActivityObject<TState> : ITimedBgActivityObject, IBgActivityObject<TState>
    {
    }
}
