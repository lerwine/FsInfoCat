using System;

namespace FsInfoCat.Background
{
    public interface ITimedBgActivityObject : IBgActivityObject
    {
        DateTime Started { get; }

        TimeSpan Duration { get; }
    }

    public interface ITimedBgActivityObject<TState> : ITimedBgActivityObject, IBgActivityObject<TState>
    {
    }
}
