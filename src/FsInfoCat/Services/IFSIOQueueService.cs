using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    public interface IBgOperationQueue : IReadOnlyCollection<IQueuedBgOperation>
    {
        /// <summary>
        /// Indicates whether a background operation is in progress.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets the current background operation or <see langword="null"/> if no background operation is in progress.
        /// </summary>
        IQueuedBgOperation CurrentOperation { get; }

        /// <summary>
        /// Enqueues a background operation, starting it immediately if no other operation is already running.
        /// </summary>
        /// <param name="activity">The activity code.</param>
        /// <param name="asyncMethod">Asyncronous method to invoke.</param>
        /// <returns>A <see cref="IQueuedBgOperation"/> object representing the background job.</returns>
        IQueuedBgOperation Enqueue(ActivityCode activity, [DisallowNull] Func<CancellationToken, Task> asyncMethod);

        /// <summary>
        /// Enqueues a background operation, starting it immediately if no other operation is already running.
        /// </summary>
        /// <param name="activity">The activity code.</param>
        /// <param name="asyncMethod">Asyncronous method to invoke.</param>
        /// <returns>A <see cref="IQueuedBgOperation"/> object representing the background job.</returns>
        IQueuedBgOperation Enqueue(ActivityCode activity, [DisallowNull] Func<IAsyncOperationProgress, Task> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <param name="activity">The activity code.</param>
        /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        /// <param name="asyncMethod">Asynchronous method that produces the result value.</param>
        /// <returns>A <see cref="IQueuedBgOperation{TResult}"/> object representing the background job.</returns>
        IQueuedBgOperation<TResult> Enqueue<TResult>(ActivityCode activity, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <param name="activity">The activity code.</param>
        /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        /// <param name="asyncMethod">Asynchronous method that produces the result value.</param>
        /// <returns>A <see cref="IQueuedBgOperation{TResult}"/> object representing the background job.</returns>
        IQueuedBgOperation<TResult> Enqueue<TResult>(ActivityCode activity, [DisallowNull] Func<IAsyncOperationProgress, Task<TResult>> asyncMethod);
    }
    /// <summary>
    /// IO-Bound background operation queue service
    /// </summary>
    public interface IFSIOQueueService : IHostedService, IBgOperationQueue
    {
    }
}
