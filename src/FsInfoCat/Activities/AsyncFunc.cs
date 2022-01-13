using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract class AsyncFunc<TEvent, TResult> : AsyncActivity<TEvent, Task<TResult>>, IAsyncFunc<TEvent, TResult>
            where TEvent : IOperationEvent
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task<TResult> Task => _completionSource.Task;

            protected AsyncFunc([DisallowNull] AsyncActivityProvider provider, [DisallowNull] TaskCompletionSource<TResult> completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(provider, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            protected void SetCompleted(Task<TResult> task, LinkedListNode<IAsyncActivity> node)
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
                        _completionSource.SetResult(task.Result);
                    }
                    finally { OnRanToCompletion(node); }
            }
        }
    }

    internal class AsyncFunc<TResult> : AsyncActivityProvider.AsyncFunc<IOperationEvent, TResult>
    {
        internal static AsyncFunc<TResult> Start([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            AsyncFunc<TResult> activity = new(provider, activityDescription, initialStatusMessage);
            LinkedListNode<IAsyncActivity> node = activity.OnStarting();
            AsyncFuncProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        private AsyncFunc([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(), activityDescription, initialStatusMessage) { }

        class AsyncFuncProgress : ActivityProgress
        {
            private AsyncFuncProgress(AsyncFunc<TResult> activity) : base(activity) { }

            internal static async Task<TResult> StartAsync(AsyncFunc<TResult> activity, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
            {
                activity.OnStarting();
                return await asyncMethodDelegate(new AsyncFuncProgress(activity));
            }
        }
    }

    internal class AsyncFunc<TState, TResult> : AsyncActivityProvider.AsyncFunc<IOperationEvent<TState>, TResult>, IAsyncFunc<IOperationEvent<TState>, TState, TResult>
    {
        public TState AsyncState { get; }

        internal static Activities.AsyncFunc<TState, TResult> Start(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            Activities.AsyncFunc<TState, TResult> activity = new(state, provider, activityDescription, initialStatusMessage);
            LinkedListNode<IAsyncActivity> node = activity.OnStarting();
            AsyncFuncProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        private AsyncFunc(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(state), activityDescription, initialStatusMessage) => AsyncState = state;

        class AsyncFuncProgress : ActivityProgress, IActivityProgress<TState>
        {
            public TState AsyncState { get; }

            private AsyncFuncProgress(Activities.AsyncFunc<TState, TResult> activity) : base(activity) => AsyncState = activity.AsyncState;

            internal static async Task<TResult> StartAsync(Activities.AsyncFunc<TState, TResult> activity, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            {
                activity.OnStarting();
                return await asyncMethodDelegate(new AsyncFuncProgress(activity));
            }
        }
    }
}
