using FsInfoCat.AsyncOps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class BackgroundOperation<TEvent, TResultEvent> : BackgroundOperationInfo<TEvent, TResultEvent, BackgroundOperation<TEvent, TResultEvent>.BackgroundProgressImpl, Task>, IBackgroundOperation
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
        {
            private readonly TaskCompletionSource _completionSource = new();

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal BackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate)
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
                        _completionSource.SetResult();
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

        class BackgroundOperation<TEvent, TResultEvent, TState> : BackgroundOperationInfo<TEvent, TResultEvent, BackgroundOperation<TEvent, TResultEvent, TState>.BackgroundProgressImpl, Task>, IBackgroundOperation<TState>
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent<TState>
        {
            private readonly TaskCompletionSource _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal BackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, TState state, Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate)
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
                        _completionSource.SetResult();
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

        class BackgroundOperation : BackgroundOperationInfo<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, BackgroundOperation.BackgroundProgressImpl, Task>, IBackgroundOperation
        {
            private readonly TaskCompletionSource _completionSource = new();

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal BackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate)
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
                        _completionSource.SetResult();
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

                protected override IBackgroundProgressEvent CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
                {
                    throw new NotImplementedException();
                }
            }
        }

        class BackgroundOperation<TState> : BackgroundOperationInfo<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>, BackgroundOperation<TState>.BackgroundProgressImpl, Task>, IBackgroundOperation<TState>
        {
            private readonly TaskCompletionSource _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task { get; }

            protected override BackgroundProgressImpl Progress { get; }

            internal BackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, TState state, Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate)
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
                        _completionSource.SetResult();
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

        class BackgroundOperation_old<TEvent, TProgress, TInstance, TTask, TOperation, TResultEvent> : Observable<TEvent>, IBackgroundOperation
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

            internal BackgroundOperation_old(TInstance progress, Func<TProgress, TTask> asyncMethodDelegate, CancellationTokenSource tokenSource)
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

        class BackgroundOperation_old : BackgroundOperation_old<IBackgroundProgressEvent, IBackgroundProgress<IBackgroundProgressEvent>, BackgroundProgress<IBackgroundProgressEvent, IBackgroundOperation, IBackgroundOperationCompletedEvent>, Task, IBackgroundOperation, IBackgroundOperationCompletedEvent>
        {
            internal BackgroundOperation_old(BackgroundProgress<IBackgroundProgressEvent, IBackgroundOperation, IBackgroundOperationCompletedEvent> progress, Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, asyncMethodDelegate, tokenSource) { }
        }

        class BackgroundOperation_old<TState> : BackgroundOperation_old<IBackgroundProgressEvent<TState>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, BackgroundProgress<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>, Task, IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>, IBackgroundOperation<TState>
        {
            public TState AsyncState { get; }

            internal BackgroundOperation_old(BackgroundProgress<TState, IBackgroundProgressEvent<TState>, IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> progress,
                Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, asyncMethodDelegate, tokenSource)
            {
                AsyncState = progress.AsyncState;
            }
        }
    }
}
