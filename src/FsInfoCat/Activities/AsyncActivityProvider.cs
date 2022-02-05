using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    public abstract partial class AsyncActivityProvider : IAsyncActivityProvider
    {
        protected internal object SyncRoot { get; } = new();

        private readonly LinkedList<IAsyncActivity> _activities = new();

        internal Observable<IAsyncActivity>.Source ActivityStartedSource { get; } = new();

        public IObservable<IAsyncActivity> ActivityStartedObservable => ActivityStartedSource.Observable;

        public int Count => _activities.Count;

        protected bool IsEmpty() => _activities.Last is null;

        protected Guid? ParentActivityId { get; }

        protected AsyncActivityProvider(Guid? parentActivityId) => ParentActivityId = parentActivityId;

        protected virtual LinkedListNode<IAsyncActivity> OnStarting(IAsyncActivity asyncActivity) => _activities.AddLast(asyncActivity);

        protected virtual void OnCompleted(LinkedListNode<IAsyncActivity> node)
        {
            Monitor.Enter(SyncRoot);
            try { _activities.Remove(node); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public IEnumerator<IAsyncActivity> GetEnumerator() => _activities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_activities).GetEnumerator();

        /// <summary>
        /// Invokes an asynchronous method.
        /// </summary>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="IAsyncAction{IOperationEvent}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncAction<IActivityEvent> InvokeAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate) => AsyncAction.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="IAsyncFunc{IOperationEvent, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncFunc<IActivityEvent, TResult> InvokeAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => AsyncFunc<TResult>.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, associating it with a user-specified value.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous activity.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="IAsyncAction{IOperationEvent{TState}, TState}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncAction<IActivityEvent<TState>, TState> InvokeAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate) => AsyncAction<TState>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, associating the function with a user-specified value.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous function.</typeparam>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous function.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="IAsyncFunc{IOperationEvent{TState}, TState, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncFunc<IActivityEvent<TState>, TState, TResult> InvokeAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            => AsyncFunc<TState, TResult>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, tracking the execution start and duration.
        /// </summary>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="ITimedAsyncAction{ITimedOperationEvent}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncAction<ITimedActivityEvent> InvokeTimedAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate) => TimedAsyncAction.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{ITimedOperationEvent, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncFunc<ITimedActivityEvent, TResult> InvokeTimedAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => TimedAsyncFunc<TResult>.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, associating it with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous activity.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="ITimedAsyncAction{ITimedOperationEvent{TState}, TState}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncAction<ITimedActivityEvent<TState>, TState> InvokeTimedAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
            => Activities.TimedAsyncAction<TState>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, associating the function with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous function.</typeparam>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous function.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{ITimedOperationEvent{TState}, TState, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncFunc<ITimedActivityEvent<TState>, TState, TResult> InvokeTimedAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            TState state, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            => Activities.TimedAsyncFunc<TState, TResult>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IDisposable SubscribeChildActivityStart([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving)
        {
            if (observer is null)
                throw new ArgumentNullException(nameof(observer));
            if (onObserving is null)
                throw new ArgumentNullException(nameof(onObserving));
            Monitor.Enter(SyncRoot);
            try
            {
                onObserving(_activities.ToArray());
                return ActivityStartedObservable.Subscribe(observer);
            }
            finally { Monitor.Exit(SyncRoot); }
        }
    }
}
