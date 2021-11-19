using System;

namespace FsInfoCat.Services
{
    public interface IBackgroundOperationCompletedEvent : IBackgroundProgressEvent, IBackgroundOperationErrorOptEvent
    {
    }

    public interface IBackgroundOperationCompletedEvent<TSTate> : IBackgroundOperationCompletedEvent, IBackgroundProgressEvent<TSTate>, IBackgroundOperationErrorOptEvent<TSTate>
    {
    }
}
