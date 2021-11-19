using System;

namespace FsInfoCat.Services
{
    public interface IBackgroundOperationErrorOptEvent : IBackgroundProgressEvent
    {
        Exception Error { get; }
    }

    public interface IBackgroundOperationErrorOptEvent<TSTate> : IBackgroundOperationErrorOptEvent, IBackgroundProgressEvent<TSTate>
    {
    }
}
