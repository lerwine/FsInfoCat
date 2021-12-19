using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Provider for starting asynchronous activities.
    /// </summary>
    public interface IAsyncActivityProvider : IReadOnlyCollection<IAsyncAction>
    {
        /// <summary>
        /// Starts an asynchronous activity that does not produce any result value.
        /// </summary>
        /// <typeparam name="TEvent">The type of the progress operation event.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="eventFactory">The progress event factory object.</param>
        /// <returns>An <see cref="IAsyncAction"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        IAsyncAction InvokeAsync<TEvent, TProgressEvent, TFinalEvent>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TEvent>, Task> asyncMethodDelegate,
            IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, IOperationInfo> eventFactory)
            where TEvent : IOperationEvent
            where TProgressEvent : TEvent, IActivityProgressEvent
            where TFinalEvent : TEvent, IOperationEvent;

        /// <summary>
        /// Starts an asynchronous activity that produces a result value.
        /// </summary>
        /// <typeparam name="TOperationEvent">The type of the progress operation event.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous activity.</typeparam>
        /// <typeparam name="TFinalEvent">The type of the final progress event.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="eventFactory">The progress event factory object.</param>
        /// <returns>An <see cref="IAsyncFunc{TResult}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        IAsyncFunc<TResult> InvokeAsync<TEvent, TOperationEvent, TResult, TFinalEvent>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TOperationEvent>, Task<TResult>> asyncMethodDelegate,
            IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, IOperationInfo> eventFactory)
            where TEvent : IOperationEvent
            where TOperationEvent : TEvent, IOperationEvent
            where TFinalEvent : TEvent, IActivityResultEvent<TResult>;

        /// <summary>
        /// Starts a timed asynchronous activity that does not produce any result value.
        /// </summary>
        /// <typeparam name="TEvent">The type of the progress operation event.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="eventFactory">The progress event factory object.</param>
        /// <returns>An <see cref="ITimedAsyncAction"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        ITimedAsyncAction InvokeAsync<TEvent, TProgressEvent, TFinalEvent>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TEvent>, Task> asyncMethodDelegate,
            IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, ITimedOperationInfo> eventFactory)
            where TEvent : ITimedOperationEvent
            where TProgressEvent : TEvent, ITimedActivityProgressEvent
            where TFinalEvent : TEvent, ITimedOperationEvent;

        /// <summary>
        /// Starts a timed asynchronous activity that produces a result value.
        /// </summary>
        /// <typeparam name="TOperationEvent">The type of the progress operation event.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous activity.</typeparam>
        /// <typeparam name="TFinalEvent">The type of the final progress event.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="eventFactory">The progress event factory object.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{TResult}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        ITimedAsyncFunc<TResult> InvokeAsync<TEvent, TOperationEvent, TResult, TFinalEvent>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TOperationEvent>, Task<TResult>> asyncMethodDelegate,
            IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, ITimedOperationInfo> eventFactory)
            where TEvent : ITimedOperationEvent
            where TOperationEvent : TEvent, ITimedOperationEvent
            where TFinalEvent : TEvent, ITimedActivityResultEvent<TResult>;

        /// <summary>
        /// Starts an asynchronous activity that does not produce any result value.
        /// </summary>
        /// <typeparam name="TEvent">The type of the progress operation event.</typeparam>
        /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="eventFactory">The progress event factory object.</param>
        /// <param name="asyncState">THe user-defined value to associate with the activity.</param>
        /// <returns>An <see cref="IAsyncAction{TState}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        IAsyncAction<TState> InvokeAsync<TEvent, TProgressEvent, TFinalEvent, TState>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TState, TEvent>, Task> asyncMethodDelegate,
            IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, IOperationInfo<TState>> eventFactory, TState asyncState)
            where TEvent : IOperationEvent<TState>
            where TProgressEvent : TEvent, IActivityProgressEvent<TState>
            where TFinalEvent : TEvent, IOperationEvent<TState>;

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TOperationEvent">The type of the progress operation event.</typeparam>
        /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous activity.</typeparam>
        /// <typeparam name="TFinalEvent">The type of the final progress event.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="eventFactory">The progress event factory object.</param>
        /// <param name="asyncState">THe user-defined value to associate with the activity.</param>
        /// <returns>An <see cref="IAsyncFunc{TState, TResult}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        IAsyncFunc<TState, TResult> InvokeAsync<TEvent, TOperationEvent, TState, TResult, TFinalEvent>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TState, TOperationEvent>, Task<TResult>> asyncMethodDelegate,
            IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, IOperationInfo<TState>> eventFactory, TState asyncState)
            where TEvent : IOperationEvent<TState>
            where TOperationEvent : TEvent, IOperationEvent<TState>
            where TFinalEvent : TEvent, IActivityResultEvent<TState, TResult>;

        /// <summary>
        /// Starts a timed asynchronous activity that does not produce any result value.
        /// </summary>
        /// <typeparam name="TEvent">The type of the progress operation event.</typeparam>
        /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="eventFactory">The progress event factory object.</param>
        /// <param name="asyncState">THe user-defined value to associate with the activity.</param>
        /// <returns>An <see cref="ITimedAsyncAction{TState}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        ITimedAsyncAction<TState> InvokeAsync<TEvent, TProgressEvent, TFinalEvent, TState>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TState, TEvent>, Task> asyncMethodDelegate,
            IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, IOperationInfo<TState>> eventFactory, TState asyncState)
            where TEvent : ITimedOperationEvent<TState>
            where TProgressEvent : TEvent, ITimedActivityProgressEvent<TState>
            where TFinalEvent : TEvent, ITimedOperationEvent<TState>;

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TOperationEvent">The type of the progress operation event.</typeparam>
        /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous activity.</typeparam>
        /// <typeparam name="TFinalEvent">The type of the final progress event.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="eventFactory">The progress event factory object.</param>
        /// <param name="asyncState">THe user-defined value to associate with the activity.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{TState, TResult}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="eventFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        ITimedAsyncFunc<TState, TResult> InvokeAsync<TEvent, TOperationEvent, TState, TResult, TFinalEvent>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TState, TOperationEvent>, Task<TResult>> asyncMethodDelegate,
            IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, IOperationInfo<TState>> eventFactory, TState asyncState)
            where TEvent : ITimedOperationEvent<TState>
            where TOperationEvent : TEvent, ITimedOperationEvent<TState>
            where TFinalEvent : TEvent, ITimedActivityResultEvent<TState, TResult>;

        /// <summary>
        /// Starts an asynchronous activity that does not produce any result value.
        /// </summary>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <returns>An <see cref="IAsyncAction"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        IAsyncAction InvokeAsync(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<IOperationEvent>, Task> asyncMethodDelegate);

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous activity.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <returns>An <see cref="IAsyncFunc{TResult}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        IAsyncFunc<TResult> InvokeAsync<TResult>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<IOperationEvent>, Task<TResult>> asyncMethodDelegate);

        /// <summary>
        /// Starts a timed asynchronous activity that does not produce any result value.
        /// </summary>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <returns>An <see cref="ITimedAsyncAction"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        ITimedAsyncAction InvokeAsync(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<ITimedOperationEvent>, Task> asyncMethodDelegate);

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous activity.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{TResult}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        ITimedAsyncFunc<TResult> InvokeAsync<TResult>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<ITimedOperationEvent>, Task<TResult>> asyncMethodDelegate);

        /// <summary>
        /// Starts an asynchronous activity that does not produce any result value.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="asyncState">THe user-defined value to associate with the activity.</param>
        /// <returns>An <see cref="IAsyncAction{TState}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        IAsyncAction<TState> InvokeAsync<TState>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TState, IOperationEvent<TState>>, Task> asyncMethodDelegate, TState asyncState);

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous activity.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="asyncState">THe user-defined value to associate with the activity.</param>
        /// <returns>An <see cref="IAsyncFunc{TState, TResult}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        IAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TState, IOperationEvent<TState>>, Task<TResult>> asyncMethodDelegate, TState asyncState);

        /// <summary>
        /// Starts a timed asynchronous activity that does not produce any result value.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="asyncState">THe user-defined value to associate with the activity.</param>
        /// <returns>An <see cref="ITimedAsyncAction{TState}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        ITimedAsyncAction<TState> InvokeAsync<TState>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TState, ITimedOperationEvent<TState>>, Task> asyncMethodDelegate, TState asyncState);

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous activity.</typeparam>
        /// <param name="activity">The description of the activity.</param>
        /// <param name="initialStatusDescription">The initial status description.</param>
        /// <param name="asyncMethodDelegate">The delegate that refers to the asynchronous method that implements the activity.</param>
        /// <param name="asyncState">THe user-defined value to associate with the activity.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{TState, TResult}"/> that represents the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="initialStatusDescription"/> is <ee langword="null"/>, <see cref="string.Empty"/> or contains only whitespace characters.</exception>
        ITimedAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TState, ITimedOperationEvent<TState>>, Task<TResult>> asyncMethodDelegate, TState asyncState);
    }
}
