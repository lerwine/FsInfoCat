using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{

    internal partial class AsyncFunc<TResult>
    {
        class AsyncFuncProgress : ActivityProgress<AsyncFunc<TResult>>
        {
            private AsyncFuncProgress(AsyncFunc<TResult> activity) : base(activity) { }

            internal static async Task<TResult> StartAsync(AsyncFunc<TResult> activity, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
            {
                activity.OnStarted();
                return await asyncMethodDelegate(new AsyncFuncProgress(activity));
            }

            protected override IOperationEvent CreateOperationEvent([DisallowNull] AsyncFunc<TResult> activity, Exception exception, StatusMessageLevel messageLevel) => new OperationEvent
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

    internal partial class AsyncFunc<TState, TResult>
    {
        class AsyncFuncProgress : ActivityProgress<AsyncFunc<TState, TResult>>, IActivityProgress<TState>
        {
            public TState AsyncState { get; }

            private AsyncFuncProgress(AsyncFunc<TState, TResult> activity) : base(activity) => AsyncState = activity.AsyncState;

            internal static async Task<TResult> StartAsync(Activities.AsyncFunc<TState, TResult> activity, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            {
                activity.OnStarted();
                return await asyncMethodDelegate(new AsyncFuncProgress(activity));
            }

            protected override IOperationEvent<TState> CreateOperationEvent([DisallowNull] AsyncFunc<TState, TResult> activity, Exception exception, StatusMessageLevel messageLevel) => new OperationEvent<TState>
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
