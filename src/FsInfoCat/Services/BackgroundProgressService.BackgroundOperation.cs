using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class BackgroundOperation<TEvent, TResultEvent> : BackgroundOperationInfo<TEvent, TResultEvent, IBackgroundProgress<TEvent>, Task>, IBackgroundOperation
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
        {
            private readonly TaskCompletionSource _completionSource = new();
            private readonly BackgroundProgressImpl _progress;

            public override Task Task => _completionSource.Task;

            protected override IBackgroundProgress<TEvent> Progress => _progress;

            private BackgroundOperation([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, CancellationToken[] linkedTokens) : base(service, linkedTokens)
            {
                _progress = new(this, activity, initialStatusDescription, parentId, eventFactory, Token);
            }

            internal static BackgroundOperation<TEvent, TResultEvent> Start([DisallowNull] BackgroundProgressService service,
                BackgroundOperationInfo parent, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription,
                [DisallowNull] Func<IBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, params CancellationToken[] linkedTokens)
            {
                BackgroundOperation<TEvent, TResultEvent> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, IBackgroundProgress<TEvent>, BackgroundOperation<TEvent, TResultEvent>>(service, parent, backgroundOperation,
                    backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
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

            protected override void RaiseOperationFaulted([DisallowNull] Exception exception)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> _eventFactory;

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                    [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, token) => _eventFactory = eventFactory;

                protected override TEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class BackgroundOperation<TEvent, TResultEvent, TState> : BackgroundOperationInfo<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, Task>, IBackgroundOperation<TState>
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent<TState>
        {
            private readonly TaskCompletionSource _completionSource;
            private readonly BackgroundProgressImpl _progress;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            protected override IBackgroundProgress<TState, TEvent> Progress => _progress;

            private BackgroundOperation([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, CancellationToken[] linkedTokens) : base(service, linkedTokens)
            {
                _completionSource = new(state);
                _progress = new(this, activity, initialStatusDescription, parentId, state, eventFactory, Token);
            }

            internal static BackgroundOperation<TEvent, TResultEvent, TState> Start([DisallowNull] BackgroundProgressService service,
                BackgroundOperationInfo parent, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, TState state,
                [DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, params CancellationToken[] linkedTokens)
            {
                BackgroundOperation<TEvent, TResultEvent, TState> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, state, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, BackgroundOperation<TEvent, TResultEvent, TState>>(service, parent, backgroundOperation,
                    backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
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

            protected override void RaiseOperationFaulted([DisallowNull] Exception exception)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress, IBackgroundProgress<TState, TEvent>
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                    [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }

                protected override TEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error) =>
                    _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class BackgroundOperation : BackgroundOperationInfo<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, IBackgroundProgress<IBackgroundProgressEvent>, Task>,
            IBackgroundOperation, IBackgroundEventFactory<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, IBackgroundProgress<IBackgroundProgressEvent>>
        {
            private readonly TaskCompletionSource _completionSource = new();
            protected readonly BackgroundProgressImpl _progress;
            private readonly Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> _onCompleted;

            public override Task Task => _completionSource.Task;

            protected override IBackgroundProgress<IBackgroundProgressEvent> Progress => _progress;

            private BackgroundOperation([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _progress = new(this, activity, initialStatusDescription, parentId, this, Token);
                _onCompleted = onCompleted ?? BackgroundProgressImpl.CreateCompletedEvent;
            }

            internal static BackgroundOperation Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, [DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
                Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, params CancellationToken[] linkedTokens)
            {
                BackgroundOperation backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, onCompleted, linkedTokens);
                OperationHelper.Start<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, IBackgroundProgress<IBackgroundProgressEvent>, BackgroundOperation>(service,
                    parent, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
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

            protected override void RaiseOperationFaulted([DisallowNull] Exception exception)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            public IBackgroundProgressEvent CreateProgressEvent([DisallowNull] IBackgroundProgress<IBackgroundProgressEvent> backgroundProgress, [DisallowNull] string statusDescription,
                [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress
            {
                private readonly IBackgroundEventFactory<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, IBackgroundProgress<IBackgroundProgressEvent>> _eventFactory;

                internal static IBackgroundOperationCompletedEvent CreateCompletedEvent(IBackgroundOperation backgroundOperation)
                {
                    throw new NotImplementedException();
                }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                    [DisallowNull] IBackgroundEventFactory<IBackgroundProgressEvent, IBackgroundOperationCompletedEvent, IBackgroundProgress<IBackgroundProgressEvent>> eventFactory,
                    CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, token) => _eventFactory = eventFactory;

                protected override IBackgroundProgressEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code,
                    byte? percentComplete, Exception error) =>
                    _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class BackgroundOperation<TState> : BackgroundOperationInfo<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>,
            IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task>, IBackgroundOperation<TState>,
            IBackgroundEventFactory<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>>
        {
            private readonly TaskCompletionSource _completionSource;
            protected readonly BackgroundProgressImpl _progress;
            private readonly Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> _onCompleted;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            protected override IBackgroundProgress<TState, IBackgroundProgressEvent<TState>> Progress => _progress;

            private BackgroundOperation([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _completionSource = new(state);
                _progress = new(this, activity, initialStatusDescription, parentId, state, this, Token);
                _onCompleted = onCompleted ?? BackgroundProgressImpl.CreateCompletedEvent;
            }
            
            internal static BackgroundOperation<TState> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent,
                [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, TState state,
                [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
                Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, params CancellationToken[] linkedTokens)
            {
                BackgroundOperation<TState> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, state, onCompleted, linkedTokens);
                OperationHelper.Start<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>,
                    BackgroundOperation<TState>>(service, parent, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource,
                    backgroundOperation.RaiseRanToCompletion);
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

            protected override void RaiseOperationFaulted([DisallowNull] Exception exception)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            public IBackgroundProgressEvent<TState> CreateProgressEvent([DisallowNull] IBackgroundProgress<IBackgroundProgressEvent<TState>> backgroundProgress,
                [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>
            {
                private readonly IBackgroundEventFactory<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>,
                    IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                    [DisallowNull] IBackgroundEventFactory<IBackgroundProgressEvent<TState>, IBackgroundOperationCompletedEvent<TState>,
                        IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }

                internal static IBackgroundOperationCompletedEvent<TState> CreateCompletedEvent(IBackgroundOperation<TState> backgroundOperation)
                {
                    throw new NotImplementedException();
                }

                protected override IBackgroundProgressEvent<TState> CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code,
                    byte? percentComplete, Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
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
