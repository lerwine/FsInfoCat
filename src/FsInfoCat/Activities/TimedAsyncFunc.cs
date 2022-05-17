using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        /// <summary>
        /// Base class for timed asynchronous activity objects that produce a result value.
        /// </summary>
        /// <typeparam name="TBaseEvent">The base type for all observed <see cref="ITimedActivityEvent"/> objects.</typeparam>
        /// <typeparam name="TOperationEvent">The type of the <typeparamref name="TBaseEvent"/> operation object which implements <see cref="ITimedOperationEvent"/>.</typeparam>
        /// <typeparam name="TResultEvent">The type of the <typeparamref name="TBaseEvent"/> result object which
        /// implements <see cref="ITimedActivityResultEvent{TResult}"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <seealso cref="TimedAsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, TTask}" />
        /// <seealso cref="ITimedAsyncFunc{TBaseEvent, TResult}" />
        internal abstract class TimedAsyncFunc<TBaseEvent, TOperationEvent, TResultEvent, TResult> : TimedAsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task<TResult>>,
            ITimedAsyncFunc<TBaseEvent, TResult>
            where TBaseEvent : ITimedActivityEvent
            where TOperationEvent : TBaseEvent, ITimedOperationEvent
            where TResultEvent : TBaseEvent, ITimedActivityResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            /// <summary>
            /// Gets the task for the asyncronous activity.
            /// </summary>
            /// <value>The task for the asyncronous activity.</value>
            public override Task<TResult> Task => _completionSource.Task;

            /// <summary>
            /// Initializes a new instance of the <see cref="TimedAsyncFunc{TBaseEvent, TOperationEvent, TResultEvent, TResult}"/> class.
            /// </summary>
            /// <param name="owner">The owner activity provider.</param>
            /// <param name="completionSource">The task completion source.</param>
            /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
            /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> or <paramref name="completionSource"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
            protected TimedAsyncFunc([DisallowNull] AsyncActivityProvider owner, [DisallowNull] TaskCompletionSource<TResult> completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(owner, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            /// <summary>
            /// Creates the result event that gets pushed after the activity has run to completion.
            /// </summary>
            /// <param name="result">The result value.</param>
            /// <returns>A <typeparamref name="TResultEvent"/> describing a timed activity that has run to completion.</returns>
            /// <remarks>Implementing classes should set <see cref="IActivityStatusInfo.StatusValue"/> to <see cref="ActivityStatus.RanToCompletion"/>, <see cref="IActivityEvent.Exception"/> should be <see langword="null"/>,
            /// <see cref="IActivityEvent.MessageLevel"/> should be <see cref="StatusMessageLevel.Information"/>, and <see cref="IActivityResultEvent{TResult}.Result"/> should be set to <paramref name="result"/>.</remarks>
            protected abstract TResultEvent CreateRanToCompletionEvent(TResult result);

            /// <summary>
            /// Called after an asynchronous activity has been completed.
            /// </summary>
            /// <param name="task">The task that implemented the asynchronous activity.</param>
            /// <param name="node">The <see cref="LinkedListNode{IAsyncActivity}"/> that was returned by <see cref="OnStarting(IAsyncActivity)"/>,
            /// which references the <see cref="IAsyncActivity"/> that ran to completion, faulted, or was canceled.</param>
            /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="node"/> is <see langword="null"/>.</exception>
            protected void SetCompleted(Task<TResult> task, LinkedListNode<IAsyncActivity> node)
            {
                try { StopTimer(); }
                finally
                {
                    try
                    {
                        if (task.IsCanceled)
                            try
                            {
                                StatusValue = ActivityStatus.Canceled;
                                EventSource.RaiseNext(CreateCanceledEvent());
                            }
                            finally
                            {
                                if (TokenSource.IsCancellationRequested)
                                    _completionSource.SetCanceled(TokenSource.Token);
                                else
                                    _completionSource.SetCanceled();
                            }
                        else if (task.IsFaulted)
                            try
                            {
                                StatusValue = ActivityStatus.Faulted;
#pragma warning disable CS8604 // Possible null reference argument.
                                EventSource.RaiseNext(CreateFaultedEvent(task.Exception));
#pragma warning restore CS8604 // Possible null reference argument.
                            }
                            finally
                            {
#pragma warning disable CS8604 // Possible null reference argument.
                                _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                            }
                        else
                            try
                            {
                                StatusValue = ActivityStatus.RanToCompletion;
                                EventSource.RaiseNext(CreateRanToCompletionEvent(task.Result));
                            }
                            finally { _completionSource.SetResult(task.Result); }
                    }
                    finally { NotifyCompleted(node); }
                }
            }
        }
    }

    /// <summary>
    /// Represents a timed asynchronous activity that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="AsyncActivityProvider.TimedAsyncFunc{TBaseEvent, TOperationEvent, TResultEvent, TResult}" />
    /// <seealso cref="ITimedActivityEvent" />
    /// <seealso cref="ITimedOperationEvent" />
    /// <seealso cref="ITimedActivityResultEvent{TResult}" />
    internal partial class TimedAsyncFunc<TResult> : AsyncActivityProvider.TimedAsyncFunc<ITimedActivityEvent, ITimedOperationEvent, ITimedActivityResultEvent<TResult>, TResult>
    {
        /// <summary>
        /// Invokes a method that asynchronously produces a result value, tracking the execution start and duration.
        /// </summary>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>A <see cref="TimedAsyncFunc{TResult}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        internal static TimedAsyncFunc<TResult> Start([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            TimedAsyncFunc<TResult> activity;
            LinkedListNode<IAsyncActivity> node;
            Monitor.Enter((owner ?? throw new ArgumentNullException(nameof(owner))).SyncRoot);
            try
            {
                activity = new(owner, activityDescription, initialStatusMessage);
                node = activity.OnStarting();
            }
            finally { Monitor.Exit(owner.SyncRoot); }
            AsyncFuncProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        /// <summary>
        /// Creates the initial event that gets pushed before a timed activity is started.
        /// </summary>
        /// <returns>An <see cref="ITimedActivityEvent" /> describing a timed activity that is about to be started.</returns>
        protected override ITimedActivityEvent CreateInitialEvent() => new TimedActivityEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Information
        };

        /// <summary>
        /// Creates the result event that gets pushed after a timed activity has run to completion.
        /// </summary>
        /// <param name="result">The result value.</param>
        /// <returns>An <see cref="ITimedActivityResultEvent{TResult}" /> describing a timed activity that has run to completion.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.RanToCompletion" />, <see cref="IActivityEvent.Exception" /> to <see langword="null" />,
        /// <see cref="IActivityEvent.MessageLevel" /> to <see cref="StatusMessageLevel.Information" />, and <see cref="IActivityResultEvent{TResult}.Result" /> to <paramref name="result" />.</remarks>
        protected override ITimedActivityResultEvent<TResult> CreateRanToCompletionEvent(TResult result) => new TimedActivityResultEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Result = result,
            Exception = null,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Information
        };

        /// <summary>
        /// Creates the result event that gets pushed after a timed activity is canceled.
        /// </summary>
        /// <returns>An <see cref="ITimedActivityResultEvent{TResult}" /> describing a timed activity that has been canceled.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.Canceled" />, <see cref="IActivityEvent.Exception" /> to <see langword="null" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="StatusMessageLevel.Warning" />.</remarks>
        protected override ITimedActivityResultEvent<TResult> CreateCanceledEvent() => new TimedResultOperationTerminatedEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = null,
            Started = Started,
            Duration = Duration,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Warning
        };

        /// <summary>
        /// Creates the result event that gets pushed after a timed activity has faulted.
        /// </summary>
        /// <param name="error">The error that caused the activity to terminate before completion.</param>
        /// <returns>A An <see cref="ITimedActivityResultEvent{TResult}" /> describing a timed activity that that has terminated before completion due to an unhandled exception.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.Faulted" />, <see cref="IActivityEvent.Exception" /> to <paramref name="error" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="StatusMessageLevel.Error" />.</remarks>
        protected override ITimedActivityResultEvent<TResult> CreateFaultedEvent(Exception error) => new TimedResultOperationTerminatedEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            Started = Started,
            Duration = Duration,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Error
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedAsyncFunc{TResult}"/> class.
        /// </summary>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
        private TimedAsyncFunc([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(owner, new(), activityDescription, initialStatusMessage) { }
    }

    /// <summary>
    /// Represents a timed asynchronous activity that is associated with a user-specified value and produces a result value.
    /// </summary>
    /// <typeparam name="TState">The type of user-specified value that is associated with the asynchronous activity.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="AsyncActivityProvider.TimedAsyncFunc{TBaseEvent, TOperationEvent, TResultEvent, TResult}" />
    /// <seealso cref="ITimedActivityEvent{TState}" />
    /// <seealso cref="ITimedOperationEvent{TState}" />
    /// <seealso cref="ITimedActivityResultEvent{TState, TResult}" />
    /// <seealso cref="ITimedAsyncFunc{TEvent, TState, TResult}" />
    /// <seealso cref="ITimedActivityEvent{TState}" />
    internal partial class TimedAsyncFunc<TState, TResult> : AsyncActivityProvider.TimedAsyncFunc<ITimedActivityEvent<TState>, ITimedOperationEvent<TState>, ITimedActivityResultEvent<TState, TResult>, TResult>, ITimedAsyncFunc<ITimedActivityEvent<TState>, TState, TResult>
    {
        /// <summary>
        /// Gets the user-defined value.
        /// </summary>
        /// <value>The user-defined vaue that is associated with the activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, associating the function with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <param name="state">The user-defined value to associate with the the asynchronous function.</param>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>A <see cref="TimedAsyncFunc{TState, TResult}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        internal static TimedAsyncFunc<TState, TResult> Start(TState state, [DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            TimedAsyncFunc<TState, TResult> activity;
            LinkedListNode<IAsyncActivity> node;
            Monitor.Enter((owner ?? throw new ArgumentNullException(nameof(owner))).SyncRoot);
            try
            {
                activity = new(state, owner, activityDescription, initialStatusMessage);
                node = activity.OnStarting();
            }
            finally { Monitor.Exit(owner.SyncRoot); }
            AsyncFuncProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
            return activity;
        }

        /// <summary>
        /// Creates the initial event that gets pushed before a timed activity is started.
        /// </summary>
        /// <returns>An <see cref="ITimedActivityEvent{TState}" /> describing a timed activity that is about to be started.</returns>
        protected override ITimedActivityEvent<TState> CreateInitialEvent() => new TimedActivityEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        /// <summary>
        /// Creates the result event that gets pushed after a timed activity has run to completion.
        /// </summary>
        /// <param name="result">The result value.</param>
        /// <returns>An <see cref="ITimedActivityResultEvent{TState, TResult}" /> describing a timed activity that has run to completion.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.RanToCompletion" />, <see cref="IActivityEvent.Exception" /> to <see langword="null" />,
        /// <see cref="IActivityEvent.MessageLevel" /> to <see cref="StatusMessageLevel.Information" />, and <see cref="IActivityResultEvent{TResult}.Result" /> to <paramref name="result" />.</remarks>
        protected override ITimedActivityResultEvent<TState, TResult> CreateRanToCompletionEvent(TResult result) => new TimedActivityResultEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Result = result,
            Exception = null,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        /// <summary>
        /// Creates the result event that gets pushed after an activity is canceled.
        /// </summary>
        /// <returns>An <see cref="ITimedActivityResultEvent{TState, TResult}" /> describing a timed activity that has been canceled.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.Canceled" />, <see cref="IActivityEvent.Exception" /> to <see langword="null" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="StatusMessageLevel.Warning" />.</remarks>
        protected override ITimedActivityResultEvent<TState, TResult> CreateCanceledEvent() => new TimedResultOperationTerminatedEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = null,
            Started = Started,
            Duration = Duration,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Warning
        };

        /// <summary>
        /// Creates the result event that gets pushed after a timed activity has faulted.
        /// </summary>
        /// <param name="error">The error that caused the activity to terminate before completion.</param>
        /// <returns>An <see cref="ITimedActivityResultEvent{TState, TResult}" /> describing an activity that that has terminated before completion due to an unhandled exception.</returns>
        /// <exception cref="ArgumentNullException">error</exception>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.Faulted" />, <see cref="IActivityEvent.Exception" /> to <paramref name="error" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="StatusMessageLevel.Error" />.</remarks>
        protected override ITimedActivityResultEvent<TState, TResult> CreateFaultedEvent(Exception error) => new TimedResultOperationTerminatedEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            Started = Started,
            Duration = Duration,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Error
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedAsyncFunc{TState, TResult}"/> class.
        /// </summary>
        /// <param name="state">The the user-defined value that is associated with this asynchronous action.</param>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
        private TimedAsyncFunc(TState state, [DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(owner, new(state), activityDescription, initialStatusMessage) => AsyncState = state;
    }
}
