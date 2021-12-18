using FsInfoCat.AsyncOps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        abstract class BackgroundOperationInfo : IBackgroundOperation
        {
            private bool _isDisposed;
            private readonly object _syncRoot = new();
            private readonly LinkedList<BackgroundOperationInfo> _operations = new();
            private readonly BackgroundProgressService _service;
            private CancellationTokenSource _tokenSource = new();
            private CancellationTokenSource _linkedTokenSource;

            protected CancellationToken Token => _linkedTokenSource.Token;

            public bool IsCancellationRequested => _linkedTokenSource.IsCancellationRequested;

            public abstract Guid OperationId { get; }

            public abstract string Activity { get; }

            public abstract string StatusDescription { get; }

            public abstract string CurrentOperation { get; }

            public abstract Guid? ParentId { get; }

            public abstract byte? PercentComplete { get; }

            public abstract Task Task { get; }

            protected BackgroundOperationInfo([DisallowNull] BackgroundProgressService service, CancellationToken[] linkedTokens)
            {
                _service = service ?? throw new ArgumentNullException(nameof(service));
                if (linkedTokens is not null && linkedTokens.Length > 0)
                    _linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(Enumerable.Repeat(_tokenSource.Token, 1).Concat(linkedTokens).ToArray());
                else
                    _linkedTokenSource = _tokenSource;
            }

            [Obsolete("use constructor with BackgroundProgressService service parameter")]
            protected BackgroundOperationInfo(CancellationToken[] linkedTokens)
            {
                if (linkedTokens is not null && linkedTokens.Length > 0)
                    _linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(Enumerable.Repeat(_tokenSource.Token, 1).Concat(linkedTokens).ToArray());
                else
                    _linkedTokenSource = _tokenSource;
            }

            protected abstract void RaiseOperationCanceled();

            protected abstract void RaiseOperationFaulted([DisallowNull] Exception exception);

            public void Cancel() => _tokenSource.Cancel();

            public void Cancel(bool throwOnFirstException) => _tokenSource.Cancel(throwOnFirstException);

            public void CancelAfter(int millisecondsDelay) => _tokenSource.CancelAfter(millisecondsDelay);

            public void CancelAfter(TimeSpan delay) => _tokenSource.CancelAfter(delay);

            protected abstract IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer);

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer) => BaseSubscribe(observer);

            protected virtual void Dispose(bool disposing)
            {
                if (!_isDisposed)
                {
                    if (disposing)
                    {
                        if (!ReferenceEquals(_tokenSource, _linkedTokenSource))
                            _linkedTokenSource.Dispose();
                        _tokenSource.Dispose();
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    _isDisposed = true;
                }
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            // ~BackgroundOperation()
            // {
            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //     Dispose(disposing: false);
            // }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            internal abstract class BackgroundProgressBase : IBackgroundProgressInfo, IBackgroundProgressFactory
            {
                private readonly BackgroundOperationInfo _operation;

                protected object SyncRoot { get; }

                public Guid OperationId { get; } = Guid.NewGuid();

                public string Activity { get; }

                public string StatusDescription { get; private set; }

                public string CurrentOperation { get; private set; }

                public Guid? ParentId { get; }

                public byte? PercentComplete { get; private set; }

                protected BackgroundProgressBase([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId)
                {
                    _operation = operation ?? throw new ArgumentNullException(nameof(operation));
                    if (string.IsNullOrWhiteSpace(activity))
                        throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
                    if (string.IsNullOrWhiteSpace(initialStatusDescription))
                        throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
                    Activity = activity;
                    StatusDescription = initialStatusDescription;
                    ParentId = parentId;
                }

                [Obsolete("Use constructor with BackgroundOperationInfo operation parameter")]
                protected BackgroundProgressBase([DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId)
                {
                    if (string.IsNullOrWhiteSpace(activity))
                        throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
                    if (string.IsNullOrWhiteSpace(initialStatusDescription))
                        throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
                    Activity = activity;
                    StatusDescription = initialStatusDescription;
                    ParentId = parentId;
                }

                protected abstract void OnProgressChanged([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error);

                protected bool SetStatus([DisallowNull] IBackgroundProgressEvent progressEvent)
                {
                    byte? percentComplete = (progressEvent ?? throw new ArgumentNullException(nameof(progressEvent))).PercentComplete;
                    string statusDescription = progressEvent.StatusDescription;
                    string currentOperation = progressEvent.CurrentOperation.EmptyIfNullOrWhiteSpace();
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(progressEvent)}.{nameof(IBackgroundProgressEvent.StatusDescription)}' cannot be null or whitespace.",
                            nameof(progressEvent));
                    if (percentComplete.HasValue && percentComplete.Value > 100)
                        throw new ArgumentException($"'{nameof(progressEvent)}.{nameof(IBackgroundProgressEvent.PercentComplete)}' cannot be greater than 100.", nameof(progressEvent));
                    lock (SyncRoot)
                    {
                        if (StatusDescription == statusDescription && CurrentOperation == currentOperation && percentComplete == PercentComplete)
                            return (progressEvent is IBackgroundOperationErrorOptEvent errorOpt && errorOpt.Error is not null) || progressEvent.Code.HasValue;
                        StatusDescription = statusDescription;
                        CurrentOperation = currentOperation;
                        PercentComplete = percentComplete;
                    }
                    return true;
                }

                public void ReportCurrentOperation([DisallowNull] string currentOperation, MessageCode code, byte percentComplete)
                {
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    string statusDescription;
                    lock (SyncRoot)
                    {
                        statusDescription = StatusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = currentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                    }
                    OnProgressChanged(statusDescription, currentOperation, code, percentComplete, null);
                }

                public void ReportCurrentOperation([DisallowNull] string currentOperation, MessageCode code)
                {
                    string statusDescription;
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        statusDescription = StatusDescription;
                        percentComplete = PercentComplete;
                        CurrentOperation = currentOperation = currentOperation.EmptyIfNullOrWhiteSpace();
                    }
                    OnProgressChanged(statusDescription, currentOperation, code, percentComplete, null);
                }

                public void ReportCurrentOperation([DisallowNull] string currentOperation, byte percentComplete)
                {
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    string statusDescription;
                    lock (SyncRoot)
                    {
                        if (CurrentOperation == (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()) && percentComplete == PercentComplete)
                            return;
                        statusDescription = StatusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = currentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, null);
                }

                public void ReportCurrentOperation([DisallowNull] string currentOperation)
                {
                    string statusDescription;
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        if (CurrentOperation == (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()))
                            return;
                        statusDescription = StatusDescription;
                        percentComplete = PercentComplete;
                        CurrentOperation = currentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, null);
                }

                public void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode code,
                    byte percentComplete)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace());
                    }
                    OnProgressChanged(statusDescription, currentOperation, code, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode code)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        StatusDescription = statusDescription;
                        percentComplete = PercentComplete;
                        CurrentOperation = (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace());
                    }
                    OnProgressChanged(statusDescription, currentOperation, code, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, byte percentComplete)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace());
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, MessageCode code, byte percentComplete)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = "";
                    }
                    OnProgressChanged(statusDescription, "", code, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, MessageCode code)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        percentComplete = PercentComplete;
                        StatusDescription = statusDescription;
                        CurrentOperation = "";
                    }
                    OnProgressChanged(statusDescription, "", code, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, byte percentComplete)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = "";
                    }
                    OnProgressChanged(statusDescription, "", null, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, MessageCode code, byte percentComplete)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    string statusDescription, currentOperation;
                    lock (SyncRoot)
                    {
                        PercentComplete = percentComplete;
                        statusDescription = StatusDescription;
                        currentOperation = CurrentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, code, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, MessageCode code)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    byte? percentComplete;
                    string statusDescription, currentOperation;
                    lock (SyncRoot)
                    {
                        percentComplete = PercentComplete;
                        statusDescription = StatusDescription;
                        currentOperation = CurrentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, code, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, byte percentComplete)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    string statusDescription, currentOperation;
                    lock (SyncRoot)
                    {
                        PercentComplete = percentComplete;
                        statusDescription = StatusDescription;
                        currentOperation = CurrentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, [DisallowNull] string currentOperation)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        percentComplete = PercentComplete;
                        StatusDescription = statusDescription;
                        CurrentOperation = (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace());
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        percentComplete = PercentComplete;
                        StatusDescription = statusDescription;
                        CurrentOperation = "";
                    }
                    OnProgressChanged(statusDescription, "", null, percentComplete, exception);
                }

                public void ReportException([DisallowNull] Exception exception)
                {
                    if (exception is null)
                        throw new ArgumentNullException(nameof(exception));
                    string statusDescription, currentOperation;
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        percentComplete = PercentComplete;
                        statusDescription = StatusDescription;
                        currentOperation = CurrentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, exception);
                }

                public void ReportStatusDescription([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode code, byte percentComplete)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace());
                    }
                    OnProgressChanged(statusDescription, currentOperation, code, percentComplete, null);
                }

                public void ReportStatusDescription([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode code)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        percentComplete = PercentComplete;
                        StatusDescription = statusDescription;
                        CurrentOperation = (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace());
                    }
                    OnProgressChanged(statusDescription, currentOperation, code, percentComplete, null);
                }

                public void ReportStatusDescription([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, byte percentComplete)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        if (CurrentOperation == (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()) && statusDescription == StatusDescription &&
                                PercentComplete == percentComplete)
                            return;
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = currentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, null);
                }

                public void ReportStatusDescription([DisallowNull] string statusDescription, MessageCode code, byte percentComplete)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = "";
                    }
                    OnProgressChanged(statusDescription, "", code, percentComplete, null);
                }

                public void ReportStatusDescription([DisallowNull] string statusDescription, MessageCode code)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        percentComplete = PercentComplete;
                        StatusDescription = statusDescription;
                        CurrentOperation = "";
                    }
                    OnProgressChanged(statusDescription, "", code, percentComplete, null);
                }

                public void ReportStatusDescription([DisallowNull] string statusDescription, byte percentComplete)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        if (statusDescription == StatusDescription && PercentComplete == percentComplete)
                            return;
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = "";
                    }
                    OnProgressChanged(statusDescription, "", null, percentComplete, null);
                }

                public void ReportStatusDescription([DisallowNull] string statusDescription, [DisallowNull] string currentOperation)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        if (CurrentOperation == (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()) && statusDescription == StatusDescription)
                            return;
                        percentComplete = PercentComplete;
                        StatusDescription = statusDescription;
                        CurrentOperation = currentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, null);
                }

                public void ReportStatusDescription([DisallowNull] string statusDescription)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    byte? percentComplete;
                    lock (SyncRoot)
                    {
                        if (statusDescription == StatusDescription)
                            return;
                        percentComplete = PercentComplete;
                        CurrentOperation = "";
                    }
                    OnProgressChanged(statusDescription, "", null, percentComplete, null);
                }

                public void ReportPercentComplete(byte percentComplete)
                {
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    string statusDescription, currentOperation;
                    lock (SyncRoot)
                    {
                        if (PercentComplete == percentComplete)
                            return;
                        statusDescription = StatusDescription;
                        PercentComplete = percentComplete;
                        currentOperation = CurrentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, null);
                }

                #region IBackgroundProgressFactory members

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(
                    [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                    [DisallowNull] Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
                    => TimedBackgroundOperation<TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, onCompleted, tokens);

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(
                    [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                    [DisallowNull] Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state)
                    => TimedBackgroundOperation<TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, onCompleted);

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(
                    [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
                    => TimedBackgroundOperation<TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, null, tokens);

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(
                    [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state)
                    => TimedBackgroundOperation<TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, null);

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
                    [DisallowNull] Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, params CancellationToken[] tokens)
                    => TimedBackgroundOperation.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, onCompleted, tokens);

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
                    [DisallowNull] Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription)
                    => TimedBackgroundOperation.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, onCompleted);

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
                    [DisallowNull] string activity, [DisallowNull] string statusDescription, params CancellationToken[] tokens)
                    => TimedBackgroundOperation.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, null, tokens);

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
                    [DisallowNull] string activity, [DisallowNull] string statusDescription)
                    => TimedBackgroundOperation.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, null);

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(
                    [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                    [DisallowNull] Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
                    => BackgroundOperation<TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, onCompleted, tokens);

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(
                    [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                    Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state)
                    => BackgroundOperation<TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, onCompleted);

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(
                    [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
                    => BackgroundOperation<TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, null, tokens);

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(
                    [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state)
                    => BackgroundOperation<TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, null);

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
                   [DisallowNull] Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity, [DisallowNull] string statusDescription,
                   params CancellationToken[] tokens)
                    => BackgroundOperation.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, onCompleted, tokens);

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
                   [DisallowNull] Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity, [DisallowNull] string statusDescription)
                    => BackgroundOperation.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, onCompleted);

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
                    [DisallowNull] string activity, [DisallowNull] string statusDescription, params CancellationToken[] tokens)
                    => BackgroundOperation.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, null, tokens);

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
                    [DisallowNull] string activity, [DisallowNull] string statusDescription)
                    => BackgroundOperation.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, null);

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(
                    [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                    [DisallowNull] Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
                    => TimedBackgroundFunc<TState, TResult>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, onCompleted, tokens);

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(
                    [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                    [DisallowNull] Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state)
                    => TimedBackgroundFunc<TState, TResult>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, onCompleted);

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(
                    [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
                    => TimedBackgroundFunc<TState, TResult>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, null, tokens);

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(
                    [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state)
                    => TimedBackgroundFunc<TState, TResult>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, null);

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(
                    [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                    [DisallowNull] Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, params CancellationToken[] tokens)
                    => TimedBackgroundFunc<TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, onCompleted, tokens);

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(
                    [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                    [DisallowNull] Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription)
                    => TimedBackgroundFunc<TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, onCompleted);

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(
                    [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, params CancellationToken[] tokens)
                    => TimedBackgroundFunc<TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, null, tokens);

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(
                    [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription)
                    => TimedBackgroundFunc<TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, null);

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(
                    [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                    [DisallowNull] Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
                    => BackgroundFunc<TState, TResult>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, onCompleted, tokens);

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(
                    [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                   [DisallowNull] Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
                   [DisallowNull] string statusDescription, TState state)
                    => BackgroundFunc<TState, TResult>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, onCompleted);

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(
                    [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
                    => BackgroundFunc<TState, TResult>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, null, tokens);

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(
                    [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, TState state)
                    => BackgroundFunc<TState, TResult>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, null);

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(
                    [DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                    [DisallowNull] Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, params CancellationToken[] tokens)
                    => BackgroundFunc<TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, onCompleted, tokens);

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(
                    [DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                    [DisallowNull] Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription)
                    => BackgroundFunc<TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, onCompleted);

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(
                    [DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription, params CancellationToken[] tokens)
                    => BackgroundFunc<TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, null, tokens);

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(
                    [DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
                    [DisallowNull] string statusDescription)
                    => BackgroundFunc<TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, null);

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TState>(
                    Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, string activity, string statusDescription,
                    TState state, params CancellationToken[] tokens)
                    => TimedBackgroundOperation<TEvent, TResultEvent, TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, eventFactory, tokens);

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TState>(
                    Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, string activity, string statusDescription,
                    TState state)
                    => TimedBackgroundOperation<TEvent, TResultEvent, TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, eventFactory);

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent>(Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, string activity, string statusDescription,
                    params CancellationToken[] tokens)
                    => TimedBackgroundOperation<TEvent, TResultEvent>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, eventFactory, tokens);

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent>(Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, string activity, string statusDescription)
                    => TimedBackgroundOperation<TEvent, TResultEvent>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, eventFactory);

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TState>(
                    Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, string activity, string statusDescription,
                    TState state, params CancellationToken[] tokens)
                    => BackgroundOperation<TEvent, TResultEvent, TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, eventFactory, tokens);

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TState>(
                    Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, string activity, string statusDescription,
                    TState state)
                    => BackgroundOperation<TEvent, TResultEvent, TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, eventFactory);

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent>(Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, string activity, string statusDescription,
                    params CancellationToken[] tokens)
                    => BackgroundOperation<TEvent, TResultEvent>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, eventFactory, tokens);

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent>(Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, string activity, string statusDescription)
                    => BackgroundOperation<TEvent, TResultEvent>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, eventFactory);

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TState, TResult>(
                    Func<ITimedBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, string activity,
                    string statusDescription, TState state, params CancellationToken[] tokens)
                    => TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, eventFactory, tokens);

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TState, TResult>(
                    Func<ITimedBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, string activity,
                    string statusDescription, TState state)
                    => TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, eventFactory);

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TResult>(
                    Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, string activity, string statusDescription,
                    params CancellationToken[] tokens)
                    => TimedBackgroundFunc<TEvent, TResultEvent, TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, eventFactory, tokens);

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TResult>(
                    Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, string activity, string statusDescription)
                    => TimedBackgroundFunc<TEvent, TResultEvent, TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, eventFactory);

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TState, TResult>(
                    Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, string activity, string statusDescription,
                    TState state, params CancellationToken[] tokens)
                    => BackgroundFunc<TEvent, TResultEvent, TResult, TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, eventFactory, tokens);

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TState, TResult>(
                    Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, string activity, string statusDescription,
                    TState state)
                    => BackgroundFunc<TEvent, TResultEvent, TResult, TState>.Start(_operation._service, _operation, activity, statusDescription, state, asyncMethodDelegate, eventFactory);

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TResult>(
                    Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, string activity, string statusDescription,
                    params CancellationToken[] tokens)
                    => BackgroundFunc<TEvent, TResultEvent, TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, eventFactory, tokens);

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TEvent, TResultEvent, TResult>(
                    Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
                    IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, string activity, string statusDescription)
                    => BackgroundFunc<TEvent, TResultEvent, TResult>.Start(_operation._service, _operation, activity, statusDescription, asyncMethodDelegate, eventFactory);

                #endregion
            }

            protected static class OperationHelper
            {
                private static LinkedListNode<BackgroundOperationInfo> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent,
                    [DisallowNull] BackgroundOperationInfo backgroundOperation, out Action onStarted)
                {
                    LinkedListNode<BackgroundOperationInfo> node;
                    bool isFirstOperation;
                    lock (service._syncRoot)
                    {
                        if (parent is null)
                        {
                            isFirstOperation = service._operations.Last is null;
                            node = service._operations.AddLast(backgroundOperation);
                        }
                        else
                        {
                            isFirstOperation = false;
                            node = parent._operations.AddLast(backgroundOperation);
                        }
                    }
                    onStarted = () =>
                    {
                        try
                        {
                            service.IsActive = true;
                            if (isFirstOperation)
                                service._activeStatusObservers.RaiseActiveStateChanged(true);
                        }
                        finally { service._stateEventObservers.RaiseStateChanged(new BackgroundProcessStartedEventArgs(backgroundOperation, null)); }
                    };
                    return node;
                }

                private static bool OnCompleted([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] LinkedListNode<BackgroundOperationInfo> node)
                {
                    lock (service._syncRoot)
                    {
                        if (parent is null)
                        {
                            service._operations.Remove(node);
                            return service._operations.Last is null;
                        }
                        parent._operations.Remove(node);
                    }
                    return false;
                }

                private static void RaiseCanceled([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] LinkedListNode<BackgroundOperationInfo> node,
                    [DisallowNull] Action onRemoved)
                {
                    bool isLastOperation = OnCompleted(service, parent, node);
                    try { onRemoved(); }
                    finally
                    {
                        try
                        {
                            if (isLastOperation && service._operations.First is null)
                            {
                                service.IsActive = false;
                                service._activeStatusObservers.RaiseActiveStateChanged(false);
                            }
                        }
                        finally { service._stateEventObservers.RaiseStateChanged(new BackgroundProcessCompletedEventArgs(node.Value, null, null, false)); }
                    }
                }

                private static void RaiseFaulted([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] LinkedListNode<BackgroundOperationInfo> node,
                    [DisallowNull] AggregateException exception, [DisallowNull] Action onRemoved)
                {
                    bool isLastOperation = OnCompleted(service, parent, node);
                    try { onRemoved(); }
                    finally
                    {
                        try
                        {
                            if (isLastOperation && service._operations.First is null)
                            {
                                service.IsActive = false;
                                service._activeStatusObservers.RaiseActiveStateChanged(false);
                            }
                        }
                        finally
                        {
                            if (exception.InnerException is AsyncOperationException asyncFailureException)
                                service._stateEventObservers.RaiseStateChanged(new BackgroundProcessFaultedEventArgs(node.Value,
                                    (exception.InnerExceptions.Count == 1) ? asyncFailureException : exception, asyncFailureException.Code));
                            else
#pragma warning disable CS8604 // Possible null reference argument.
                                service._stateEventObservers.RaiseStateChanged(new BackgroundProcessFaultedEventArgs(node.Value,
                                    (exception.InnerExceptions.Count == 1) ? exception.InnerException : exception, ErrorCode.Unexpected));
#pragma warning restore CS8604 // Possible null reference argument.
                        }
                    }
                }

                private static void RaiseRanToCompletion([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] LinkedListNode<BackgroundOperationInfo> node,
                    [DisallowNull] Action onRemoved)
                {
                    bool isLastOperation = OnCompleted(service, parent, node);
                    try { onRemoved(); }
                    finally
                    {
                        try
                        {
                            if (isLastOperation && service._operations.First is null)
                            {
                                service.IsActive = false;
                                service._activeStatusObservers.RaiseActiveStateChanged(false);
                            }
                        }
                        finally { service._stateEventObservers.RaiseStateChanged(new BackgroundProcessCompletedEventArgs(node.Value, null, null, true)); }
                    }
                }

                private static void OnActionCompletion([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent,
                    [DisallowNull] LinkedListNode<BackgroundOperationInfo> node, [DisallowNull] TaskCompletionSource completionSource,
                    [DisallowNull] Action raiseRanToCompletion, [DisallowNull] Task task)
                {
                    // TODO: Use linked list from parent if not null
                    if (task.IsCanceled)
                        try { RaiseCanceled(service, parent, node, () => completionSource.SetCanceled()); }
                        finally { node.Value.RaiseOperationCanceled(); }
                    else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                        try
                        {
                            RaiseFaulted(service, parent, node, task.Exception, () => completionSource.SetException(task.Exception));
                        }
                        finally
                        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                            node.Value.RaiseOperationFaulted((task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        }
#pragma warning restore CS8604 // Possible null reference argument.
                    else
                        try { RaiseRanToCompletion(service, parent, node, () => completionSource.SetResult()); }
                        finally { raiseRanToCompletion(); }
                }

                private static void OnFuncCompletion<TResult>([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent,
                    [DisallowNull] LinkedListNode<BackgroundOperationInfo> node, [DisallowNull] TaskCompletionSource<TResult> completionSource,
                    [DisallowNull] Action<TResult> raiseRanToCompletion, [DisallowNull] Task<TResult> task)
                {
                    // TODO: Use linked list from parent if not null
                    if (task.IsCanceled)
                        try { RaiseCanceled(service, parent, node, () => completionSource.SetCanceled()); }
                        finally { node.Value.RaiseOperationCanceled(); }
                    else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                        try
                        {
                            RaiseFaulted(service, parent, node, task.Exception, () => completionSource.SetException(task.Exception));
                        }
                        finally
                        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                            node.Value.RaiseOperationFaulted((task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        }
#pragma warning restore CS8604 // Possible null reference argument.
                    else
                        try { RaiseRanToCompletion(service, parent, node, () => completionSource.SetResult(task.Result)); }
                        finally { raiseRanToCompletion(task.Result); }
                }

                internal static void Start<TEvent, TResultEvent, TProgress, TOperation, TResult>([DisallowNull] BackgroundProgressService service,
                    BackgroundOperationInfo parent, [DisallowNull] TOperation backgroundOperation, [DisallowNull] TProgress progress,
                    [DisallowNull] Stopwatch stopwatch, [DisallowNull] Func<TProgress, Task<TResult>> asyncMethodDelegate,
                    [DisallowNull] TaskCompletionSource<TResult> completionSource, [DisallowNull] Action<TResult> raiseRanToCompletion)
                    where TEvent : ITimedBackgroundProgressEvent
                    where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TResult>
                    where TProgress : ITimedBackgroundProgress<TEvent>
                    where TOperation : BackgroundOperationInfo, ITimedBackgroundFunc<TResult>
                {
                    LinkedListNode<BackgroundOperationInfo> node = Start(service, parent, backgroundOperation, out Action onStarted);
                    async Task<TResult> Run()
                    {
                        onStarted();
                        stopwatch.Start();
                        return await asyncMethodDelegate(progress);
                    }
                    Run().ContinueWith(task =>
                    {
                        try { stopwatch.Stop(); }
                        finally { OnFuncCompletion(service, parent, node, completionSource, raiseRanToCompletion, task); }
                    });
                }

                internal static void Start<TEvent, TResultEvent, TProgress, TOperation, TResult>([DisallowNull] BackgroundProgressService service,
                    BackgroundOperationInfo parent, [DisallowNull] TOperation backgroundOperation, [DisallowNull] TProgress progress,
                    [DisallowNull] Func<TProgress, Task<TResult>> asyncMethodDelegate, [DisallowNull] TaskCompletionSource<TResult> completionSource,
                    [DisallowNull] Action<TResult> raiseRanToCompletion)
                    where TEvent : IBackgroundProgressEvent
                    where TResultEvent : TEvent, IBackgroundOperationResultEvent<TResult>
                    where TProgress : IBackgroundProgress<TEvent>
                    where TOperation : BackgroundOperationInfo, IBackgroundFunc<TResult>
                {
                    LinkedListNode<BackgroundOperationInfo> node = Start(service, parent, backgroundOperation, out Action onStarted);
                    async Task<TResult> Run()
                    {
                        onStarted();
                        return await asyncMethodDelegate(progress);
                    }
                    Run().ContinueWith(task => OnFuncCompletion(service, parent, node, completionSource, raiseRanToCompletion, task));
                }

                internal static void Start<TEvent, TResultEvent, TProgress, TOperation>([DisallowNull] BackgroundProgressService service,
                    BackgroundOperationInfo parent, [DisallowNull] TOperation backgroundOperation, [DisallowNull] TProgress progress,
                    [DisallowNull] Stopwatch stopwatch, [DisallowNull] Func<TProgress, Task> asyncMethodDelegate,
                    [DisallowNull] TaskCompletionSource completionSource, [DisallowNull] Action raiseRanToCompletion)
                    where TEvent : ITimedBackgroundProgressEvent
                    where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent
                    where TProgress : ITimedBackgroundProgress<TEvent>
                    where TOperation : BackgroundOperationInfo, ITimedBackgroundOperation
                {
                    LinkedListNode<BackgroundOperationInfo> node = Start(service, parent, backgroundOperation, out Action onStarted);
                    async Task Run()
                    {
                        onStarted();
                        stopwatch.Start();
                        await asyncMethodDelegate(progress);
                    }
                    Run().ContinueWith(task =>
                    {
                        try { stopwatch.Stop(); }
                        finally { OnActionCompletion(service, parent, node, completionSource, raiseRanToCompletion, task); }
                    });
                }

                internal static void Start<TEvent, TResultEvent, TProgress, TOperation>([DisallowNull] BackgroundProgressService service,
                    BackgroundOperationInfo parent, [DisallowNull] TOperation backgroundOperation, [DisallowNull] TProgress progress,
                    [DisallowNull] Func<TProgress, Task> asyncMethodDelegate, [DisallowNull] TaskCompletionSource completionSource,
                    [DisallowNull] Action raiseRanToCompletion)
                    where TEvent : IBackgroundProgressEvent
                    where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
                    where TProgress : IBackgroundProgress<TEvent>
                    where TOperation : BackgroundOperationInfo, IBackgroundOperation
                {
                    LinkedListNode<BackgroundOperationInfo> node = Start(service, parent, backgroundOperation, out Action onStarted);
                    async Task Run()
                    {
                        onStarted();
                        await asyncMethodDelegate(progress);
                    }
                    Run().ContinueWith(task => OnActionCompletion(service, parent, node, completionSource, raiseRanToCompletion, task));
                }
            }
        }

        abstract class BackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask> : BackgroundOperationInfo, IObservable<TEvent>
            where TEvent : IBackgroundProgressEvent
            //where TProgress : BackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask>.BackgroundProgress
            where TProgress : IBackgroundProgress<TEvent>
            where TTask : Task
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
        {
            protected abstract TProgress Progress { get; }

            public override Guid OperationId => Progress.OperationId;

            public override string Activity => Progress.Activity;

            public override string StatusDescription => Progress.StatusDescription;

            public override string CurrentOperation => Progress.CurrentOperation;

            public override Guid? ParentId => Progress.ParentId;

            public override byte? PercentComplete => Progress.PercentComplete;

            public IDisposable Subscribe([DisallowNull] IObserver<TEvent> observer)
            {
                throw new NotImplementedException();
            }

            protected BackgroundOperationInfo([DisallowNull] BackgroundProgressService service, CancellationToken[] linkedTokens) : base(service, linkedTokens)
            {
            }

            internal abstract class BackgroundProgress : BackgroundProgressBase, IBackgroundProgress<TEvent>
            {
                public CancellationToken Token { get; }

                protected BackgroundProgress([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, CancellationToken token) :
                    base(operation, activity, initialStatusDescription, parentId)
                {
                    Token = token;
                }

                private void RaiseProgressChanged([DisallowNull] TEvent value)
                {
                    throw new NotImplementedException();
                }

                protected abstract TEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error);

                protected override void OnProgressChanged([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error) => RaiseProgressChanged(CreateEvent(statusDescription, currentOperation, code, percentComplete, error));

                public void Report(TEvent value)
                {
                    if (SetStatus(value))
                        RaiseProgressChanged(value);
                }
            }
        }
    }
}
