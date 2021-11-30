using System;

namespace FsInfoCat.AsyncOps
{
    public interface ITimedBackgroundOperation : IBackgroundOperation, ITimedBackgroundProgressInfo, IObservable<ITimedBackgroundProgressEvent>
    {
    }

    public interface ITimedBackgroundOperation<TState> : ITimedBackgroundOperation, IBackgroundOperation<TState>, ITimedBackgroundProgressInfo<TState>, IObservable<ITimedBackgroundProgressEvent<TState>>
    {
    }
}
