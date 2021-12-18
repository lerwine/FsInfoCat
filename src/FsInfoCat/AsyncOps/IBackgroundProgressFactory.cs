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
        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object to indicate the asynchronous operation ran to completion.</typeparam>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation<TState> InvokeAsync<TEvent, TResultEvent, TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent<TState>;

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object to indicate the asynchronous operation ran to completion.</typeparam>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation<TState> InvokeAsync<TEvent, TResultEvent, TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent<TState>;

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
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object to indicate the asynchronous operation ran to completion.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation InvokeAsync<TEvent, TResultEvent>([DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent;

        /// <summary>
        /// Starts a timed asynchronous operation.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object to indicate the asynchronous operation ran to completion.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="ITimedBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundOperation InvokeAsync<TEvent, TResultEvent>([DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent;

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
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object to indicate the asynchronous operation ran to completion.</typeparam>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation<TState> InvokeAsync<TEvent, TResultEvent, TState>([DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent<TState>;

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object to indicate the asynchronous operation ran to completion.</typeparam>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation{TState}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation<TState> InvokeAsync<TEvent, TResultEvent, TState>([DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent<TState>;

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
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object to indicate the asynchronous operation ran to completion.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation InvokeAsync<TEvent, TResultEvent>([DisallowNull] Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent;

        /// <summary>
        /// Starts an asynchronous operation.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object to indicate the asynchronous operation ran to completion.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="IBackgroundOperation"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundOperation InvokeAsync<TEvent, TResultEvent>([DisallowNull] Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent;

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
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object that contains the result value.</typeparam>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TEvent, TResultEvent, TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TState, TResult>;

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object that contains the result value.</typeparam>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TEvent, TResultEvent, TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TState, TResult>;

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
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object that contains the result value.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TResult> InvokeAsync<TEvent, TResultEvent, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TResult>;

        /// <summary>
        /// Starts a timed asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object that contains the result value.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="ITimedBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        ITimedBackgroundFunc<TResult> InvokeAsync<TEvent, TResultEvent, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TResult>;

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
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object that contains the result value.</typeparam>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TState, TResult> InvokeAsync<TEvent, TResultEvent, TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TState, TResult>;

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object that contains the result value.</typeparam>
        /// <typeparam name="TState">The type of object to associate with the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="state">The object to associate with the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TState, TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TState, TResult> InvokeAsync<TEvent, TResultEvent, TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TState, TResult>;

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
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object that contains the result value.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TResult> InvokeAsync<TEvent, TResultEvent, TResult>([DisallowNull] Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TResult>;

        /// <summary>
        /// Starts an asynchronous operation to produce a result value.
        /// <summary>
        /// <typeparam name="TEvent">The base type of event notification objects.</typeparam>
        /// <typeparam name="TResultEvent">The type of event notification object that contains the result value.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the asynchronous operation.</typeparam>
        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>
        /// <param name="eventFactory">Object that creates <typeparamref name="TEvent"/> notification objects.</param>
        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>
        /// <param name="statusDescription">The initial status description for the background operation.</param>
        /// <returns>An <see cref="IBackgroundFunc{TResult}"/> object representing the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>
        /// or contains only white-space characters.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>
        IBackgroundFunc<TResult> InvokeAsync<TEvent, TResultEvent, TResult>([DisallowNull] Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TResult>;

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
