using System;

namespace FsInfoCat.Services
{
    public interface IBackgroundOperationErrorEvent : IBackgroundOperationErrorOptEvent
    {
        new MessageCode Code { get; }
    }

    public interface IBackgroundOperationErrorEvent<TState> : IBackgroundOperationErrorEvent, IBackgroundOperationErrorOptEvent<TState>
    {
    }
}
