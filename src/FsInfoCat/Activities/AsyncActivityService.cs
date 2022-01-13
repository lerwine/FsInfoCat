using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    public partial class AsyncActivityService : BackgroundService, IAsyncActivityService
    {
        private readonly object _syncRoot = new();
        private readonly RootProvider _provider;
        private readonly ILogger<AsyncActivityService> _logger;

        public bool IsActive => _provider.IsActive;

        public IObservable<IAsyncActivity> StateChangeObservable => _provider.StateChangeObservable;

        public int Count => _provider.Count;

        private AsyncActivityService([DisallowNull] ILogger<AsyncActivityService> logger) => _logger = logger;

        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(AsyncActivityService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<IAsyncActivityService>(serviceProvider => new AsyncActivityService(serviceProvider.GetRequiredService<ILogger<AsyncActivityService>>()));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() =>
        {
            Task task;
            try { stoppingToken.WaitHandle.WaitOne(); }
            finally
            {
                lock (_syncRoot)
                {
                    IAsyncActivity[] actions = _provider.ToArray();
                    task = Task.WhenAll(actions.Select(o => o.Task).Where(t => !t.IsCompleted).ToArray()).ContinueWith(t =>
                    {
                        try { _provider.StateChangeSource.Dispose(); }
                        finally { _provider.ActiveStatusSource.Dispose(); }
                    });
                    foreach (IAsyncActivity op in actions)
                    {
                        if (!op.TokenSource.IsCancellationRequested)
                            op.TokenSource.Cancel(true);
                    }
                }
            }
            task.Wait();
        }, stoppingToken);

        public IEnumerator<IAsyncActivity> GetEnumerator() => _provider.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_provider).GetEnumerator();

        public IAsyncAction<IOperationEvent> InvokeAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
            => _provider.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IAsyncFunc<IOperationEvent, TResult> InvokeAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => _provider.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);

        public IAsyncAction<IOperationEvent<TState>, TState> InvokeAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate) => _provider.InvokeAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);

        public IAsyncFunc<IOperationEvent<TState>, TState, TResult> InvokeAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate) => _provider.InvokeAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);

        public ITimedAsyncAction<ITimedOperationEvent> InvokeTimedAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate) => _provider.InvokeTimedAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncFunc<ITimedOperationEvent, TResult> InvokeTimedAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => _provider.InvokeTimedAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);

        public ITimedAsyncAction<ITimedOperationEvent<TState>, TState> InvokeTimedAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate) => _provider.InvokeTimedAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);

        public ITimedAsyncFunc<ITimedOperationEvent<TState>, TState, TResult> InvokeTimedAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate) => _provider.InvokeTimedAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);

        public IDisposable Subscribe(IObserver<bool> observer) => _provider.ActiveStatusSource.Observable.Subscribe(observer);

        public IDisposable SubscribeStateChange([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving) => _provider.SubscribeStateChange(observer, onObserving);
    }

}
