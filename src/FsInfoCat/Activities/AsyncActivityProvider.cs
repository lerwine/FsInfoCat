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

        internal Observable<IAsyncActivity>.Source StateChangeSource { get; } = new();

        public IObservable<IAsyncActivity> StateChangeObservable => StateChangeSource.Observable;

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

        public IAsyncAction<IActivityEvent> InvokeAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate) => AsyncAction.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IAsyncFunc<IActivityEvent, TResult> InvokeAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => AsyncFunc<TResult>.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IAsyncAction<IActivityEvent<TState>, TState> InvokeAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate) => Activities.AsyncAction<TState>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IAsyncFunc<IActivityEvent<TState>, TState, TResult> InvokeAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            => Activities.AsyncFunc<TState, TResult>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncAction<ITimedActivityEvent> InvokeTimedAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate) => TimedAsyncAction.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncFunc<ITimedActivityEvent, TResult> InvokeTimedAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => TimedAsyncFunc<TResult>.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncAction<ITimedActivityEvent<TState>, TState> InvokeTimedAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
            => Activities.TimedAsyncAction<TState>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncFunc<ITimedActivityEvent<TState>, TState, TResult> InvokeTimedAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            TState state, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            => Activities.TimedAsyncFunc<TState, TResult>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IDisposable SubscribeStateChange([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving)
        {
            if (observer is null)
                throw new ArgumentNullException(nameof(observer));
            if (onObserving is null)
                throw new ArgumentNullException(nameof(onObserving));
            Monitor.Enter(SyncRoot);
            try
            {
                onObserving(_activities.ToArray());
                return StateChangeObservable.Subscribe(observer);
            }
            finally { Monitor.Exit(SyncRoot); }
        }
    }
}
