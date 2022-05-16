using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Base class for objects that can start asynchronous activities.
    /// </summary>
    /// <seealso cref="IAsyncActivityProvider" />
    /// <remarks>Implementing classes should call <see cref="OnStarting(IAsyncActivity)"/> before starting an asynchonous activity
    /// and <see cref="OnCompleted(LinkedListNode{IAsyncActivity})"/> after the activity has completed, including canceled and faulted activities.</remarks>
    public abstract partial class AsyncActivityProvider : IAsyncActivityProvider
    {
        private readonly LinkedList<IAsyncActivity> _activities = new();

        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the thread synchronization lock object.
        /// </summary>
        /// <value>The thread synchronization lock object.</value>
        protected internal object SyncRoot { get; } = new();

        /// <summary>
        /// Gets the observable source for activity start events.
        /// </summary>
        /// <value>The <see cref="Observable{IAsyncActivity}.Source"/> for pushing activity start events.</value>
        internal Observable<IAsyncActivity>.Source ActivityStartedSource { get; } = new();

        /// <summary>
        /// Gets the provider for activity start notifications.
        /// </summary>
        /// <value>The provider that can be used to subscribe for activity start notifications.</value>
        public IObservable<IAsyncActivity> ActivityStartedObservable => ActivityStartedSource.Observable;

        /// <summary>
        /// Gets the number of activities that this provider is running.
        /// </summary>
        /// <value>The count of <see cref="IAsyncActivity"/> objects representing activities that this provider is runnning.</value>
        public int Count => _activities.Count;

        /// <summary>
        /// Determines whether this provider has no running activities.
        /// </summary>
        /// <returns><see langword="true"/> if this provider has no running activities; otherwise, <see langword="false"/>.</returns>
        protected bool IsEmpty() => _activities.Last is null;

        /// <summary>
        /// Gets the unique identifier of the parent activity.
        /// </summary>
        /// <value>The <see cref="Guid" /> value that is unique to the parent activity or <see langword="null" /> if there is no parent activity.</value>
        protected Guid? ParentActivityId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncActivityProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger for the progress object.</param>
        /// <param name="parentActivityId">The parent activity identifier or <see langword="null"/> if there is no parent activity.</param>
        protected AsyncActivityProvider([DisallowNull] ILogger logger, Guid? parentActivityId) => (Logger, ParentActivityId) = (logger, parentActivityId);

        /// <summary>
        /// Adds an <see cref="IAsyncActivity"/> that is about to start.
        /// </summary>
        /// <param name="asyncActivity">The asynchronous activity that is starting.</param>
        /// <returns>The <see cref="LinkedListNode{IAsyncActivity}"/> that was appended.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncActivity"/> is <see langword="null"/>.</exception>
        /// <remarks>This appends the activity to the underlying list and should only be called when the current thread has an exclusive <see cref="Monitor"/> lock
        /// on <see cref="SyncRoot"/>.</remarks>
        protected virtual LinkedListNode<IAsyncActivity> OnStarting([DisallowNull] IAsyncActivity asyncActivity)
        {
            if (asyncActivity is null) throw new ArgumentNullException(nameof(asyncActivity));
            Logger.LogDebug("Adding activity {ActivityId} to backing list; ParentActivityId={ParentActivityId}; ShortDescription={ShortDescription}", asyncActivity.ActivityId, asyncActivity.ParentActivityId, asyncActivity.ShortDescription);
            return _activities.AddLast(asyncActivity);
        }

        /// <summary>
        /// Notifies this provider than an <see cref="IAsyncActivity"/> has been completed.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{IAsyncActivity}"/> to remove which references the <see cref="IAsyncActivity"/> that ran to completion, faulted,
        /// or was canceled.</param>
        /// <remarks>This obtains an exclusive <see cref="Monitor"/> lock on <see cref="SyncRoot"/> and removes the specified <paramref name="node"/> from the underlying
        /// list.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        protected virtual void OnCompleted([DisallowNull] LinkedListNode<IAsyncActivity> node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Logger.LogDebug("Removing activity {ActivityId} from backing list; ShortDescription={ShortDescription}", node.Value?.ActivityId, node.Value?.ShortDescription);
            Monitor.Enter(SyncRoot);
            try { _activities.Remove(node); }
            finally { Monitor.Exit(SyncRoot); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the running activities started by this provider.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the running <see cref="IAsyncActivity"/> objects started by this provider.</returns>
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
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncAction<IActivityEvent> InvokeAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
        {
            Logger.LogDebug(@"Invoking async action {activityDescription}
initialStatusMessage={initialStatusMessage}
ParentActivityId={ParentActivityId}", activityDescription, initialStatusMessage, ParentActivityId);

            //.ContinueWith(task => activity.SetCompleted(task, node));
            return AsyncAction.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);
        }

        /// <summary>
        /// Invokes a method that asynchronously produces a result value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="IAsyncFunc{IOperationEvent, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncFunc<IActivityEvent, TResult> InvokeAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            Logger.LogDebug(@"Invoking async function {activityDescription}
initialStatusMessage={initialStatusMessage}
ParentActivityId={ParentActivityId}", activityDescription, initialStatusMessage, ParentActivityId);
            return AsyncFunc<TResult>.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);
        }

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
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncAction<IActivityEvent<TState>, TState> InvokeAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            Logger.LogDebug(@"Invoking async action {activityDescription}
initialStatusMessage={initialStatusMessage}
ParentActivityId={ParentActivityId}
state={state}", activityDescription, initialStatusMessage, ParentActivityId, state);
            return AsyncAction<TState>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);
        }

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
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncFunc<IActivityEvent<TState>, TState, TResult> InvokeAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            Logger.LogDebug(@"Invoking async function {activityDescription}
initialStatusMessage={initialStatusMessage}
ParentActivityId={ParentActivityId}
state={state}", activityDescription, initialStatusMessage, ParentActivityId, state);
            return AsyncFunc<TState, TResult>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);
        }

        /// <summary>
        /// Invokes an asynchronous method, tracking the execution start and duration.
        /// </summary>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="ITimedAsyncAction{ITimedOperationEvent}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncAction<ITimedActivityEvent> InvokeTimedAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
        {
            Logger.LogDebug(@"Invoking timed async action {activityDescription}
initialStatusMessage={initialStatusMessage}
ParentActivityId={ParentActivityId}", activityDescription, initialStatusMessage, ParentActivityId);
            return TimedAsyncAction.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);
        }

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{ITimedOperationEvent, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncFunc<ITimedActivityEvent, TResult> InvokeTimedAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            Logger.LogDebug(@"Invoking timed async function {activityDescription}
initialStatusMessage={initialStatusMessage}
ParentActivityId={ParentActivityId}", activityDescription, initialStatusMessage, ParentActivityId);
            return TimedAsyncFunc<TResult>.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);
        }

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
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncAction<ITimedActivityEvent<TState>, TState> InvokeTimedAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            Logger.LogDebug(@"Invoking timed async action {activityDescription}
initialStatusMessage={initialStatusMessage}
ParentActivityId={ParentActivityId}
state={state}", activityDescription, initialStatusMessage, ParentActivityId, state);
            return TimedAsyncAction<TState>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);
        }

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, associating the function with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous function.</typeparam>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous function.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{ITimedOperationEvent{TState}, TState, TResult}" /> object that can be used to monitor and/or cancel the asynchronous
        /// function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncFunc<ITimedActivityEvent<TState>, TState, TResult> InvokeTimedAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            TState state, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            Logger.LogDebug(@"Invoking timed async function {activityDescription}
initialStatusMessage={initialStatusMessage}
ParentActivityId={ParentActivityId}
state={state}", activityDescription, initialStatusMessage, ParentActivityId, state);
            return TimedAsyncFunc<TState, TResult>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);
        }

        /// <summary>
        /// Notifies this activity source that an observer is to receive activity start notifications, providing a list of existing activities.
        /// </summary>
        /// <param name="observer">The object that is to receive activity start notifications.</param>
        /// <param name="onObserving">The callback method that provides a list of existing activities immediately before the observer is registered to receive activity start
        /// notifications.</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications before this has finished sending them.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="onObserving"/> is <see langword="null"/>.</exception>
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
