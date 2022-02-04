using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract class AsyncFunc<TBaseEvent, TOperationEvent, TResultEvent, TResult> : AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task<TResult>>, IAsyncFunc<TBaseEvent, TResult>
            where TBaseEvent : IActivityEvent
            where TOperationEvent : TBaseEvent, IOperationEvent
            where TResultEvent : TBaseEvent, IActivityResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task<TResult> Task => _completionSource.Task;

            protected AsyncFunc([DisallowNull] AsyncActivityProvider provider, [DisallowNull] TaskCompletionSource<TResult> completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(provider, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            protected abstract TResultEvent CreateRanToCompletionEvent(TResult result);

            protected void SetCompleted(Task<TResult> task, LinkedListNode<IAsyncActivity> node)
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

    internal partial class AsyncFunc<TResult> : AsyncActivityProvider.AsyncFunc<IActivityEvent, IOperationEvent, IActivityResultEvent<TResult>, TResult>
    {
        internal static AsyncFunc<TResult> Start([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            AsyncFunc<TResult> activity;
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

        protected override IActivityEvent CreateInitialEvent() => new ActivityEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityResultEvent<TResult> CreateRanToCompletionEvent(TResult result) => new ActivityResultEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Result = result,
            Exception = null,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityResultEvent<TResult> CreateCanceledEvent() => new ResultOperationTerminatedEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override IActivityResultEvent<TResult> CreateFaultedEvent(Exception error) => new ResultOperationTerminatedEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Error
        };

        private AsyncFunc([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(), activityDescription, initialStatusMessage) { }
    }

    internal partial class AsyncFunc<TState, TResult> : AsyncActivityProvider.AsyncFunc<IActivityEvent<TState>, IOperationEvent<TState>, IActivityResultEvent<TState, TResult>, TResult>, IAsyncFunc<IActivityEvent<TState>, TState, TResult>
    {
        public TState AsyncState { get; }

        internal static Activities.AsyncFunc<TState, TResult> Start(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            Activities.AsyncFunc<TState, TResult> activity;
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

        protected override IActivityResultEvent<TState, TResult> CreateRanToCompletionEvent(TResult result) => new ActivityResultEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Result = result,
            Exception = null,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityResultEvent<TState, TResult> CreateCanceledEvent() => new ResultOperationTerminatedEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override IActivityResultEvent<TState, TResult> CreateFaultedEvent(Exception error) => new ResultOperationTerminatedEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Error
        };

        private AsyncFunc(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(state), activityDescription, initialStatusMessage) => AsyncState = state;
    }
}
