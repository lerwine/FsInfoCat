using System;

namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundOperationErrorOptEvent : IBackgroundProgressEvent
    {
        Exception Error { get; }
    }

    public interface IBackgroundOperationErrorOptEvent<TSTate> : IBackgroundOperationErrorOptEvent, IBackgroundProgressEvent<TSTate>
    {
    }
}
