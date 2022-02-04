using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract class TimedAsyncAction<TBaseEvent, TOperationEvent, TResultEvent> : TimedAsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task>, ITimedAsyncAction<TBaseEvent>
            where TBaseEvent : ITimedActivityEvent
            where TOperationEvent : TBaseEvent, ITimedOperationEvent
            where TResultEvent : TBaseEvent, ITimedActivityCompletedEvent
        {
            private readonly TaskCompletionSource _completionSource;

            public override Task Task => _completionSource.Task;

            protected TimedAsyncAction([DisallowNull] AsyncActivityProvider provider, [DisallowNull] TaskCompletionSource completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(provider, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            protected abstract TResultEvent CreateRanToCompletionEvent();

            protected void SetCompleted(Task task, LinkedListNode<IAsyncActivity> node)
            {
                try { StopTimer(); }
                finally
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
    }

    internal sealed partial class TimedAsyncAction : AsyncActivityProvider.TimedAsyncAction<ITimedActivityEvent, ITimedOperationEvent, ITimedActivityCompletedEvent>, ITimedAsyncAction<ITimedActivityEvent>
    {
        internal static TimedAsyncAction Start([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            TimedAsyncAction activity;
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

        protected override ITimedActivityEvent CreateInitialEvent() => new TimedActivityEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override ITimedActivityCompletedEvent CreateRanToCompletionEvent() => new TimedActivityCompletedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Exception = null,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override ITimedActivityCompletedEvent CreateCanceledEvent() => new TimedOperationTerminatedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override ITimedActivityCompletedEvent CreateFaultedEvent(Exception error) => new TimedOperationTerminatedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Error
        };

        private TimedAsyncAction([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(), activityDescription, initialStatusMessage) { }
    }

    internal sealed partial class TimedAsyncAction<TState> : AsyncActivityProvider.TimedAsyncAction<ITimedActivityEvent<TState>, ITimedOperationEvent<TState>, ITimedActivityCompletedEvent<TState>>, ITimedAsyncAction<ITimedActivityEvent<TState>, TState>
    {
        public TState AsyncState { get; }

        internal static TimedAsyncAction<TState> Start(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription,
            [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            Activities.TimedAsyncAction<TState> activity;
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

        protected override ITimedActivityEvent<TState> CreateInitialEvent() => new TimedActivityEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override ITimedActivityCompletedEvent<TState> CreateRanToCompletionEvent() => new TimedActivityCompletedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Exception = null,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override ITimedActivityCompletedEvent<TState> CreateCanceledEvent() => new TimedOperationTerminatedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override ITimedActivityCompletedEvent<TState> CreateFaultedEvent(Exception error) => new TimedOperationTerminatedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Error
        };

        private TimedAsyncAction(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(state), activityDescription, initialStatusMessage) => AsyncState = state;
    }
}
