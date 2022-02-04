using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract class TimedAsyncFunc<TBaseEvent, TOperationEvent, TResultEvent, TResult> : TimedAsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task<TResult>>, ITimedAsyncFunc<TBaseEvent, TResult>
            where TBaseEvent : ITimedActivityEvent
            where TOperationEvent : TBaseEvent, ITimedOperationEvent
            where TResultEvent : TBaseEvent, ITimedActivityResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task<TResult> Task => _completionSource.Task;

            protected TimedAsyncFunc([DisallowNull] AsyncActivityProvider provider, [DisallowNull] TaskCompletionSource<TResult> completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(provider, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            protected abstract TResultEvent CreateRanToCompletionEvent(TResult result);

            protected void SetCompleted(Task<TResult> task, LinkedListNode<IAsyncActivity> node)
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
                                EventSource.RaiseNext(CreateRanToCompletionEvent(task.Result));
                            }
                            finally { _completionSource.SetResult(task.Result); }
                    }
                    finally { NotifyCompleted(node); }
                }
            }
        }
    }

    internal partial class TimedAsyncFunc<TResult> : AsyncActivityProvider.TimedAsyncFunc<ITimedActivityEvent, ITimedOperationEvent, ITimedActivityResultEvent<TResult>, TResult>
    {
        internal static TimedAsyncFunc<TResult> Start([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            TimedAsyncFunc<TResult> activity;
            LinkedListNode<IAsyncActivity> node;
            Monitor.Enter((provider ?? throw new ArgumentNullException(nameof(provider))).SyncRoot);
            try
            {
                activity = new(provider, activityDescription, initialStatusMessage);
                node = activity.OnStarting();
            }
            finally { Monitor.Exit(provider.SyncRoot); }
            AsyncFuncProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
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

        protected override ITimedActivityResultEvent<TResult> CreateRanToCompletionEvent(TResult result) => new TimedActivityResultEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Result = result,
            Exception = null,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override ITimedActivityResultEvent<TResult> CreateCanceledEvent() => new TimedResultOperationTerminatedEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = null,
            Started = Started,
            Duration = Duration,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override ITimedActivityResultEvent<TResult> CreateFaultedEvent(Exception error) => new TimedResultOperationTerminatedEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            Started = Started,
            Duration = Duration,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Error
        };

        private TimedAsyncFunc([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(), activityDescription, initialStatusMessage) { }
    }

    internal partial class TimedAsyncFunc<TState, TResult> : AsyncActivityProvider.TimedAsyncFunc<ITimedActivityEvent<TState>, ITimedOperationEvent<TState>, ITimedActivityResultEvent<TState, TResult>, TResult>, ITimedAsyncFunc<ITimedActivityEvent<TState>, TState, TResult>
    {
        public TState AsyncState { get; }

        internal static Activities.TimedAsyncFunc<TState, TResult> Start(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            Activities.TimedAsyncFunc<TState, TResult> activity;
            LinkedListNode<IAsyncActivity> node;
            Monitor.Enter((provider ?? throw new ArgumentNullException(nameof(provider))).SyncRoot);
            try
            {
                activity = new(state, provider, activityDescription, initialStatusMessage);
                node = activity.OnStarting();
            }
            finally { Monitor.Exit(provider.SyncRoot); }
            AsyncFuncProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
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

        protected override ITimedActivityResultEvent<TState, TResult> CreateRanToCompletionEvent(TResult result) => new TimedActivityResultEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Result = result,
            Exception = null,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override ITimedActivityResultEvent<TState, TResult> CreateCanceledEvent() => new TimedResultOperationTerminatedEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = null,
            Started = Started,
            Duration = Duration,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override ITimedActivityResultEvent<TState, TResult> CreateFaultedEvent(Exception error) => new TimedResultOperationTerminatedEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            Started = Started,
            Duration = Duration,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Error
        };

        private TimedAsyncFunc(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(state), activityDescription, initialStatusMessage) => AsyncState = state;
    }
}
