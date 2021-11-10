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

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="state">The state.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgOperation{TState}"/> object representing the background job.</returns>
        //IQueuedBgOperation<TState> Enqueue<TState, TProgress>(ActivityCode activity, TState state, [DisallowNull] Func<TState, IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TState, TProgress, Task> asyncMethod) where TProgress : IStatusReportable;

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="state">The state.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgOperation{TState}"/> object representing the background job.</returns>
        //IQueuedBgOperation<TState> Enqueue<TState, TProgress>(ActivityCode activity, TState state, [DisallowNull] Func<TState, IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TProgress, Task> asyncMethod) where TProgress : IStatusReportable;

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="state">The state.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgOperation{TState}"/> object representing the background job.</returns>
        //IQueuedBgOperation<TState> Enqueue<TState, TProgress>(ActivityCode activity, TState state, [DisallowNull] Func<IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TState, TProgress, Task> asyncMethod) where TProgress : IStatusReportable;

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="state">The state.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgOperation{TState}"/> object representing the background job.</returns>
        //IQueuedBgOperation<TState> Enqueue<TState, TProgress>(ActivityCode activity, TState state, [DisallowNull] Func<IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TProgress, CancellationToken, Task> asyncMethod) where TProgress : IStatusReportable;

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="T">The type of the t progress.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgOperation{TState}"/> object representing the background job.</returns>
        //IQueuedBgOperation Enqueue<T>(ActivityCode activity, [DisallowNull] Func<IQueuedBgOperation, CancellationToken, T> progressFactory, [DisallowNull] Func<T, Task> asyncMethod)
        //    where T : IStatusReportable;

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="T">The type of the asynchronous state value.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="state">The state.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgOperation{TState}"/> object representing the background job.</returns>
        IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, [DisallowNull] Func<T, IStatusReportable, Task> asyncMethod);

        IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<T, IStatusReportable, Task> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="T">The type of the asynchronous state value.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="state">The state.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgOperation{TState}"/> object representing the background job.</returns>
        IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, [DisallowNull] Func<IStatusReportable, Task> asyncMethod);

        IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <param name="activity">The activity code.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgOperation"/> object representing the background job.</returns>
        IQueuedBgOperation Enqueue(ActivityCode activity, [DisallowNull] Func<IStatusReportable, Task> asyncMethod);

        IQueuedBgOperation Enqueue(ActivityCode activity, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="T">The type of the asynchronous state value.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="state">The state.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgOperation{T}"/> object representing the background job.</returns>
        IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, [DisallowNull] Func<T, CancellationToken, Task> asyncMethod);

        IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<T, CancellationToken, Task> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="T">The type of the asynchronous state value.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="state">The state.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgOperation{T}"/> object representing the background job.</returns>
        IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, [DisallowNull] Func<CancellationToken, Task> asyncMethod);

        IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <param name="activity">The activity code.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgOperation"/> object representing the background job.</returns>
        IQueuedBgOperation Enqueue(ActivityCode activity, [DisallowNull] Func<CancellationToken, Task> asyncMethod);

        IQueuedBgOperation Enqueue(ActivityCode activity, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task> asyncMethod);

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="state">The state.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        //IQueuedBgProducer<TState, TResult> Enqueue<TState, TProgress, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<TState, IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TState, TProgress, CancellationToken, Task<TResult>> asyncMethod) where TProgress : IStatusReportable;

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="state">The state.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        //IQueuedBgProducer<TState, TResult> Enqueue<TState, TProgress, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<TState, IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TProgress, CancellationToken, Task<TResult>> asyncMethod) where TProgress : IStatusReportable;

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="state">The state.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        //IQueuedBgProducer<TState, TResult> Enqueue<TState, TProgress, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TState, TProgress, CancellationToken, Task<TResult>> asyncMethod) where TProgress : IStatusReportable;

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="state">The state.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        //IQueuedBgProducer<TState, TResult> Enqueue<TState, TProgress, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TProgress, CancellationToken, Task<TResult>> asyncMethod) where TProgress : IStatusReportable;

        ///// <summary>
        ///// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        ///// </summary>
        ///// <typeparam name="TProgress">The type of the t progress.</typeparam>
        ///// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        ///// <param name="activity">The activity code.</param>
        ///// <param name="progressFactory">The progress factory.</param>
        ///// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        ///// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        //IQueuedBgProducer<TResult> Enqueue<TProgress, TResult>(ActivityCode activity,  [DisallowNull] Func<IQueuedBgOperation, TProgress> progressFactory,
        //    [DisallowNull] Func<TProgress, CancellationToken, Task<TResult>> asyncMethod) where TProgress : IStatusReportable;

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="state">The state.</param>
        /// <param name="progressFactory">The progress factory.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<TState, IStatusReportable, Task<TResult>> asyncMethod);

        IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, IStatusReportable, Task<TResult>> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="state">The state.</param>
        /// <param name="progressFactory">The progress factory.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<IStatusReportable, Task<TResult>> asyncMethod);

        IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task<TResult>> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="T">The type of result value produced by the background operation.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        IQueuedBgProducer<T> Enqueue<T>(ActivityCode activity, [DisallowNull] Func<IStatusReportable, Task<T>> asyncMethod);

        IQueuedBgProducer<T> Enqueue<T>(ActivityCode activity, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task<T>> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="state">The state.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<TState, CancellationToken, Task<TResult>> asyncMethod);

        IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, CancellationToken, Task<TResult>> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="TState">The type of the asynchronous state value.</typeparam>
        /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="state">The state.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncMethod);

        IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncMethod);

        /// <summary>
        /// Enqueues a background operation that will produce a result value, starting it immediately if no other operation is already running.
        /// </summary>
        /// <typeparam name="T">The type of result value produced by the background operation.</typeparam>
        /// <param name="activity">The activity code.</param>
        /// <param name="asyncMethod">The delegate that refers to an asynchronous method.</param>
        /// <returns>A <see cref="IQueuedBgProducer{TState, TResult}"/> object representing the background job.</returns>
        IQueuedBgProducer<T> Enqueue<T>(ActivityCode activity, [DisallowNull] Func<CancellationToken, Task<T>> asyncMethod);

        IQueuedBgProducer<T> Enqueue<T>(ActivityCode activity, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task<T>> asyncMethod);
    }
}
