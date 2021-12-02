using System;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents a progress event for the start of a timed asynchronous operation.
    /// </summary>
    /// <remarks>This pushes <see cref="ITimedBackgroundProgressEvent"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundProgressStartedEvent" />
    /// <seealso cref="ITimedBackgroundProgressEvent" />
    /// <seealso cref="IObservable{ITimedBackgroundProgressEvent}" />
    public interface ITimedBackgroundProgressStartedEvent : IBackgroundProgressStartedEvent, ITimedBackgroundProgressEvent, IObservable<ITimedBackgroundProgressEvent>
    {
    }

    /// <summary>
    /// Represents a progress event for the start of a timed asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="ITimedBackgroundProgressEvent{TState}"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="ITimedBackgroundProgressStartedEvent" />
    /// <seealso cref="IBackgroundProgressStartedEvent{TState}" />
    /// <seealso cref="ITimedBackgroundProgressEvent{TState}" />
    /// <seealso cref="IObservable{ITimedBackgroundProgressEvent{TState}}" />
    public interface ITimedBackgroundProgressStartedEvent<TState> : ITimedBackgroundProgressStartedEvent, IBackgroundProgressStartedEvent<TState>, ITimedBackgroundProgressEvent<TState>, IObservable<ITimedBackgroundProgressEvent<TState>>
    {
    }
}
