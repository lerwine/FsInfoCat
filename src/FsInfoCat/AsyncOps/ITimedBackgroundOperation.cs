using System;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents a timed asynchronous operation.
    /// </summary>
    /// <remarks>This pushes <see cref="ITimedBackgroundProgressEvent"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundOperation" />
    /// <seealso cref="ITimedBackgroundProgressInfo" />
    /// <seealso cref="ITimedBackgroundProgressEvent" />
    /// <seealso cref="IObservable{ITimedBackgroundProgressEvent}" />
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundOperation : IBackgroundOperation, ITimedBackgroundProgressInfo, IObservable<ITimedBackgroundProgressEvent>
    {
    }

    /// <summary>
    /// Represents a timed asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="ITimedBackgroundProgressEvent{TState}"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="ITimedBackgroundOperation" />
    /// <seealso cref="IBackgroundOperation{TState}" />
    /// <seealso cref="ITimedBackgroundProgressInfo{TState}" />
    /// <seealso cref="ITimedBackgroundProgressEvent{TState}" />
    /// <seealso cref="IObservable{ITimedBackgroundProgressEvent{TState}}" />
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundOperation<TState> : IBackgroundOperation<TState>, ITimedBackgroundProgressInfo<TState>, IObservable<ITimedBackgroundProgressEvent<TState>>, ITimedBackgroundOperation
    {
    }
}
