using System;

namespace FsInfoCat.Services
{
    public interface IBackgroundOperationCompletedEvent : IBackgroundProgressEvent, IBackgroundOperationErrorOptEvent
    {
        bool RanToCompletion { get; }
    }

    public interface IBackgroundOperationCompletedEvent<TState> : IBackgroundOperationCompletedEvent, IBackgroundProgressEvent<TState>, IBackgroundOperationErrorOptEvent<TState>
    {
    }
}
