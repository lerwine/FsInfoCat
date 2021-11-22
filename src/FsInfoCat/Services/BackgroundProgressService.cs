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

        public IDisposable Subscribe([DisallowNull] IObserver<BackgroundProcessStateEventArgs> observer)
        {
            // TODO: Implement Subscribe
            throw new NotImplementedException();
        }

        public IDisposable Subscribe([DisallowNull] IObserver<bool> observer)
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IBackgroundOperation> GetEnumerator()
        {
            // TODO: Implement GetEnumerator
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private void RaiseOperationStarted(IBackgroundOperation operation)
        {
            // TODO: Implement RaiseOperationStarted
            throw new NotImplementedException();
        }

        private void RaiseOperationCompleted([DisallowNull] Task task, [DisallowNull] LinkedListNode<IBackgroundOperation> node)
        {
            // TODO: Implement RaiseOperationCompleted
            throw new NotImplementedException();
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
            LinkedListNode<IBackgroundOperation> node = _operations.AddLast(operation);
            RaiseOperationStarted(operation);
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
