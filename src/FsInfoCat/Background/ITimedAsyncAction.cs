using System;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncAction : ITimedBgActivityObject, IObservable<ITimedBgStatusEventArgs>, IAsyncAction
    {
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncAction<TState> : IAsyncAction<TState>, IObservable<ITimedBgStatusEventArgs<TState>>, ITimedBgActivityObject<TState>, ITimedBgActivityObject, IObservable<ITimedBgStatusEventArgs>, IAsyncAction
    {
    }
}
