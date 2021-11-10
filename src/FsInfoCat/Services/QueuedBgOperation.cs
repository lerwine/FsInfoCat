using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    public partial class FSIOQueueService
    {
        internal abstract partial class QueuedBgOperation<TState, TTask> : IQueuedBgOperation<TState>
            where TTask : Task
        {
            private readonly FSIOQueueService _service;
            private readonly IProgress<IAsyncOperationInfo> _progressHandler;
            private readonly Action _setCompleted;
            private readonly CancellationTokenSource _tokenSource = new();
            private readonly Stopwatch _stopwatch = new();
            private OperationStatus _operationStatus;

            internal object SyncRoot { get; } = new();

            public Guid ConcurrencyId { get; }

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

            public IAsyncOperationInfo ParentOperation { get; }
            
            object IAsyncOperationInfo.AsyncState => AsyncState;

            object IAsyncResult.AsyncState => AsyncState;

            public ActivityCode Activity { get; }

            public MessageCode StatusDescription => _operationStatus.StatusDescription;

            ActivityCode? IAsyncOperationInfo.Activity => Activity;

            MessageCode? IAsyncOperationInfo.StatusDescription => StatusDescription;

            public string CurrentOperation => _operationStatus.CurrentOperation;

            public TState AsyncState { get; }

            Task IBgOperation.Task => Task;

            protected QueuedBgOperation(Guid concurrencyId, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] FSIOQueueService service, ActivityCode activity, MessageCode statusDescription,
                IAsyncOperationInfo parentOperation = null)
            {
                _service = service ?? throw new ArgumentNullException(nameof(service));
                _progressHandler = progressHandler;
                ParentOperation = parentOperation;
                ConcurrencyId = concurrencyId;
                AsyncState = state;
                Activity = activity;
                _operationStatus = new(statusDescription, "");
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
                        service._queue.AddLast((this, new())).Value.StartSource.Token.Register(OnStart);
                });
            }

            protected IStatusReportable CreateReportable(CancellationToken token) => new StatusReportable(this, _progressHandler, token);

            [Obsolete]
            protected QueuedBgOperation(Guid concurrencyId, [DisallowNull] FSIOQueueService service, ActivityCode activity, MessageCode statusDescription, IAsyncOperationInfo parentOperation = null)
            {
                _service = service ?? throw new ArgumentNullException(nameof(service));
                ParentOperation = parentOperation;
                ConcurrencyId = concurrencyId;
                //Reportable = new StatusReportable(this);
                Activity = activity;
                _operationStatus = new(statusDescription, "");
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
                        service._queue.AddLast((this, new())).Value.StartSource.Token.Register(OnStart);
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

            public void CancelAfter(int millisecondsDelay) => _tokenSource.CancelAfter(millisecondsDelay);

            public void CancelAfter(TimeSpan delay) => _tokenSource.CancelAfter(delay);
        }

        //[Obsolete("Use FSIOQueueService.QueuedBgOperation<TState, TProgress, TTask>")]
        //internal abstract partial class QueuedBgOperation<TTask> : IQueuedBgOperation
        //    where TTask : Task
        //{
        //    private readonly FSIOQueueService _service;
        //    private readonly Action _setCompleted;
        //    private readonly CancellationTokenSource _tokenSource = new();
        //    private readonly Stopwatch _stopwatch = new();
        //    private OperationStatus _operationStatus;

        //    internal object SyncRoot { get; } = new();

        //    public Guid ConcurrencyId { get; }

        //    public TTask Task { get; private set; }

        //    public AsyncJobStatus Status { get; private set; } = AsyncJobStatus.WaitingToRun;

        //    public DateTime Started { get; private set; }

        //    public TimeSpan Elapsed => _stopwatch.Elapsed;

        //    public WaitHandle AsyncWaitHandle { get; }

        //    public bool CompletedSynchronously => ((IAsyncResult)Task)?.CompletedSynchronously ?? false;

        //    public bool IsCompleted => Status switch
        //    {
        //        AsyncJobStatus.Canceled or AsyncJobStatus.Faulted or AsyncJobStatus.Succeeded => true,
        //        _ => false,
        //    };

        //    public IAsyncOperationInfo ParentOperation { get; }

        //    protected IStatusReportable Reportable { get; }

        //    object IAsyncResult.AsyncState => (Task ?? throw new InvalidOperationException()).AsyncState;

        //    public ActivityCode Activity { get; }

        //    public MessageCode StatusDescription => _operationStatus.StatusDescription;

        //    ActivityCode? IAsyncOperationInfo.Activity => Activity;

        //    MessageCode? IAsyncOperationInfo.StatusDescription => StatusDescription;

        //    public string CurrentOperation => _operationStatus.CurrentOperation;

        //    public object AsyncState { get; private set; }

        //    Task IBgOperation.Task => Task;

        //    protected QueuedBgOperation(Guid concurrencyId, [DisallowNull] FSIOQueueService service, ActivityCode activity, MessageCode statusDescription, IAsyncOperationInfo parentOperation = null)
        //    {
        //        _service = service ?? throw new ArgumentNullException(nameof(service));
        //        ParentOperation = parentOperation;
        //        ConcurrencyId = concurrencyId;
        //        //Reportable = new StatusReportable(this);
        //        Activity = activity;
        //        _operationStatus = new(statusDescription, "");
        //        AsyncWaitHandle = WaitHandleRelay.CreateManualSetOnly(out _setCompleted);
        //        _tokenSource.Token.Register(OnCancelRequested);
        //        WithQueueLocked(() =>
        //        {
        //            if (service.CurrentOperation is null)
        //            {
        //                _service.IsActive = true;
        //                service.CurrentOperation = this;
        //                Started = DateTime.Now;
        //                (Task = StartAsync(_tokenSource.Token)).ContinueWith(OnCompleted);
        //                _service.CurrentOperation = this;
        //            }
        //            else
        //            {
        //                CancellationTokenSource tokenSource = new();
        //                service._queue.AddLast((this, tokenSource));
        //                tokenSource.Token.Register(OnStart);
        //            }
        //        });
        //    }

        //    private void WithQueueLocked(Action action)
        //    {
        //        Monitor.Enter(_service._syncRoot);
        //        try
        //        {
        //            Monitor.Enter(SyncRoot);
        //            try { action(); }
        //            finally { Monitor.Exit(SyncRoot); }
        //        }
        //        finally { Monitor.Exit(_service._syncRoot); }
        //    }

        //    private void OnStart()
        //    {
        //        Started = DateTime.Now;
        //        WithQueueLocked(() =>
        //        {
        //            (Task = StartAsync(_tokenSource.Token)).ContinueWith(OnCompleted);
        //            _service.CurrentOperation = this;
        //        });
        //    }

        //    private void OnCancelRequested() => WithQueueLocked(() =>
        //    {
        //        if (Task is null)
        //        {
        //            _tokenSource.Dispose();
        //            Started = DateTime.Now;
        //            Task = CreateCanceled(_tokenSource.Token);
        //            Status = AsyncJobStatus.Canceled;
        //            _service.Dequeue(this);
        //            _setCompleted();
        //        }
        //        else
        //            switch (Status)
        //            {
        //                case AsyncJobStatus.WaitingToRun:
        //                    Started = DateTime.Now;
        //                    Status = AsyncJobStatus.Cancelling;
        //                    break;
        //                case AsyncJobStatus.Running:
        //                    Status = AsyncJobStatus.Cancelling;
        //                    break;
        //                default:
        //                    break;
        //            }
        //    });

        //    protected abstract TTask CreateCanceled(CancellationToken cancellationToken);

        //    protected void SetStarted() => WithQueueLocked(() =>
        //    {
        //        switch (Status)
        //        {
        //            case AsyncJobStatus.Cancelling:
        //                Started = DateTime.Now;
        //                return;
        //            case AsyncJobStatus.WaitingToRun:
        //                _stopwatch.Start();
        //                Status = AsyncJobStatus.Running;
        //                break;
        //        }
        //    });

        //    private void OnCompleted([DisallowNull] Task task) => WithQueueLocked(() =>
        //    {
        //        if (_stopwatch.IsRunning)
        //            _stopwatch.Stop();
        //        if (task.IsCanceled)
        //            Status = AsyncJobStatus.Canceled;
        //        else if (task.IsFaulted)
        //            Status = AsyncJobStatus.Faulted;
        //        else
        //            Status = AsyncJobStatus.Succeeded;
        //        _setCompleted();
        //        _tokenSource.Dispose();
        //    });

        //    protected abstract TTask StartAsync(CancellationToken cancellationToken);

        //    public void Cancel() => _tokenSource.Cancel();

        //    public void CancelAfter(int millisecondsDelay) => _tokenSource.CancelAfter(millisecondsDelay);

        //    public void CancelAfter(TimeSpan delay) => _tokenSource.CancelAfter(delay);
        //}
    }

    internal sealed class QueuedBgOperation<TState> : FSIOQueueService.QueuedBgOperation<TState, Task>
    {
        private readonly Func<CancellationToken, Task> _asyncFunction;

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, IStatusReportable, Task> asyncMethod,
            ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
            : this(service, state, progressHandler, asyncMethod, activity, statusDescription, Guid.NewGuid()) { }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task> asyncMethod,
            ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
            : this(service, state, progressHandler, asyncMethod, activity, statusDescription, Guid.NewGuid()) { }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, IStatusReportable, Task> asyncMethod,
            ActivityCode activity, Guid concurrencyId)
            : this(service, state, progressHandler, asyncMethod, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task> asyncMethod,
            ActivityCode activity, Guid concurrencyId)
            : this(service, state, progressHandler, asyncMethod, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, IStatusReportable, Task> asyncMethod,
            ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription, null)
        {
            _asyncFunction = token => asyncMethod(state, CreateReportable(token));
        }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task> asyncMethod,
            ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription, null)
        {
            _asyncFunction = token => asyncMethod(CreateReportable(token));
        }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, TState, CancellationToken, Task> asyncMethod,
            ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription)
        {
            _asyncFunction = token => asyncMethod(CreateReportable(token), state, token);
        }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, CancellationToken, Task> asyncMethod,
            ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription)
        {
            _asyncFunction = token => asyncMethod(CreateReportable(token), token);
        }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, CancellationToken, Task> asyncMethod, ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
            : this(service, state, progressHandler, asyncMethod, activity, statusDescription, Guid.NewGuid()) { }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task> asyncMethod, ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
            : this(service, state, progressHandler, asyncMethod, activity, statusDescription, Guid.NewGuid()) { }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, CancellationToken, Task> asyncMethod, ActivityCode activity, Guid concurrencyId)
            : this(service, state, progressHandler, asyncMethod, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task> asyncMethod, ActivityCode activity, Guid concurrencyId)
            : this(service, state, progressHandler, asyncMethod, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, CancellationToken, Task> asyncMethod, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription)
        {
            _asyncFunction = token => asyncMethod(state, token);
        }

        internal QueuedBgOperation([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task> asyncMethod, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription)
        {
            _asyncFunction = asyncMethod;
        }

        protected override Task CreateCanceled(CancellationToken cancellationToken) => Task.FromCanceled(cancellationToken);

        protected async override Task StartAsync(CancellationToken cancellationToken)
        {
            SetStarted();
            await _asyncFunction(cancellationToken);
        }
    }

    //[Obsolete("Use QueuedBgOperation<TState, TProgress>")]
    //internal sealed class QueuedBgOperation : FSIOQueueService.QueuedBgOperation<Task>
    //{
    //    private readonly Func<CancellationToken, Task> _asyncFunction;

    //    [Obsolete]
    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task> asyncFunction) : this(service, asyncFunction, ActivityCode.CrawlingFileSystem) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<IStatusReportable, CancellationToken, Task> asyncFunction, ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
    //        : this(service, asyncFunction, activity, statusDescription, Guid.NewGuid()) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<IStatusReportable, CancellationToken, Task> asyncFunction, ActivityCode activity, Guid concurrencyId)
    //        : this(service, asyncFunction, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<IStatusReportable, CancellationToken, Task> asyncFunction, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
    //        : base(concurrencyId, service, activity, statusDescription)
    //    {
    //        _asyncFunction = token => asyncFunction(Reportable, token);
    //    }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task> asyncFunction, ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
    //        : this(service, asyncFunction, activity, statusDescription, Guid.NewGuid()) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task> asyncFunction, ActivityCode activity, Guid concurrencyId)
    //        : this(service, asyncFunction, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task> asyncFunction, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
    //        : base(concurrencyId, service, activity, statusDescription)
    //    {
    //        _asyncFunction = asyncFunction;
    //    }

    //    protected override Task CreateCanceled(CancellationToken cancellationToken) => Task.FromCanceled(cancellationToken);

    //    protected async override Task StartAsync(CancellationToken cancellationToken)
    //    {
    //        SetStarted();
    //        await _asyncFunction(cancellationToken);
    //    }
    //}

    internal sealed class QueuedBgProducer<TState, TResult> : FSIOQueueService.QueuedBgOperation<TState, Task<TResult>>, IQueuedBgProducer<TState, TResult>
    {
        private readonly Func<CancellationToken, Task<TResult>> _asyncFunction;

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity,
            MessageCode statusDescription = MessageCode.BackgroundJobPending)
            : this(service, state, progressHandler, asyncFunction, activity, statusDescription, Guid.NewGuid()) { }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity,
            MessageCode statusDescription = MessageCode.BackgroundJobPending)
            : this(service, state, progressHandler, asyncFunction, activity, statusDescription, Guid.NewGuid()) { }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription)
        {
            _asyncFunction = token => asyncFunction(state, token);
        }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription)
        {
            _asyncFunction = asyncFunction;
        }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, Guid concurrencyId)
            : this(service, state, progressHandler, asyncFunction, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, Guid concurrencyId)
            : this(service, state, progressHandler, asyncFunction, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, IStatusReportable, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
            : this(service, state, progressHandler, asyncFunction, activity, statusDescription, Guid.NewGuid()) { }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
            : this(service, state, progressHandler, asyncFunction, activity, statusDescription, Guid.NewGuid()) { }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, IStatusReportable, Task<TResult>> asyncFunction, ActivityCode activity, Guid concurrencyId)
            : this(service, state, progressHandler, asyncFunction, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task<TResult>> asyncFunction, ActivityCode activity, Guid concurrencyId)
            : this(service, state, progressHandler, asyncFunction, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<TState, IStatusReportable, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription)
        {
            _asyncFunction = token => asyncFunction(state, CreateReportable(token));
        }

        internal QueuedBgProducer([DisallowNull] FSIOQueueService service, TState state, IProgress<IAsyncOperationInfo> progressHandler, [DisallowNull] Func<IStatusReportable, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
            : base(concurrencyId, state, progressHandler, service, activity, statusDescription)
        {
            _asyncFunction = token => asyncFunction(CreateReportable(token));
        }

        protected override Task<TResult> CreateCanceled(CancellationToken cancellationToken) => System.Threading.Tasks.Task.FromCanceled<TResult>(cancellationToken);

        protected async override Task<TResult> StartAsync(CancellationToken cancellationToken)
        {
            SetStarted();
            return await _asyncFunction(cancellationToken);
        }
    }

    //[Obsolete("Use QueuedBgProducer<TState, TProgress, TResult>")]
    //internal sealed class QueuedBgOperation<TResult> : FSIOQueueService.QueuedBgOperation<Task<TResult>>, IQueuedBgProducer<TResult>
    //{
    //    private readonly Func<CancellationToken, Task<TResult>> _asyncFunction;

    //    object IAsyncResult.AsyncState => (Task ?? throw new InvalidOperationException()).AsyncState;

    //    [Obsolete]
    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction) : this(service, asyncFunction, ActivityCode.CrawlingFileSystem) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
    //        : this(service, asyncFunction, activity, statusDescription, Guid.NewGuid()) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
    //        : base(concurrencyId, service, activity, statusDescription)
    //    {
    //        _asyncFunction = asyncFunction;
    //    }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, Guid concurrencyId)
    //        : this(service, asyncFunction, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<IStatusReportable, CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription = MessageCode.BackgroundJobPending)
    //        : this(service, asyncFunction, activity, statusDescription, Guid.NewGuid()) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<IStatusReportable, CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, Guid concurrencyId)
    //        : this(service, asyncFunction, activity, MessageCode.BackgroundJobPending, concurrencyId) { }

    //    internal QueuedBgOperation([DisallowNull] FSIOQueueService service, [DisallowNull] Func<IStatusReportable, CancellationToken, Task<TResult>> asyncFunction, ActivityCode activity, MessageCode statusDescription, Guid concurrencyId)
    //        : base(concurrencyId, service, activity, statusDescription)
    //    {
    //        _asyncFunction = token => asyncFunction(Reportable, token);
    //    }

    //    protected override Task<TResult> CreateCanceled(CancellationToken cancellationToken) => System.Threading.Tasks.Task.FromCanceled<TResult>(cancellationToken);

    //    protected async override Task<TResult> StartAsync(CancellationToken cancellationToken)
    //    {
    //        SetStarted();
    //        return await _asyncFunction(cancellationToken);
    //    }
    //}
}
