using FsInfoCat.AsyncOps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class TimedBackgroundOperation<TEvent, TProgress, TInstance, TTask, TOperation, TResultEvent> : Observable<TEvent>, ITimedBackgroundOperation
            where TEvent : ITimedBackgroundProgressEvent
            where TProgress : ITimedBackgroundProgress<TEvent>
            where TInstance : TimedBackgroundProgress<TEvent, TOperation, TResultEvent>, TProgress
            where TOperation : ITimedBackgroundOperation
            where TTask : Task
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent
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

            public TimeSpan Duration => _progress.Duration;

            internal TimedBackgroundOperation(TInstance progress, Func<TProgress, TTask> asyncMethodDelegate, CancellationTokenSource tokenSource)
            {
                _tokenSource = tokenSource;
                _progress = progress;
                progress.ProgressChanged += OnProgressChanged;
                Task = asyncMethodDelegate(progress) ?? throw new InvalidOperationException();
            }

            private void OnProgressChanged(object sender, TEvent e)
            {
                // TODO: Implement OnProgressChanged
                throw new NotImplementedException();
            }

            public void Cancel() => _tokenSource.Cancel();

            public void Cancel(bool throwOnFirstException) => _tokenSource.Cancel(throwOnFirstException);

            public void CancelAfter(int millisecondsDelay) => _tokenSource.CancelAfter(millisecondsDelay);

            public void CancelAfter(TimeSpan delay) => _tokenSource.CancelAfter(delay);

            public IDisposable Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
                => ObserverSubscriptionRelay<TEvent, ITimedBackgroundProgressEvent>.Create(this, observer);

            public IDisposable Subscribe(IObserver<IBackgroundProgressEvent> observer)
                => ObserverSubscriptionRelay<TEvent, IBackgroundProgressEvent>.Create(this, observer);
        }

        class TimedBackgroundOperation : TimedBackgroundOperation<ITimedBackgroundProgressEvent, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, TimedBackgroundProgress<ITimedBackgroundProgressEvent, ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>, Task, ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>
        {
            internal TimedBackgroundOperation(TimedBackgroundProgress<ITimedBackgroundProgressEvent, ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> progress, Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, async p =>
                {
                    progress.StartTimer();
                    try { await (asyncMethodDelegate(p) ?? throw new InvalidOperationException()); }
                    finally { progress.StopTimer(); }
                }, tokenSource)
            { }
        }

        class TimedBackgroundOperation<TState> : TimedBackgroundOperation<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>,
            TimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>>, Task, ITimedBackgroundOperation<TState>,
            ITimedBackgroundOperationCompletedEvent<TState>>, ITimedBackgroundOperation<TState>
        {
            public TState AsyncState { get; }

            internal TimedBackgroundOperation(TimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> progress,
                Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, async p =>
                {
                    progress.StartTimer();
                    try { await (asyncMethodDelegate(p) ?? throw new InvalidOperationException()); }
                    finally { progress.StopTimer(); }
                }, tokenSource)
            {
                AsyncState = progress.AsyncState;
            }

            public IDisposable Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
                => ObserverSubscriptionRelay<ITimedBackgroundProgressEvent<TState>, IBackgroundProgressEvent<TState>>.Create(this, observer);
        }
    }
}
