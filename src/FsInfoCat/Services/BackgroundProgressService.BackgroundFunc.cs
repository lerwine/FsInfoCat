using FsInfoCat.AsyncOps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class BackgroundFunc<TEvent, TResultEvent, TResult> : BackgroundOperationInfo<TEvent, TResultEvent, BackgroundFunc<TEvent, TResultEvent, TResult>.BackgroundProgressImpl, Task<TResult>>, IBackgroundFunc<TResult>
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource = new();

            public override Task<TResult> Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            Task IBackgroundOperation.Task => Task;

            internal BackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate)
            {
                Task = _completionSource.Task;
                Progress = new(activity, initialStatusDescription, parentId, Token);
                asyncMethodDelegate(Progress).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        _completionSource.SetCanceled();
                    else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                        _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                    else
                        _completionSource.SetResult(task.Result);
                });
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress
            {
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, CancellationToken token) : base(activity, initialStatusDescription, parentId, token)
                {
                }

                protected override TEvent CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
                {
                    throw new NotImplementedException();
                }
            }
        }

        class BackgroundFunc<TEvent, TResultEvent, TResult, TState> : BackgroundOperationInfo<TEvent, TResultEvent, BackgroundFunc<TEvent, TResultEvent, TResult, TState>.BackgroundProgressImpl, Task<TResult>>, IBackgroundFunc<TState, TResult>
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TState, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task<TResult> Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            Task IBackgroundOperation.Task => Task;

            internal BackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, TState state, Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate)
            {
                _completionSource = new(state);
                Task = _completionSource.Task;
                Progress = new(activity, initialStatusDescription, parentId, state, Token);
                asyncMethodDelegate(Progress).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        _completionSource.SetCanceled();
                    else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                        _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                    else
                        _completionSource.SetResult(task.Result);
                });
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress, IBackgroundProgress<TState, TEvent>
            {
                public TState AsyncState { get; }

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, TState state, CancellationToken token) : base(activity, initialStatusDescription, parentId, token)
                {
                    AsyncState = state;
                }

                protected override TEvent CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
                {
                    throw new NotImplementedException();
                }
            }
        }

        class BackgroundFunc<TResult> : BackgroundOperationInfo<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>, BackgroundFunc<TResult>.BackgroundProgressImpl, Task>, IBackgroundOperation
        {
            private readonly TaskCompletionSource<TResult> _completionSource = new();

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal BackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate)
            {
                Task = _completionSource.Task;
                Progress = new(activity, initialStatusDescription, parentId, Token);
                asyncMethodDelegate(Progress).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        _completionSource.SetCanceled();
                    else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                        _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                    else
                        _completionSource.SetResult(task.Result);
                });
            }

            internal class BackgroundProgressImpl : BackgroundProgress
            {
                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, CancellationToken token) : base(activity, initialStatusDescription, parentId, token)
                {
                }

                protected override IBackgroundProgressEvent CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
                {
                    throw new NotImplementedException();
                }
            }
        }

        class BackgroundFunc<TState, TResult> : BackgroundOperationInfo<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>, BackgroundFunc<TState, TResult>.BackgroundProgressImpl, Task<TResult>>, IBackgroundFunc<TState, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task<TResult> Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            Task IBackgroundOperation.Task => Task;

            internal BackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, TState state, Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate)
            {
                _completionSource = new(state);
                Task = _completionSource.Task;
                Progress = new(activity, initialStatusDescription, parentId, state, Token);
                asyncMethodDelegate(Progress).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        _completionSource.SetCanceled();
                    else if (task.IsFaulted)
#pragma warning disable CS8604 // Possible null reference argument.
                        _completionSource.SetException(task.Exception);
#pragma warning restore CS8604 // Possible null reference argument.
                    else
                        _completionSource.SetResult(task.Result);
                });
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>
            {
                public TState AsyncState { get; }

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, TState state, CancellationToken token) : base(activity, initialStatusDescription, parentId, token)
                {
                    AsyncState = state;
                }

                protected override IBackgroundProgressEvent<TState> CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
                {
                    throw new NotImplementedException();
                }
            }
        }

        class BackgroundFunc_old<TResult> : BackgroundOperation_old<IBackgroundProgressEvent, IBackgroundProgress<IBackgroundProgressEvent>, BackgroundProgress<IBackgroundProgressEvent, IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>, Task<TResult>, IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>, IBackgroundFunc<TResult>
        {
            internal BackgroundFunc_old(BackgroundProgress<IBackgroundProgressEvent, IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> progress,
                Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, asyncMethodDelegate, tokenSource) { }
        }

        class BackgroundFunc_old<TState, TResult> : BackgroundOperation_old<IBackgroundProgressEvent<TState>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, BackgroundProgress<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>, Task<TResult>, IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>, IBackgroundFunc<TState, TResult>
        {
            public TState AsyncState { get; }

            internal BackgroundFunc_old(BackgroundProgress<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> progress,
                Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, asyncMethodDelegate, tokenSource)
            {
                AsyncState = progress.AsyncState;
            }
        }
    }
}
