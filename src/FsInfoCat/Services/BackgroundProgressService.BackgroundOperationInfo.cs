using FsInfoCat.AsyncOps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        abstract class BackgroundOperationInfo : IBackgroundProgressInfo, IDisposable
        {
            private bool _isDisposed;

            private CancellationTokenSource _tokenSource = new();

            protected CancellationToken Token => _tokenSource.Token;

            public bool IsCancellationRequested => throw new NotImplementedException();

            public Guid OperationId => throw new NotImplementedException();

            public string Activity => throw new NotImplementedException();

            public string StatusDescription => throw new NotImplementedException();

            public string CurrentOperation => throw new NotImplementedException();

            public Guid? ParentId => throw new NotImplementedException();

            public byte? PercentComplete => throw new NotImplementedException();

            public void Cancel()
            {
                throw new NotImplementedException();
            }

            public void Cancel(bool throwOnFirstException)
            {
                throw new NotImplementedException();
            }

            public void CancelAfter(int millisecondsDelay)
            {
                throw new NotImplementedException();
            }

            public void CancelAfter(TimeSpan delay)
            {
                throw new NotImplementedException();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_isDisposed)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects)
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
                protected object SyncRoot { get; }

                public Guid OperationId { get; } = Guid.NewGuid();

                public string Activity { get; }

                public string StatusDescription { get; private set; }

                public string CurrentOperation { get; private set; }

                public Guid? ParentId { get; }

                public byte? PercentComplete { get; private set; }

                protected BackgroundProgressBase(string activity, string initialStatusDescription, Guid? parentId)
                {
                    if (string.IsNullOrWhiteSpace(activity))
                        throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
                    if (string.IsNullOrWhiteSpace(initialStatusDescription))
                        throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
                    Activity = activity;
                    StatusDescription = initialStatusDescription;
                    ParentId = parentId;
                }

                protected abstract void OnProgressChanged(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error);

                protected bool SetStatus(IBackgroundProgressEvent progressEvent)
                {
                    byte? percentComplete = progressEvent.PercentComplete;
                    string statusDescription = progressEvent.StatusDescription;
                    string currentOperation = progressEvent.CurrentOperation.EmptyIfNullOrWhiteSpace();
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(progressEvent)}.{nameof(IBackgroundProgressEvent.StatusDescription)}' cannot be null or whitespace.", nameof(progressEvent));
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

                public void ReportCurrentOperation(string currentOperation, MessageCode code, byte percentComplete)
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

                public void ReportCurrentOperation(string currentOperation, MessageCode code)
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

                public void ReportCurrentOperation(string currentOperation, byte percentComplete)
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

                public void ReportCurrentOperation(string currentOperation)
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

                public void ReportException(Exception exception, string statusDescription, string currentOperation, MessageCode code, byte percentComplete)
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

                public void ReportException(Exception exception, string statusDescription, string currentOperation, MessageCode code)
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

                public void ReportException(Exception exception, string statusDescription, string currentOperation, byte percentComplete)
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

                public void ReportException(Exception exception, string statusDescription, MessageCode code, byte percentComplete)
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

                public void ReportException(Exception exception, string statusDescription, MessageCode code)
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

                public void ReportException(Exception exception, string statusDescription, byte percentComplete)
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

                public void ReportException(Exception exception, MessageCode code, byte percentComplete)
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

                public void ReportException(Exception exception, MessageCode code)
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

                public void ReportException(Exception exception, byte percentComplete)
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

                public void ReportException(Exception exception, string statusDescription, string currentOperation)
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

                public void ReportException(Exception exception, string statusDescription)
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

                public void ReportException(Exception exception)
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

                public void ReportStatusDescription(string statusDescription, string currentOperation, MessageCode code, byte percentComplete)
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

                public void ReportStatusDescription(string statusDescription, string currentOperation, MessageCode code)
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

                public void ReportStatusDescription(string statusDescription, string currentOperation, byte percentComplete)
                {
                    if (string.IsNullOrWhiteSpace(statusDescription))
                        throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                    if (percentComplete > 100)
                        throw new ArgumentOutOfRangeException(nameof(percentComplete));
                    lock (SyncRoot)
                    {
                        if (CurrentOperation == (currentOperation = currentOperation.EmptyIfNullOrWhiteSpace()) && statusDescription == StatusDescription && PercentComplete == percentComplete)
                            return;
                        StatusDescription = statusDescription;
                        PercentComplete = percentComplete;
                        CurrentOperation = currentOperation;
                    }
                    OnProgressChanged(statusDescription, currentOperation, null, percentComplete, null);
                }

                public void ReportStatusDescription(string statusDescription, MessageCode code, byte percentComplete)
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

                public void ReportStatusDescription(string statusDescription, MessageCode code)
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

                public void ReportStatusDescription(string statusDescription, byte percentComplete)
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

                public void ReportStatusDescription(string statusDescription, string currentOperation)
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

                public void ReportStatusDescription(string statusDescription)
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

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription, TState state)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription)
                {
                    throw new NotImplementedException();
                }

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state)
                {
                    throw new NotImplementedException();
                }

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription, TState state)
                {
                    throw new NotImplementedException();
                }

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription)
                {
                    throw new NotImplementedException();
                }

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                IBackgroundOperation IBackgroundProgressFactory.InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, TState state)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription)
                {
                    throw new NotImplementedException();
                }

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state)
                {
                    throw new NotImplementedException();
                }

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, TState state)
                {
                    throw new NotImplementedException();
                }

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription)
                {
                    throw new NotImplementedException();
                }

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens)
                {
                    throw new NotImplementedException();
                }

                IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription)
                {
                    throw new NotImplementedException();
                }

                #endregion
            }
        }

        abstract class BackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask> : BackgroundOperationInfo, IObservable<TEvent>
            where TEvent : IBackgroundProgressEvent
            where TProgress : BackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask>.BackgroundProgress
            where TTask : Task
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
        {
            public abstract TTask Task { get; }

            protected abstract TProgress Progress { get; }

            public IDisposable Subscribe(IObserver<TEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal abstract class BackgroundProgress : BackgroundProgressBase, IBackgroundProgress<TEvent>
            {
                public CancellationToken Token { get; }

                protected BackgroundProgress(string activity, string initialStatusDescription, Guid? parentId, CancellationToken token) : base(activity, initialStatusDescription, parentId)
                {
                    Token = token;
                }

                private void RaiseProgressChanged(TEvent value)
                {
                    throw new NotImplementedException();
                }

                protected abstract TEvent CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error);

                protected override void OnProgressChanged(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error) =>
                    RaiseProgressChanged(CreateEvent(statusDescription, currentOperation, code, percentComplete, error));

                public void Report(TEvent value)
                {
                    if (SetStatus(value))
                        RaiseProgressChanged(value);
                }
            }
        }
    }
}