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

            /// <summary>
            /// Start as an asynchronous operation.
            /// </summary>
            /// <param name="activity">The activity to start.</param>
            /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces a result value.</param>
            /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> or <paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
            internal static async Task<TResult> StartAsync([DisallowNull] AsyncFunc<TResult> activity, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
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
            /// <returns>An <see cref="IOperationEvent" /> object describing the event.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="activity"/> is <see langword="null"/>.</exception>
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
            /// <summary>
            /// Gets the user-defined value.
            /// </summary>
            /// <value>The user-defined vaue that is associated with the activity.</value>
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
                activity.OnBeforeAwaitTask();
                return await task;
            }

            /// <summary>
            /// Creates an operational event object.
            /// </summary>
            /// <param name="activity">The source activity of the event.</param>
            /// <param name="exception">The exception associated with the operational event or <see langword="null" /> if there is no exception.</param>
            /// <param name="messageLevel">The message level for the operational event.</param>
            /// <returns>An <see cref="IOperationEvent{TState}" /> object describing the event.</returns>
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
