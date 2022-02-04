using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal sealed partial class AsyncAction
    {
        class AsyncActionProgress : ActivityProgress<AsyncAction>
        {
            private AsyncActionProgress(AsyncAction activity) : base(activity) { }

            internal static async Task StartAsync(AsyncAction activity, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
            {
                activity.OnStarted();
                await asyncMethodDelegate(new AsyncActionProgress(activity));
            }

            protected override IOperationEvent CreateOperationEvent([DisallowNull] AsyncAction activity, Exception exception, StatusMessageLevel messageLevel) => new OperationEvent
            {
                ActivityId = activity.ActivityId,
                ParentActivityId = activity.ParentActivityId,
                ShortDescription = activity.ShortDescription,
                StatusValue = activity.StatusValue,
                StatusMessage = activity.StatusMessage,
                CurrentOperation = activity.CurrentOperation,
                PercentComplete = activity.PercentComplete,
                Exception = exception,
                MessageLevel = messageLevel
            };
        }
    }

    internal sealed partial class AsyncAction<TState>
    {
        class AsyncActionProgress : ActivityProgress<AsyncAction<TState>>, IActivityProgress<TState>
        {
            public TState AsyncState { get; }

            private AsyncActionProgress(AsyncAction<TState> activity) : base(activity) => AsyncState = activity.AsyncState;

            internal static async Task StartAsync(Activities.AsyncAction<TState> activity, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
            {
                activity.OnStarted();
                await asyncMethodDelegate(new AsyncActionProgress(activity));
            }

            protected override IOperationEvent<TState> CreateOperationEvent([DisallowNull] AsyncAction<TState> activity, Exception exception, StatusMessageLevel messageLevel) => new OperationEvent<TState>
            {
                ActivityId = activity.ActivityId,
                ParentActivityId = activity.ParentActivityId,
                ShortDescription = activity.ShortDescription,
                StatusValue = activity.StatusValue,
                StatusMessage = activity.StatusMessage,
                CurrentOperation = activity.CurrentOperation,
                PercentComplete = activity.PercentComplete,
                Exception = exception,
                AsyncState = activity.AsyncState,
                MessageLevel = messageLevel
            };
        }
    }
}
