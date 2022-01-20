using System;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncAction : ITimedBgActivityObject, IObservable<ITimedBgStatusEventArgs>, IAsyncAction
    {
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncAction<TState> : IAsyncAction<TState>, IObservable<ITimedBgStatusEventArgs<TState>>, ITimedBgActivityObject<TState>, ITimedBgActivityObject, IObservable<ITimedBgStatusEventArgs>, IAsyncAction
    {
    }
}
