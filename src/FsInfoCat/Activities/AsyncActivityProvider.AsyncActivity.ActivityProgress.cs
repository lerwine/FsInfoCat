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
        internal partial class AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask>
            where TTask : Task
            where TBaseEvent : IActivityEvent
            where TOperationEvent : TBaseEvent, IOperationEvent
            where TResultEvent : TBaseEvent, IActivityCompletedEvent
        {
            internal abstract class ActivityProgress<TActivity> : AsyncActivityProvider, IActivityProgress
                where TActivity : AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask>, IOperationInfo
            {
                private readonly TActivity _activity;

                public ActivityStatus StatusValue => _activity.StatusValue;

                public string CurrentOperation => _activity.CurrentOperation;

                public int PercentComplete => _activity.PercentComplete;

                public Guid ActivityId => _activity.ActivityId;

                Guid? IActivityInfo.ParentActivityId => ParentActivityId;

                public string ShortDescription => _activity.ShortDescription;

                public string StatusMessage => _activity.StatusMessage;

                public CancellationToken Token { get; }

                internal ActivityProgress([DisallowNull] TActivity activity) : base((activity ?? throw new ArgumentNullException(nameof(activity))).ParentActivityId) => (_activity, Token) = (activity, _activity.TokenSource.Token);

                protected abstract TOperationEvent CreateOperationEvent([DisallowNull] TActivity activity, Exception exception, StatusMessageLevel messageLevel);

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

                public void Report(int value)
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

                //public IDisposable SubscribeStateChange([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving)
                //{
                //    throw new NotImplementedException();
                //}

                //IEnumerator IEnumerable.GetEnumerator()
                //{
                //    throw new NotImplementedException();
                //}

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

                void IProgress<Exception>.Report(Exception value) => Report(value, false);
            }
        }
    }
}
