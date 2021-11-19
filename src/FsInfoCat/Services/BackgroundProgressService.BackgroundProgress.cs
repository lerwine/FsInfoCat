using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        internal class BackgroundProgress<TEvent, TOperation, TResultEvent> : IBackgroundProgress<TEvent>
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
            where TOperation : IBackgroundOperation
        {
            private readonly object _syncRoot = new();
            private readonly BackgroundProgressService _service;
            private readonly Func<IBackgroundProgressInfo, Exception, TEvent> _eventFactory;

            internal event EventHandler<TEvent> ProgressChanged;

            public CancellationToken Token { get; }

            public Guid OperationId { get; } = Guid.NewGuid();

            public string Activity { get; }

            public string StatusDescription { get; private set; }

            public string CurrentOperation { get; private set; }

            public Guid? ParentId { get; }

            internal TOperation Operation { get; }

            internal BackgroundProgress(BackgroundProgressService service, Func<BackgroundProgress<TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TOperation> operationFactory,
                Func<IBackgroundProgressInfo, Exception, TEvent> eventFactory, string activity, string statusDescription, Guid? parentId, params CancellationToken[] tokens)
            {
                _service = service;
                _eventFactory = eventFactory;
                ParentId = parentId;
                Activity = activity;
                StatusDescription = statusDescription;
                CurrentOperation = "";
                CancellationTokenSource primaryTokenSource = new();
                if (tokens is null || tokens.Length == 0)
                {
                    Token = primaryTokenSource.Token;
                    Operation = operationFactory(this, primaryTokenSource, null);
                }
                else
                {
                    CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(Enumerable.Repeat(primaryTokenSource.Token, 1).Concat(tokens).ToArray());
                    Token = linkedTokenSource.Token;
                    Operation = operationFactory(this, primaryTokenSource, linkedTokenSource);
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
                    e = _eventFactory(this, null);
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
                    e = _eventFactory(this, null);
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
                    e = _eventFactory(this, null);
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
                    e = _eventFactory(this, exception);
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
                    e = _eventFactory(this, exception);
                }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(e);
            }

            public void ReportException(Exception exception)
            {
                TEvent e;
                Monitor.Enter(_syncRoot);
                try { e = _eventFactory(this, exception); }
                finally { Monitor.Exit(_syncRoot); }
                RaiseProgressChanged(e);
            }

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

            public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId, tokens);

            public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                Func<IBackgroundOperation<TState>, IBackgroundOperationCompleteEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompleteEvent<TState>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId, tokens);

            public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId);

            public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                Func<IBackgroundOperation<TState>, IBackgroundOperationCompleteEvent<TState>> onCompleted, string activity, string statusDescription, TState state)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompleteEvent<TState>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, OperationId);

            public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, OperationId, tokens);

            public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted,
                string activity, string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity,
                    statusDescription, OperationId, tokens);

            public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, OperationId);

            public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted,
                string activity, string statusDescription)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, OperationId);

            public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity,
                string statusDescription, TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId, tokens);

            public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription,
                TState state, params CancellationToken[] tokens)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompleteEvent<TState>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId, tokens);

            public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity,
                string statusDescription, TState state)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId);

            public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription,
                TState state)
                => _service.InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompleteEvent<TState>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, OperationId);

            public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription,
                params CancellationToken[] tokens)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, OperationId, tokens);

            public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, OperationId, tokens);

            public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, OperationId);

            public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription)
                => _service.InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, OperationId);
        }

        internal class BackgroundProgress<TState, TEvent, TOperation, TResultEvent> : BackgroundProgress<TEvent, TOperation, TResultEvent>, IBackgroundProgress<TState, TEvent>, IBackgroundProgressInfo<TState>
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationCompleteEvent<TState>
            where TOperation : IBackgroundOperation<TState>
        {
            public TState AsyncState { get; }

            internal BackgroundProgress(BackgroundProgressService service, Func<BackgroundProgress<TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TOperation> operationFactory,
                Func<IBackgroundProgressInfo<TState>, Exception, TEvent> eventFactory, string activity, string statusDescription, TState state, Guid? parentId, params CancellationToken[] tokens)
                : base(service, (p, t1, t2) => operationFactory((BackgroundProgress<TState, TEvent, TOperation, TResultEvent>)p, t1, t2), (p, e) => eventFactory((IBackgroundProgressInfo<TState>)p, e), activity, statusDescription, parentId, tokens)
            {
                AsyncState = state;
            }
        }
    }
}
