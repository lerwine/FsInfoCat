using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract class AsyncAction<TEvent> : AsyncActivity<TEvent, Task>, IAsyncAction<TEvent>
            where TEvent : IOperationEvent
        {
            private readonly TaskCompletionSource _completionSource;

            public override Task Task => _completionSource.Task;

            protected AsyncAction([DisallowNull] AsyncActivityProvider provider, [DisallowNull] TaskCompletionSource completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(provider, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            protected void SetCompleted(Task task, LinkedListNode<IAsyncActivity> node)
            {
                if (task.IsCanceled)
                    try
                    {
                        StatusValue = ActivityStatus.Canceled;
                        if (TokenSource.IsCancellationRequested)
                            _completionSource.SetCanceled(TokenSource.Token);
                        else
                            _completionSource.SetCanceled();

                    }
                    finally { OnCanceled(node); }
                else if (task.IsFaulted)
                    try
                    {
                        StatusValue = ActivityStatus.Faulted;
                        _completionSource.SetException(task.Exception);

                    }
                    finally { OnFaulted(node, task.Exception); }
                else
                    try
                    {
                        StatusValue = ActivityStatus.RanToCompletion;
                        _completionSource.SetResult();
                    }
                    finally { OnRanToCompletion(node); }
            }
        }
    }

    internal sealed class AsyncAction : AsyncActivityProvider.AsyncAction<IOperationEvent>
    {
        internal static AsyncAction Start([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            AsyncAction activity = new(provider, activityDescription, initialStatusMessage);
            LinkedListNode<IAsyncActivity> node = activity.OnStarting();
            AsyncActionProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        private AsyncAction([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(), activityDescription, initialStatusMessage) { }

        class AsyncActionProgress : ActivityProgress
        {
            private AsyncActionProgress(AsyncAction activity) : base(activity) { }

            internal static async Task StartAsync(AsyncAction activity, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
            {
                activity.OnStarted();
                await asyncMethodDelegate(new AsyncActionProgress(activity));
            }
        }
    }

    internal sealed class AsyncAction<TState> : AsyncActivityProvider.AsyncAction<IOperationEvent<TState>>, IAsyncAction<IOperationEvent<TState>, TState>
    {
        public TState AsyncState { get; }

        internal static Activities.AsyncAction<TState> Start(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription,
            [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            Activities.AsyncAction<TState> activity = new(state, provider, activityDescription, initialStatusMessage);
            LinkedListNode<IAsyncActivity> node = activity.OnStarting();
            AsyncActionProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        private AsyncAction(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(state), activityDescription, initialStatusMessage) => AsyncState = state;

        class AsyncActionProgress : ActivityProgress, IActivityProgress<TState>
        {
            public TState AsyncState { get; }

            private AsyncActionProgress(Activities.AsyncAction<TState> activity) : base(activity) => AsyncState = activity.AsyncState;

            internal static async Task StartAsync(Activities.AsyncAction<TState> activity, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
            {
                activity.OnStarting();
                await asyncMethodDelegate(new AsyncActionProgress(activity));
            }
        }
    }
}
