using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace FsInfoCat.Services
{
    public partial class FSIOQueueService
    {
        internal abstract class QueuedBgOperation<TTask> : IQueuedBgOperation
            where TTask : Task
        {
            private readonly FSIOQueueService _service;
            private readonly Action _setCompleted;
            private readonly CancellationTokenSource _tokenSource = new();
            private readonly Stopwatch _stopwatch = new();

            internal object SyncRoot { get; } = new();

            public TTask Task { get; private set; }

            public AsyncJobStatus Status { get; private set; } = AsyncJobStatus.WaitingToRun;

            public DateTime Started { get; private set; }

            public TimeSpan Elapsed => _stopwatch.Elapsed;

            public WaitHandle AsyncWaitHandle { get; }

            public bool CompletedSynchronously => ((IAsyncResult)Task)?.CompletedSynchronously ?? false;

            public bool IsCompleted => Status switch
            {
                AsyncJobStatus.Canceled or AsyncJobStatus.Faulted or AsyncJobStatus.Succeeded => true,
                _ => false,
            };

            object IAsyncResult.AsyncState => (Task ?? throw new InvalidOperationException()).AsyncState;

            Task IQueuedBgOperation.Task => Task;

            protected QueuedBgOperation([DisallowNull] FSIOQueueService service)
            {
                _service = service ?? throw new ArgumentNullException(nameof(service));
                AsyncWaitHandle = WaitHandleRelay.CreateManualSetOnly(out _setCompleted);
                _tokenSource.Token.Register(OnCancelRequested);
                WithQueueLocked(() =>
                {
                    if (service.CurrentOperation is null)
                    {
                        _service.IsActive = true;
                        service.CurrentOperation = this;
                        Started = DateTime.Now;
                        (Task = StartAsync(_tokenSource.Token)).ContinueWith(OnCompleted);
                        _service.CurrentOperation = this;
                    }
                    else
                    {
                        CancellationTokenSource tokenSource = new();
                        service._queue.AddLast((this, tokenSource));
                        tokenSource.Token.Register(OnStart);
                    }
                });
            }

            private void WithQueueLocked(Action action)
            {
                Monitor.Enter(_service._syncRoot);
                try
                {
                    Monitor.Enter(SyncRoot);
                    try { action(); }
                    finally { Monitor.Exit(SyncRoot); }
                }
                finally { Monitor.Exit(_service._syncRoot); }
            }

            private void OnStart()
            {
                Started = DateTime.Now;
                WithQueueLocked(() =>
                {
                    (Task = StartAsync(_tokenSource.Token)).ContinueWith(OnCompleted);
                    _service.CurrentOperation = this;
                });
            }

            private void OnCancelRequested() => WithQueueLocked(() =>
            {
                if (Task is null)
                {
                    _tokenSource.Dispose();
                    Started = DateTime.Now;
                    Task = CreateCanceled(_tokenSource.Token);
                    Status = AsyncJobStatus.Canceled;
                    _service.Dequeue(this);
                    _setCompleted();
                }
                else
                    switch (Status)
                    {
                        case AsyncJobStatus.WaitingToRun:
                            Started = DateTime.Now;
                            Status = AsyncJobStatus.Cancelling;
                            break;
                        case AsyncJobStatus.Running:
                            Status = AsyncJobStatus.Cancelling;
                            break;
                        default:
                            break;
                    }
            });

            protected abstract TTask CreateCanceled(CancellationToken cancellationToken);

            protected void SetStarted() => WithQueueLocked(() =>
            {
                switch (Status)
                {
                    case AsyncJobStatus.Cancelling:
                        Started = DateTime.Now;
                        return;
                    case AsyncJobStatus.WaitingToRun:
                        _stopwatch.Start();
                        Status = AsyncJobStatus.Running;
                        break;
                }
            });

            private void OnCompleted([DisallowNull] Task task) => WithQueueLocked(() =>
            {
                if (_stopwatch.IsRunning)
                    _stopwatch.Stop();
                if (task.IsCanceled)
                    Status = AsyncJobStatus.Canceled;
                else if (task.IsFaulted)
                    Status = AsyncJobStatus.Faulted;
                else
                    Status = AsyncJobStatus.Succeeded;
                _setCompleted();
                _tokenSource.Dispose();
            });

            protected abstract TTask StartAsync(CancellationToken cancellationToken);

            public void Cancel() => _tokenSource.Cancel();

            public void Cancel(bool throwOnFirstException) => _tokenSource.Cancel(throwOnFirstException);

            public void CancelAfter(int millisecondsDelay) => _tokenSource.CancelAfter(millisecondsDelay);

            public void CancelAfter(TimeSpan delay) => _tokenSource.CancelAfter(delay);
        }
    }

    internal sealed class QueuedBgOperation : FSIOQueueService.QueuedBgOperation<Task>
    {
        private readonly Func<CancellationToken, Task> _asyncFunction;

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task> asyncFunction)
            : base(service)
        {
            _asyncFunction = asyncFunction;
        }

        protected override Task CreateCanceled(CancellationToken cancellationToken) => Task.FromCanceled(cancellationToken);

        protected async override Task StartAsync(CancellationToken cancellationToken)
        {
            SetStarted();
            await _asyncFunction(cancellationToken);
        }
    }

    internal sealed class QueuedBgOperation<TResult> : FSIOQueueService.QueuedBgOperation<Task<TResult>>, IQueuedBgOperation<TResult>
    {
        private readonly Func<CancellationToken, Task<TResult>> _asyncFunction;

        object IAsyncResult.AsyncState => (Task ?? throw new InvalidOperationException()).AsyncState;

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction)
            : base(service)
        {
            _asyncFunction = asyncFunction;
        }

        protected override Task<TResult> CreateCanceled(CancellationToken cancellationToken) => System.Threading.Tasks.Task.FromCanceled<TResult>(cancellationToken);

        protected async override Task<TResult> StartAsync(CancellationToken cancellationToken)
        {
            SetStarted();
            return await _asyncFunction(cancellationToken);
        }
    }
}
