using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract partial class AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask> : IAsyncActivity, IObservable<TBaseEvent>
            where TTask : Task
            where TBaseEvent : IActivityEvent
            where TOperationEvent : TBaseEvent, IOperationEvent
            where TResultEvent : TBaseEvent, IActivityCompletedEvent
        {
            private readonly AsyncActivityProvider _owner;

            public abstract TTask Task { get; }

            Task IAsyncActivity.Task => Task;

            public CancellationTokenSource TokenSource { get; } = new();

            public ActivityStatus StatusValue { get; protected set; } = ActivityStatus.WaitingToRun;

            public Guid ActivityId { get; } = Guid.NewGuid();

            public Guid? ParentActivityId => _owner.ParentActivityId;

            public string ShortDescription { get; }

            public string StatusMessage { get; private set; }

            public string CurrentOperation { get; private set; } = "";

            public int PercentComplete { get; private set; } = -1;

            protected Observable<TBaseEvent>.Source EventSource { get; } = new();

            public IObservable<IAsyncActivity> StateChangeObservable => _owner.StateChangeObservable;

            public int Count => _owner.Count;

            protected AsyncActivity([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            {
                _owner = owner ?? throw new ArgumentNullException(nameof(owner));
                if (activityDescription is null || (ShortDescription = activityDescription.Trim()).Length == 0)
                    throw new ArgumentException($"'{nameof(activityDescription)}' cannot be null or whitespace.", nameof(activityDescription));
                if (initialStatusMessage is null || (StatusMessage = initialStatusMessage.Trim()).Length == 0)
                    throw new ArgumentException($"'{nameof(initialStatusMessage)}' cannot be null or whitespace.", nameof(initialStatusMessage));
            }

            public IDisposable Subscribe(IObserver<TBaseEvent> observer) => EventSource.Observable.Subscribe(observer);

            protected LinkedListNode<IAsyncActivity> OnStarting()
            {
                LinkedListNode<IAsyncActivity> node = _owner.OnStarting(this);
                return node;
            }

            protected virtual void OnStarted()
            {
                StatusValue = ActivityStatus.Running;
                EventSource.RaiseNext(CreateInitialEvent());
            }

            protected abstract TBaseEvent CreateInitialEvent();

            protected abstract TResultEvent CreateCanceledEvent();

            protected abstract TResultEvent CreateFaultedEvent(Exception error);

            protected void NotifyCompleted(LinkedListNode<IAsyncActivity> node) => _owner.OnCompleted(node);

            public IDisposable SubscribeStateChange([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving) => _owner.SubscribeStateChange(observer, onObserving);

            public IEnumerator<IAsyncActivity> GetEnumerator() => _owner.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_owner).GetEnumerator();
        }
    }
}
