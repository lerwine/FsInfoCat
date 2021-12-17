namespace FsInfoCat.AsyncOps
{
    interface IBackgroundEventFactory<TEvent, TResultEvent, TProgress> : IBackgroundProgressEventFactory<TEvent, TProgress>
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
            where TProgress : IBackgroundProgress<TEvent>
    {
    }

    interface IBackgroundEventFactory<TEvent, TResultEvent, TProgress, TResult> : IBackgroundProgressEventFactory<TEvent, TProgress>
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TResult>
            where TProgress : IBackgroundProgress<TEvent>
    {
    }
}
