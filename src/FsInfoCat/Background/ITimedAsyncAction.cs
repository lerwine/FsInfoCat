using System;

namespace FsInfoCat.Background
{
    public interface ITimedAsyncAction : ITimedBgActivityObject, IObservable<ITimedBgStatusEventArgs>, IAsyncAction
    {
    }

    public interface ITimedAsyncAction<TState> : IAsyncAction<TState>, IObservable<ITimedBgStatusEventArgs<TState>>, ITimedBgActivityObject<TState>, ITimedBgActivityObject, IObservable<ITimedBgStatusEventArgs>, IAsyncAction
    {
    }
}
