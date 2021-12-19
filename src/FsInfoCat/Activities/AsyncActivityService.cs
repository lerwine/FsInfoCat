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

namespace FsInfoCat.Activities
{
    partial class AsyncActivityService : BackgroundService, IAsyncActivityService
    {
        private readonly object _syncRoot = new();
        private readonly LinkedList<AsyncOperation> _operations = new();
        private readonly StateEventObservers _stateEventObservers = new();
        private readonly ActiveStatusObservers _activeStatusObservers = new();
        private readonly ILogger<AsyncActivityService> _logger;

        public bool IsActive { get; private set; }

        public int Count => _operations.Count;

        public AsyncActivityService([DisallowNull] ILogger<AsyncActivityService> logger) => _logger = logger;

        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(AsyncActivityService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<IAsyncActivityService>(serviceProvider => new AsyncActivityService(serviceProvider.GetRequiredService<ILogger<AsyncActivityService>>()));
        }

        public IAsyncAction InvokeAsync<TEvent, TProgressEvent, TFinalEvent>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TEvent>, Task> asyncMethodDelegate,
            IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, IOperationInfo> eventFactory)
            where TEvent : IOperationEvent
            where TProgressEvent : IActivityProgressEvent, TEvent
            where TFinalEvent : IOperationEvent, TEvent
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            if (eventFactory is null)
                throw new ArgumentNullException(nameof(eventFactory));
            throw new NotImplementedException();
        }

        public IAsyncFunc<TResult> InvokeAsync<TEvent, TOperationEvent, TResult, TFinalEvent>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TOperationEvent>, Task<TResult>> asyncMethodDelegate,
            IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, IOperationInfo> eventFactory)
            where TEvent : IOperationEvent
            where TOperationEvent : IOperationEvent, TEvent
            where TFinalEvent : IActivityResultEvent<TResult>, TEvent
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            if (eventFactory is null)
                throw new ArgumentNullException(nameof(eventFactory));
            throw new NotImplementedException();
        }

        public ITimedAsyncAction InvokeAsync<TEvent, TProgressEvent, TFinalEvent>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TEvent>, Task> asyncMethodDelegate,
            IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, ITimedOperationInfo> eventFactory)
            where TEvent : ITimedOperationEvent
            where TProgressEvent : ITimedActivityProgressEvent, TEvent
            where TFinalEvent : ITimedOperationEvent, TEvent
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            if (eventFactory is null)
                throw new ArgumentNullException(nameof(eventFactory));
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TResult> InvokeAsync<TEvent, TOperationEvent, TResult, TFinalEvent>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TOperationEvent>, Task<TResult>> asyncMethodDelegate,
            IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, ITimedOperationInfo> eventFactory)
            where TEvent : ITimedOperationEvent
            where TOperationEvent : ITimedOperationEvent, TEvent
            where TFinalEvent : ITimedActivityResultEvent<TResult>, TEvent
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            if (eventFactory is null)
                throw new ArgumentNullException(nameof(eventFactory));
            throw new NotImplementedException();
        }

        public IAsyncAction<TState> InvokeAsync<TEvent, TProgressEvent, TFinalEvent, TState>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TState, TEvent>, Task> asyncMethodDelegate,
            IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, IOperationInfo<TState>> eventFactory, TState asyncState)
            where TEvent : IOperationEvent<TState>
            where TProgressEvent : IActivityProgressEvent<TState>, TEvent
            where TFinalEvent : IOperationEvent<TState>, TEvent
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            if (eventFactory is null)
                throw new ArgumentNullException(nameof(eventFactory));
            throw new NotImplementedException();
        }

        public IAsyncFunc<TState, TResult> InvokeAsync<TEvent, TOperationEvent, TState, TResult, TFinalEvent>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TState, TOperationEvent>, Task<TResult>> asyncMethodDelegate,
            IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, IOperationInfo<TState>> eventFactory, TState asyncState)
            where TEvent : IOperationEvent<TState>
            where TOperationEvent : IOperationEvent<TState>, TEvent
            where TFinalEvent : IActivityResultEvent<TState, TResult>, TEvent
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            if (eventFactory is null)
                throw new ArgumentNullException(nameof(eventFactory));
            throw new NotImplementedException();
        }

        public ITimedAsyncAction<TState> InvokeAsync<TEvent, TProgressEvent, TFinalEvent, TState>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TState, TEvent>, Task> asyncMethodDelegate,
            IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, IOperationInfo<TState>> eventFactory, TState asyncState)
            where TEvent : ITimedOperationEvent<TState>
            where TProgressEvent : ITimedActivityProgressEvent<TState>, TEvent
            where TFinalEvent : ITimedOperationEvent<TState>, TEvent
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            if (eventFactory is null)
                throw new ArgumentNullException(nameof(eventFactory));
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TState, TResult> InvokeAsync<TEvent, TOperationEvent, TState, TResult, TFinalEvent>(string activity, string initialStatusDescription,
            Func<ITimedAsyncActivityProgress<TState, TOperationEvent>, Task<TResult>> asyncMethodDelegate, IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, IOperationInfo<TState>> eventFactory, TState asyncState)
            where TEvent : ITimedOperationEvent<TState>
            where TOperationEvent : ITimedOperationEvent<TState>, TEvent
            where TFinalEvent : ITimedActivityResultEvent<TState, TResult>, TEvent
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            if (eventFactory is null)
                throw new ArgumentNullException(nameof(eventFactory));
            throw new NotImplementedException();
        }

        public IAsyncAction InvokeAsync(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<IOperationEvent>, Task> asyncMethodDelegate)
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            throw new NotImplementedException();
        }

        public IAsyncFunc<TResult> InvokeAsync<TResult>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<IOperationEvent>, Task<TResult>> asyncMethodDelegate)
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            throw new NotImplementedException();
        }

        public ITimedAsyncAction InvokeAsync(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<ITimedOperationEvent>, Task> asyncMethodDelegate)
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TResult> InvokeAsync<TResult>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<ITimedOperationEvent>, Task<TResult>> asyncMethodDelegate)
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            throw new NotImplementedException();
        }

        public IAsyncAction<TState> InvokeAsync<TState>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TState, IOperationEvent<TState>>, Task> asyncMethodDelegate, TState asyncState)
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            throw new NotImplementedException();
        }

        public IAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(string activity, string initialStatusDescription, Func<IAsyncActivityProgress<TState, IOperationEvent<TState>>, Task<TResult>> asyncMethodDelegate, TState asyncState)
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            throw new NotImplementedException();
        }

        public ITimedAsyncAction<TState> InvokeAsync<TState>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TState, ITimedOperationEvent<TState>>, Task> asyncMethodDelegate, TState asyncState)
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(string activity, string initialStatusDescription, Func<ITimedAsyncActivityProgress<TState, ITimedOperationEvent<TState>>, Task<TResult>> asyncMethodDelegate, TState asyncState)
        {
            if (string.IsNullOrWhiteSpace(activity))
                throw new ArgumentException($"'{nameof(activity)}' cannot be null or whitespace.", nameof(activity));
            if (string.IsNullOrWhiteSpace(initialStatusDescription))
                throw new ArgumentException($"'{nameof(initialStatusDescription)}' cannot be null or whitespace.", nameof(initialStatusDescription));
            if (asyncMethodDelegate is null)
                throw new ArgumentNullException(nameof(asyncMethodDelegate));
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<IOperationEvent> observer, Action<IReadOnlyList<IOperationEvent>> getActiveOperationsOnObserving)
        {
            if (observer is null)
                throw new ArgumentNullException(nameof(observer));
            lock (_syncRoot)
            {
                getActiveOperationsOnObserving?.Invoke(_operations.Select(o => o.LatestEvent).ToArray());
                return _stateEventObservers.Subscribe(observer);
            }
        }

        public IDisposable Subscribe(IObserver<IOperationEvent> observer) => _stateEventObservers.Subscribe(observer);

        public IDisposable Subscribe(IObserver<bool> observer) => _activeStatusObservers.Subscribe(observer);

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() =>
        {
            Task task;
            try { stoppingToken.WaitHandle.WaitOne(); }
            finally
            {
                lock (_syncRoot)
                {
                    AsyncOperation[] actions = _operations.ToArray();
                    task = Task.WhenAll(actions.Select(o => o.GetTask()).Where(t => !t.IsCompleted).ToArray()).ContinueWith(t =>
                    {
                        try { _stateEventObservers.Dispose(); }
                        finally { _activeStatusObservers.Dispose(); }
                    });
                    foreach (AsyncOperation op in actions)
                        op.Cancel();
                }
            }
            task.Wait();
        });

        public IEnumerator<IAsyncAction> GetEnumerator() => _operations.Select(o => o.GetOperation()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_operations.Select(o => o.GetOperation())).GetEnumerator();

        class AsyncOperation
        {
            internal IOperationEvent LatestEvent { get; private set; }

            internal IAsyncAction GetOperation() => throw new NotImplementedException();

            internal Task GetTask() => throw new NotImplementedException();

            internal void Cancel() => throw new NotImplementedException();
        }

        class StateEventObservers : Observable<IOperationEvent>
        {
            internal void RaiseStateChanged(IOperationEvent backgroundProcessEventArgs) => RaiseNext(backgroundProcessEventArgs);
        }

        class ActiveStatusObservers : Observable<bool>
        {
            internal void RaiseActiveStateChanged(bool isActive) => RaiseNext(isActive);
        }
    }
}
