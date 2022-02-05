using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal sealed partial class TimedAsyncAction
    {
        /// <summary>
        /// The progress reporter and nested <see cref="AsyncActivityProvider"/> for <see cref="TimedAsyncAction"/> objects.
        /// </summary>
        /// <seealso cref="AsyncActivityProvider.AsyncActivity{ITimedActivityEvent, ITimedOperationEvent, ITimedActivityCompletedEvent, Task}.ActivityProgress{TimedAsyncAction}" />
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
                activity.OnBeforeAwaitTask();
                await task;
            }

            /// <summary>
            /// Creates an operational event object.
            /// </summary>
            /// <param name="activity">The source activity of the event.</param>
            /// <param name="exception">The exception associated with the operational event or <see langword="null" /> if there is no exception.</param>
            /// <param name="messageLevel">The message level for the operational event.</param>
            /// <returns>An <see cref="ITimedOperationEvent" /> object describing the event.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> is <see langword="null"/>.</exception>
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
        /// <summary>
        /// The progress reporter and nested <see cref="AsyncActivityProvider"/> for <see cref="TimedAsyncAction{TState}"/> objects.
        /// </summary>
        /// <seealso cref="AsyncActivityProvider.AsyncActivity{ITimedActivityEvent{TState}, ITimedOperationEvent{TState}, ITimedActivityCompletedEvent{TState}, Task}.ActivityProgress{TimedAsyncAction{TState}}" />
        /// <seealso cref="IActivityProgress{TState}" />
        class AsyncActionProgress : ActivityProgress<TimedAsyncAction<TState>>, IActivityProgress<TState>
        {
            /// <summary>
            /// Gets the user-defined value.
            /// </summary>
            /// <value>The user-defined vaue that is associated with the activity.</value>
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
                activity.OnBeforeAwaitTask();
                await task;
            }

            /// <summary>
            /// Creates an operational event object.
            /// </summary>
            /// <param name="activity">The source activity of the event.</param>
            /// <param name="exception">The exception associated with the operational event or <see langword="null" /> if there is no exception.</param>
            /// <param name="messageLevel">The message level for the operational event.</param>
            /// <returns>An <see cref="ITimedOperationEvent{TState}" /> object describing the event.</returns>
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
