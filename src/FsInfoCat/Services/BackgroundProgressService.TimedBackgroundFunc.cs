using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class TimedBackgroundFunc<TEvent, TResultEvent, TResult> : TimedBackgroundOperationInfo<TEvent, TResultEvent, TimedBackgroundFunc<TEvent, TResultEvent, TResult>.BackgroundProgressImpl, Task<TResult>>, ITimedBackgroundFunc<TResult>
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource = new();

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<TEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private TimedBackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, Token, eventFactory);
            }

            internal static TimedBackgroundFunc<TEvent, TResultEvent, TResult> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId,
                Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundFunc<TEvent, TResultEvent, TResult> backgroundOperation = new(activity, initialStatusDescription, parentId, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TimedBackgroundFunc<TEvent, TResultEvent, TResult>, TResult>(service, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate,
                    backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            private void RaiseRanToCompletion(TResult result)
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
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> _eventFactory;

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, CancellationToken token, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    _eventFactory = eventFactory;
                }
            }
        }

        class TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState> : TimedBackgroundOperationInfo<TEvent, TResultEvent, TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState>.BackgroundProgressImpl, Task<TResult>>, ITimedBackgroundFunc<TState, TResult>
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TState, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<TEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private TimedBackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, TState state, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new(state);
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, state, Token, eventFactory);
            }

            internal static TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId, TState state,
                Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState> backgroundOperation = new(activity, initialStatusDescription, parentId, state, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState>, TResult>(service, backgroundOperation, backgroundOperation.Progress,
                    asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            private void RaiseRanToCompletion(TResult result)
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
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, TState state, CancellationToken token, IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    AsyncState = state;
                    _eventFactory = eventFactory;
                }

                public TState AsyncState { get; }

                private readonly IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> _eventFactory;
            }
        }

        class TimedBackgroundFunc<TResult> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>, TimedBackgroundFunc<TResult>.BackgroundProgressImpl, Task<TResult>>,
            ITimedBackgroundFunc<TResult>, IBackgroundEventFactory<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<ITimedBackgroundProgressEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private TimedBackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new();
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, Token);
            }

            internal static TimedBackgroundFunc<TResult> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId,
                Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundFunc<TResult> backgroundOperation = new(activity, initialStatusDescription, parentId, linkedTokens);
                OperationHelper.Start<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, TimedBackgroundFunc<TResult>, TResult>(service, backgroundOperation,
                    backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            private void RaiseRanToCompletion(TResult result)
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
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, CancellationToken token) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token) { }
            }
        }

        class TimedBackgroundFunc<TState, TResult> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>,
            TimedBackgroundFunc<TState, TResult>.BackgroundProgressImpl, Task<TResult>>, ITimedBackgroundFunc<TState, TResult>,
            IBackgroundEventFactory<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            public TState AsyncState => Progress.AsyncState;

            protected override IBackgroundProgressEventFactory<ITimedBackgroundProgressEvent<TState>, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private TimedBackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, TState state, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new(state);
                Progress = new(activity, initialStatusDescription, parentId, Stopwatch, state, Token);
            }

            internal static TimedBackgroundFunc<TState, TResult> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId, TState state,
                Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundFunc<TState, TResult> backgroundOperation = new(activity, initialStatusDescription, parentId, state, linkedTokens);
                OperationHelper.Start<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, TimedBackgroundFunc<TState, TResult>, TResult>(service,
                    backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            private void RaiseRanToCompletion(TResult result)
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

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, TState state, CancellationToken token)
                    : base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    AsyncState = state;
                }
            }
        }

        class TimedBackgroundFunc_old<TResult> : TimedBackgroundOperation_old<ITimedBackgroundProgressEvent, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>,
            TimedBackgroundProgress<ITimedBackgroundProgressEvent, ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>>, Task<TResult>, ITimedBackgroundFunc<TResult>,
            ITimedBackgroundOperationResultEvent<TResult>>, ITimedBackgroundFunc<TResult>
        {
            internal TimedBackgroundFunc_old(TimedBackgroundProgress<ITimedBackgroundProgressEvent, ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> progress,
                Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, async p =>
                {
                    progress.StartTimer();
                    TResult result;
                    try { result = await (asyncMethodDelegate(p) ?? throw new InvalidOperationException()); }
                    finally { progress.StopTimer(); }
                    return result;
                }, tokenSource)
            { }
        }

        class TimedBackgroundFunc_old<TState, TResult> : TimedBackgroundOperation_old<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>,
            TimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>, Task<TResult>,
            ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>, ITimedBackgroundFunc<TState, TResult>
        {
            public TState AsyncState { get; }

            internal TimedBackgroundFunc_old(TimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> progress,
                Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, async p =>
                {
                    progress.StartTimer();
                    TResult result;
                    try { result = await (asyncMethodDelegate(p) ?? throw new InvalidOperationException()); }
                    finally { progress.StopTimer(); }
                    return result;
                }, tokenSource)
            {
                AsyncState = progress.AsyncState;
            }

            public IDisposable Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
                => ObserverSubscriptionRelay<ITimedBackgroundProgressEvent<TState>, IBackgroundProgressEvent<TState>>.Create(this, observer);
        }
    }
}
