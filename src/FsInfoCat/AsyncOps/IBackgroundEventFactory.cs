using System;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes a type that generates background operation event objects.
    /// </summary>
    /// <typeparam name="TEvent">The type of the progress event.</typeparam>
    /// <typeparam name="TResultEvent">The type of the event that indicates that the background operation has run to completion.</typeparam>
    /// <typeparam name="TProgress">The type of the progress context object.</typeparam>
    /// <seealso cref="IBackgroundProgressEventFactory{TEvent, TProgress}" />
    public interface IBackgroundEventFactory<TEvent, TResultEvent, TProgress> : IBackgroundProgressEventFactory<TEvent, TProgress>
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
            where TProgress : IBackgroundProgress<TEvent>
    {
    }

    /// <summary>
    /// Describes a type that generates background operation event objects.
    /// </summary>
    /// <typeparam name="TEvent">The type of the progress event.</typeparam>
    /// <typeparam name="TResultEvent">The type of the event that indicates that the background operation has run to completion.</typeparam>
    /// <typeparam name="TProgress">The type of the progress context object.</typeparam>
    /// <typeparam name="TResult">The type of the value produced by the background operation.</typeparam>
    /// <seealso cref="IBackgroundProgressEventFactory{TEvent, TProgress}" />
    public interface IBackgroundEventFactory<TEvent, TResultEvent, TProgress, TResult> : IBackgroundProgressEventFactory<TEvent, TProgress>
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TResult>
            where TProgress : IBackgroundProgress<TEvent>
    {
    }
}
