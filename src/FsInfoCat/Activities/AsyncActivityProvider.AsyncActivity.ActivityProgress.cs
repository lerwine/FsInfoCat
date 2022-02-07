using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal partial class AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask>
            where TTask : Task
            where TBaseEvent : IActivityEvent
            where TOperationEvent : TBaseEvent, IOperationEvent
            where TResultEvent : TBaseEvent, IActivityCompletedEvent
        {
            /// <summary>
            /// The base progress reporter and nested <see cref="AsyncActivityProvider"/> for <see cref="AsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, TTask}"/> objects.
            /// </summary>
            /// <typeparam name="TActivity">The type of the related <see cref="AsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, TTask}"/> object that also implements <see cref="IOperationInfo"/>.</typeparam>
            /// <seealso cref="AsyncActivityProvider" />
            /// <seealso cref="IActivityProgress" />
            internal abstract class ActivityProgress<TActivity> : AsyncActivityProvider, IActivityProgress
                where TActivity : AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask>, IOperationInfo
            {
                private readonly TActivity _activity;

                /// <summary>
                /// Gets the activity lifecycle status value.
                /// </summary>
                /// <value>An <see cref="ActivityState" /> value that indicates the lifecycle status of the activity.</value>
                public ActivityStatus StatusValue => _activity.StatusValue;

                /// <summary>
                /// Gets the <see cref="IOperationInfo.CurrentOperation"/> of the associated <typeparamref name="TActivity"/>.
                /// </summary>
                /// <value>The description of the current operation of the many required to accomplish the activity or <see cref="string.Empty" /> if no operation has been started or no operation description has been provided.</value>
                /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.currentoperation" target="_blank">ProgressRecord.CurrentOperation</a> property
                /// and should never be <see langword="null" />.</remarks>
                public string CurrentOperation => _activity.CurrentOperation;

                /// <summary>
                /// Gets the <see cref="IOperationInfo.PercentComplete"/> of the associated <typeparamref name="TActivity"/>.
                /// </summary>
                /// <value>The estimate of the percentage of total work for the activity that is completed as a value from <c>0</c> to <c>100</c> or <c>-1</c> to indicate that the percentage completed should not be displayed.</value>
                /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.percentcomplete" target="_blank">ProgressRecord.ParentActivityId</a> property.</remarks>
                public int PercentComplete => _activity.PercentComplete;

                /// <summary>
                /// Gets the unique identifier of the associated activity.
                /// </summary>
                /// <value>The <see cref="IActivityInfo.ActivityId"/> of the associated <typeparamref name="TActivity"/>.</value>
                /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid" target="_blank">ProgressRecord.ActivityId</a> property.</remarks>
                public Guid ActivityId => _activity.ActivityId;

                Guid? IActivityInfo.ParentActivityId => ParentActivityId;

                /// <summary>
                /// Gets the <see cref="IActivityInfo.ShortDescription"/> of the associated <typeparamref name="TActivity"/>.
                /// </summary>
                /// <value>A <see cref="string" /> that containing a short description the activity.</value>
                /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activity" target="_blank">ProgressRecord.Activity</a> property
                /// and should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
                public string ShortDescription => _activity.ShortDescription;

                /// <summary>
                /// Gets the <see cref="IActivityInfo.StatusMessage"/> of the associated <typeparamref name="TActivity"/>.
                /// </summary>
                /// <value>A <see cref="string" /> that contains a short message describing current status of the activity.</value>
                /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.statusDescription" target="_blank">ProgressRecord.StatusDescription</a> property
                /// and should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
                public string StatusMessage => _activity.StatusMessage;

                /// <summary>
                /// Gets the cancellation token for the current asynchronous operation.
                /// </summary>
                /// <value>The <see cref="CancellationToken" /> for the associated <typeparamref name="TActivity"/>.</value>
                public CancellationToken Token { get; }

                /// <summary>
                /// Initializes a new instance of the <see cref="ActivityProgress{TActivity}"/> class.
                /// </summary>
                /// <param name="activity">The associated activity object.</param>
                /// <exception cref="ArgumentNullException"><paramref name="activity"/> is <see langword="null"/>.</exception>
                internal ActivityProgress([DisallowNull] TActivity activity) : base((activity ?? throw new ArgumentNullException(nameof(activity))).ParentActivityId) => (_activity, Token) = (activity, activity.TokenSource.Token);

                /// <summary>
                /// Creates an operational event object.
                /// </summary>
                /// <param name="activity">The source activity of the event.</param>
                /// <param name="exception">The exception associated with the operational event or <see langword="null"/> if there is no exception.</param>
                /// <param name="messageLevel">The message level for the operational event.</param>
                /// <returns>A <typeparamref name="TOperationEvent"/> object describing the event.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="activity"/> is <see langword="null"/>.</exception>
                protected abstract TOperationEvent CreateOperationEvent([DisallowNull] TActivity activity, Exception exception, StatusMessageLevel messageLevel);

                /// <summary>
                /// Updates the <see cref="IActivityInfo.StatusMessage" />, <see cref="IOperationInfo.CurrentOperation" /> and <see cref="IOperationInfo.PercentComplete" /> properties for the associated <typeparamref name="TActivity"/>
                /// and reports a non-fatal operation error.
                /// </summary>
                /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
                /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage" />.</param>
                /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation" />.</param>
                /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete" /> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
                /// specified.</param>
                /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning" /> if <see langword="true" />; otherwise, <see cref="StatusMessageLevel.Error" /> if <see langword="false" />.</param>
                /// <exception cref="System.ArgumentException">statusDescription</exception>
                /// <exception cref="System.ArgumentOutOfRangeException">percentComplete</exception>
                /// <exception cref="System.ArgumentNullException">error</exception>
                /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error" /> as the <see cref="IActivityEvent.Exception" />,
                /// even if the value of <see cref="IActivityInfo.StatusMessage" />, <see cref="IOperationInfo.CurrentOperation" /> or <see cref="IOperationInfo.PercentComplete" /> does not change.
                /// <para>Any white space in <paramref name="statusDescription" /> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage" />.</para><para>If <paramref name="currentOperation" /> is <see langword="null" /> it will converted to a <see cref="string.Empty" />; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void Report([DisallowNull] Exception error, string statusDescription, string currentOperation, int percentComplete, bool isWarning)
                {
                    if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                        throw new ArgumentException($"{nameof(statusDescription)} cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete < -1)
                        percentComplete = -1;
                    else if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    if (error is null)
                        throw new ArgumentNullException(nameof(error));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        _activity.StatusMessage = statusDescription;
                        _activity.PercentComplete = percentComplete;
                        _activity.CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                        operationEvent = CreateOperationEvent(_activity, error, isWarning ? StatusMessageLevel.Warning : StatusMessageLevel.Error);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IActivityInfo.StatusMessage" />, <see cref="IOperationInfo.CurrentOperation" /> and <see cref="IOperationInfo.PercentComplete" /> properties for the associated <typeparamref name="TActivity"/>
                /// and reports the changed operation status.
                /// </summary>
                /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage" />.</param>
                /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation" />.</param>
                /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete" /> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
                /// specified.</param>
                /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel" /> value of the event.</param>
                /// <exception cref="System.ArgumentException">statusDescription</exception>
                /// <exception cref="System.ArgumentOutOfRangeException">percentComplete</exception>
                /// <remarks>If the value of <see cref="IActivityInfo.StatusMessage" />, <see cref="IOperationInfo.CurrentOperation" /> or <see cref="IOperationInfo.PercentComplete" /> changes, then an activity notification event will be
                /// pushed to reflect the new operational state; otherwise, this will have no effect.
                /// <para>Any white space in <paramref name="statusDescription" /> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage" />.</para><para>If <paramref name="currentOperation" /> is <see langword="null" /> it will converted to a <see cref="string.Empty" />; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void Report([DisallowNull] string statusDescription, string currentOperation, int percentComplete, StatusMessageLevel messageLevel)
                {
                    if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                        throw new ArgumentException($"{nameof(statusDescription)} cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete < -1)
                        percentComplete = -1;
                    else if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_activity.PercentComplete == percentComplete)
                        {
                            if (_activity.StatusMessage == statusDescription)
                            {
                                if ((currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()) == _activity.CurrentOperation)
                                    return;
                                _activity.CurrentOperation = currentOperation;
                            }
                            else
                            {
                                _activity.StatusMessage = statusDescription;
                                _activity.CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                            }
                        }
                        else
                        {
                            _activity.StatusMessage = statusDescription;
                            _activity.PercentComplete = percentComplete;
                            _activity.CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                        }
                        operationEvent = CreateOperationEvent(_activity, null, messageLevel);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IActivityInfo.StatusMessage" /> and <see cref="IOperationInfo.PercentComplete" /> properties for for the associated <typeparamref name="TActivity"/> and reports a non-fatal operation error.
                /// </summary>
                /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
                /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage" />.</param>
                /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation" />.</param>
                /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning" /> if <see langword="true" />; otherwise, <see cref="StatusMessageLevel.Error" /> if <see langword="false" />.</param>
                /// <exception cref="System.ArgumentException">statusDescription</exception>
                /// <exception cref="System.ArgumentNullException">error</exception>
                /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error" /> as the <see cref="IActivityEvent.Exception" />,
                /// even if the value of <see cref="IActivityInfo.StatusMessage" /> or <see cref="IOperationInfo.CurrentOperation" /> does not change.
                /// <para>The <see cref="IOperationInfo.CurrentOperation" /> property will be set to a <see cref="string.Empty" /> on this progress object and on the pushed <see cref="IOperationEvent" />.</para><para>Any white space in <paramref name="statusDescription" /> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage" />.</para><para>If <paramref name="currentOperation" /> is <see langword="null" /> it will converted to a <see cref="string.Empty" />; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, string currentOperation, bool isWarning)
                {
                    if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                        throw new ArgumentException($"{nameof(statusDescription)} cannot be null or whitespace.", nameof(statusDescription));
                    if (error is null)
                        throw new ArgumentNullException(nameof(error));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        _activity.StatusMessage = statusDescription;
                        _activity.CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                        operationEvent = CreateOperationEvent(_activity, error, isWarning ? StatusMessageLevel.Warning : StatusMessageLevel.Error);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IActivityInfo.StatusMessage" /> and <see cref="IOperationInfo.PercentComplete" /> properties for for the associated <typeparamref name="TActivity"/> and reports a non-fatal operation error.
                /// </summary>
                /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
                /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage" />.</param>
                /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete" /> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
                /// specified.</param>
                /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning" /> if <see langword="true" />; otherwise, <see cref="StatusMessageLevel.Error" /> if <see langword="false" />.</param>
                /// <exception cref="System.ArgumentException">statusDescription</exception>
                /// <exception cref="System.ArgumentOutOfRangeException">percentComplete</exception>
                /// <exception cref="System.ArgumentNullException">error</exception>
                /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error" /> as the <see cref="IActivityEvent.Exception" />,
                /// even if the value of <see cref="IActivityInfo.StatusMessage" /> or <see cref="IOperationInfo.PercentComplete" /> does not change.
                /// <para>The <see cref="IOperationInfo.CurrentOperation" /> property will be set to a <see cref="string.Empty" /> on this progress object and on the pushed <see cref="IOperationEvent" />.</para><para>Any white space in <paramref name="statusDescription" /> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage" />.</para></remarks>
                public void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, int percentComplete, bool isWarning)
                {
                    if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                        throw new ArgumentException($"{nameof(statusDescription)} cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete < -1)
                        percentComplete = -1;
                    else if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    if (error is null)
                        throw new ArgumentNullException(nameof(error));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_activity.StatusMessage != statusDescription)
                        {
                            _activity.StatusMessage = statusDescription;
                            _activity.CurrentOperation = "";
                        }
                        _activity.PercentComplete = percentComplete;
                        operationEvent = CreateOperationEvent(_activity, error, isWarning ? StatusMessageLevel.Warning : StatusMessageLevel.Error);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IActivityInfo.StatusMessage" /> and <see cref="IOperationInfo.CurrentOperation" /> properties for the associated <typeparamref name="TActivity"/> and reports the changed operation status.
                /// </summary>
                /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage" />.</param>
                /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation" />.</param>
                /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel" /> value of the event.</param>
                /// <exception cref="System.ArgumentException">statusDescription</exception>
                /// <remarks>If the value of <see cref="IActivityInfo.StatusMessage" /> or <see cref="IOperationInfo.CurrentOperation" /> changes, then an activity notification event will be pushed to reflect the new operational state;
                /// otherwise, this will have no effect.
                /// <para>The pushed <see cref="IOperationEvent" /> will be populated with the latest <see cref="IOperationInfo.PercentComplete" /> value.</para><para>Any white space in <paramref name="statusDescription" /> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage" />.</para><para>If <paramref name="currentOperation" /> is <see langword="null" /> it will converted to a <see cref="string.Empty" />; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void Report([DisallowNull] string statusDescription, string currentOperation, StatusMessageLevel messageLevel)
                {
                    if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                        throw new ArgumentException($"{nameof(statusDescription)} cannot be null or whitespace.", nameof(statusDescription));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_activity.StatusMessage == statusDescription)
                        {
                            if ((currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()) == _activity.CurrentOperation)
                                return;
                            _activity.CurrentOperation = currentOperation;
                        }
                        else
                        {
                            _activity.StatusMessage = statusDescription;
                            _activity.CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                        }
                        operationEvent = CreateOperationEvent(_activity, null, messageLevel);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IActivityInfo.StatusMessage" /> and <see cref="IOperationInfo.PercentComplete" /> properties for the associated <typeparamref name="TActivity"/> and reports a non-fatal operation error.
                /// </summary>
                /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
                /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage" />.</param>
                /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning" /> if <see langword="true" />; otherwise, <see cref="StatusMessageLevel.Error" /> if <see langword="false" />.</param>
                /// <exception cref="System.ArgumentException">statusDescription</exception>
                /// <exception cref="System.ArgumentNullException">error</exception>
                /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error" /> as the <see cref="IActivityEvent.Exception" />,
                /// even if the value of <see cref="IOperationInfo.CurrentOperation" /> does not change.
                /// <para>The pushed <see cref="IOperationEvent" /> will be populated with the latest <see cref="IOperationInfo.PercentComplete" />, <see cref="IActivityInfo.StatusMessage" /> values.</para><para>Any white space in <paramref name="statusDescription" /> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage" />.</para></remarks>
                public void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, bool isWarning)
                {
                    if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                        throw new ArgumentException($"{nameof(statusDescription)} cannot be null or whitespace.", nameof(statusDescription));
                    if (error is null)
                        throw new ArgumentNullException(nameof(error));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_activity.StatusMessage != statusDescription)
                        {
                            _activity.StatusMessage = statusDescription;
                            _activity.CurrentOperation = "";
                        }
                        operationEvent = CreateOperationEvent(_activity, error, isWarning ? StatusMessageLevel.Warning : StatusMessageLevel.Error);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.CurrentOperation"/> properties for the associated <typeparamref name="TActivity"/> and reports the changed operation status.
                /// </summary>
                /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
                /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel"/> value of the event.</param>
                /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
                /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
                /// <remarks>If the value of <see cref="IActivityInfo.StatusMessage"/> or <see cref="IOperationInfo.CurrentOperation"/> changes, then an activity notification event will be pushed to reflect the new operational state;
                /// otherwise, this will have no effect.
                /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/> value.</para>
                /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
                /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void Report(string value, StatusMessageLevel messageLevel)
                {
                    if ((value = value.AsWsNormalizedOrEmpty()).Length == 0)
                        throw new ArgumentException($"{nameof(value)} cannot be null or whitespace.", nameof(value));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_activity.StatusMessage == value)
                            return;
                        _activity.StatusMessage = value;
                        _activity.CurrentOperation = "";
                        operationEvent = CreateOperationEvent(_activity, null, messageLevel);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IOperationInfo.CurrentOperation"/> property for the associated <typeparamref name="TActivity"/> and reports a non-fatal operation error.
                /// </summary>
                /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
                /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning"/> if <see langword="true"/>; otherwise, <see cref="StatusMessageLevel.Error"/> if <see langword="false"/>.</param>
                /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
                public void Report(Exception value, bool isWarning)
                {
                    if (value is null)
                        throw new ArgumentNullException(nameof(value));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try { operationEvent = CreateOperationEvent(_activity, value, isWarning ? StatusMessageLevel.Warning : StatusMessageLevel.Error); }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IOperationInfo.CurrentOperation" /> and <see cref="IOperationInfo.PercentComplete" /> properties for the associated <typeparamref name="TActivity"/> and reports a non-fatal operation error.
                /// </summary>
                /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
                /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation" />.</param>
                /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete" /> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
                /// specified.</param>
                /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning" /> if <see langword="true" />; otherwise, <see cref="StatusMessageLevel.Error" /> if <see langword="false" />.</param>
                /// <exception cref="System.ArgumentOutOfRangeException">percentComplete</exception>
                /// <exception cref="System.ArgumentNullException">error</exception>
                /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error" /> as the <see cref="IActivityEvent.Exception" />,
                /// even if the value of <see cref="IOperationInfo.CurrentOperation" /> or <see cref="IOperationInfo.PercentComplete" /> does not change.
                /// <para>The pushed <see cref="IOperationEvent" /> will be populated with the latest <see cref="IActivityInfo.StatusMessage" /> value.</para><para>If <paramref name="currentOperation" /> is <see langword="null" /> it will converted to a <see cref="string.Empty" />; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void ReportCurrentOperation([DisallowNull] Exception error, string currentOperation, int percentComplete, bool isWarning)
                {
                    if (percentComplete < -1)
                        percentComplete = -1;
                    else if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    if (error is null)
                        throw new ArgumentNullException(nameof(error));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        _activity.PercentComplete = percentComplete;
                        _activity.CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                        operationEvent = CreateOperationEvent(_activity, error, isWarning ? StatusMessageLevel.Warning : StatusMessageLevel.Error);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IOperationInfo.CurrentOperation" /> and <see cref="IOperationInfo.PercentComplete" /> properties for the associated <typeparamref name="TActivity"/> and reports the changed operation status.
                /// </summary>
                /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation" />.</param>
                /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete" /> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
                /// specified.</param>
                /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel" /> value of the event.</param>
                /// <exception cref="System.ArgumentOutOfRangeException">percentComplete</exception>
                /// <remarks>If the value of <see cref="IOperationInfo.CurrentOperation" /> or <paramref name="IOperationInfo.PercentComplete" /> changes, then an activity notification event will be pushed to reflect the new operational
                /// state; otherwise, this will have no effect.
                /// <para>The pushed <see cref="IOperationEvent" /> will be populated with the latest <see cref="IActivityInfo.StatusMessage" /> value.</para><para>If <paramref name="currentOperation" /> is <see langword="null" /> it will converted to a <see cref="string.Empty" />; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void ReportCurrentOperation(string currentOperation, int percentComplete, StatusMessageLevel messageLevel)
                {
                    if (percentComplete < -1)
                        percentComplete = -1;
                    else if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_activity.PercentComplete == percentComplete)
                        {
                            if ((currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()) == _activity.CurrentOperation)
                                return;
                            _activity.CurrentOperation = currentOperation;
                        }
                        else
                        {
                            _activity.PercentComplete = percentComplete;
                            _activity.CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                        }
                        operationEvent = CreateOperationEvent(_activity, null, messageLevel);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                /// <summary>
                /// Updates the <see cref="IOperationInfo.CurrentOperation" /> property for the associated <typeparamref name="TActivity"/> and reports a non-fatal operation error.
                /// </summary>
                /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
                /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation" />.</param>
                /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning" /> if <see langword="true" />; otherwise, <see cref="StatusMessageLevel.Error" /> if <see langword="false" />.</param>
                /// <exception cref="System.ArgumentNullException">error</exception>
                /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error" /> as the <see cref="IActivityEvent.Exception" />,
                /// even if the value of <see cref="IOperationInfo.CurrentOperation" /> does not change.
                /// <para>The pushed <see cref="IOperationEvent" /> will be populated with the latest <see cref="IOperationInfo.PercentComplete" />, <see cref="IActivityInfo.StatusMessage" /> values.</para><para>If <paramref name="currentOperation" /> is <see langword="null" /> it will converted to a <see cref="string.Empty" />; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void ReportCurrentOperation([DisallowNull] Exception error, string currentOperation, bool isWarning)
                {
                    if (error is null)
                        throw new ArgumentNullException(nameof(error));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        _activity.CurrentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                        operationEvent = CreateOperationEvent(_activity, error, isWarning ? StatusMessageLevel.Warning : StatusMessageLevel.Error);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                }

                /// <summary>
                /// Updates the value of updating <see cref="IOperationInfo.CurrentOperation" /> for the associated <typeparamref name="TActivity"/> and reports the changed operation status.
                /// </summary>
                /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation" />.</param>
                /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel" /> value of the event.</param>
                /// <remarks>If the value of <see cref="IOperationInfo.CurrentOperation" /> changes, then an activity notification event will be pushed to reflect the new operational state; otherwise, this will have no effect.
                /// <para>The pushed <see cref="IOperationEvent" /> will be populated with the latest <see cref="IOperationInfo.PercentComplete" />, <see cref="IActivityInfo.StatusMessage" /> values.</para><para>If <paramref name="currentOperation" /> is <see langword="null" /> it will converted to a <see cref="string.Empty" />; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
                public void ReportCurrentOperation(string currentOperation, StatusMessageLevel messageLevel)
                {
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_activity.CurrentOperation == (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()))
                            return;
                        _activity.CurrentOperation = currentOperation;
                        operationEvent = CreateOperationEvent(_activity, null, messageLevel);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                void IActivityProgress.Report(Exception error, string statusDescription, string currentOperation, int percentComplete) => Report(error, statusDescription, currentOperation, percentComplete, false);

                void IActivityProgress.Report(string statusDescription, string currentOperation, int percentComplete) => Report(statusDescription, currentOperation, percentComplete, StatusMessageLevel.Information);

                void IActivityProgress.Report(Exception error, string statusDescription, string currentOperation) => Report(error, statusDescription, currentOperation, false);

                void IActivityProgress.Report(Exception error, string statusDescription, int percentComplete) => Report(error, statusDescription, percentComplete, false);

                void IActivityProgress.Report(string statusDescription, string currentOperation) => Report(statusDescription, currentOperation, StatusMessageLevel.Information);

                void IActivityProgress.Report(Exception error, string statusDescription) => Report(error, statusDescription, false);

                void IActivityProgress.ReportCurrentOperation(Exception error, string currentOperation, int percentComplete) => ReportCurrentOperation(error, currentOperation, percentComplete, false);

                void IActivityProgress.ReportCurrentOperation(string currentOperation, int percentComplete) => ReportCurrentOperation(currentOperation, percentComplete, StatusMessageLevel.Information);

                void IActivityProgress.ReportCurrentOperation(Exception error, string currentOperation) => ReportCurrentOperation(error, currentOperation, false);

                void IActivityProgress.ReportCurrentOperation(string currentOperation) => ReportCurrentOperation(currentOperation, StatusMessageLevel.Information);

                void IProgress<string>.Report(string value) => Report(value, StatusMessageLevel.Information);

                void IProgress<int>.Report(int value)
                {
                    if (value < -1)
                        value = -1;
                    else if (value > 100)
                        throw new ArgumentOutOfRangeException(nameof(value));
                    TOperationEvent operationEvent;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_activity.PercentComplete == value)
                            return;
                        _activity.PercentComplete = value;
                        operationEvent = CreateOperationEvent(_activity, null, StatusMessageLevel.Information);
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    _activity.EventSource.RaiseNext(operationEvent);
                }

                void IProgress<Exception>.Report(Exception value) => Report(value, false);
            }
        }
    }
}
