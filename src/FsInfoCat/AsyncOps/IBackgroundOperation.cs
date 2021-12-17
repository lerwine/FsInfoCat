using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents an asynchronous operation.
    /// </summary>
    /// <remarks>This pushes <see cref="IBackgroundProgressEvent"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="ICancellableOperation" />
    /// <seealso cref="IDisposable" />
    public interface IBackgroundOperation : ICancellableOperation, IDisposable
    {
        /// <summary>
        /// Gets the task for the background operation.
        /// </summary>
        /// <value>The task that is performing the background operation.</value>
        Task Task { get; }
    }

    /// <summary>
    /// Represents an asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <remarks>This pushes <see cref="IBackgroundProgressEvent{TState}"/> notifications which indicate the progress of the background operation.</remarks>
    /// <seealso cref="IBackgroundOperation" />
    /// <seealso cref="ICancellableOperation{TState}" />
    public interface IBackgroundOperation<TState> : ICancellableOperation<TState>, IBackgroundOperation
    {
    }
}
