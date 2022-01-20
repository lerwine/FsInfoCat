using System;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents a timed asynchronous operation that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="ITimedBackgroundProgressEvent"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundFunc{TResult}" />
    /// <seealso cref="ITimedBackgroundOperation" />
    /// <seealso cref="ITimedBackgroundProgressEvent" />
    /// <seealso cref="IObservable{ITimedBackgroundProgressEvent}" />
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundFunc<TResult> : IBackgroundFunc<TResult>, ITimedBackgroundOperation, IObservable<ITimedBackgroundProgressEvent>
    {
    }

    /// <summary>
    /// Represents a timed asynchronous operation that produces a result value.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="ITimedBackgroundProgressEvent{TState}"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="ITimedBackgroundFunc{TResult}" />
    /// <seealso cref="IBackgroundFunc{TState, TResult}" />
    /// <seealso cref="ITimedBackgroundOperation{TState}" />
    /// <seealso cref="ITimedBackgroundProgressEvent{TState}" />
    /// <seealso cref="IObservable{ITimedBackgroundProgressEvent{TState}}" />
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBackgroundFunc<TState, TResult> : IBackgroundFunc<TState, TResult>, ITimedBackgroundOperation<TState>, IObservable<ITimedBackgroundProgressEvent<TState>>, ITimedBackgroundFunc<TResult>
    {
    }
}
