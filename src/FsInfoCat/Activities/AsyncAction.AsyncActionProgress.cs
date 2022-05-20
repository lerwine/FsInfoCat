using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    internal sealed partial class AsyncAction
    {
        sealed class AsyncActionProgress : ActivityProgress<AsyncAction>
        {
            private AsyncActionProgress([DisallowNull] AsyncAction activity) : base(Hosting.GetRequiredService<ILogger<AsyncActionProgress>>(), activity) { }

            /// <summary>
            /// Start as an asynchronous operation.
            /// </summary>
            /// <param name="activity">The activity to start.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task StartAsync([DisallowNull] AsyncAction activity, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
            {
                if (asyncMethodDelegate is null) throw new ArgumentNullException(nameof(asyncMethodDelegate));
                activity.Logger.LogDebug("Invoking asyncMethodDelegate for AsyncAction {ActivityId} ({ShortDescription})", activity.ActivityId, activity.ShortDescription);
                Task task = asyncMethodDelegate(new AsyncActionProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnBeforeAwaitTask();
                activity.Logger.LogDebug("Awaiting task completion for AsyncAction {ActivityId} ({ShortDescription})", activity.ActivityId, activity.ShortDescription);
                await task;
                activity.Logger.LogDebug("Task for AsyncAction completed successfully", activity.ActivityId, activity.ShortDescription);
            }

            /// <summary>
            /// Creates an operational event object.
            /// </summary>
            /// <param name="activity">The source activity of the event.</param>
            /// <param name="exception">The exception associated with the operational event or <see langword="null" /> if there is no exception.</param>
            /// <param name="messageLevel">The message level for the operational event.</param>
            /// <returns>An <see cref="IOperationEvent" /> object describing the event.</returns>
            protected override IOperationEvent CreateOperationEvent([DisallowNull] AsyncAction activity, Exception exception, Model.StatusMessageLevel messageLevel) => new OperationEvent
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
        sealed class AsyncActionProgress : ActivityProgress<AsyncAction<TState>>, IActivityProgress<TState>
        {
            /// <summary>
            /// Gets the user-defined value.
            /// </summary>
            /// <value>The user-defined vaue that is associated with the activity.</value>
            public TState AsyncState { get; }

            private AsyncActionProgress([DisallowNull] AsyncAction<TState> activity) : base(Hosting.GetRequiredService<ILogger<AsyncActionProgress>>(), activity) => AsyncState = activity.AsyncState;

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
                activity.Logger.LogDebug("Invoking asyncMethodDelegate for AsyncAction {ActivityId} ({ShortDescription})", activity.ActivityId, activity.ShortDescription);
                Task task = asyncMethodDelegate(new AsyncActionProgress(activity));
                if (task is null)
                    throw new InvalidOperationException();
                activity.OnBeforeAwaitTask();
                activity.Logger.LogDebug("Awaiting task completion for AsyncAction {ActivityId} ({ShortDescription})", activity.ActivityId, activity.ShortDescription);
                await task;
                activity.Logger.LogDebug("Task for AsyncAction completed successfully", activity.ActivityId, activity.ShortDescription);
            }

            /// <summary>
            /// Creates an operational event object.
            /// </summary>
            /// <param name="activity">The source activity of the event.</param>
            /// <param name="exception">The exception associated with the operational event or <see langword="null" /> if there is no exception.</param>
            /// <param name="messageLevel">The message level for the operational event.</param>
            /// <returns>An <see cref="IOperationEvent{TState}" /> object describing the event.</returns>
            protected override IOperationEvent<TState> CreateOperationEvent([DisallowNull] AsyncAction<TState> activity, Exception exception, Model.StatusMessageLevel messageLevel) => new OperationEvent<TState>
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
