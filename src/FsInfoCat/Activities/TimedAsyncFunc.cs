using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract class TimedAsyncFunc<TEvent, TResult> : TimedAsyncActivity<TEvent, Task<TResult>>, ITimedAsyncFunc<TEvent, TResult>
            where TEvent : ITimedOperationEvent
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task<TResult> Task => _completionSource.Task;

            protected TimedAsyncFunc([DisallowNull] AsyncActivityProvider provider, [DisallowNull] TaskCompletionSource<TResult> completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
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

    internal class TimedAsyncFunc<TResult> : AsyncActivityProvider.TimedAsyncFunc<ITimedOperationEvent, TResult>
    {
        internal static TimedAsyncFunc<TResult> Start([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            TimedAsyncFunc<TResult> activity = new(provider, activityDescription, initialStatusMessage);
            LinkedListNode<IAsyncActivity> node = activity.OnStarting();
            AsyncFuncProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        private TimedAsyncFunc([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(), activityDescription, initialStatusMessage) { }

        class AsyncFuncProgress : ActivityProgress
        {
            private AsyncFuncProgress(TimedAsyncFunc<TResult> activity) : base(activity) { }

            internal static async Task<TResult> StartAsync(TimedAsyncFunc<TResult> activity, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
            {
                activity.OnStarting();
                return await asyncMethodDelegate(new AsyncFuncProgress(activity));
            }
        }
    }

    internal class TimedAsyncFunc<TState, TResult> : AsyncActivityProvider.TimedAsyncFunc<ITimedOperationEvent<TState>, TResult>, ITimedAsyncFunc<ITimedOperationEvent<TState>, TState, TResult>
    {
        public TState AsyncState { get; }

        internal static Activities.TimedAsyncFunc<TState, TResult> Start(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            Activities.TimedAsyncFunc<TState, TResult> activity = new(state, provider, activityDescription, initialStatusMessage);
            LinkedListNode<IAsyncActivity> node = activity.OnStarting();
            AsyncFuncProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        private TimedAsyncFunc(TState state, [DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(provider, new(state), activityDescription, initialStatusMessage) => AsyncState = state;

        class AsyncFuncProgress : ActivityProgress, IActivityProgress<TState>
        {
            public TState AsyncState { get; }

            private AsyncFuncProgress(Activities.TimedAsyncFunc<TState, TResult> activity) : base(activity) => AsyncState = activity.AsyncState;

            internal static async Task<TResult> StartAsync(Activities.TimedAsyncFunc<TState, TResult> activity, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            {
                activity.OnStarting();
                return await asyncMethodDelegate(new AsyncFuncProgress(activity));
            }
        }
    }
}
