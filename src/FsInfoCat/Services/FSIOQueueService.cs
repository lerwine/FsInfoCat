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
    public partial class FSIOQueueService : BackgroundService, IFSIOQueueService
    {
        private object _syncRoot = new();
        private AutoResetEvent _processNextEvent = new(false);
        private readonly ILogger<FSIOQueueService> _logger;
        private readonly LinkedList<(IQueuedBgOperation Target, CancellationTokenSource StartSource)> _queue = new();

        public FSIOQueueService([DisallowNull] ILogger<FSIOQueueService> logger)
        {
            _logger = logger;
        }

        public bool IsActive { get; private set; }

        public int Count => _queue.Count;

        public IQueuedBgOperation CurrentOperation { get; set; }

        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(FSIOQueueService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<IFSIOQueueService>(serviceProvider => new FSIOQueueService(serviceProvider.GetRequiredService<ILogger<FSIOQueueService>>()));
        }

        //public IQueuedBgOperation Enqueue(ActivityCode activity, [DisallowNull] Func<CancellationToken, Task> asyncFunction) => new QueuedBgOperation(this, asyncFunction, activity);

        //public IQueuedBgOperation<TResult> Enqueue<TResult>(ActivityCode activity, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction) => new Services.QueuedBgOperation<TResult>(this, asyncFunction, activity);

        public IEnumerator<IQueuedBgOperation> GetEnumerator() => _queue.Select(t => t.Target).GetEnumerator();

        private bool Dequeue(IQueuedBgOperation operation)
        {
            if (operation == null)
                return false;
            for (LinkedListNode<(IQueuedBgOperation Target, CancellationTokenSource StartSource)> node = _queue.First; node is not null; node = node.Next)
            {
                if (ReferenceEquals(node.Value.Target, operation))
                {
                    _queue.Remove(node);
                    return true;
                }
            }
            return false;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() =>
        {
            try { stoppingToken.WaitHandle.WaitOne(); }
            finally
            {
                lock (_queue)
                {
                    for (LinkedListNode<(IQueuedBgOperation Target, CancellationTokenSource StartSource)> node = _queue.First; node is not null; node = node.Next)
                    {
                        node.Value.Target.Cancel();
                        node.Value.StartSource.Dispose();
                    }
                    _queue.Clear();
                    IQueuedBgOperation item = CurrentOperation;
                    CurrentOperation = null;
                    IsActive = false;
                    item?.Cancel();
                }
            }
        });

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_queue.Select(t => t.Target)).GetEnumerator();

        public IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, [DisallowNull] Func<T, IStatusReportable, Task> asyncMethod) =>
            new QueuedBgOperation<T>(this, state, null, asyncMethod, activity);

        public IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, [DisallowNull] Func<IStatusReportable, Task> asyncMethod) =>
            new QueuedBgOperation<T>(this, state, null, asyncMethod, activity);

        public IQueuedBgOperation Enqueue(ActivityCode activity, [DisallowNull] Func<IStatusReportable, Task> asyncMethod) => new QueuedBgOperation<object>(this, null, null, asyncMethod, activity);

        public IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, [DisallowNull] Func<T, CancellationToken, Task> asyncMethod) =>
            new QueuedBgOperation<T>(this, state, null, asyncMethod, activity);

        public IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, [DisallowNull] Func<CancellationToken, Task> asyncMethod) =>
            new QueuedBgOperation<T>(this, state, null, asyncMethod, activity);

        public IQueuedBgOperation Enqueue(ActivityCode activity, [DisallowNull] Func<CancellationToken, Task> asyncMethod) => new QueuedBgOperation<object>(this, null, null, asyncMethod, activity);

        public IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<TState, IStatusReportable, Task<TResult>> asyncMethod) =>
            new QueuedBgProducer<TState, TResult>(this, state, null, asyncMethod, activity);

        public IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<IStatusReportable, Task<TResult>> asyncMethod) =>
            new QueuedBgProducer<TState, TResult>(this, state, null, asyncMethod, activity);

        public IQueuedBgProducer<T> Enqueue<T>(ActivityCode activity, [DisallowNull] Func<IStatusReportable, Task<T>> asyncMethod) =>
            new QueuedBgProducer<object, T>(this, null, null, asyncMethod, activity);

        public IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<TState, CancellationToken, Task<TResult>> asyncMethod) =>
            new QueuedBgProducer<TState, TResult>(this, state, null, asyncMethod, activity);

        public IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncMethod) =>
            new QueuedBgProducer<TState, TResult>(this, state, null, asyncMethod, activity);

        public IQueuedBgProducer<T> Enqueue<T>(ActivityCode activity, [DisallowNull] Func<CancellationToken, Task<T>> asyncMethod) =>
            new QueuedBgProducer<object, T>(this, null, null, asyncMethod, activity);

        public IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<T, IStatusReportable, Task> asyncMethod) =>
            new QueuedBgOperation<T>(this, state, progressHandler, asyncMethod, activity);

        public IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task> asyncMethod) =>
            new QueuedBgOperation<T>(this, state, progressHandler, asyncMethod, activity);

        public IQueuedBgOperation Enqueue(ActivityCode activity, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task> asyncMethod) =>
            new QueuedBgOperation<object>(this, null, progressHandler, asyncMethod, activity);

        public IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<T, CancellationToken, Task> asyncMethod) =>
            new QueuedBgOperation<T>(this, state, progressHandler, asyncMethod, activity);

        public IQueuedBgOperation<T> Enqueue<T>(ActivityCode activity, T state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task> asyncMethod) =>
            new QueuedBgOperation<T>(this, state, progressHandler, asyncMethod, activity);

        public IQueuedBgOperation Enqueue(ActivityCode activity, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task> asyncMethod) =>
            new QueuedBgOperation<object>(this, null, progressHandler, asyncMethod, activity);

        public IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, IStatusReportable, Task<TResult>> asyncMethod) =>
            new QueuedBgProducer<TState, TResult>(this, state, progressHandler, asyncMethod, activity);

        public IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task<TResult>> asyncMethod) =>
            new QueuedBgProducer<TState, TResult>(this, state, progressHandler, asyncMethod, activity);

        public IQueuedBgProducer<T> Enqueue<T>(ActivityCode activity, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task<T>> asyncMethod) =>
            new QueuedBgProducer<object, T>(this, null, progressHandler, asyncMethod, activity);

        public IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, CancellationToken, Task<TResult>> asyncMethod) =>
            new QueuedBgProducer<TState, TResult>(this, state, progressHandler, asyncMethod, activity);

        public IQueuedBgProducer<TState, TResult> Enqueue<TState, TResult>(ActivityCode activity, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncMethod) =>
            new QueuedBgProducer<TState, TResult>(this, state, progressHandler, asyncMethod, activity);

        public IQueuedBgProducer<T> Enqueue<T>(ActivityCode activity, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task<T>> asyncMethod) =>
            new QueuedBgProducer<object, T>(this, null, progressHandler, asyncMethod, activity);
    }
}
