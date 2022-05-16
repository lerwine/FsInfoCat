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
    /// <summary>
    /// Concrete implementation of the <see cref="IAsyncActivityService"/>
    /// </summary>
    /// <seealso cref="BackgroundService" />
    /// <seealso cref="IAsyncActivityService" />
    public partial class AsyncActivityService : BackgroundService, IAsyncActivityService
    {
        private readonly object _syncRoot = new();
        private readonly RootProvider _provider = new();
        private readonly ILogger<AsyncActivityService> _logger;

        /// <summary>
        /// Gets or sets a value indicating whether any <see cref="IAsyncActivity"/> objects started by this <c>AsyncActivityService</c> are still running.
        /// </summary>
        /// <value><see langword="true"/> if one or more <see cref="IAsyncActivity"/> objects started by this <c>AsyncActivityService</c> are still running;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsActive => _provider.IsActive;

        /// <summary>
        /// Gets the provider for activity start notifications.
        /// </summary>
        /// <value>The provider that can be used to subscribe for activity start notifications.</value>
        public IObservable<IAsyncActivity> ActivityStartedObservable => _provider.ActivityStartedObservable;

        /// <summary>
        /// Gets the number of activities that the associated <see cref="RootProvider"/> is running.
        /// </summary>
        /// <value>The count of <see cref="IAsyncActivity"/> objects representing activities that the associated <see cref="RootProvider"/> is runnning.</value>
        public int Count => _provider.Count;

        private AsyncActivityService([DisallowNull] ILogger<AsyncActivityService> logger) => _logger = logger;

        /// <summary>
        /// Instantiates the concrete <see cref="IAsyncActivityService"/> as a hosted service.
        /// </summary>
        /// <param name="services">The service descriptor collection.</param>
        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(AsyncActivityService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<IAsyncActivityService>(serviceProvider => new AsyncActivityService(serviceProvider.GetRequiredService<ILogger<AsyncActivityService>>()));
        }

        /// <summary>
        /// This method is called when the <see cref="AsyncActivityService" /> starts.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)" /> is called.</param>
        /// <returns>A <see cref="Task" /> that represents the long running operations.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() =>
        {
            Task task;
            try { stoppingToken.WaitHandle.WaitOne(); }
            finally
            {
                _logger.LogDebug("Stopping AsyncActivityService");
                lock (_syncRoot)
                {
                    IAsyncActivity[] actions = _provider.ToArray();
                    _logger.LogDebug("AsyncActivityService waiting for {TaskCount} tasks", actions.Length);
                    task = Task.WhenAll(actions.Select(o => o.Task).Where(t => !t.IsCompleted).ToArray()).ContinueWith(t =>
                    {
                        _logger.LogDebug("Disposing AsyncActivityService observable sources");
                        try { _provider.ActivityStartedSource.Dispose(); }
                        finally { _provider.ActiveStatusSource.Dispose(); }
                    });
                    foreach (IAsyncActivity op in actions)
                    {
                        if (op.TokenSource.IsCancellationRequested)
                            _logger.LogDebug("AsyncActivityService activity {ActivityId} ({ShortDescription}) already canceled", op.ActivityId, op.ShortDescription);
                        else
                        {
                            _logger.LogDebug("AsyncActivityService cancelling Activity {ActivityId} ({ShortDescription})", op.ActivityId, op.ShortDescription);
                            op.TokenSource.Cancel(true);
                        }
                    }
                }
            }
            task.Wait();
            _logger.LogDebug("All AsyncActivityService tasks complete");
        }, stoppingToken);

        /// <summary>
        /// Returns an enumerator that iterates through the running activities started by the associated <see cref="RootProvider"/>.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the running <see cref="IAsyncActivity"/> objects started by the
        /// associated <see cref="RootProvider"/>.</returns>
        public IEnumerator<IAsyncActivity> GetEnumerator() => _provider.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_provider).GetEnumerator();

        /// <summary>
        /// Invokes an asynchronous method.
        /// </summary>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="IAsyncAction{IOperationEvent}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncAction<IActivityEvent> InvokeAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
            => _provider.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="IAsyncFunc{IOperationEvent, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncFunc<IActivityEvent, TResult> InvokeAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => _provider.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, associating it with a user-specified value.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous activity.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="IAsyncAction{IOperationEvent{TState}, TState}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncAction<IActivityEvent<TState>, TState> InvokeAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate) => _provider.InvokeAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, associating the function with a user-specified value.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous function.</typeparam>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous function.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="IAsyncFunc{IOperationEvent{TState}, TState, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public IAsyncFunc<IActivityEvent<TState>, TState, TResult> InvokeAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate) => _provider.InvokeAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, tracking the execution start and duration.
        /// </summary>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="ITimedAsyncAction{ITimedOperationEvent}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncAction<ITimedActivityEvent> InvokeTimedAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate) => _provider.InvokeTimedAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{ITimedOperationEvent, TResult}" /> object that can be used to monitor and/or cancel the asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncFunc<ITimedActivityEvent, TResult> InvokeTimedAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage,
            [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate) => _provider.InvokeTimedAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);

        /// <summary>
        /// Invokes an asynchronous method, associating it with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous activity.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous activity.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method.</param>
        /// <returns>An <see cref="ITimedAsyncAction{ITimedOperationEvent{TState}, TState}" /> object that can be used to monitor and/or cancel the asynchronous activity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncAction<ITimedActivityEvent<TState>, TState> InvokeTimedAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate) => _provider.InvokeTimedAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);

        /// <summary>
        /// Invokes a method that asynchronously produces a result value, associating the function with a user-specified value and tracking the execution start and duration.
        /// </summary>
        /// <typeparam name="TState">The type of the user-defined value that will be associated with the asynchronous function.</typeparam>
        /// <typeparam name="TResult">The type of the result value produced by the asynchronous function.</typeparam>
        /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
        /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
        /// <param name="state">The user-defined value to associate with the the asynchronous function.</param>
        /// <param name="asyncMethodDelegate">A reference to an asynchronous method that produces the result value.</param>
        /// <returns>An <see cref="ITimedAsyncFunc{ITimedOperationEvent{TState}, TState, TResult}" /> object that can be used to monitor and/or cancel the asynchronous
        /// function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is <see langword="null"/>,
        /// <see cref="string.Empty"/> or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned a <see langword="null"/> value.</exception>
        public ITimedAsyncFunc<ITimedActivityEvent<TState>, TState, TResult> InvokeTimedAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state,
            [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate) => _provider.InvokeTimedAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);

        /// <summary>
        /// Notifies the provider that an observer is to receive <see cref="IsActive"/> change notifications.
        /// </summary>
        /// <param name="observer">The object that is to receive <see cref="IsActive"/> change notifications.</param>
        /// <returns>A reference to an interface that allows observers to stop receiving <see cref="IsActive"/> change notifications before the provider has finished sending
        /// them.</returns>
        public IDisposable Subscribe(IObserver<bool> observer) => _provider.ActiveStatusSource.Observable.Subscribe(observer);

        /// <summary>
        /// Notifies this activity source that an observer is to receive activity start notifications, providing a list of existing activities.
        /// </summary>
        /// <param name="observer">The object that is to receive activity start notifications.</param>
        /// <param name="onObserving">The callback method that provides a list of existing activities immediately before the observer is registered to receive activity start
        /// notifications.</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications before this has finished sending them.</returns>
        public IDisposable SubscribeChildActivityStart([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving) => _provider.SubscribeChildActivityStart(observer, onObserving);
    }

}
