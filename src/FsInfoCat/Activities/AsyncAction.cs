using Microsoft.Extensions.Logging;
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
        /// Base class for asynchronous activity objects that do not produce a result value.
        /// </summary>
        /// <typeparam name="TBaseEvent">The base type for all observed <see cref="IActivityEvent"/> objects.</typeparam>
        /// <typeparam name="TOperationEvent">The type of the <typeparamref name="TBaseEvent"/> operation object which implements <see cref="IOperationEvent"/>.</typeparam>
        /// <typeparam name="TResultEvent">The type of the <typeparamref name="TBaseEvent"/> result object which implements <see cref="IActivityCompletedEvent"/>.</typeparam>
        /// <seealso cref="AsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, Task}" />
        /// <seealso cref="IAsyncAction{TBaseEvent}" />
        internal abstract class AsyncAction<TBaseEvent, TOperationEvent, TResultEvent> : AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, Task>, IAsyncAction<TBaseEvent>
            where TBaseEvent : IActivityEvent
            where TOperationEvent : TBaseEvent, IOperationEvent
            where TResultEvent : TBaseEvent, IActivityCompletedEvent
        {
            private readonly TaskCompletionSource _completionSource;

            public override Task Task => _completionSource.Task;

            /// <summary>
            /// Initializes a new instance of the <see cref="AsyncAction{TBaseEvent, TOperationEvent, TResultEvent}"/> class.
            /// </summary>
            /// <param name="owner">The owner activity provider.</param>
            /// <param name="completionSource">The task completion source.</param>
            /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
            /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> or <paramref name="completionSource"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
            protected AsyncAction([DisallowNull] AsyncActivityProvider owner, [DisallowNull] TaskCompletionSource completionSource, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(owner, activityDescription, initialStatusMessage)
            {
                _completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
            }

            /// <summary>
            /// Creates the result event that gets pushed after the activity has run to completion.
            /// </summary>
            /// <returns>A <typeparamref name="TResultEvent"/> describing an activity that has run to completion.</returns>
            /// <remarks>Implementing classes should set <see cref="IActivityStatusInfo.StatusValue"/> to <see cref="ActivityStatus.RanToCompletion"/>, <see cref="IActivityEvent.Exception"/> should be <see langword="null"/>,
            /// and <see cref="IActivityEvent.MessageLevel"/> should be <see cref="Model.StatusMessageLevel.Information"/>.</remarks>
            protected abstract TResultEvent CreateRanToCompletionEvent();

            /// <summary>
            /// Called after an asynchronous activity has been completed.
            /// </summary>
            /// <param name="task">The task that implemented the asynchronous activity.</param>
            /// <param name="node">The <see cref="LinkedListNode{IAsyncActivity}"/> that was returned by <see cref="OnStarting(IAsyncActivity)"/>,
            /// which references the <see cref="IAsyncActivity"/> that ran to completion, faulted, or was canceled.</param>
            /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="node"/> is <see langword="null"/>.</exception>
            protected void SetCompleted([DisallowNull] Task task, [DisallowNull] LinkedListNode<IAsyncActivity> node)
            {
                if (task is null) throw new ArgumentNullException(nameof(task));
                if (node is null) throw new ArgumentNullException(nameof(node));
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
                            EventSource.RaiseNext(CreateRanToCompletionEvent());
                        }
                        finally { _completionSource.SetResult(); }
                }
                finally { NotifyCompleted(node); }
            }
        }
    }

    /// <summary>
    /// Represents an asynchronous activity that does not produce a result value.
    /// </summary>
    /// <seealso cref="AsyncActivityProvider.AsyncAction{IActivityEvent, IOperationEvent, IActivityCompletedEvent}" />
    internal sealed partial class AsyncAction : AsyncActivityProvider.AsyncAction<IActivityEvent, IOperationEvent, IActivityCompletedEvent>
    {
        /// <summary>
        /// Invokes an asynchronous method.
        /// </summary>
        /// <param name="owner">The asynchronous activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="AsyncAction"/> object that can be used to monitor and/or cancel the asynchronous activity..</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        internal static AsyncAction Start([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
        {
            AsyncAction activity;
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

        /// <summary>
        /// Creates the initial event that gets pushed before an activity is started.
        /// </summary>
        /// <returns>An <see cref="IActivityEvent" /> describing an activity that is about to be started.</returns>
        protected override IActivityEvent CreateInitialEvent() => new ActivityEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            MessageLevel = Model.StatusMessageLevel.Information
        };

        /// <summary>
        /// Creates the result event that gets pushed after the activity has run to completion.
        /// </summary>
        /// <returns>An <see cref="IActivityCompletedEvent" /> describing an activity that has run to completion.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.RanToCompletion" />, <see cref="ActivityEvent.Exception" /> to <see langword="null" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="Model.StatusMessageLevel.Information" />.</remarks>
        protected override IActivityCompletedEvent CreateRanToCompletionEvent() => new ActivityCompletedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Exception = null,
            MessageLevel = Model.StatusMessageLevel.Information
        };

        /// <summary>
        /// Creates the result event that gets pushed after an activity is canceled.
        /// </summary>
        /// <returns>An <see cref="IActivityCompletedEvent" /> describing an activity that has been canceled.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.Canceled" />, <see cref="IActivityEvent.Exception" /> to <see langword="null" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="Model.StatusMessageLevel.Warning" />.</remarks>
        protected override IActivityCompletedEvent CreateCanceledEvent() => new OperationTerminatedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = Model.StatusMessageLevel.Warning
        };

        /// <summary>
        /// Creates the result event that gets pushed after an activity has faulted.
        /// </summary>
        /// <param name="error">The error that caused the activity to terminate before completion.</param>
        /// <returns>An <see cref="IActivityCompletedEvent" /> describing an activity that has terminated before completion due to an unhandled exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.Faulted" />, <see cref="IActivityEvent.Exception" /> to <paramref name="error" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="Model.StatusMessageLevel.Error" />.</remarks>
        protected override IActivityCompletedEvent CreateFaultedEvent([DisallowNull] Exception error) => new OperationTerminatedEvent
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            MessageLevel = Model.StatusMessageLevel.Error
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncAction"/> class.
        /// </summary>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
        private AsyncAction([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(owner, new(), activityDescription, initialStatusMessage) { }
    }

    /// <summary>
    /// Represents an asyncronous activity that is associated with a user-specified value and does not produce a result value.
    /// </summary>
    /// <typeparam name="TState">The type of user-specified value that is associated with the asynchronous activity.</typeparam>
    /// <seealso cref="AsyncActivityProvider.AsyncAction{TBaseEvent, TOperationEvent, TResultEvent}" />
    /// <seealso cref="IActivityEvent{TState}">TBaseEvent (IActivityEvent{<typeparamref name="TState"/>})</seealso>
    /// <seealso cref="IOperationEvent{TState}">TOperationEvent (IOperationEvent{<typeparamref name="TState"/>})</seealso>
    /// <seealso cref="IActivityCompletedEvent{TState}">TResultEvent (IActivityCompletedEvent{<typeparamref name="TState"/>})</seealso>
    /// <seealso cref="IAsyncAction{TEvent, TState}" />
    internal sealed partial class AsyncAction<TState> : AsyncActivityProvider.AsyncAction<IActivityEvent<TState>, IOperationEvent<TState>, IActivityCompletedEvent<TState>>, IAsyncAction<IActivityEvent<TState>, TState>
    {
        /// <summary>
        /// Gets the user-defined value.
        /// </summary>
        /// <value>The user-defined vaue that is associated with the activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Invokes an asynchronous method, associating it with a user-specified value.
        /// </summary>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="AsyncAction{TState}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        internal static AsyncAction<TState> Start(TState state, [DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription,
            [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            AsyncAction<TState> activity;
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

        /// <summary>
        /// Creates the initial event that gets pushed before an activity is started.
        /// </summary>
        /// <returns>An <see cref="IActivityEvent{TState}" /> describing an activity that is about to be started.</returns>
        protected override IActivityEvent<TState> CreateInitialEvent() => new ActivityEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusMessage = StatusMessage,
            Exception = null,
            AsyncState = AsyncState,
            MessageLevel = Model.StatusMessageLevel.Information
        };

        /// <summary>
        /// Creates the result event that gets pushed after the activity has run to completion.
        /// </summary>
        /// <returns>An <see cref="IActivityCompletedEvent{TState}" /> describing an activity that has run to completion.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.RanToCompletion" />, <see cref="ActivityEvent.Exception" /> to <see langword="null" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="Model.StatusMessageLevel.Information" />.</remarks>
        protected override IActivityCompletedEvent<TState> CreateRanToCompletionEvent() => new ActivityCompletedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.RanToCompletion,
            StatusMessage = StatusMessage,
            Exception = null,
            AsyncState = AsyncState,
            MessageLevel = Model.StatusMessageLevel.Information
        };

        /// <summary>
        /// Creates the result event that gets pushed after an activity is canceled.
        /// </summary>
        /// <returns>An <see cref="IActivityCompletedEvent{TState}" /> describing an activity that has been canceled.</returns>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.Canceled" />, <see cref="IActivityEvent.Exception" /> to <see langword="null" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="Model.StatusMessageLevel.Warning" />.</remarks>
        protected override IActivityCompletedEvent<TState> CreateCanceledEvent() => new OperationTerminatedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Canceled,
            StatusMessage = StatusMessage,
            Exception = null,
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = Model.StatusMessageLevel.Warning
        };

        /// <summary>
        /// Creates the result event that gets pushed after an activity has faulted.
        /// </summary>
        /// <param name="error">The error that caused the activity to terminate before completion.</param>
        /// <returns>An <see cref="IActivityCompletedEvent{TState}" /> describing an activity that that has terminated before completion due to an unhandled exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <remarks>This sets <see cref="IActivityStatusInfo.StatusValue" /> to <see cref="ActivityStatus.Faulted" />, <see cref="IActivityEvent.Exception" /> to <paramref name="error" />,
        /// and <see cref="IActivityEvent.MessageLevel" /> to <see cref="Model.StatusMessageLevel.Error" />.</remarks>
        protected override IActivityCompletedEvent<TState> CreateFaultedEvent([DisallowNull] Exception error) => new OperationTerminatedEvent<TState>
        {
            ActivityId = ActivityId,
            ParentActivityId = ParentActivityId,
            ShortDescription = ShortDescription,
            StatusValue = ActivityStatus.Faulted,
            StatusMessage = StatusMessage,
            Exception = error ?? throw new ArgumentNullException(nameof(error)),
            PercentComplete = PercentComplete,
            CurrentOperation = CurrentOperation,
            AsyncState = AsyncState,
            MessageLevel = Model.StatusMessageLevel.Error
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncAction{TState}"/> class.
        /// </summary>
        /// <param name="state">The the user-defined value that is associated with this asynchronous action.</param>
        /// <param name="owner">The owner activity provider.</param>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
        private AsyncAction(TState state, [DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            : base(owner, new(state), activityDescription, initialStatusMessage) => AsyncState = state;
    }
}
