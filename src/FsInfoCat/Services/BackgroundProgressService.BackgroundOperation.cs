using FsInfoCat.AsyncOps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class BackgroundOperation<TEvent, TProgress, TInstance, TTask, TOperation, TResultEvent> : Observable<TEvent>, IBackgroundOperation
            where TEvent : IBackgroundProgressEvent
            where TProgress : IBackgroundProgress<TEvent>
            where TInstance : BackgroundProgress<TEvent, TOperation, TResultEvent>, TProgress
            where TOperation : IBackgroundOperation
            where TTask : Task
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
        {
            private readonly CancellationTokenSource _tokenSource;
            private readonly TProgress _progress;

            public TTask Task { get; }

            public bool IsCancellationRequested => _tokenSource.IsCancellationRequested;

            public Guid OperationId => _progress.OperationId;

            public string Activity => _progress.Activity;

            public string StatusDescription => _progress.StatusDescription;

            public string CurrentOperation => _progress.CurrentOperation;

            public Guid? ParentId => _progress.ParentId;

            public byte? PercentComplete => _progress.PercentComplete;

            Task IBackgroundOperation.Task => Task;

            internal BackgroundOperation(TInstance progress, Func<TProgress, TTask> asyncMethodDelegate, CancellationTokenSource tokenSource)
            {
                _tokenSource = tokenSource;
                _progress = progress;
                progress.ProgressChanged += OnProgressChanged;
                Task = asyncMethodDelegate(progress) ?? throw new InvalidOperationException();
            }

            private void OnProgressChanged(object sender, TEvent e) => RaiseNext(e);

            public void Cancel() => _tokenSource.Cancel();

            public void Cancel(bool throwOnFirstException) => _tokenSource.Cancel(throwOnFirstException);

            public void CancelAfter(int millisecondsDelay) => _tokenSource.CancelAfter(millisecondsDelay);

            public void CancelAfter(TimeSpan delay) => _tokenSource.CancelAfter(delay);

            public IDisposable Subscribe(IObserver<IBackgroundProgressEvent> observer) => ObserverSubscriptionRelay<TEvent, IBackgroundProgressEvent>.Create(this, observer);
        }

        class BackgroundOperation : BackgroundOperation<IBackgroundProgressEvent, IBackgroundProgress<IBackgroundProgressEvent>, BackgroundProgress<IBackgroundProgressEvent, IBackgroundOperation, IBackgroundOperationCompletedEvent>, Task, IBackgroundOperation, IBackgroundOperationCompletedEvent>
        {
            internal BackgroundOperation(BackgroundProgress<IBackgroundProgressEvent, IBackgroundOperation, IBackgroundOperationCompletedEvent> progress, Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, asyncMethodDelegate, tokenSource) { }
        }

        class BackgroundOperation<TState> : BackgroundOperation<IBackgroundProgressEvent<TState>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, BackgroundProgress<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>, Task, IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>, IBackgroundOperation<TState>
        {
            public TState AsyncState { get; }

            internal BackgroundOperation(BackgroundProgress<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> progress,
                Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, asyncMethodDelegate, tokenSource)
            {
                AsyncState = progress.AsyncState;
            }
        }
    }
}
