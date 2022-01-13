using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    public abstract partial class AsyncActivityProvider : IAsyncActivityProvider
    {
        protected object SyncRoot { get; } = new();

        private readonly LinkedList<IAsyncActivity> _activities = new();

        internal Observable<IAsyncActivity>.Source StateChangeSource => throw new NotImplementedException();

        public IObservable<IAsyncActivity> StateChangeObservable => StateChangeSource.Observable;

        public int Count => _activities.Count;

        protected bool IsEmpty() => _activities.Last is null;

        protected Guid? ParentActivityId { get; }

        protected AsyncActivityProvider(Guid? parentActivityId)
        {
            ParentActivityId = parentActivityId;
        }

        protected virtual LinkedListNode<IAsyncActivity> OnStarting(IAsyncActivity asyncActivity)
        {
            LinkedListNode<IAsyncActivity> node = _activities.AddLast(asyncActivity);
            return node;
        }

        protected virtual void OnCompleted(LinkedListNode<IAsyncActivity> node)
        {
            _activities.Remove(node);
        }

        public IEnumerator<IAsyncActivity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IAsyncAction<IOperationEvent> InvokeAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate) => AsyncAction.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IAsyncFunc<IOperationEvent, TResult> InvokeAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => AsyncFunc<TResult>.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IAsyncAction<IOperationEvent<TState>, TState> InvokeAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate) => Activities.AsyncAction<TState>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IAsyncFunc<IOperationEvent<TState>, TState, TResult> InvokeAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            => Activities.AsyncFunc<TState, TResult>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncAction<ITimedOperationEvent> InvokeTimedAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate) => TimedAsyncAction.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncFunc<ITimedOperationEvent, TResult> InvokeTimedAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => TimedAsyncFunc<TResult>.Start(this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncAction<ITimedOperationEvent<TState>, TState> InvokeTimedAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
            => Activities.TimedAsyncAction<TState>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncFunc<ITimedOperationEvent<TState>, TState, TResult> InvokeTimedAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            TState state, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            => Activities.TimedAsyncFunc<TState, TResult>.Start(state, this, activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IDisposable SubscribeStateChange([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
