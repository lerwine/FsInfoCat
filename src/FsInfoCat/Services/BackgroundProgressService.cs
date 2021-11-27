using FsInfoCat.AsyncOps;
using FsInfoCat.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService : BackgroundService, IBackgroundProgressService
    {
        private readonly object _syncRoot = new();
        private readonly ILogger<BackgroundProgressService> _logger;
        private readonly LinkedList<IBackgroundOperation> _operations = new();
        private readonly StateEventObservers _stateEventObservers = new();
        private readonly ActiveStatusObservers _activeStatusObservers = new();

        class StateEventObservers : Observable<BackgroundProcessStateEventArgs>
        {
            internal void RaiseStateChanged(BackgroundProcessStateEventArgs backgroundProcessEventArgs) => RaiseNext(backgroundProcessEventArgs);
        }

        class ActiveStatusObservers : Observable<bool>
        {
            internal void RaiseActiveStateChanged(bool isActive) => RaiseNext(isActive);
        }

        public bool IsActive { get; private set; }

        int IReadOnlyCollection<IBackgroundOperation>.Count => _operations.Count;

        public BackgroundProgressService([DisallowNull] ILogger<BackgroundProgressService> logger)
        {
            _logger = logger;
        }

        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(BackgroundProgressService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<IBackgroundProgressService>(serviceProvider => new BackgroundProgressService(serviceProvider.GetRequiredService<ILogger<BackgroundProgressService>>()));
        }

        public IDisposable Subscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer) => _stateEventObservers.Subscribe(observer);

        public IDisposable Subscribe([DisallowNull] IObserver<bool> observer) => _activeStatusObservers.Subscribe(observer);

#pragma warning disable CA2016 // Forward the 'CancellationToken' parameter to methods that take one
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() =>
#pragma warning restore CA2016 // Forward the 'CancellationToken' parameter to methods that take one
        {
            Task task;
            try { stoppingToken.WaitHandle.WaitOne(); }
            finally
            {
                lock (_syncRoot)
                {
                    IBackgroundOperation[] actions = _operations.ToArray();
                    task = Task.WhenAll(actions.Select(o => o.Task).ToArray()).ContinueWith(t =>
                    {
                        try { _stateEventObservers.Dispose(); }
                        finally { _activeStatusObservers.Dispose(); }
                    });
                    foreach (IBackgroundOperation op in actions)
                        op.Cancel();
                }
            }
            task.Wait();
        });

        public IEnumerator<IBackgroundOperation> GetEnumerator()
        {
            // TODO: Implement GetEnumerator
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private void RaiseOperationStarted(IBackgroundOperation operation, bool isFirstOperation)
        {
            try
            {
                if (isFirstOperation)
                    _activeStatusObservers.RaiseActiveStateChanged(true);
            }
            finally { _stateEventObservers.RaiseStateChanged(new BackgroundProcessStartedEventArgs(this, operation, null)); }
        }

        private void RaiseOperationCompleted([DisallowNull] Task task, [DisallowNull] LinkedListNode<IBackgroundOperation> node)
        {
            bool isLastOperation;
            lock (_syncRoot)
            {
                isLastOperation = ReferenceEquals(node, _operations.First) && ReferenceEquals(node, _operations.Last);
                _operations.Remove(node);
            }
            try
            {
                if (task.IsCanceled)
                    _stateEventObservers.RaiseStateChanged(new BackgroundProcessCompletedEventArgs(this, node.Value, null, null, false));
                else if (task.IsFaulted)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    if (task.Exception.InnerException is AsyncOperationFailureException asyncFailureException)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        _stateEventObservers.RaiseStateChanged(new BackgroundProcessFaultedEventArgs(this, node.Value, (task.Exception.InnerExceptions.Count == 1) ? asyncFailureException : task.Exception, asyncFailureException.ErrorCode ?? ErrorCode.Unexpected));
                    else
#pragma warning disable CS8604 // Possible null reference argument.
                        _stateEventObservers.RaiseStateChanged(new BackgroundProcessFaultedEventArgs(this, node.Value, (task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception, ErrorCode.Unexpected));
#pragma warning restore CS8604 // Possible null reference argument.
                }
                else
                    _stateEventObservers.RaiseStateChanged(new BackgroundProcessCompletedEventArgs(this, node.Value, null, null, true));
            }
            finally
            {
                if (isLastOperation && _operations.First is null)
                    _activeStatusObservers.RaiseActiveStateChanged(false);
            }
        }

        private void OnReport(IBackgroundProgressEvent progressEvent)
        {
            // TODO: Implement OnReport
            throw new NotImplementedException();
        }

        private TOperation Create<TEvent, TOperation, TInstance, TResultEvent, TProgress>([DisallowNull] TProgress progress, [DisallowNull] Func<TOperation, TResultEvent> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription,Guid? parentId, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent
            where TOperation : IBackgroundOperation
            where TInstance : Observable<TEvent>, TOperation
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
            where TProgress : BackgroundProgress<TEvent, TOperation, TResultEvent>
        {
            TOperation operation = progress.Operation;
            bool isFirstOperation;
            LinkedListNode<IBackgroundOperation> node;
            lock (_syncRoot)
            {
                isFirstOperation = _operations.First is null;
                node = _operations.AddLast(operation);
            }
            RaiseOperationStarted(operation, isFirstOperation);
            if (onCompleted is null)
                operation.Task.ContinueWith(task => RaiseOperationCompleted(task, node));
            else
                operation.Task.ContinueWith(task =>
                {
                    try { progress.Report(onCompleted(operation)); }
                    finally { RaiseOperationCompleted(task, node); }
                });
            return operation;
        }

        internal TOperation InvokeAsync<TState, TEvent, TOperation, TInstance, TResultEvent>(
            Func<BackgroundProgress<TState, TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TInstance> operationFactory, Func<IBackgroundProgressInfo<TState>, Exception, TEvent> eventFactory, Func<TOperation, TResultEvent> onCompleted, string activity,
            string statusDescription, TState state, Guid? parentId, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent<TState>
            where TOperation : IBackgroundOperation<TState>
            where TInstance : Observable<TEvent>, TOperation
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent<TState>
        {
            if ((activity = activity.AsWsNormalizedOrEmpty()).Length == 0)
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
            return Create<TEvent, TOperation, TInstance, TResultEvent, BackgroundProgress<TState, TEvent, TOperation, TResultEvent>>(new(this, (p, t1, t2) => operationFactory((BackgroundProgress<TState, TEvent, TOperation, TResultEvent>)p, t1, t2), eventFactory, activity, statusDescription, state, parentId, tokens), onCompleted, activity, statusDescription, parentId, tokens);
        }

        internal TOperation InvokeAsync<TEvent, TOperation, TInstance, TResultEvent>(
            Func<BackgroundProgress<TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TInstance> operationFactory, Func<IBackgroundProgressInfo, Exception, TEvent> eventFactory, Func<TOperation, TResultEvent> onCompleted, string activity,
            string statusDescription, Guid? parentId, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent
            where TOperation : IBackgroundOperation
            where TInstance : Observable<TEvent>, TOperation
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
        {
            if ((activity = activity.AsWsNormalizedOrEmpty()).Length == 0)
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if ((statusDescription = statusDescription.AsWsNormalizedOrEmpty()).Length == 0)
                throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
            return Create<TEvent, TOperation, TInstance, TResultEvent, BackgroundProgress<TEvent, TOperation, TResultEvent>>(new(this, operationFactory, eventFactory, activity, statusDescription, parentId, tokens), onCompleted, activity, statusDescription, parentId, tokens);
        }

        // TODO: Implement CreateEvent with exception
        internal static IBackgroundProgressEvent<TState> CreateEvent<TState>(IBackgroundProgressInfo<TState> progress, Exception exception) => (exception is null) ? new BackgroundProgressEventArgs<TState>(progress) : throw new NotImplementedException();

        internal static IBackgroundProgressEvent CreateEvent(IBackgroundProgressInfo progress, Exception exception) => new BackgroundProgressEventArgs(progress);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
            => InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, null, tokens);

        public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
            => InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, null, tokens);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state)
            => InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, null);

        public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state)
            => InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, state, null);

        public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens)
            => InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, null, tokens);

        public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted,
            string activity, string statusDescription, params CancellationToken[] tokens)
            => InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity,
                statusDescription, null, tokens);

        public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription)
            => InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, null);

        public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted,
            string activity, string statusDescription)
            => InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, onCompleted, activity, statusDescription, null);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity,
            string statusDescription, TState state, params CancellationToken[] tokens)
            => InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, null, tokens);

        public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription,
            TState state, params CancellationToken[] tokens)
            => InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, null, tokens);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity,
            string statusDescription, TState state)
            => InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, BackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, null);

        public IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription,
            TState state)
            => InvokeAsync<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, BackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, state, null);

        public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription,
            params CancellationToken[] tokens)
            => InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, null, tokens);

        public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens)
            => InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, null, tokens);

        public IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription)
            => InvokeAsync<IBackgroundProgressEvent, IBackgroundFunc<TResult>, BackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, null);

        public IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription)
            => InvokeAsync<IBackgroundProgressEvent, IBackgroundOperation, BackgroundOperation, IBackgroundOperationCompletedEvent>((p, t1, t2) => new(p, asyncMethodDelegate, t1), CreateEvent, null, activity, statusDescription, null);
    }
}
