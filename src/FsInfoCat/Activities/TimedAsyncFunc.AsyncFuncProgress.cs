using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal partial class TimedAsyncFunc<TResult>
    {
        class AsyncFuncProgress : ActivityProgress<TimedAsyncFunc<TResult>>
        {
            private AsyncFuncProgress(TimedAsyncFunc<TResult> activity) : base(activity) { }

            internal static async Task<TResult> StartAsync(TimedAsyncFunc<TResult> activity, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
            {
                activity.OnStarted();
                return await asyncMethodDelegate(new AsyncFuncProgress(activity));
            }

            protected override ITimedOperationEvent CreateOperationEvent([DisallowNull] TimedAsyncFunc<TResult> activity, Exception exception, StatusMessageLevel messageLevel) => new TimedOperationEvent
            {
                ActivityId = activity.ActivityId,
                ParentActivityId = activity.ParentActivityId,
                ShortDescription = activity.ShortDescription,
                StatusValue = activity.StatusValue,
                StatusMessage = activity.StatusMessage,
                CurrentOperation = activity.CurrentOperation,
                PercentComplete = activity.PercentComplete,
                Exception = exception,
                Started = activity.Started,
                Duration = activity.Duration,
                MessageLevel = messageLevel
            };
        }
    }

    internal partial class TimedAsyncFunc<TState, TResult>
    {
        class AsyncFuncProgress : ActivityProgress<TimedAsyncFunc<TState, TResult>>, IActivityProgress<TState>
        {
            public TState AsyncState { get; }

            private AsyncFuncProgress(TimedAsyncFunc<TState, TResult> activity) : base(activity) => AsyncState = activity.AsyncState;

            internal static async Task<TResult> StartAsync(TimedAsyncFunc<TState, TResult> activity, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            {
                activity.OnStarted();
                return await asyncMethodDelegate(new AsyncFuncProgress(activity));
            }

            protected override ITimedOperationEvent<TState> CreateOperationEvent([DisallowNull] TimedAsyncFunc<TState, TResult> activity, Exception exception, StatusMessageLevel messageLevel) => new TimedOperationEvent<TState>
            {
                ActivityId = activity.ActivityId,
                ParentActivityId = activity.ParentActivityId,
                ShortDescription = activity.ShortDescription,
                StatusValue = activity.StatusValue,
                StatusMessage = activity.StatusMessage,
                CurrentOperation = activity.CurrentOperation,
                PercentComplete = activity.PercentComplete,
                Exception = exception,
                Started = activity.Started,
                Duration = activity.Duration,
                AsyncState = activity.AsyncState,
                MessageLevel = messageLevel
            };
        }
    }
}
