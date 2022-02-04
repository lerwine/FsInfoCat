using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a provider that can be used to start asynchronous activities.
    /// </summary>
    /// <seealso cref="IAsyncActivitySource" />
    public interface IAsyncActivityProvider : IAsyncActivitySource
    {
        /// <summary>
        /// Invokes an asynchronous method.
        /// </summary>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="IAsyncAction{IOperationEvent}"/> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        IAsyncAction<IActivityEvent> InvokeAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="IAsyncFunc{IOperationEvent, TResult}"/> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        IAsyncFunc<IActivityEvent, TResult> InvokeAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, tracking the execution start and duration.
        /// </summary>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="ITimedAsyncAction{ITimedOperationEvent}"/> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        ITimedAsyncAction<ITimedActivityEvent> InvokeTimedAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{ITimedOperationEvent, TResult}"/> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        ITimedAsyncFunc<ITimedActivityEvent, TResult> InvokeTimedAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, associating it with a user-specified value.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous activity.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="IAsyncAction{IOperationEvent{TState}, TState}"/> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        IAsyncAction<IActivityEvent<TState>, TState> InvokeAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, associating the function with a user-specified value.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous function.</typeparam>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous function.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="IAsyncFunc{IOperationEvent{TState}, TState, TResult}"/> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        IAsyncFunc<IActivityEvent<TState>, TState, TResult> InvokeAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, associating it with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous activity.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="ITimedAsyncAction{ITimedOperationEvent{TState}, TState}"/> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        ITimedAsyncAction<ITimedActivityEvent<TState>, TState> InvokeTimedAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, associating the function with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous function.</typeparam>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous function.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{ITimedOperationEvent{TState}, TState, TResult}"/> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        ITimedAsyncFunc<ITimedActivityEvent<TState>, TState, TResult> InvokeTimedAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);
    }
}
