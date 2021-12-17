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

            public override Task<TResult> Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            Task IBackgroundOperation.Task => Task;

            internal TimedBackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate)
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
                            _completionSource.SetResult(task.Result);
                    }
                });
            }

            private static async Task<TResult> Go(ITimedBackgroundProgress<TEvent> progress, Stopwatch stopwatch, Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate)
            {
                stopwatch.Start();
                return await asyncMethodDelegate(progress);
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
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, CancellationToken token) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token) { }
            }
        }

        class TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState> : TimedBackgroundOperationInfo<TEvent, TResultEvent, TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState>.BackgroundProgressImpl, Task<TResult>>, ITimedBackgroundFunc<TState, TResult>
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TState, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public TState AsyncState => Progress.AsyncState;

            Task IBackgroundOperation.Task => Task;

            protected override BackgroundProgressImpl Progress { get; }

            public override Task<TResult> Task { get; }

            internal TimedBackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, TState state, Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate)
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
                            _completionSource.SetResult(task.Result);
                    }
                });
            }

            private static async Task<TResult> Go(ITimedBackgroundProgress<TEvent> progress, Stopwatch stopwatch, Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate)
            {
                stopwatch.Start();
                return await asyncMethodDelegate(progress);
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
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, TState state, CancellationToken token) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    AsyncState = state;
                }

                public TState AsyncState { get; }
            }
        }

        class TimedBackgroundFunc<TResult> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>, TimedBackgroundFunc<TResult>.BackgroundProgressImpl, Task<TResult>>, ITimedBackgroundFunc<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task<TResult> Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            Task IBackgroundOperation.Task => Task;

            internal TimedBackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate)
            {
                _completionSource = new();
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
                            _completionSource.SetResult(task.Result);
                    }
                });
            }

            private static async Task<TResult> Go(ITimedBackgroundProgress<ITimedBackgroundProgressEvent> progress, Stopwatch stopwatch, Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate)
            {
                stopwatch.Start();
                return await asyncMethodDelegate(progress);
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress
            {
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, CancellationToken token) :
                    base(activity, initialStatusDescription, parentId, stopwatch, token) { }
            }
        }

        class TimedBackgroundFunc<TResult, TState> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>, TimedBackgroundFunc<TResult, TState>.BackgroundProgressImpl, Task<TResult>>, ITimedBackgroundFunc<TState, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public override Task<TResult> Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            public TState AsyncState => Progress.AsyncState;

            Task IBackgroundOperation.Task => Task;

            internal TimedBackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, TState state, Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate)
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
                            _completionSource.SetResult(task.Result);
                    }
                });
            }

            private static async Task<TResult> Go(ITimedBackgroundProgress<ITimedBackgroundProgressEvent<TState>> progress, Stopwatch stopwatch, Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate)
            {
                stopwatch.Start();
                return await asyncMethodDelegate(progress);
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
