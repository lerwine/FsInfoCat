using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal sealed partial class TimedAsyncAction
    {
        class AsyncActionProgress : ActivityProgress<TimedAsyncAction>
        {
            private AsyncActionProgress(TimedAsyncAction activity) : base(activity) { }

            internal static async Task StartAsync(TimedAsyncAction activity, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
            {
                activity.OnStarted();
                await asyncMethodDelegate(new AsyncActionProgress(activity));
            }

            protected override ITimedOperationEvent CreateOperationEvent([DisallowNull] TimedAsyncAction activity, Exception exception, StatusMessageLevel messageLevel) => new TimedOperationEvent
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

    internal sealed partial class TimedAsyncAction<TState>
    {
        class AsyncActionProgress : ActivityProgress<TimedAsyncAction<TState>>, IActivityProgress<TState>
        {
            public TState AsyncState { get; }

            private AsyncActionProgress(TimedAsyncAction<TState> activity) : base(activity) => AsyncState = activity.AsyncState;

            internal static async Task StartAsync(TimedAsyncAction<TState> activity, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
            {
                activity.OnStarted();
                await asyncMethodDelegate(new AsyncActionProgress(activity));
            }

            protected override ITimedOperationEvent<TState> CreateOperationEvent([DisallowNull] TimedAsyncAction<TState> activity, Exception exception, StatusMessageLevel messageLevel) => new TimedOperationEvent<TState>
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
