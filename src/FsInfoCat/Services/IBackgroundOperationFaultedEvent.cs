namespace FsInfoCat.Services
{
    public interface IBackgroundOperationFaultedEvent : IBackgroundOperationCompletedEvent, IBackgroundOperationErrorEvent
    {
    }

    public interface IBackgroundOperationFaultedEvent<TSTate> : IBackgroundOperationFaultedEvent, IBackgroundOperationCompletedEvent<TSTate>, IBackgroundOperationErrorEvent<TSTate>
    {
    }
}
