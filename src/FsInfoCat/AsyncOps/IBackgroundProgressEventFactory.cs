namespace FsInfoCat.AsyncOps
{
    interface IBackgroundProgressEventFactory<TEvent, TProgress>
            where TEvent : IBackgroundProgressEvent
            where TProgress : IBackgroundProgress<TEvent>
    {
    }
}
