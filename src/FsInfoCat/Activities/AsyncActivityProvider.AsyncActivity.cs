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
        internal abstract partial class AsyncActivity<TEvent, TTask> : AsyncActivityProvider, IAsyncActivity, IObservable<TEvent>
            where TTask : Task
            where TEvent : IOperationEvent
        {
            private readonly AsyncActivityProvider _provider;

            public abstract TTask Task { get; }

            Task IAsyncActivity.Task => Task;

            public CancellationTokenSource TokenSource { get; } = new();

            public ActivityStatus StatusValue { get; protected set; } = ActivityStatus.WaitingToRun;

            public Guid ActivityId { get; } = Guid.NewGuid();

            Guid? IActivityInfo.ParentActivityId => ParentActivityId;

            public string ShortDescription { get; }

            public string StatusMessage { get; private set; }

            public string CurrentOperation { get; private set; } = "";

            public int PercentComplete { get; private set; } = -1;

            protected AsyncActivity([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base((provider as IAsyncActivity)?.ActivityId)
            {
                _provider = provider ?? throw new ArgumentNullException(nameof(provider));
                if (activityDescription is null || (ShortDescription = activityDescription.Trim()).Length == 0)
                    throw new ArgumentException($"'{nameof(activityDescription)}' cannot be null or whitespace.", nameof(activityDescription));
                if (initialStatusMessage is null || (StatusMessage = initialStatusMessage.Trim()).Length == 0)
                    throw new ArgumentException($"'{nameof(initialStatusMessage)}' cannot be null or whitespace.", nameof(initialStatusMessage));
            }

            public IDisposable Subscribe(IObserver<TEvent> observer)
            {
                throw new NotImplementedException();
            }

            protected LinkedListNode<IAsyncActivity> OnStarting()
            {
                LinkedListNode<IAsyncActivity> node = _provider.OnStarting(this);
                return node;
            }

            protected virtual void OnStarted()
            {
                StatusValue = ActivityStatus.Running;
                // TODO: Add to collection
            }

            protected virtual void OnCanceled(LinkedListNode<IAsyncActivity> node)
            {
                _provider.OnCompleted(node);
            }

            protected virtual void OnRanToCompletion(LinkedListNode<IAsyncActivity> node)
            {
                _provider.OnCompleted(node);
            }

            protected virtual void OnFaulted(LinkedListNode<IAsyncActivity> node, Exception exception)
            {
                _provider.OnCompleted(node);
            }
        }
    }
}
