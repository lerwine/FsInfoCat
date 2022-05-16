using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        /// <summary>
        /// Base class for asynchronous activity objects.
        /// </summary>
        /// <typeparam name="TBaseEvent">The base <see cref="IActivityEvent"/> type for all progress update objects.</typeparam>
        /// <typeparam name="TOperationEvent">The type of the <typeparamref name="TBaseEvent"/> object which implements <see cref="IOperationEvent"/> for active operation events.</typeparam>
        /// <typeparam name="TResultEvent">The type of the <typeparamref name="TBaseEvent"/> operation result object which implements <see cref="IActivityCompletedEvent"/>.</typeparam>
        /// <typeparam name="TTask">The type of the <see cref="Task"/> that implements the activity.</typeparam>
        /// <seealso cref="IAsyncActivity" />
        /// <seealso cref="IObservable{TBaseEvent}" />
        internal abstract partial class AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask> : IAsyncActivity, IObservable<TBaseEvent>
            where TTask : Task
            where TBaseEvent : IActivityEvent
            where TOperationEvent : TBaseEvent, IOperationEvent
            where TResultEvent : TBaseEvent, IActivityCompletedEvent
        {
            private readonly AsyncActivityProvider _owner;

            /// <summary>
            /// Gets the task for the asyncronous activity.
            /// </summary>
            /// <value>The task for the asyncronous activity.</value>
            public abstract TTask Task { get; }

            Task IAsyncActivity.Task => Task;

            /// <summary>
            /// Gets the token source that can be used to cancel the asyncronous activity.
            /// </summary>
            /// <value>The token source that can be used to cancel the asyncronous activity.</value>
            public CancellationTokenSource TokenSource { get; } = new();

            /// <summary>
            /// Gets the activity lifecycle status value.
            /// </summary>
            /// <value>An <see cref="ActivityStatus" /> value that indicates the lifecycle status of the activity.</value>
            public ActivityStatus StatusValue { get; protected set; } = ActivityStatus.WaitingToRun;

            /// <summary>
            /// Gets the unique identifier of the current activity.
            /// </summary>
            /// <value>The <see cref="Guid" /> value that is unique to the described activity.</value>
            /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid" target="_blank">ProgressRecord.ActivityId</a> property.</remarks>
            public Guid ActivityId { get; } = Guid.NewGuid();

            /// <summary>
            /// Gets the <see cref="IActivityInfo.ParentActivityId" /> value of the owner <see cref="AsyncActivityProvider"/>.
            /// </summary>
            /// <value>The <see cref="Guid" /> value that is unique to the parent activity or <see langword="null" /> if there is no parent activity.</value>
            /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.parentactivityid" target="_blank">ProgressRecord.ParentActivityId</a> property.</remarks>
            public Guid? ParentActivityId => _owner.ParentActivityId;

            /// <summary>
            /// Gets the short description of the current activity.
            /// </summary>
            /// <value>A <see cref="string" /> that describes the activity.</value>
            /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activity" target="_blank">ProgressRecord.Activity</a> property
            /// and should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
            public string ShortDescription { get; }

            /// <summary>
            /// Gets the description of the activity's current status.
            /// </summary>
            /// <value>A <see cref="string" /> that contains a short message describing current status of the activity.</value>
            /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.statusDescription" target="_blank">ProgressRecord.StatusDescription</a> property
            /// and should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
            public string StatusMessage { get; private set; }

            /// <summary>
            /// Gets the description of the current operation of the many required to accomplish the activity.
            /// </summary>
            /// <value>The description of the current operation being performed or <see cref="string.Empty" /> if no operation has been started or no operation description has been provided.</value>
            /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.currentoperation" target="_blank">ProgressRecord.CurrentOperation</a> property
            /// and should never be <see langword="null" />.</remarks>
            public string CurrentOperation { get; private set; } = "";

            /// <summary>
            /// Gets and sets the estimate of the percentage of total work for the activity that is completed.
            /// </summary>
            /// <value>The estimated percentage completion value from <c>0</c> to <c>100</c> or <c>-1</c> to indicate that the percentage completed should not be displayed.</value>
            /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.percentcomplete" target="_blank">ProgressRecord.ParentActivityId</a> property.</remarks>
            public int PercentComplete { get; private set; } = -1;

            /// <summary>
            /// Gets the source object for progress update events.
            /// </summary>
            /// <value>The source for pushing progress update events.</value>
            protected Observable<TBaseEvent>.Source EventSource { get; } = new();

            /// <summary>
            /// Gets the <c><see cref="IObservable{T}">IObservable</see>&lt;<see cref="IAsyncActivity"/>&gt;</c> of the owner <see cref="AsyncActivityProvider"/>.
            /// </summary>
            /// <value>The provider that can be used to subscribe for activity start notifications.</value>
            public IObservable<IAsyncActivity> ActivityStartedObservable => _owner.ActivityStartedObservable;

            /// <summary>
            /// Gets the number of activities the owner <see cref="AsyncActivityProvider"/> is running.
            /// </summary>
            /// <value>The count of <see cref="IAsyncActivity"/> objects representing activities that the owner <see cref="AsyncActivityProvider"/> is runnning.</value>
            public int Count => _owner.Count;

            protected ILogger Logger { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="AsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, TTask}"/> class.
            /// </summary>
            /// <param name="owner">The owner activity provider.</param>
            /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
            /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
            protected AsyncActivity([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
            {
                Logger = (_owner = owner ?? throw new ArgumentNullException(nameof(owner))).Logger;
                if (activityDescription is null || (ShortDescription = activityDescription.Trim()).Length == 0)
                    throw new ArgumentException($"'{nameof(activityDescription)}' cannot be null or whitespace.", nameof(activityDescription));
                if (initialStatusMessage is null || (StatusMessage = initialStatusMessage.Trim()).Length == 0)
                    throw new ArgumentException($"'{nameof(initialStatusMessage)}' cannot be null or whitespace.", nameof(initialStatusMessage));
            }

            /// <summary>
            /// Notifies this provider that an observer is to receive progress update notifications.
            /// </summary>
            /// <param name="observer">The object that is to receive progress update notifications.</param>
            /// <returns>A reference to an interface that allows observers to stop receiving progress update notifications before the provider has finished sending them.</returns>
            public IDisposable Subscribe(IObserver<TBaseEvent> observer) => EventSource.Observable.Subscribe(observer);

            /// <summary>
            /// Notifies the owning <see cref="AsyncActivityProvider"/> that the current activity is starting.
            /// </summary>
            /// <returns>The <see cref="LinkedListNode{IAsyncActivity}"/> that was appended to the underlying list of the parent <see cref="AsyncActivityProvider"/>.</returns>
            /// <remarks>This appends the activity to the underlying list if the parent <see cref="AsyncActivityProvider"/> and should only be called when the current thread has an exclusive <see cref="Monitor"/> lock on the owning
            /// provider's <see cref="SyncRoot"/>.</remarks>
            protected LinkedListNode<IAsyncActivity> OnStarting() => _owner.OnStarting(this);

            /// <summary>
            /// Called when the associated task is about to be run.
            /// </summary>
            protected virtual void OnBeforeAwaitTask()
            {
                Logger.LogDebug("Setting activity {ActivityId} StatusValue to {StatusValue}", ActivityId, ActivityStatus.Running);
                StatusValue = ActivityStatus.Running;
                TBaseEvent initialEvent = CreateInitialEvent();
                Logger.LogDebug(@"Raising initial event ActivityId={ActivityId}; ParentActivityId={ParentActivityId}; MessageLevel={MessageLevel}
ShortDescription={ShortDescription}
StatusMessage={StatusMessage}
Exception={Exception}", initialEvent.ActivityId, initialEvent.ParentActivityId, initialEvent.ShortDescription, initialEvent.MessageLevel, initialEvent.StatusMessage, initialEvent.Exception);
                EventSource.RaiseNext(initialEvent);
                Logger.LogDebug("Raising activity {ActivityId} Started event; ParentActivityId={ParentActivityId}", ActivityId, ParentActivityId);
                _owner.ActivityStartedSource.RaiseNext(this);
            }

            /// <summary>
            /// Creates the initial event that gets pushed before an activity is started.
            /// </summary>
            /// <returns>A <typeparamref name="TBaseEvent"/> describing an activity that is about to be started.</returns>
            protected abstract TBaseEvent CreateInitialEvent();

            /// <summary>
            /// Creates the result event that gets pushed after an activity is canceled.
            /// </summary>
            /// <returns>A <typeparamref name="TResultEvent"/> describing an activity that has been canceled.</returns>
            /// <remarks>Implementing classes should set <see cref="IActivityStatusInfo.StatusValue"/> to <see cref="ActivityStatus.Canceled"/>, <see cref="IActivityEvent.Exception"/> should be <see langword="null"/>,
            /// and <see cref="IActivityEvent.MessageLevel"/> should be <see cref="StatusMessageLevel.Warning"/>.</remarks>
            protected abstract TResultEvent CreateCanceledEvent();

            /// <summary>
            /// Creates the result event that gets pushed after an activity has faulted.
            /// </summary>
            /// <param name="error">The error that caused the activity to terminate before completion.</param>
            /// <returns>A <typeparamref name="TResultEvent"/> describing an activity that that has terminated before completion due to an unhandled exception.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
            /// <remarks>Implementing classes should set <see cref="IActivityStatusInfo.StatusValue"/> to <see cref="ActivityStatus.Faulted"/>, <see cref="IActivityEvent.Exception"/> should have the value of <paramref name="error"/>,
            /// and <see cref="IActivityEvent.MessageLevel"/> should be <see cref="StatusMessageLevel.Error"/>.</remarks>
            protected abstract TResultEvent CreateFaultedEvent([DisallowNull] Exception error);

            /// <summary>
            /// Notifies owner <see cref="AsyncActivityProvider"/> than an <see cref="IAsyncActivity"/> has been completed.
            /// </summary>
            /// <param name="node">The <c><see cref="LinkedListNode{T}">"LinkedListNode</see>&lt;<see cref="IAsyncActivity"/>&gt;</c> that was returned by <see cref="OnStarting()"/> which references the <see cref="IAsyncActivity"/> that ran to completion, faulted, or was canceled.</param>
            /// <remarks>This obtains an exclusive <see cref="Monitor"/> lock on <see cref="SyncRoot"/> and removes the specified <paramref name="node"/> from the underlying list.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            protected void NotifyCompleted([DisallowNull] LinkedListNode<IAsyncActivity> node)
            {
                try { _owner.OnCompleted(node); }
                finally
                {
                    Logger.LogDebug("Disposing event source; ActivityId={ActivityId}", ActivityId);
                    EventSource.Dispose();
                }
            }

            /// <summary>
            /// Notifies this activity source that an observer is to receive nested activity start notifications, providing a list of existing nested activities.
            /// </summary>
            /// <param name="observer">The object that is to receive activity start notifications.</param>
            /// <param name="onObserving">The callback method that provides a list of existing activities immediately before the observer is registered to receive activity start notifications.</param>
            /// <returns>A reference to an interface that allows observers to stop receiving notifications before this has finished sending them.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="observer"/> is <see langword="null"/>.</exception>
            public IDisposable SubscribeChildActivityStart([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving) => _owner.SubscribeChildActivityStart(observer, onObserving);

            /// <summary>
            /// Returns an enumerator that iterates through the running activities started by this activity.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the running <see cref="IAsyncActivity"/> objects started by this activity.</returns>
            public IEnumerator<IAsyncActivity> GetEnumerator() => _owner.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_owner).GetEnumerator();
        }
    }
}
