using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class BackgroundFunc<TEvent, TResultEvent, TResult> : BackgroundOperationInfo<TEvent, TResultEvent, IBackgroundProgress<TEvent>, Task<TResult>>, IBackgroundFunc<TResult>
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource = new();
            private readonly BackgroundProgressImpl _progress;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override IBackgroundProgress<TEvent> Progress => _progress;

            private BackgroundFunc([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, CancellationToken[] linkedTokens)
                : base(service, linkedTokens) => _progress = new(this, activity, initialStatusDescription, parentId, eventFactory, Token);

            internal static BackgroundFunc<TEvent, TResultEvent, TResult> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, [DisallowNull] Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, params CancellationToken[] linkedTokens)
            {
                BackgroundFunc<TEvent, TResultEvent, TResult> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, IBackgroundProgress<TEvent>, BackgroundFunc<TEvent, TResultEvent, TResult>, TResult>(service, parent, backgroundOperation,
                    backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            private void RaiseRanToCompletion(TResult result)
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

            internal class BackgroundProgressImpl : BackgroundProgress
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> _eventFactory;

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                    [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory,
                    CancellationToken token) : base(operation, activity, initialStatusDescription, parentId, token) => _eventFactory = eventFactory;

                protected override TEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class BackgroundFunc<TEvent, TResultEvent, TResult, TState> : BackgroundOperationInfo<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, Task<TResult>>,
            IBackgroundFunc<TState, TResult>
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TState, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;
            private readonly BackgroundProgressImpl _progress;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override IBackgroundProgress<TState, TEvent> Progress => _progress;

            private BackgroundFunc([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _completionSource = new(state);
                _progress = new(this, activity, initialStatusDescription, parentId, state, eventFactory, Token);
            }

            internal static BackgroundFunc<TEvent, TResultEvent, TResult, TState> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent,
                [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, TState state,
                [DisallowNull] Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, params CancellationToken[] linkedTokens)
            {
                BackgroundFunc<TEvent, TResultEvent, TResult, TState> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, state, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, BackgroundFunc<TEvent, TResultEvent, TResult, TState>, TResult>(service, parent,
                    backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            private void RaiseRanToCompletion(TResult result)
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

            internal class BackgroundProgressImpl : BackgroundProgress, IBackgroundProgress<TState, TEvent>
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                    [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }

                protected override TEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class BackgroundFunc<TResult> : BackgroundOperationInfo<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>, IBackgroundProgress<IBackgroundProgressEvent>, Task>,
            IBackgroundFunc<TResult>, IBackgroundEventFactory<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>, IBackgroundProgress<IBackgroundProgressEvent>, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource = new();
            protected readonly BackgroundProgressImpl _progress;
            private readonly Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> _onCompleted;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override IBackgroundProgress<IBackgroundProgressEvent> Progress => _progress;

            private BackgroundFunc([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, CancellationToken[] linkedTokens) : base(service, linkedTokens)
            {
                _progress = new(this, activity, initialStatusDescription, parentId, this, Token);
                _onCompleted = onCompleted ?? BackgroundProgressImpl.CreateCompletedEvent;
            }

            internal static BackgroundFunc<TResult> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent,
                [DisallowNull] string activity, [DisallowNull] string initialStatusDescription,
                [DisallowNull] Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, params CancellationToken[] linkedTokens)
            {
                BackgroundFunc<TResult> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, onCompleted, linkedTokens);
                OperationHelper.Start<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>, IBackgroundProgress<IBackgroundProgressEvent>,
                    BackgroundFunc<TResult>, TResult>(service, parent, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource,
                    backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            private void RaiseRanToCompletion(TResult result) => _progress.Report(_onCompleted(this));

            protected override void RaiseOperationCanceled()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationFaulted([DisallowNull] Exception exception)
            {
                throw new NotImplementedException();
            }

            public IBackgroundProgressEvent CreateProgressEvent([DisallowNull] IBackgroundProgress<IBackgroundProgressEvent> backgroundProgress,
                [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress
            {
                private readonly IBackgroundEventFactory<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>,
                    IBackgroundProgress<IBackgroundProgressEvent>, TResult> _eventFactory;

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                    [DisallowNull] IBackgroundEventFactory<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>, IBackgroundProgress<IBackgroundProgressEvent>, TResult> eventFactory,
                    CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, token) => _eventFactory = eventFactory;

                internal static IBackgroundOperationResultEvent<TResult> CreateCompletedEvent(IBackgroundFunc<TResult> backgroundOperation)
                {
                    throw new NotImplementedException();
                }

                protected override IBackgroundProgressEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code,
                    byte? percentComplete, Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class BackgroundFunc<TState, TResult> : BackgroundOperationInfo<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>,
            IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>>, IBackgroundFunc<TState, TResult>,
            IBackgroundEventFactory<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>,
                TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;
            protected readonly BackgroundProgressImpl _progress;
            private readonly Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> _onCompleted;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override IBackgroundProgress<TState, IBackgroundProgressEvent<TState>> Progress => _progress;

            private BackgroundFunc([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _completionSource = new(state);
                _progress = new(this, activity, initialStatusDescription, parentId, state, this, Token);
                _onCompleted = onCompleted ?? BackgroundProgressImpl.CreateCompletedEvent;
            }

            internal static BackgroundFunc<TState, TResult> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, TState state,
                [DisallowNull] Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, params CancellationToken[] linkedTokens)
            {
                BackgroundFunc<TState, TResult> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, state, onCompleted, linkedTokens);
                OperationHelper.Start<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>,
                    BackgroundFunc<TState, TResult>, TResult>(service, parent, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource,
                    backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            private void RaiseRanToCompletion(TResult result) => _progress.Report(_onCompleted(this));

            protected override void RaiseOperationCanceled()
            {
                throw new NotImplementedException();
            }

            protected override void RaiseOperationFaulted([DisallowNull] Exception exception)
            {
                throw new NotImplementedException();
            }

            public IBackgroundProgressEvent<TState> CreateProgressEvent([DisallowNull] IBackgroundProgress<IBackgroundProgressEvent<TState>> backgroundProgress,
                [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
            {
                if (backgroundProgress is null) throw new ArgumentNullException(nameof(backgroundProgress));
                if (string.IsNullOrWhiteSpace(statusDescription)) throw new ArgumentException($"'{nameof(statusDescription)}' cannot be null or whitespace.", nameof(statusDescription));
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>
            {
                private readonly IBackgroundEventFactory<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>,
                    IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, TResult> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                    [DisallowNull] IBackgroundEventFactory<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>,
                        IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, TResult> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }

                internal static IBackgroundOperationResultEvent<TState, TResult> CreateCompletedEvent(IBackgroundFunc<TState, TResult> backgroundOperation)
                {
                    throw new NotImplementedException();
                }

                protected override IBackgroundProgressEvent<TState> CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code,
                    byte? percentComplete, Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
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
