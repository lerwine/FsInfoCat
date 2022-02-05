using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{

    internal partial class AsyncFunc<TResult>
    {
        class AsyncFuncProgress : ActivityProgress<AsyncFunc<TResult>>
        {
            private AsyncFuncProgress([DisallowNull] AsyncFunc<TResult> activity) : base(activity) { }

            internal static async Task<TResult> StartAsync([DisallowNull] AsyncFunc<TResult> activity, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                Task<TResult> task = asyncMethodDelegate(new AsyncFuncProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnStarted();
                return await task;
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

            private AsyncFuncProgress([DisallowNull] AsyncFunc<TState, TResult> activity) : base(activity) => AsyncState = activity.AsyncState;

            /// <summary>
            /// Starts an asynchronous activity that produces a result value.
            /// </summary>
            /// <param name="activity">The pending activity object.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
            /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task<TResult> StartAsync([DisallowNull] AsyncFunc<TState, TResult> activity, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                Task<TResult> task = asyncMethodDelegate(new AsyncFuncProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnStarted();
                return await task;
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
