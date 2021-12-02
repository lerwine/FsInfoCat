using System;

namespace FsInfoCat.BgOps
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ICustomAsyncOperation<TEvent> : IAsyncAction, IObservable<TEvent>
        where TEvent : IAsyncOpEventArgs
    {
        new TEvent LastEvent { get; }
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ICustomAsyncOperation<TState, TEvent> : ICustomAsyncOperation<TEvent>, IAsyncAction<TState>
        where TEvent : IAsyncOpEventArgs<TState>
    {
    }
}
