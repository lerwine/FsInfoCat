using FsInfoCat.AsyncOps;
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
        private readonly LinkedList<BackgroundOperationInfo> _operations = new();
        private readonly ILogger<BackgroundProgressService> _logger;
        private readonly StateEventObservers _stateEventObservers = new();
        private readonly ActiveStatusObservers _activeStatusObservers = new();

        public bool IsActive { get; private set; }

        int IReadOnlyCollection<IBackgroundOperation>.Count => _operations.Count;

        public BackgroundProgressService([DisallowNull] ILogger<BackgroundProgressService> logger) => _logger = logger;

        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(BackgroundProgressService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<IBackgroundProgressService>(serviceProvider => new BackgroundProgressService(serviceProvider.GetRequiredService<ILogger<BackgroundProgressService>>()));
        }

        public IDisposable Subscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer) => _stateEventObservers.Subscribe(observer);

        public IDisposable Subscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer, Action<IReadOnlyList<IBackgroundOperation>> getActiveOperationsOnObserving)
        {
            lock (_syncRoot)
            {
                getActiveOperationsOnObserving?.Invoke(_operations.ToArray());
                return _stateEventObservers.Subscribe(observer);
            }
        }

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

        public IEnumerator<IBackgroundOperation> GetEnumerator() => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_operations).GetEnumerator();

        private void OnReport([DisallowNull] IBackgroundProgressEvent progressEvent)
        {
            _stateEventObservers.RaiseStateChanged(progressEvent);
        }

        public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            => TimedBackgroundFunc<TState, TResult>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, onCompleted, tokens);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            => BackgroundFunc<TState, TResult>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, onCompleted, tokens);

        public ITimedBackgroundOperation<TState> InvokeAsync<TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            => TimedBackgroundOperation<TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, onCompleted, tokens);

        public IBackgroundOperation<TState> InvokeAsync<TState>([DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
           [DisallowNull] Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
           [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            => BackgroundOperation<TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, onCompleted, tokens);

        public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            => TimedBackgroundFunc<TState, TResult>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, onCompleted);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            => BackgroundFunc<TState, TResult>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, onCompleted);

        public ITimedBackgroundOperation<TState> InvokeAsync<TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            => TimedBackgroundOperation<TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, onCompleted);

        public IBackgroundOperation<TState> InvokeAsync<TState>([DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
           [DisallowNull] Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, [DisallowNull] string activity,
           [DisallowNull] string statusDescription, TState state)
            => BackgroundOperation<TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, onCompleted);

        public ITimedBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            => TimedBackgroundFunc<TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, onCompleted, tokens);

        public IBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity, [DisallowNull] string statusDescription,
            params CancellationToken[] tokens)
            => BackgroundFunc<TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, onCompleted, tokens);

        public ITimedBackgroundOperation InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity, [DisallowNull] string statusDescription,
            params CancellationToken[] tokens)
            => TimedBackgroundOperation.Start(this, null, activity, statusDescription, asyncMethodDelegate, onCompleted, tokens);

        public IBackgroundOperation InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity, [DisallowNull] string statusDescription,
            params CancellationToken[] tokens)
            => BackgroundOperation.Start(this, null, activity, statusDescription, asyncMethodDelegate, onCompleted, tokens);

        public ITimedBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
           [DisallowNull] Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity,
           [DisallowNull] string statusDescription)
            => TimedBackgroundFunc<TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, onCompleted);

        public IBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, [DisallowNull] string activity, [DisallowNull] string statusDescription)
            => BackgroundFunc<TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, onCompleted);

        public ITimedBackgroundOperation InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity, [DisallowNull] string statusDescription)
            => TimedBackgroundOperation.Start(this, null, activity, statusDescription, asyncMethodDelegate, onCompleted);

        public IBackgroundOperation InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, [DisallowNull] string activity, [DisallowNull] string statusDescription)
            => BackgroundOperation.Start(this, null, activity, statusDescription, asyncMethodDelegate, onCompleted);

        public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            => TimedBackgroundFunc<TState, TResult>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, null, tokens);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            => BackgroundFunc<TState, TResult>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, null, tokens);

        public ITimedBackgroundOperation<TState> InvokeAsync<TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            => TimedBackgroundOperation<TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, null, tokens);

        public IBackgroundOperation<TState> InvokeAsync<TState>([DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            => BackgroundOperation<TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, null, tokens);

        public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            => TimedBackgroundFunc<TState, TResult>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, null);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            => BackgroundFunc<TState, TResult>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, null);

        public ITimedBackgroundOperation<TState> InvokeAsync<TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            => TimedBackgroundOperation<TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, null);

        public IBackgroundOperation<TState> InvokeAsync<TState>([DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, TState state)
            => BackgroundOperation<TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, null);

        public ITimedBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            => TimedBackgroundFunc<TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, null, tokens);

        public IBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            => BackgroundFunc<TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, null, tokens);

        public ITimedBackgroundOperation InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            => TimedBackgroundOperation.Start(this, null, activity, statusDescription, asyncMethodDelegate, null, tokens);

        public IBackgroundOperation InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            => BackgroundOperation.Start(this, null, activity, statusDescription, asyncMethodDelegate, null, tokens);

        public ITimedBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription)
            => TimedBackgroundFunc<TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, null);

        public IBackgroundFunc<TResult> InvokeAsync<TResult>([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription)
            => BackgroundFunc<TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, null);

        public ITimedBackgroundOperation InvokeAsync([DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            [DisallowNull] string activity, [DisallowNull] string statusDescription)
            => TimedBackgroundOperation.Start(this, null, activity, statusDescription, asyncMethodDelegate, null);

        public IBackgroundOperation InvokeAsync([DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            => BackgroundOperation.Start(this, null, activity, statusDescription, asyncMethodDelegate, null);

        public ITimedBackgroundOperation<TState> InvokeAsync<TEvent, TResultEvent, TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : ITimedBackgroundOperationCompletedEvent<TState>, TEvent
            => TimedBackgroundOperation<TEvent, TResultEvent, TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, eventFactory, tokens);

        public ITimedBackgroundOperation<TState> InvokeAsync<TEvent, TResultEvent, TState>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : ITimedBackgroundOperationCompletedEvent<TState>, TEvent
            => TimedBackgroundOperation<TEvent, TResultEvent, TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, eventFactory);

        public ITimedBackgroundOperation InvokeAsync<TEvent, TResultEvent>([DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : ITimedBackgroundOperationCompletedEvent, TEvent
            => TimedBackgroundOperation<TEvent, TResultEvent>.Start(this, null, activity, statusDescription, asyncMethodDelegate, eventFactory, tokens);

        public ITimedBackgroundOperation InvokeAsync<TEvent, TResultEvent>([DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : ITimedBackgroundOperationCompletedEvent, TEvent
            => TimedBackgroundOperation<TEvent, TResultEvent>.Start(this, null, activity, statusDescription, asyncMethodDelegate, eventFactory);

        public IBackgroundOperation<TState> InvokeAsync<TEvent, TResultEvent, TState>(
            [DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : IBackgroundOperationCompletedEvent<TState>, TEvent
            => BackgroundOperation<TEvent, TResultEvent, TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, eventFactory, tokens);

        public IBackgroundOperation<TState> InvokeAsync<TEvent, TResultEvent, TState>(
            [DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : IBackgroundOperationCompletedEvent<TState>, TEvent
            => BackgroundOperation<TEvent, TResultEvent, TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, eventFactory);

        public IBackgroundOperation InvokeAsync<TEvent, TResultEvent>([DisallowNull] Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : IBackgroundOperationCompletedEvent, TEvent
            => BackgroundOperation<TEvent, TResultEvent>.Start(this, null, activity, statusDescription, asyncMethodDelegate, eventFactory, tokens);

        public IBackgroundOperation InvokeAsync<TEvent, TResultEvent>([DisallowNull] Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : IBackgroundOperationCompletedEvent, TEvent
            => BackgroundOperation<TEvent, TResultEvent>.Start(this, null, activity, statusDescription, asyncMethodDelegate, eventFactory);

        public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TEvent, TResultEvent, TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : ITimedBackgroundOperationResultEvent<TState, TResult>, TEvent
            => TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, eventFactory,
                tokens);

        public ITimedBackgroundFunc<TState, TResult> InvokeAsync<TEvent, TResultEvent, TState, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : ITimedBackgroundOperationResultEvent<TState, TResult>, TEvent
            => TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, eventFactory);

        public ITimedBackgroundFunc<TResult> InvokeAsync<TEvent, TResultEvent, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : ITimedBackgroundOperationResultEvent<TResult>, TEvent
            => TimedBackgroundFunc<TEvent, TResultEvent, TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, eventFactory, tokens);

        public ITimedBackgroundFunc<TResult> InvokeAsync<TEvent, TResultEvent, TResult>(
            [DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : ITimedBackgroundOperationResultEvent<TResult>, TEvent
            => TimedBackgroundFunc<TEvent, TResultEvent, TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, eventFactory);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TEvent, TResultEvent, TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : IBackgroundOperationResultEvent<TState, TResult>, TEvent
            => BackgroundFunc<TEvent, TResultEvent, TResult, TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, eventFactory,
                tokens);

        public IBackgroundFunc<TState, TResult> InvokeAsync<TEvent, TResultEvent, TState, TResult>(
            [DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, TState state)
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : IBackgroundOperationResultEvent<TState, TResult>, TEvent
            => BackgroundFunc<TEvent, TResultEvent, TResult, TState>.Start(this, null, activity, statusDescription, state, asyncMethodDelegate, eventFactory);

        public IBackgroundFunc<TResult> InvokeAsync<TEvent, TResultEvent, TResult>([DisallowNull] Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription, params CancellationToken[] tokens)
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : IBackgroundOperationResultEvent<TResult>, TEvent
            => BackgroundFunc<TEvent, TResultEvent, TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, eventFactory, tokens);

        public IBackgroundFunc<TResult> InvokeAsync<TEvent, TResultEvent, TResult>([DisallowNull] Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
            [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, [DisallowNull] string activity,
            [DisallowNull] string statusDescription)
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : IBackgroundOperationResultEvent<TResult>, TEvent
            => BackgroundFunc<TEvent, TResultEvent, TResult>.Start(this, null, activity, statusDescription, asyncMethodDelegate, eventFactory);

        class StateEventObservers : Observable<IBackgroundProgressEvent>
        {
            internal void RaiseStateChanged(IBackgroundProgressEvent backgroundProcessEventArgs) => RaiseNext(backgroundProcessEventArgs);
        }

        class ActiveStatusObservers : Observable<bool>
        {
            internal void RaiseActiveStateChanged(bool isActive) => RaiseNext(isActive);
        }
    }
}
