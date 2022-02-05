using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal sealed partial class TimedAsyncAction
    {
        class AsyncActionProgress : ActivityProgress<TimedAsyncAction>
        {
            private AsyncActionProgress([DisallowNull] TimedAsyncAction activity) : base(activity) { }

            /// <summary>
            /// Starts a timed asynchronous activity.
            /// </summary>
            /// <param name="activity">The pending activity object.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task StartAsync([DisallowNull] TimedAsyncAction activity, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                Task task = asyncMethodDelegate(new AsyncActionProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnStarted();
                await task;
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

            private AsyncActionProgress([DisallowNull] TimedAsyncAction<TState> activity) : base(activity) => AsyncState = activity.AsyncState;

            /// <summary>
            /// Starts a timed asynchronous activity that is associated with a user-specified value.
            /// </summary>
            /// <param name="activity">The pending activity object.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task StartAsync([DisallowNull] TimedAsyncAction<TState> activity, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                Task task = asyncMethodDelegate(new AsyncActionProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnStarted();
                await task;
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
