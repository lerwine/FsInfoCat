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

            public override Task Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<TEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private BackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                Progress = new(activity, initialStatusDescription, parentId, Token, eventFactory);
            }

            internal static BackgroundOperation<TEvent, TResultEvent> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId,
                Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, params CancellationToken[] linkedTokens)
            {
                BackgroundOperation<TEvent, TResultEvent> backgroundOperation = new(activity, initialStatusDescription, parentId, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, IBackgroundProgress<TEvent>, BackgroundOperation<TEvent, TResultEvent>>(service, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate,
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

            internal class BackgroundProgressImpl : BackgroundProgress
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> _eventFactory;

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, CancellationToken token, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory) : base(activity, initialStatusDescription, parentId, token)
                {
                    _eventFactory = eventFactory;
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

            public override Task Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<TEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private BackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, TState state, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new(state);
                Progress = new(activity, initialStatusDescription, parentId, state, Token, eventFactory);
            }

            internal static BackgroundOperation<TEvent, TResultEvent, TState> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId, TState state,
                Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, params CancellationToken[] linkedTokens)
            {
                BackgroundOperation<TEvent, TResultEvent, TState> backgroundOperation = new(activity, initialStatusDescription, parentId, state, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, BackgroundOperation<TEvent, TResultEvent, TState>>(service, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate,
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

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress, IBackgroundProgress<TState, TEvent>
            {
                public TState AsyncState { get; }

                private readonly IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> _eventFactory;

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, TState state, CancellationToken token, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory) : base(activity, initialStatusDescription, parentId, token)
                {
                    AsyncState = state;
                    _eventFactory = eventFactory;
                }

                protected override TEvent CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
                {
                    throw new NotImplementedException();
                }
            }
        }

        class BackgroundOperation : BackgroundOperationInfo<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, BackgroundOperation.BackgroundProgressImpl, Task>, IBackgroundOperation,
            IBackgroundEventFactory<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, IBackgroundProgress<IBackgroundProgressEvent>>
        {
            private readonly TaskCompletionSource _completionSource = new();

            public override Task Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<IBackgroundProgressEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private BackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                Progress = new(activity, initialStatusDescription, parentId, Token);
            }

            internal static BackgroundOperation Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId,
                Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, params CancellationToken[] linkedTokens)
            {
                BackgroundOperation backgroundOperation = new(activity, initialStatusDescription, parentId, linkedTokens);
                OperationHelper.Start<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, IBackgroundProgress<IBackgroundProgressEvent>, BackgroundOperation>(service, backgroundOperation, backgroundOperation.Progress,
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

        class BackgroundOperation<TState> : BackgroundOperationInfo<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>, BackgroundOperation<TState>.BackgroundProgressImpl, Task>, IBackgroundOperation<TState>,
            IBackgroundEventFactory<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>>
        {
            private readonly TaskCompletionSource _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<IBackgroundProgressEvent<TState>, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private BackgroundOperation(string activity, string initialStatusDescription, Guid? parentId, TState state, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new(state);
                Progress = new(activity, initialStatusDescription, parentId, state, Token);
            }

            internal static BackgroundOperation<TState> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId, TState state,
                Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, params CancellationToken[] linkedTokens)
            {
                BackgroundOperation<TState> backgroundOperation = new(activity, initialStatusDescription, parentId, state, linkedTokens);
                OperationHelper.Start<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, BackgroundOperation<TState>>(service,
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
