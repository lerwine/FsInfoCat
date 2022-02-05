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
        /// <typeparam name="TResultEvent">The type of the <typeparamref name="TBaseEvent"/> result object which implements <see cref="ITimedActivityResultEvent{TResult}"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <seealso cref="TimedAsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, Task{TResult}}" />
        /// <seealso cref="ITimedAsyncFunc{TBaseEvent, TResult}" />
        internal abstract class TimedAsyncFunc<TBaseEvent, TOperationEvent, TResultEvent, TResult> : TimedAsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task<TResult>>, ITimedAsyncFunc<TBaseEvent, TResult>
            where TBaseEvent : ITimedActivityEvent
            where TOperationEvent : TBaseEvent, ITimedOperationEvent
            where TResultEvent : TBaseEvent, ITimedActivityResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

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

            protected abstract TResultEvent CreateRanToCompletionEvent(TResult result);

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
                                EventSource.RaiseNext(CreateFaultedEvent(task.Exception));
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
    /// <seealso cref="AsyncActivityProvider.TimedAsyncFunc{ITimedActivityEvent, ITimedOperationEvent, ITimedActivityResultEvent{TResult}, TResult}" />
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
    /// <seealso cref="AsyncActivityProvider.TimedAsyncFunc{ITimedActivityEvent{TState}, ITimedOperationEvent{TState}, ITimedActivityResultEvent{TState, TResult}, TResult}" />
    /// <seealso cref="ITimedAsyncFunc{ITimedActivityEvent{TState}, TState, TResult}" />
    internal partial class TimedAsyncFunc<TState, TResult> : AsyncActivityProvider.TimedAsyncFunc<ITimedActivityEvent<TState>, ITimedOperationEvent<TState>, ITimedActivityResultEvent<TState, TResult>, TResult>, ITimedAsyncFunc<ITimedActivityEvent<TState>, TState, TResult>
    {
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
