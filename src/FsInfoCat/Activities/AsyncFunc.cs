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
        /// Base class for asynchronous activity objects that produce a result value.
        /// </summary>
        /// <typeparam name="TBaseEvent">The base type for all observed <see cref="IActivityEvent"/> objects.</typeparam>
        /// <typeparam name="TOperationEvent">The type of the <typeparamref name="TBaseEvent"/> operation object which implements <see cref="IOperationEvent"/>.</typeparam>
        /// <typeparam name="TResultEvent">The type of the <typeparamref name="TBaseEvent"/> result object which implements <see cref="IActivityResultEvent{TResult}"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <seealso cref="AsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, Task{TResult}}" />
        /// <seealso cref="IAsyncFunc{TBaseEvent, TResult}" />
        internal abstract class AsyncFunc<TBaseEvent, TOperationEvent, TResultEvent, TResult> : AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task<TResult>>, IAsyncFunc<TBaseEvent, TResult>
            where TBaseEvent : IActivityEvent
            where TOperationEvent : TBaseEvent, IOperationEvent
            where TResultEvent : TBaseEvent, IActivityResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task<TResult> Task => _completionSource.Task;

            /// <summary>
            /// Initializes a new instance of the <see cref="AsyncFunc{TBaseEvent, TOperationEvent, TResultEvent, TResult}"/> class.
            /// </summary>
            /// <param name="owner">The owner activity provider.</param>
            /// <param name="completionSource">The task completion source.</param>
            /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
            /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> or <paramref name="completionSource"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
            protected AsyncFunc([DisallowNull] AsyncActivityProvider owner, [DisallowNull] TaskCompletionSource<TResult> completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(owner, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            protected abstract TResultEvent CreateRanToCompletionEvent(TResult result);

            protected void SetCompleted(Task<TResult> task, LinkedListNode<IAsyncActivity> node)
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

    /// <summary>
    /// Represents an asynchronous activity that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="AsyncActivityProvider.AsyncFunc{IActivityEvent, IOperationEvent, IActivityResultEvent{TResult}, TResult}" />
    partial class AsyncFunc<TResult> : AsyncActivityProvider.AsyncFunc<IActivityEvent, IOperationEvent, IActivityResultEvent<TResult>, TResult>
    {
        /// <summary>
        /// Invokes a method that asynchronously produces a result value.
        /// </summary>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="AsyncFunc{TResult}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        internal static AsyncFunc<TResult> Start([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            AsyncFunc<TResult> activity;
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

        protected override IActivityEvent CreateInitialEvent() => new ActivityEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityResultEvent<TResult> CreateRanToCompletionEvent(TResult result) => new ActivityResultEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Result = result,
            Exception = null,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityResultEvent<TResult> CreateCanceledEvent() => new ResultOperationTerminatedEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override IActivityResultEvent<TResult> CreateFaultedEvent(Exception error) => new ResultOperationTerminatedEvent<TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = StatusMessageLevel.Error
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncFunc{TResult}"/> class.
        /// </summary>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
        private AsyncFunc([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(owner, new(), activityDescription, initialStatusMessage) { }
    }

    /// <summary>
    /// Represents an asynchronous activity that is associated with a user-specified value and produces a result value.
    /// </summary>
    /// <typeparam name="TState">The type of user-specified value that is associated with the asynchronous activity.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="AsyncActivityProvider.AsyncFunc{IActivityEvent{TState}, IOperationEvent{TState}, IActivityResultEvent{TState, TResult}, TResult}" />
    /// <seealso cref="IAsyncFunc{IActivityEvent{TState}, TState, TResult}" />
    partial class AsyncFunc<TState, TResult> : AsyncActivityProvider.AsyncFunc<IActivityEvent<TState>, IOperationEvent<TState>, IActivityResultEvent<TState, TResult>, TResult>, IAsyncFunc<IActivityEvent<TState>, TState, TResult>
    {
        public TState AsyncState { get; }

        /// <summary>
        /// Invokes a method that asynchronously produces a result value.
        /// </summary>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="AsyncFunc{TState, TResult}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        internal static AsyncFunc<TState, TResult> Start(TState state, [DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            AsyncFunc<TState, TResult> activity;
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

        protected override IActivityEvent<TState> CreateInitialEvent() => new ActivityEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityResultEvent<TState, TResult> CreateRanToCompletionEvent(TResult result) => new ActivityResultEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Result = result,
            Exception = null,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Information
        };

        protected override IActivityResultEvent<TState, TResult> CreateCanceledEvent() => new ResultOperationTerminatedEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Warning
        };

        protected override IActivityResultEvent<TState, TResult> CreateFaultedEvent(Exception error) => new ResultOperationTerminatedEvent<TState, TResult>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Result = default,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = StatusMessageLevel.Error
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncFunc{TState, TResult}"/> class.
        /// </summary>
        /// <param name="state">The the user-defined value that is associated with this asynchronous action.</param>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
        private AsyncFunc(TState state, [DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(owner, new(state), activityDescription, initialStatusMessage) => AsyncState = state;
    }
}
