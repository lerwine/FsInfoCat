using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract class AsyncAction<TBaseEvent, TOperationEvent, TResultEvent> : AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task>, IAsyncAction<TBaseEvent>
            where TBaseEvent : IActivityEvent
            where TOperationEvent : TBaseEvent, IOperationEvent
            where TResultEvent : TBaseEvent, IActivityCompletedEvent
        {
            private readonly TaskCompletionSource _completionSource;

            public override Task Task => _completionSource.Task;

            protected AsyncAction([DisallowNull] AsyncActivityProvider provider, [DisallowNull] TaskCompletionSource completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(provider, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            protected abstract TResultEvent CreateRanToCompletionEvent();

            protected void SetCompleted(Task task, LinkedListNode<IAsyncActivity> node)
            {
                try
                {
                    if (task.IsCanceled)
                        try
                        {
                            StatusValue = ActivityStatus.Canceled;
                            EventSource.RaiseNext(CreateCanceledEvent());
                        }
                        finally
                        {
                            if (TokenSource.IsCancellationRequested)
                                _completionSource.SetCanceled(TokenSource.Token);
                            else
                                _completionSource.SetCanceled();
                        }
                    else if (task.IsFaulted)
                        try
                        {
                            StatusValue = ActivityStatus.Faulted;
                            EventSource.RaiseNext(CreateFaultedEvent(task.Exception));
                        }
                        finally
                        {
#pragma warning disable CS8604 // Possible null reference argument.
                            _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                        }
                    else
                        try
                        {
                            StatusValue = ActivityStatus.RanToCompletion;
                            EventSource.RaiseNext(CreateRanToCompletionEvent());
                        }
                        finally { _completionSource.SetResult(); }
                }
                finally { NotifyCompleted(node); }
            }
        }
    }

    internal sealed partial class AsyncAction : AsyncActivityProvider.AsyncAction<IActivityEvent, IOperationEvent, IActivityCompletedEvent>
    {
        internal static AsyncAction Start([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            AsyncAction activity;
            LinkedListNode<IAsyncActivity> node;
            Monitor.Enter((provider ?? throw new ArgumentNullException(nameof(provider))).SyncRoot);
            try
            {
                activity = new(provider, activityDescription, initialStatusMessage);
                node = activity.OnStarting();
            }
            finally { Monitor.Exit(provider.SyncRoot); }
            AsyncActionProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        protected override IActivityEvent CreateInitialEvent() => new ActivityEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityCompletedEvent CreateRanToCompletionEvent() => new ActivityCompletedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Exception = null,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityCompletedEvent CreateCanceledEvent() => new OperationTerminatedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override IActivityCompletedEvent CreateFaultedEvent(Exception error) => new OperationTerminatedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Error
        };

        private AsyncAction([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(), activityDescription, initialStatusMessage) { }
    }

    internal sealed partial class AsyncAction<TState> : AsyncActivityProvider.AsyncAction<IActivityEvent<TState>, IOperationEvent<TState>, IActivityCompletedEvent<TState>>, IAsyncAction<IActivityEvent<TState>, TState>
    {
        public TState AsyncState { get; }

        internal static Activities.AsyncAction<TState> Start(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription,
            [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            Activities.AsyncAction<TState> activity;
            LinkedListNode<IAsyncActivity> node;
            Monitor.Enter((provider ?? throw new ArgumentNullException(nameof(provider))).SyncRoot);
            try
            {
                activity = new(state, provider, activityDescription, initialStatusMessage);
                node = activity.OnStarting();
            }
            finally { Monitor.Exit(provider.SyncRoot); }
            AsyncActionProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        protected override IActivityEvent<TState> CreateInitialEvent() => new ActivityEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityCompletedEvent<TState> CreateRanToCompletionEvent() => new ActivityCompletedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Exception = null,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityCompletedEvent<TState> CreateCanceledEvent() => new OperationTerminatedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override IActivityCompletedEvent<TState> CreateFaultedEvent(Exception error) => new OperationTerminatedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Error
        };

        private AsyncAction(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(state), activityDescription, initialStatusMessage) => AsyncState = state;
    }
}
