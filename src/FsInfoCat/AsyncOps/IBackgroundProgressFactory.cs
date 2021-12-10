using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents an object that can be used to start new background operations.
    /// </summary>
    public interface IBackgroundProgressFactory
    {
        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state,
        //    params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
        //    Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state,
        //    params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
        //    Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state,
        //    params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
        //    Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
        //    Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
        //    Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
        //    Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
        //    Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
        //    Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation.
        ///// </summary>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
        //    Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation.
        ///// </summary>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
        //    Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
        //    Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
        //    Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation.
        ///// </summary>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
        //    Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation.
        ///// </summary>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
        //    Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
        //    string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
        //    string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundOperation{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
        //    string statusDescription, TState state, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundOperation{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
        //    string statusDescription, TState state, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
        //    string activity, string statusDescription, TState state);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
        //    string activity, string statusDescription, TState state);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
        //    string statusDescription, TState state);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="state">The object to associate with the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
        //    string statusDescription, TState state);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity,
        //    string statusDescription, params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription,
        //    params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation.
        ///// </summary>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription,
        //    params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation.
        ///// </summary>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        ///// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription,
        //    params CancellationToken[] tokens);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity,
        //    string statusDescription);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation to produce a result value.
        ///// </summary>
        ///// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription);

        ///// <summary>
        ///// Starts a timed asynchronous asynchronous operation.
        ///// </summary>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription);

        ///// <summary>
        ///// Starts an asynchronous asynchronous operation.
        ///// </summary>
        ///// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        ///// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        ///// <param name="statusDescription">The initial status description for the background operation.</param>
        ///// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        ///// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/> or contains only white-space characters.</exception>
        ///// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        //IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription);

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationCompletedEvent{TState}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation<TState> InvokeAsync<TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens);

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationCompletedEvent{TState}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation<TState> InvokeAsync<TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state);

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation<TState> InvokeAsync<TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens);

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation<TState> InvokeAsync<TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state);

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationCompletedEvent"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens);

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationCompletedEvent"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription);

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, params CancellationToken[] tokens);

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription);

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="IBackgroundOperationCompletedEvent{TState}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation<TState> InvokeAsync<TState>([DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens);

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="IBackgroundOperationCompletedEvent{TState}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation<TState> InvokeAsync<TState>([DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state);

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation<TState> InvokeAsync<TState>([DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens);

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation<TState> InvokeAsync<TState>([DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, TState state);

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="IBackgroundOperationCompletedEvent"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens);

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="IBackgroundOperationCompletedEvent"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription);

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens);

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription);

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens);

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state);

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens);

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, TState state);

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TResult}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens);

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="ITimedBackgroundOperationResultEvent{TResult}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription);

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens);

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription);

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="IBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens);

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="IBackgroundOperationResultEvent{TState, TResult}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state);

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens);

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state);

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="IBackgroundOperationResultEvent{TResult}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens);

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="onCompleted">Gets called to create the final <see cref="IBackgroundOperationResultEvent{TResult}"/> notification object.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription);

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, params CancellationToken[] tokens);

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription);
    }
}
