using FsInfoCat.AsyncOps;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        internal abstract class BackgroundProgressBase</*TProgress, */TEvent, TOperation, TResultEvent>
            //where TProgress : IBackgroundProgressInfo
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
            where TOperation : IBackgroundOperation
        {
            private readonly object _syncRoot = new();
            private readonly BackgroundProgressService _service;

            internal event EventHandler<TEvent> ProgressChanged;

            public CancellationToken Token { get; }

            public Guid OperationId { get; } = Guid.NewGuid();

            public string Activity { get; }

            public string StatusDescription { get; private set; }

            public string CurrentOperation { get; private set; }

            public Guid? ParentId { get; }

            internal TOperation Operation { get; }

            public byte? PercentComplete { get; private set; }

            internal BackgroundProgressBase(BackgroundProgressService service,
                Func<BackgroundProgressBase<TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TOperation> operationFactory,
                string activity, string statusDescription, Guid? parentId, params CancellationToken[] tokens)
            {
                _service = service;
                ParentId = parentId;
                Activity = activity;
                StatusDescription = statusDescription;
                CurrentOperation = "";
                CancellationTokenSource primaryTokenSource = new();
                if (tokens is null || tokens.Length == 0)
                {
                    Token = primaryTokenSource.Token;
                    Operation = operationFactory(this, primaryTokenSource, null) ?? throw new InvalidOperationException();
                }
                else
                {
                    CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(Enumerable.Repeat(primaryTokenSource.Token, 1).Concat(tokens).ToArray());
                    Token = linkedTokenSource.Token;
                    Operation = operationFactory(this, primaryTokenSource, linkedTokenSource) ?? throw new InvalidOperationException();
                }
            }

            public void ReportCurrentOperation(string currentOperation)
            {
                TEvent e;
                Monitor.Enter(_syncRoot);
                try
                {
                    if ((currentOperation = currentOperation.AsWsNormalizedOrEmpty()) == CurrentOperation)
                        return;
                    CurrentOperation = currentOperation;
                    e = CreateEventObject(null);
                }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(e);
            }

            public void ReportStatusDescription(string statusDescription, string currentOperation)
            {
                if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                    throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                TEvent e;
                Monitor.Enter(_syncRoot);
                try
                {
                    if (statusDescription == StatusDescription && (currentOperation = currentOperation.AsWsNormalizedOrEmpty()) == CurrentOperation)
                        return;
                    StatusDescription = statusDescription;
                    CurrentOperation = currentOperation;
                    e = CreateEventObject(null);
                }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(e);
            }

            public void ReportStatusDescription(string statusDescription)
            {
                if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                    throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                TEvent e;
                Monitor.Enter(_syncRoot);
                try
                {
                    if (statusDescription == StatusDescription)
                        return;
                    StatusDescription = statusDescription;
                    CurrentOperation = "";
                    e = CreateEventObject(null);
                }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(e);
            }

            public void ReportException(Exception exception, string statusDescription, string currentOperation)
            {
                if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                    throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                TEvent e;
                Monitor.Enter(_syncRoot);
                try
                {
                    StatusDescription = statusDescription;
                    CurrentOperation = currentOperation.AsWsNormalizedOrEmpty();
                    e = CreateEventObject(exception);
                }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(e);
            }

            public void ReportException(Exception exception, string statusDescription)
            {
                if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                    throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                TEvent e;
                Monitor.Enter(_syncRoot);
                try
                {
                    StatusDescription = statusDescription;
                    CurrentOperation = "";
                    e = CreateEventObject(exception);
                }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(e);
            }

            public void ReportException(Exception exception)
            {
                TEvent e;
                Monitor.Enter(_syncRoot);
                try { e = CreateEventObject(exception); }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(e);
            }

            public void ReportCurrentOperation(string currentOperation, MessageCode code, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportCurrentOperation(string currentOperation, MessageCode code)
            {
                throw new NotImplementedException();
            }

            public void ReportCurrentOperation(string currentOperation, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, string statusDescription, string currentOperation, MessageCode code, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, string statusDescription, string currentOperation, MessageCode code)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, string statusDescription, string currentOperation, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, string statusDescription, MessageCode code, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, string statusDescription, MessageCode code)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, string statusDescription, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, MessageCode code, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, MessageCode code)
            {
                throw new NotImplementedException();
            }

            public void ReportException(Exception exception, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportStatusDescription(string statusDescription, string currentOperation, MessageCode code, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportStatusDescription(string statusDescription, string currentOperation, MessageCode code)
            {
                throw new NotImplementedException();
            }

            public void ReportStatusDescription(string statusDescription, string currentOperation, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportStatusDescription(string statusDescription, MessageCode code, int percentComplete)
            {
                throw new NotImplementedException();
            }

            public void ReportStatusDescription(string statusDescription, MessageCode code)
            {
                throw new NotImplementedException();
            }

            public void ReportStatusDescription(string statusDescription, int percentComplete)
            {
                throw new NotImplementedException();
            }

            protected abstract TEvent CreateEventObject(Exception exception);

            public void Report(TEvent value)
            {
                string statusDescription = value.StatusDescription.AsWsNormalizedOrEmpty();
                if (statusDescription.Length == 0)
                    throw new ArgumentException($"'{nameof(value)}.{nameof(IBackgroundProgressInfo.StatusDescription)}' cannot be null or whitespace.", nameof(value));
                if (value.OperationId != OperationId)
                    throw new ArgumentException($"'{nameof(value)}.{nameof(IBackgroundProgressInfo.OperationId)}' does not match the current {nameof(OperationId)}.", nameof(value));
                if (value.ParentId != ParentId)
                    throw new ArgumentException($"'{nameof(value)}.{nameof(IBackgroundProgressInfo.ParentId)}' does not match the current {nameof(ParentId)}.", nameof(value));
                if (value.Activity.AsWsNormalizedOrEmpty() != Activity)
                    throw new ArgumentException($"'{nameof(value)}.{nameof(IBackgroundProgressInfo.Activity)}' does not match the current {nameof(Activity)}.", nameof(value));
                Monitor.Enter(_syncRoot);
                try
                {
                    StatusDescription = statusDescription;
                    CurrentOperation = value.CurrentOperation.AsWsNormalizedOrEmpty();
                }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(value);
            }

            private void RaiseProgressChanged(TEvent value)
            {
                try { ProgressChanged?.Invoke(this, value); }
                finally { _service.OnReport(value); }
            }

            public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state,
                params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundFunc<TState, TResult>, TimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId, tokens);

            public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId, tokens);

            public ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperation<TState>, TimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId, tokens);

            public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId, tokens);

            public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state)
                => _service.InvokeAsync<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundFunc<TState, TResult>, TimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId);

            public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId);

            public ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state)
                => _service.InvokeAsync<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperation<TState>, TimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId);

            public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId);

            public ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<ITimedBackgroundProgressEvent, ITimedBackgroundFunc<TResult>, TimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1),
                    CreateEvent, onCompleted, activity, statusDescription, OperationId, tokens);

            public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1),
                    CreateEvent, onCompleted, activity, statusDescription, OperationId, tokens);

            public ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
                Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<ITimedBackgroundProgressEvent, ITimedBackgroundOperation, TimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent,
                    onCompleted, activity,
                    statusDescription, OperationId, tokens);

            public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted,
                string activity, string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent,
                    onCompleted, activity,
                    statusDescription, OperationId, tokens);

            public ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription)
                => _service.InvokeAsync<ITimedBackgroundProgressEvent, ITimedBackgroundFunc<TResult>, TimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1),
                    CreateEvent, onCompleted, activity, statusDescription, OperationId);

            public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1),
                    CreateEvent, onCompleted, activity, statusDescription, OperationId);

            public ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
                Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription)
                => _service.InvokeAsync<ITimedBackgroundProgressEvent, ITimedBackgroundOperation, TimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent,
                    onCompleted, activity, statusDescription, OperationId);

            public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted,
                string activity, string statusDescription)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent,
                    onCompleted, activity, statusDescription, OperationId);

            public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundFunc<TState, TResult>, TimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId, tokens);

            public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity,
                string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId, tokens);

            public ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
                string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperation<TState>, TimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId, tokens);

            public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription,
                TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId, tokens);

            public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                string activity, string statusDescription, TState state)
                => _service.InvokeAsync<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundFunc<TState, TResult>, TimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId);

            public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity,
                string statusDescription, TState state)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId);

            public ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
                string statusDescription, TState state)
                => _service.InvokeAsync<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperation<TState>, TimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId);

            public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription,
                TState state)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>((p, t1, t2)
                    => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId);

            public ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity,
                string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<ITimedBackgroundProgressEvent, ITimedBackgroundFunc<TResult>, TimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1),
                    CreateEvent, null, activity, statusDescription, OperationId, tokens);

            public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription,
                params CancellationToken[] tokens)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1),
                    CreateEvent, null, activity, statusDescription, OperationId, tokens);

            public ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription,
                params CancellationToken[] tokens)
                => _service.InvokeAsync<ITimedBackgroundProgressEvent, ITimedBackgroundOperation, TimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent,
                    null, activity, statusDescription, OperationId, tokens);

            public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent,
                    null, activity, statusDescription, OperationId, tokens);

            public ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription)
                => _service.InvokeAsync<ITimedBackgroundProgressEvent, ITimedBackgroundFunc<TResult>, TimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1),
                    CreateEvent, null, activity, statusDescription, OperationId);

            public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1),
                    CreateEvent, null, activity, statusDescription, OperationId);

            public ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription)
                => _service.InvokeAsync<ITimedBackgroundProgressEvent, ITimedBackgroundOperation, TimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent,
                    null, activity, statusDescription, OperationId);

            public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent,
                    null, activity, statusDescription, OperationId);
        }
    }
}
