using System;

namespace FsInfoCat.Services
{
    public interface IBackgroundOperationCompletedEvent : IBackgroundProgressEvent, IBackgroundOperationErrorOptEvent
    {
    }

    public interface IBackgroundOperationCompletedEvent<TState> : IBackgroundOperationCompletedEvent, IBackgroundProgressEvent<TState>, IBackgroundOperationErrorOptEvent<TState>
    {
    }
}
