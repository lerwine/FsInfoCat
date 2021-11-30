using System;

namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundProgressStartedEvent : IBackgroundProgressStartedEvent, ITimedBackgroundProgressEvent, IObservable<ITimedBackgroundProgressEvent>
    {
    }

    public interface ITimedBackgroundProgressStartedEvent<TState> : ITimedBackgroundProgressStartedEvent, IBackgroundProgressStartedEvent<TState>, ITimedBackgroundProgressEvent<TState>, IObservable<ITimedBackgroundProgressEvent<TState>>
    {
    }
}
