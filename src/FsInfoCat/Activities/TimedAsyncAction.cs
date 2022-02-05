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
        /// Base class for timed asynchronous activity objects that do not produce a result value.
        /// </summary>
        /// <typeparam name="TBaseEvent">The base type for all observed <see cref="ITimedActivityEvent"/> objects.</typeparam>
        /// <typeparam name="TOperationEvent">The type of the <typeparamref name="TBaseEvent"/> operation object which implements <see cref="ITimedOperationEvent"/>.</typeparam>
        /// <typeparam name="TResultEvent">The type of the <typeparamref name="TBaseEvent"/> result object which implements <see cref="ITimedActivityCompletedEvent"/>.</typeparam>
        /// <seealso cref="TimedAsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, Task}" />
        /// <seealso cref="ITimedAsyncAction{TBaseEvent}" />
        internal abstract class TimedAsyncAction<TBaseEvent, TOperationEvent, TResultEvent> : TimedAsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task>, ITimedAsyncAction<TBaseEvent>
            where TBaseEvent : ITimedActivityEvent
            where TOperationEvent : TBaseEvent, ITimedOperationEvent
            where TResultEvent : TBaseEvent, ITimedActivityCompletedEvent
        {
            private readonly TaskCompletionSource _completionSource;

            public override Task Task => _completionSource.Task;

            /// <summary>
            /// Initializes a new instance of the <see cref="TimedAsyncAction{TBaseEvent, TOperationEvent, TResultEvent}"/> class.
            /// </summary>
            /// <param name="owner">The owner activity provider.</param>
            /// <param name="completionSource">The task completion source.</param>
            /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
            /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> or <paramref name="completionSource"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
            protected TimedAsyncAction([DisallowNull] AsyncActivityProvider owner, [DisallowNull] TaskCompletionSource completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(owner, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            protected abstract TResultEvent CreateRanToCompletionEvent();

            protected void SetCompleted(Task task, LinkedListNode<IAsyncActivity> node)
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
                                EventSource.RaiseNext(CreateRanToCompletionEvent());
                            }
                            finally { _completionSource.SetResult(); }
                    }
                    finally { NotifyCompleted(node); }
                }
            }
        }
    }

    /// <summary>
    /// Represents a timed asynchronous activity that does not produce a result value.
    /// </summary>
    /// <seealso cref="AsyncActivityProvider.TimedAsyncAction{ITimedActivityEvent, ITimedOperationEvent, ITimedActivityCompletedEvent}" />
    /// <seealso cref="ITimedAsyncAction{ITimedActivityEvent}" />
    internal sealed partial class TimedAsyncAction : AsyncActivityProvider.TimedAsyncAction<ITimedActivityEvent, ITimedOperationEvent, ITimedActivityCompletedEvent>, ITimedAsyncAction<ITimedActivityEvent>
    {
        /// <summary>
        /// Invokes an asynchronous method, tracking the execution start and duration.
        /// </summary>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>A <see cref="TimedAsyncAction" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        internal static TimedAsyncAction Start([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            TimedAsyncAction activity;
            LinkedListNode<IAsyncActivity> node;
            Monitor.Enter((owner ?? throw new ArgumentNullException(nameof(owner))).SyncRoot);
            try
            {
                activity = new(owner, activityDescription, initialStatusMessage);
                node = activity.OnStarting();
            }
            finally { Monitor.Exit(owner.SyncRoot); }
            AsyncActionProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
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

        protected override ITimedActivityCompletedEvent CreateRanToCompletionEvent() => new TimedActivityCompletedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Exception = null,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override ITimedActivityCompletedEvent CreateCanceledEvent() => new TimedOperationTerminatedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override ITimedActivityCompletedEvent CreateFaultedEvent(Exception error) => new TimedOperationTerminatedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            Started = Started,
            Duration = Duration,
            MessageLevel = StatusMessageLevel.Error
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedAsyncAction"/> class.
        /// </summary>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
        private TimedAsyncAction([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(owner, new(), activityDescription, initialStatusMessage) { }
    }

    /// <summary>
    /// Represents a timed asyncronous activity that is associated with a user-specified value and does not produce a result value.
    /// </summary>
    /// <typeparam name="TState">The type of user-specified value that is associated with the asynchronous activity.</typeparam>
    /// <seealso cref="AsyncActivityProvider.TimedAsyncAction{ITimedActivityEvent{TState}, ITimedOperationEvent{TState}, ITimedActivityCompletedEvent{TState}}" />
    /// <seealso cref="ITimedAsyncAction{ITimedActivityEvent{TState}, TState}" />
    internal sealed partial class TimedAsyncAction<TState> : AsyncActivityProvider.TimedAsyncAction<ITimedActivityEvent<TState>, ITimedOperationEvent<TState>, ITimedActivityCompletedEvent<TState>>, ITimedAsyncAction<ITimedActivityEvent<TState>, TState>
    {
        public TState AsyncState { get; }

        /// <summary>
        /// Invokes an asynchronous method, associating it with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>A <see cref="TimedAsyncAction{TState}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        internal static TimedAsyncAction<TState> Start(TState state, [DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription,
            [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            TimedAsyncAction<TState> activity;
            LinkedListNode<IAsyncActivity> node;
            Monitor.Enter((owner ?? throw new ArgumentNullException(nameof(owner))).SyncRoot);
            try
            {
                activity = new(state, owner, activityDescription, initialStatusMessage);
                node = activity.OnStarting();
            }
            finally { Monitor.Exit(owner.SyncRoot); }
            AsyncActionProgress.StartAsync(activity, asyncMethodDelegate).ContinueWith(task => activity.SetCompleted(task, node));
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

        protected override ITimedActivityCompletedEvent<TState> CreateRanToCompletionEvent() => new TimedActivityCompletedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Exception = null,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override ITimedActivityCompletedEvent<TState> CreateCanceledEvent() => new TimedOperationTerminatedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override ITimedActivityCompletedEvent<TState> CreateFaultedEvent(Exception error) => new TimedOperationTerminatedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            Started = Started,
            Duration = Duration,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Error
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedAsyncAction{TState}"/> class.
        /// </summary>
        /// <param name="state">The the user-defined value that is associated with this asynchronous action.</param>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
        private TimedAsyncAction(TState state, [DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(owner, new(state), activityDescription, initialStatusMessage) => AsyncState = state;
    }
}
