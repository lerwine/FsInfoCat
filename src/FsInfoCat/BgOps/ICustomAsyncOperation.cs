using System;

namespace FsInfoCat.BgOps
{
    public interface ICustomAsyncOperation<TEvent> : IAsyncAction, IObservable<TEvent>
        where TEvent : IAsyncOpEventArgs
    {
        new TEvent LastEvent { get; }
    }

    public interface ICustomAsyncOperation<TState, TEvent> : ICustomAsyncOperation<TEvent>, IAsyncAction<TState>
        where TEvent : IAsyncOpEventArgs<TState>
    {
    }
}
