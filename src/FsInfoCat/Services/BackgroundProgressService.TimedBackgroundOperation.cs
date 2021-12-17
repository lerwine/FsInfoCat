using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class TimedBackgroundOperation<TEvent, TResultEvent> : TimedBackgroundOperationInfo<TEvent, TResultEvent, TimedBackgroundOperation<TEvent, TResultEvent>.BackgroundProgressImpl, Task>, ITimedBackgroundOperation
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent
        {
            private readonly TaskCompletionSource _completionSource = new();

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal TimedBackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate)
            {
                Task = _completionSource.Task;
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, Token);
                Go(Progress, Stopwatch, asyncMethodDelegate).ContinueWith(task =>
                {
                    try { Stopwatch.Stop(); }
                    finally
                    {
                        if (task.IsCanceled)
                            _completionSource.SetCanceled();
                        else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                            _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                        else
                            _completionSource.SetResult();
                    }
                });
            }

            private static async Task Go(ITimedBackgroundProgress<TEvent> progress, Stopwatch stopwatch, Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate)
            {
                stopwatch.Start();
                await asyncMethodDelegate(progress);
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent>.Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress
            {
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, CancellationToken token) : base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                }
            }
        }

        class TimedBackgroundOperation<TEvent, TResultEvent, TState> : TimedBackgroundOperationInfo<TEvent, TResultEvent, TimedBackgroundOperation<TEvent, TResultEvent, TState>.BackgroundProgressImpl, Task>, ITimedBackgroundOperation<TState>
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent<TState>
        {
            private readonly TaskCompletionSource _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal TimedBackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, TState state, Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate)
            {
                _completionSource = new(state);
                Task = _completionSource.Task;
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, state, Token);
                Go(Progress, Stopwatch, asyncMethodDelegate).ContinueWith(task =>
                {
                    try { Stopwatch.Stop(); }
                    finally
                    {
                        if (task.IsCanceled)
                            _completionSource.SetCanceled();
                        else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                            _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                        else
                            _completionSource.SetResult();
                    }
                });
            }

            private static async Task Go(ITimedBackgroundProgress<TState, TEvent> progress, Stopwatch stopwatch, Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate)
            {
                stopwatch.Start();
                await asyncMethodDelegate(progress);
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent<TState>>.Subscribe(IObserver<ITimedBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent>.Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress, ITimedBackgroundProgress<TState, TEvent>
            {
                public TState AsyncState { get; }

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, TState state, CancellationToken token) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    AsyncState = state;
                }
            }
        }

        class TimedBackgroundOperation : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent, TimedBackgroundOperation.BackgroundProgressImpl, Task>, ITimedBackgroundOperation
        {
            private readonly TaskCompletionSource _completionSource = new();

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal TimedBackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate)
            {
                Task = _completionSource.Task;
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, Token);
                Go(Progress, Stopwatch, asyncMethodDelegate).ContinueWith(task =>
                {
                    try { Stopwatch.Stop(); }
                    finally
                    {
                        if (task.IsCanceled)
                            _completionSource.SetCanceled();
                        else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                            _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                        else
                            _completionSource.SetResult();
                    }
                });
            }

            private static async Task Go(ITimedBackgroundProgress<ITimedBackgroundProgressEvent> progress, Stopwatch stopwatch, Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate)
            {
                stopwatch.Start();
                await asyncMethodDelegate(progress);
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress
            {
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, CancellationToken token) : base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                }
            }
        }

        class TimedBackgroundOperation<TState> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>, TimedBackgroundOperation<TState>.BackgroundProgressImpl, Task>, ITimedBackgroundOperation<TState>
        {
            private readonly TaskCompletionSource _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal TimedBackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, TState state, Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate)
            {
                _completionSource = new(state);
                Task = _completionSource.Task;
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, state, Token);
                Go(Progress, Stopwatch, asyncMethodDelegate).ContinueWith(task =>
                {
                    try { Stopwatch.Stop(); }
                    finally
                    {
                        if (task.IsCanceled)
                            _completionSource.SetCanceled();
                        else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                            _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                        else
                            _completionSource.SetResult();
                    }
                });
            }

            private static async Task Go(ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>> progress, Stopwatch stopwatch, Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate)
            {
                stopwatch.Start();
                await asyncMethodDelegate(progress);
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent>.Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>
            {
                public TState AsyncState { get; }

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, TState state, CancellationToken token) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    AsyncState = state;
                }
            }
        }

        class TimedBackgroundOperation_old<TEvent, TProgress, TInstance, TTask, TOperation, TResultEvent> : Observable<TEvent>, ITimedBackgroundOperation
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

            internal TimedBackgroundOperation_old(TInstance progress, Func<TProgress, TTask> asyncMethodDelegate, CancellationTokenSource tokenSource)
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

            public IDisposable Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
                => ObserverSubscriptionRelay<TEvent, ITimedBackgroundProgressEvent>.Create(this, observer);

            public IDisposable Subscribe(IObserver<IBackgroundProgressEvent> observer)
                => ObserverSubscriptionRelay<TEvent, IBackgroundProgressEvent>.Create(this, observer);
        }

        class TimedBackgroundOperation_old : TimedBackgroundOperation_old<ITimedBackgroundProgressEvent, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, TimedBackgroundProgress<ITimedBackgroundProgressEvent, ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>, Task, ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>
        {
            internal TimedBackgroundOperation_old(TimedBackgroundProgress<ITimedBackgroundProgressEvent, ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> progress, Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, async p =>
                {
                    progress.StartTimer();
                    try { await (asyncMethodDelegate(p) ?? throw new InvalidOperationException()); }
                    finally { progress.StopTimer(); }
                }, tokenSource)
            { }
        }

        class TimedBackgroundOperation_old<TState> : TimedBackgroundOperation_old<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>,
            TimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>>, Task, ITimedBackgroundOperation<TState>,
            ITimedBackgroundOperationCompletedEvent<TState>>, ITimedBackgroundOperation<TState>
        {
            public TState AsyncState { get; }

            internal TimedBackgroundOperation_old(TimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> progress,
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
