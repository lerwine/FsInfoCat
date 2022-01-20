using System;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents an asynchronous operation that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="IBackgroundProgressEvent"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundOperation" />
    /// <seealso cref="IBackgroundProgressEvent" />
    /// <seealso cref="IObservable{IBackgroundProgressEvent}" />
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundFunc<TResult> : IBackgroundOperation, IObservable<IBackgroundProgressEvent>
    {
        /// <summary>
        /// Gets the task for the background operation.
        /// </summary>
        /// <value>The task that produces the result value for the background operation.</value>
        new Task<TResult> Task { get; }
    }

    /// <summary>
    /// Represents an asynchronous operation that produces a result value.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="IBackgroundProgressEvent{TState}"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundFunc{TResult}" />
    /// <seealso cref="IBackgroundOperation{TState}" />
    /// <seealso cref="IBackgroundProgressEvent{TState}" />
    /// <seealso cref="IObservable{IBackgroundProgressEvent{TState}}" />
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundFunc<TState, TResult> : IBackgroundOperation<TState>, IObservable<IBackgroundProgressEvent<TState>>, IBackgroundFunc<TResult>
    {
    }
}
