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

            public override Task Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<TEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private TimedBackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, Token, eventFactory);
            }

            internal static TimedBackgroundOperation<TEvent, TResultEvent> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId,
                Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundOperation<TEvent, TResultEvent> backgroundOperation = new(activity, initialStatusDescription, parentId, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TimedBackgroundOperation<TEvent, TResultEvent>>(service, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate,
                    backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            private void RaiseRanToCompletion()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationCanceled()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationFaulted(Exception exception)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent>.Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> _eventFactory;

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, CancellationToken token, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory) : base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    _eventFactory = eventFactory;
                }
            }
        }

        class TimedBackgroundOperation<TEvent, TResultEvent, TState> : TimedBackgroundOperationInfo<TEvent, TResultEvent, TimedBackgroundOperation<TEvent, TResultEvent, TState>.BackgroundProgressImpl, Task>, ITimedBackgroundOperation<TState>
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent<TState>
        {
            private readonly TaskCompletionSource _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<TEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private TimedBackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, TState state, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new(state);
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, state, Token, eventFactory);
            }

            internal static TimedBackgroundOperation<TEvent, TResultEvent, TState> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId, TState state,
                Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundOperation<TEvent, TResultEvent, TState> backgroundOperation = new(activity, initialStatusDescription, parentId, state, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TimedBackgroundOperation<TEvent, TResultEvent, TState>>(service, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate,
                    backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            private void RaiseRanToCompletion()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationCanceled()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationFaulted(Exception exception)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent<TState>>.Subscribe(IObserver<ITimedBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent>.Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress, ITimedBackgroundProgress<TState, TEvent>
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, TState state, CancellationToken token, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }
            }
        }

        class TimedBackgroundOperation : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent, TimedBackgroundOperation.BackgroundProgressImpl, Task>, ITimedBackgroundOperation,
            IBackgroundEventFactory<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>>
        {
            private readonly TaskCompletionSource _completionSource = new();

            public override Task Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<ITimedBackgroundProgressEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private TimedBackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, Token);
            }

            internal static TimedBackgroundOperation Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId,
                Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundOperation backgroundOperation = new(activity, initialStatusDescription, parentId, linkedTokens);
                OperationHelper.Start<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, TimedBackgroundOperation>(service, backgroundOperation, backgroundOperation.Progress,
                    asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            private void RaiseRanToCompletion()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationCanceled()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationFaulted(Exception exception)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
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

        class TimedBackgroundOperation<TState> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>, TimedBackgroundOperation<TState>.BackgroundProgressImpl, Task>,
            ITimedBackgroundOperation<TState>, IBackgroundEventFactory<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>>
        {
            private readonly TaskCompletionSource _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<ITimedBackgroundProgressEvent<TState>, BackgroundProgressImpl> EventFactory => this;

            private TimedBackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, TState state, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new(state);
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, state, Token);
            }

            internal static TimedBackgroundOperation<TState> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId, TState state,
                Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundOperation<TState> backgroundOperation = new(activity, initialStatusDescription, parentId, state, linkedTokens);
                OperationHelper.Start<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, TimedBackgroundOperation<TState>>(service,
                    backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            private void RaiseRanToCompletion()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationCanceled()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationFaulted(Exception exception)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
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
