using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal sealed partial class AsyncAction
    {
        class AsyncActionProgress : ActivityProgress<AsyncAction>
        {
            private AsyncActionProgress([DisallowNull] AsyncAction activity) : base(activity) { }

            /// <summary>
            /// Start as an asynchronous operation.
            /// </summary>
            /// <param name="activity">The activity to start.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
            /// <returns>A Task representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task StartAsync([DisallowNull] AsyncAction activity, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                Task task = asyncMethodDelegate(new AsyncActionProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnStarted();
                await task;
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

            private AsyncActionProgress([DisallowNull] AsyncAction<TState> activity) : base(activity) => AsyncState = activity.AsyncState;

            /// <summary>
            /// Start as an asynchronous operation that is associated with a user-specified value.
            /// </summary>
            /// <param name="activity">The activity to start.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
            /// <returns>A Task representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task StartAsync([DisallowNull] AsyncAction<TState> activity, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                Task task = asyncMethodDelegate(new AsyncActionProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnStarted();
                await task;
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
