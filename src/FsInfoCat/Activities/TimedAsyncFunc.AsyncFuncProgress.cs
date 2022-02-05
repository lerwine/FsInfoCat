using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal partial class TimedAsyncFunc<TResult>
    {
        class AsyncFuncProgress : ActivityProgress<TimedAsyncFunc<TResult>>
        {
            private AsyncFuncProgress([DisallowNull] TimedAsyncFunc<TResult> activity) : base(activity) { }

            /// <summary>
            /// Starts a timed asynchronous activity that produces a result value.
            /// </summary>
            /// <param name="activity">The pending activity object.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
            /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task<TResult> StartAsync([DisallowNull] TimedAsyncFunc<TResult> activity, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                Task<TResult> task = asyncMethodDelegate(new AsyncFuncProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnStarted();
                return await task;
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

            private AsyncFuncProgress([DisallowNull] TimedAsyncFunc<TState, TResult> activity) : base(activity) => AsyncState = activity.AsyncState;

            /// <summary>
            /// Starts a timed asynchronous activity that is associated with a user-specified value and produces a result value.
            /// </summary>
            /// <param name="activity">The pending activity object.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
            /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task<TResult> StartAsync([DisallowNull] TimedAsyncFunc<TState, TResult> activity, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                Task<TResult> task = asyncMethodDelegate(new AsyncFuncProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnStarted();
                return await task;
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
