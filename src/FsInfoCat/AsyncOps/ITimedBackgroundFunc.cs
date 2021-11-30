using System;

namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundFunc<TResult> : IBackgroundFunc<TResult>, ITimedBackgroundOperation, IObservable<ITimedBackgroundProgressEvent>
    {
    }

    public interface ITimedBackgroundFunc<TState, TResult> : ITimedBackgroundFunc<TResult>, IBackgroundFunc<TState, TResult>, ITimedBackgroundOperation<TState>, IObservable<ITimedBackgroundProgressEvent<TState>>
    {
    }
}
