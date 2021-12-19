using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class TimedBackgroundFunc<TEvent, TResultEvent, TResult> : TimedBackgroundOperationInfo<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, Task<TResult>>,
            ITimedBackgroundFunc<TResult>
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource = new();
            private readonly BackgroundProgressImpl _progress;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override ITimedBackgroundProgress<TEvent> Progress => _progress;

            private TimedBackgroundFunc([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _progress = new(this, activity, initialStatusDescription, parentId, Stopwatch, eventFactory, Token);
            }

            internal static TimedBackgroundFunc<TEvent, TResultEvent, TResult> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, [DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundFunc<TEvent, TResultEvent, TResult> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TimedBackgroundFunc<TEvent, TResultEvent, TResult>, TResult>(service, parent, backgroundOperation,
                    backgroundOperation.Progress, backgroundOperation.Stopwatch, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation._progress.RaiseRanToCompletion);
                return backgroundOperation;
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
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

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TResult> eventFactory, CancellationToken token) :
                    base(operation, activity, initialStatusDescription, parentId, stopwatch, token) => _eventFactory = eventFactory;

                internal void RaiseRanToCompletion(TResult result)
                {
                    throw new NotImplementedException();
                }

                internal void RaiseOperationCanceled()
                {
                    throw new NotImplementedException();
                }

                internal void RaiseOperationFaulted([DisallowNull] Exception exception)
                {
                    throw new NotImplementedException();
                }

                protected override TEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState> : TimedBackgroundOperationInfo<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, Task<TResult>>,
            ITimedBackgroundFunc<TState, TResult>
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationResultEvent<TState, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;
            private readonly BackgroundProgressImpl _progress;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override ITimedBackgroundProgress<TState, TEvent> Progress => _progress;

            private TimedBackgroundFunc([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _completionSource = new(state);
                _progress = new(this, activity, initialStatusDescription, parentId, Stopwatch, state, eventFactory, Token);
            }

            internal static TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent,
                [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, TState state,
                [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory,
                params CancellationToken[] linkedTokens)
            {
                TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, state, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TimedBackgroundFunc<TEvent, TResultEvent, TResult, TState>, TResult>(service,
                    parent, backgroundOperation, backgroundOperation.Progress, backgroundOperation.Stopwatch, asyncMethodDelegate, backgroundOperation._completionSource,
                    backgroundOperation._progress.RaiseRanToCompletion);
                return backgroundOperation;
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent<TState>>.Subscribe(IObserver<ITimedBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<ITimedBackgroundProgressEvent>.Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress, ITimedBackgroundProgress<TState, TEvent>
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    TState state, [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TResult> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }

                internal void RaiseRanToCompletion(TResult result)
                {
                    throw new NotImplementedException();
                }

                internal void RaiseOperationCanceled()
                {
                    throw new NotImplementedException();
                }

                internal void RaiseOperationFaulted([DisallowNull] Exception exception)
                {
                    throw new NotImplementedException();
                }

                protected override TEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete,
                    Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class TimedBackgroundFunc<TResult> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>,
            ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>>, ITimedBackgroundFunc<TResult>,
            IBackgroundEventFactory<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;
            protected readonly BackgroundProgressImpl _progress;
            private readonly Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> _onCompleted;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override ITimedBackgroundProgress<ITimedBackgroundProgressEvent> Progress => _progress;

            private TimedBackgroundFunc([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _completionSource = new();
                _progress = new(this, activity, initialStatusDescription, parentId, Stopwatch, this, Token);
                _onCompleted = onCompleted ?? BackgroundProgressImpl.CreateCompletedEvent;
            }

            internal static TimedBackgroundFunc<TResult> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription,
                [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
                Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundFunc<TResult> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, onCompleted, linkedTokens);
                OperationHelper.Start<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>,
                    TimedBackgroundFunc<TResult>, TResult>(service, parent, backgroundOperation, backgroundOperation.Progress, backgroundOperation.Stopwatch, asyncMethodDelegate,
                    backgroundOperation._completionSource, backgroundOperation._progress.RaiseRanToCompletion);
                return backgroundOperation;
            }

            protected override IDisposable BaseSubscribe([DisallowNull] IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            public ITimedBackgroundProgressEvent CreateProgressEvent([DisallowNull] IBackgroundProgress<ITimedBackgroundProgressEvent> backgroundProgress,
                [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress
            {
                private readonly IBackgroundEventFactory<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>,
                    ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, TResult> _eventFactory;

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    [DisallowNull] IBackgroundEventFactory<ITimedBackgroundProgressEvent, ITimedBackgroundOperationResultEvent<TResult>,
                        ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, TResult> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, stopwatch, token) => _eventFactory = eventFactory;

                internal void RaiseRanToCompletion(TResult result) => _progress.Report(_onCompleted(this));

                internal void RaiseOperationCanceled()
                {
                    throw new NotImplementedException();
                }

                internal void RaiseOperationFaulted([DisallowNull] Exception exception)
                {
                    throw new NotImplementedException();
                }

                internal static ITimedBackgroundOperationResultEvent<TResult> CreateCompletedEvent(ITimedBackgroundFunc<TResult> backgroundOperation)
                {
                    throw new NotImplementedException();
                }

                protected override ITimedBackgroundProgressEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code,
                    byte? percentComplete, Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class TimedBackgroundFunc<TState, TResult> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>,
            ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>>, ITimedBackgroundFunc<TState, TResult>,
            IBackgroundEventFactory<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>,
                ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;
            protected readonly BackgroundProgressImpl _progress;
            private readonly Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> _onCompleted;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>> Progress => _progress;

            public TState AsyncState => Progress.AsyncState;

            private TimedBackgroundFunc([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _completionSource = new(state);
                _progress = new(this, activity, initialStatusDescription, parentId, Stopwatch, state, this, Token);
                _onCompleted = onCompleted ?? BackgroundProgressImpl.CreateCompletedEvent;
            }

            internal static TimedBackgroundFunc<TState, TResult> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, TState state,
                [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
                Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundFunc<TState, TResult> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, state, onCompleted, linkedTokens);
                OperationHelper.Start<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>,
                    ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, TimedBackgroundFunc<TState, TResult>, TResult>(service, parent, backgroundOperation,
                    backgroundOperation.Progress, backgroundOperation.Stopwatch, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation._progress.RaiseRanToCompletion);
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

            IDisposable IObservable<ITimedBackgroundProgressEvent>.Subscribe(IObserver<ITimedBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            public ITimedBackgroundProgressEvent<TState> CreateProgressEvent([DisallowNull] IBackgroundProgress<ITimedBackgroundProgressEvent<TState>> backgroundProgress,
                [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : TimedBackgroundProgress, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>
            {
                private readonly IBackgroundEventFactory<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>,
                    ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, TResult> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    TState state,
                    [DisallowNull] IBackgroundEventFactory<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationResultEvent<TState, TResult>,
                        ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, TResult> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }

                internal void RaiseRanToCompletion(TResult result) => _progress.Report(_onCompleted(this));

                internal void RaiseOperationCanceled()
                {
                    throw new NotImplementedException();
                }

                internal void RaiseOperationFaulted([DisallowNull] Exception exception)
                {
                    throw new NotImplementedException();
                }

                internal static ITimedBackgroundOperationResultEvent<TState, TResult> CreateCompletedEvent(ITimedBackgroundFunc<TState, TResult> backgroundOperation)
                {
                    throw new NotImplementedException();
                }

                protected override ITimedBackgroundProgressEvent<TState> CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code,
                    byte? percentComplete, Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }
    }
}
