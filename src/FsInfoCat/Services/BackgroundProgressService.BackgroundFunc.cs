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

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<TEvent, BackgroundProgressImpl> EventFactory { get; }

            private BackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                Progress = new(activity, initialStatusDescription, parentId, Token, eventFactory);
            }

            internal static BackgroundFunc<TEvent, TResultEvent, TResult> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId,
                Func<IBackgroundProgress<TEvent>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory, params CancellationToken[] linkedTokens)
            {
                BackgroundFunc<TEvent, TResultEvent, TResult> backgroundOperation = new(activity, initialStatusDescription, parentId, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, IBackgroundProgress<TEvent>, BackgroundFunc<TEvent, TResultEvent, TResult>, TResult>(service, backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate,
                    backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
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

            protected override void RaiseOperationFaulted(Exception exception)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress
            {
                private readonly IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> _eventFactory;

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, CancellationToken token, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TEvent>, TResult> eventFactory) : base(activity, initialStatusDescription, parentId, token)
                {
                    _eventFactory = eventFactory;
                }

                protected override TEvent CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
                {
                    throw new NotImplementedException();
                }
            }
        }

        class BackgroundFunc<TEvent, TResultEvent, TResult, TState> : BackgroundOperationInfo<TEvent, TResultEvent, BackgroundFunc<TEvent, TResultEvent, TResult, TState>.BackgroundProgressImpl, Task<TResult>>,
            IBackgroundFunc<TState, TResult>
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationResultEvent<TState, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<TEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private BackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, TState state, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new(state);
                Progress = new(activity, initialStatusDescription, parentId, state, Token, eventFactory);
            }

            internal static BackgroundFunc<TEvent, TResultEvent, TResult, TState> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId, TState state,
                Func<IBackgroundProgress<TState, TEvent>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory, params CancellationToken[] linkedTokens)
            {
                BackgroundFunc<TEvent, TResultEvent, TResult, TState> backgroundOperation = new(activity, initialStatusDescription, parentId, state, eventFactory, linkedTokens);
                OperationHelper.Start<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, BackgroundFunc<TEvent, TResultEvent, TResult, TState>, TResult>(service, backgroundOperation, backgroundOperation.Progress,
                    asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            IDisposable IObservable<IBackgroundProgressEvent<TState>>.Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
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

            protected override void RaiseOperationFaulted(Exception exception)
            {
                throw new NotImplementedException();
            }

            internal class BackgroundProgressImpl : BackgroundProgress, IBackgroundProgress<TState, TEvent>
            {
                public TState AsyncState { get; }

                private readonly IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> _eventFactory;

                internal BackgroundProgressImpl(string activity, string initialStatusDescription, Guid? parentId, TState state, CancellationToken token, IBackgroundEventFactory<TEvent, TResultEvent, IBackgroundProgress<TState, TEvent>, TResult> eventFactory) : base(activity, initialStatusDescription, parentId, token)
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

        class BackgroundFunc<TResult> : BackgroundOperationInfo<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>, BackgroundFunc<TResult>.BackgroundProgressImpl, Task>, IBackgroundFunc<TResult>,
            IBackgroundEventFactory<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>, IBackgroundProgress<IBackgroundProgressEvent>, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource = new();

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<IBackgroundProgressEvent, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private BackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                Progress = new(activity, initialStatusDescription, parentId, Token);
            }

            internal static BackgroundFunc<TResult> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId,
                Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, params CancellationToken[] linkedTokens)
            {
                BackgroundFunc<TResult> backgroundOperation = new(activity, initialStatusDescription, parentId, linkedTokens);
                OperationHelper.Start<IBackgroundProgressEvent, IBackgroundOperationResultEvent<TResult>, IBackgroundProgress<IBackgroundProgressEvent>, BackgroundFunc<TResult>, TResult>(service, backgroundOperation,
                    backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
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

            protected override void RaiseOperationFaulted(Exception exception)
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

        class BackgroundFunc<TState, TResult> : BackgroundOperationInfo<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>, BackgroundFunc<TState, TResult>.BackgroundProgressImpl, Task<TResult>>,
            IBackgroundFunc<TState, TResult>, IBackgroundEventFactory<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, TResult>
        {
            private readonly TaskCompletionSource<TResult> _completionSource;

            public TState AsyncState => Progress.AsyncState;

            public override Task Task => _completionSource.Task;

            Task<TResult> IBackgroundFunc<TResult>.Task => _completionSource.Task;

            protected override BackgroundProgressImpl Progress { get; }

            protected override IBackgroundProgressEventFactory<IBackgroundProgressEvent<TState>, BackgroundProgressImpl> EventFactory => throw new NotImplementedException();

            private BackgroundFunc(string activity, string initialStatusDescription, Guid? parentId, TState state, CancellationToken[] linkedTokens) : base(linkedTokens)
            {
                _completionSource = new(state);
                Progress = new(activity, initialStatusDescription, parentId, state, Token);
            }

            internal static BackgroundFunc<TState, TResult> Start(BackgroundProgressService service, string activity, string initialStatusDescription, Guid? parentId, TState state,
                Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, params CancellationToken[] linkedTokens)
            {
                BackgroundFunc<TState, TResult> backgroundOperation = new(activity, initialStatusDescription, parentId, state, linkedTokens);
                OperationHelper.Start<IBackgroundProgressEvent<TState>, IBackgroundOperationResultEvent<TState, TResult>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, BackgroundFunc<TState, TResult>, TResult>(service,
                    backgroundOperation, backgroundOperation.Progress, asyncMethodDelegate, backgroundOperation._completionSource, backgroundOperation.RaiseRanToCompletion);
                return backgroundOperation;
            }

            IDisposable IObservable<IBackgroundProgressEvent>.Subscribe(IObserver<IBackgroundProgressEvent> observer)
            {
                throw new NotImplementedException();
            }

            protected override IDisposable BaseSubscribe(IObserver<IBackgroundProgressEvent> observer)
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

            protected override void RaiseOperationFaulted(Exception exception)
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
