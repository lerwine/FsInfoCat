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
        class TimedBackgroundOperation<TEvent, TResultEvent> : TimedBackgroundOperationInfo<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, Task>, ITimedBackgroundOperation
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent
        {
            private readonly TaskCompletionSource _completionSource = new();
            private readonly BackgroundProgressImpl _progress;

            public override Task Task => _completionSource.Task;

            protected override ITimedBackgroundProgress<TEvent> Progress => _progress;

            private TimedBackgroundOperation([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, CancellationToken[] linkedTokens) : base(service, linkedTokens)
            {
                _progress = new(this, activity, initialStatusDescription, parentId, Stopwatch, eventFactory, Token);
            }

            internal static TimedBackgroundOperation<TEvent, TResultEvent> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, [DisallowNull] Func<ITimedBackgroundProgress<TEvent>, Task> asyncMethodDelegate,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundOperation<TEvent, TResultEvent> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>, TimedBackgroundOperation<TEvent, TResultEvent>>(service, parent, backgroundOperation,
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
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> _eventFactory;

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TEvent>> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, stopwatch, token) => _eventFactory = eventFactory;

                internal void RaiseRanToCompletion()
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

        class TimedBackgroundOperation<TEvent, TResultEvent, TState> : TimedBackgroundOperationInfo<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, Task>,
            ITimedBackgroundOperation<TState>
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent<TState>
        {
            private readonly TaskCompletionSource _completionSource;
            private readonly BackgroundProgressImpl _progress;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            protected override ITimedBackgroundProgress<TState, TEvent> Progress => _progress;

            private TimedBackgroundOperation([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _completionSource = new(state);
                _progress = new(this, activity, initialStatusDescription, parentId, Stopwatch, state, eventFactory, Token);
            }

            internal static TimedBackgroundOperation<TEvent, TResultEvent, TState> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, TState state, [DisallowNull] Func<ITimedBackgroundProgress<TState, TEvent>, Task> asyncMethodDelegate,
                [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundOperation<TEvent, TResultEvent, TState> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, state, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>, TimedBackgroundOperation<TEvent, TResultEvent, TState>>(service, parent, backgroundOperation,
                    backgroundOperation.Progress, backgroundOperation.Stopwatch, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation._progress.RaiseRanToCompletion);
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
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    TState state, [DisallowNull] IBackgroundEventFactory<TEvent, TResultEvent, ITimedBackgroundProgress<TState, TEvent>> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }

                internal void RaiseRanToCompletion()
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

        class TimedBackgroundOperation : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent,
            ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task>, ITimedBackgroundOperation,
            IBackgroundEventFactory<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>>
        {
            private readonly TaskCompletionSource _completionSource = new();
            protected readonly BackgroundProgressImpl _progress;
            private readonly Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> _onCompleted;

            public override Task Task => _completionSource.Task;

            protected override ITimedBackgroundProgress<ITimedBackgroundProgressEvent> Progress => _progress;

            private TimedBackgroundOperation([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId,
                Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, CancellationToken[] linkedTokens)
                : base(service, linkedTokens)
            {
                _progress = new(this, activity, initialStatusDescription, parentId, Stopwatch, this, Token);
                _onCompleted = onCompleted ?? BackgroundProgressImpl.CreateCompletedEvent;
            }

            internal static TimedBackgroundOperation Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, [DisallowNull] Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
                Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundOperation backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, onCompleted, linkedTokens);
                OperationHelper.Start<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>,
                    TimedBackgroundOperation>(service, parent, backgroundOperation, backgroundOperation.Progress, backgroundOperation.Stopwatch, asyncMethodDelegate,
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
                private readonly IBackgroundEventFactory<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent,
                    ITimedBackgroundProgress<ITimedBackgroundProgressEvent>> _eventFactory;

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    [DisallowNull] IBackgroundEventFactory<ITimedBackgroundProgressEvent, ITimedBackgroundOperationCompletedEvent,
                        ITimedBackgroundProgress<ITimedBackgroundProgressEvent>> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, stopwatch, token) => _eventFactory = eventFactory;

                internal void RaiseRanToCompletion()
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

                internal static ITimedBackgroundOperationCompletedEvent CreateCompletedEvent(ITimedBackgroundOperation backgroundOperation)
                {
                    throw new NotImplementedException();
                }

                protected override ITimedBackgroundProgressEvent CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code,
                    byte? percentComplete, Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }

        class TimedBackgroundOperation<TState> : TimedBackgroundOperationInfo<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>,
            ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task>,
            ITimedBackgroundOperation<TState>, IBackgroundEventFactory<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>,
                ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>>
        {
            private readonly TaskCompletionSource _completionSource;
            protected readonly BackgroundProgressImpl _progress;
            private readonly Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> _onCompleted;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            protected override ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>> Progress => _progress;

            private TimedBackgroundOperation([DisallowNull] BackgroundProgressService service, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, TState state,
                Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, CancellationToken[] linkedTokens) : base(service, linkedTokens)
            {
                _completionSource = new(state);
                _progress = new(this, activity, initialStatusDescription, parentId, Stopwatch, state, this, Token);
                _onCompleted = onCompleted ?? BackgroundProgressImpl.CreateCompletedEvent;
            }

            internal static TimedBackgroundOperation<TState> Start([DisallowNull] BackgroundProgressService service, BackgroundOperationInfo parent, [DisallowNull] string activity,
                [DisallowNull] string initialStatusDescription, TState state,
               [DisallowNull] Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
               Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, params CancellationToken[] linkedTokens)
            {
                TimedBackgroundOperation<TState> backgroundOperation = new(service, activity, initialStatusDescription, parent?.OperationId, state, onCompleted, linkedTokens);
                OperationHelper.Start<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>,
                    ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, TimedBackgroundOperation<TState>>(service, parent, backgroundOperation, backgroundOperation.Progress,
                    backgroundOperation.Stopwatch, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation._progress.RaiseRanToCompletion);
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
                private readonly IBackgroundEventFactory<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>,
                    ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>> _eventFactory;

                public TState AsyncState { get; }

                internal BackgroundProgressImpl([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    TState state,
                    [DisallowNull] IBackgroundEventFactory<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundOperationCompletedEvent<TState>,
                        ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>> eventFactory, CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, stopwatch, token)
                {
                    _eventFactory = eventFactory;
                    AsyncState = state;
                }

                internal void RaiseRanToCompletion()
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

                internal static ITimedBackgroundOperationCompletedEvent<TState> CreateCompletedEvent(ITimedBackgroundOperation<TState> backgroundOperation)
                {
                    throw new NotImplementedException();
                }

                protected override ITimedBackgroundProgressEvent<TState> CreateEvent([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode? code,
                    byte? percentComplete, Exception error) => _eventFactory.CreateProgressEvent(this, statusDescription, currentOperation, code, percentComplete, error);
            }
        }
    }
}
