using System;

namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundOperationErrorOptEvent : IBackgroundProgressEvent
    {
        Exception Error { get; }
    }

    public interface IBackgroundOperationErrorOptEvent<TState> : IBackgroundOperationErrorOptEvent, IBackgroundProgressEvent<TState>
    {
    }
}
