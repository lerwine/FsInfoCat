using System;

namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundProgressStartedEvent : IBackgroundProgressEvent, IObservable<IBackgroundProgressEvent>
    {
    }

    public interface IBackgroundProgressStartedEvent<TState> : IBackgroundProgressStartedEvent, IBackgroundProgressEvent<TState>, IObservable<IBackgroundProgressEvent<TState>>
    {
    }
}
