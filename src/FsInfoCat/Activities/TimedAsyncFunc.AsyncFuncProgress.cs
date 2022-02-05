using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal partial class TimedAsyncFunc<TResult>
    {
        /// <summary>
        /// The progress reporter and nested <see cref="AsyncActivityProvider"/> for <see cref="TimedAsyncFunc{TResult}"/> objects.
        /// </summary>
        /// <seealso cref="AsyncActivityProvider.AsyncActivity{ITimedActivityEvent, ITimedOperationEvent, ITimedActivityResultEvent{TResult}, Task{TResult}}.ActivityProgress{TimedAsyncFunc{TResult}}" />
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
                activity.OnBeforeAwaitTask();
                return await task;
            }

            /// <summary>
            /// Creates an operational event object.
            /// </summary>
            /// <param name="activity">The source activity of the event.</param>
            /// <param name="exception">The exception associated with the operational event or <see langword="null" /> if there is no exception.</param>
            /// <param name="messageLevel">The message level for the operational event.</param>
            /// <returns>An <see cref="ITimedOperationEvent" /> object describing the event.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> is <see langword="null"/>.</exception>
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
        /// <summary>
        /// The progress reporter and nested <see cref="AsyncActivityProvider"/> for <see cref="TimedAsyncFunc{TState, TResult}"/> objects.
        /// </summary>
        /// <seealso cref="AsyncActivityProvider.AsyncActivity{ITimedActivityEvent{TState}, ITimedOperationEvent{TState}, ITimedActivityResultEvent{TState, TResult}, Task{TResult}}.ActivityProgress{TimedAsyncFunc{TState, TResult}}" />
        /// <seealso cref="IActivityProgress{TState}" />
        class AsyncFuncProgress : ActivityProgress<TimedAsyncFunc<TState, TResult>>, IActivityProgress<TState>
        {
            /// <summary>
            /// Gets the user-defined value.
            /// </summary>
            /// <value>The user-defined vaue that is associated with the activity.</value>
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
                activity.OnBeforeAwaitTask();
                return await task;
            }

            /// <summary>
            /// Creates an operational event object.
            /// </summary>
            /// <param name="activity">The source activity of the event.</param>
            /// <param name="exception">The exception associated with the operational event or <see langword="null" /> if there is no exception.</param>
            /// <param name="messageLevel">The message level for the operational event.</param>
            /// <returns>An <see cref="ITimedOperationEvent{TState}" /> object describing the event.</returns>
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
