using FsInfoCat.AsyncOps;
using System;
using System.Threading;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {

        internal class BackgroundProgress<TEvent, TOperation, TResultEvent> : BackgroundProgressBase<TEvent, TOperation, TResultEvent>, IBackgroundProgress<TEvent>
            where TEvent : IBackgroundProgressEvent
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent
            where TOperation : IBackgroundOperation
        {
            private readonly Func<IBackgroundProgressInfo, Exception, TEvent> _eventFactory;

            internal BackgroundProgress(BackgroundProgressService service, Func<BackgroundProgress<TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TOperation> operationFactory,
                Func<IBackgroundProgressInfo, Exception, TEvent> eventFactory, string activity, string statusDescription, Guid? parentId, params CancellationToken[] tokens)
                : base(service, (p, t1, t2) => operationFactory((BackgroundProgress<TEvent, TOperation, TResultEvent>)p, t1, t2), activity, statusDescription, parentId, tokens)
            {
                _eventFactory = eventFactory;
            }

            protected override TEvent CreateEventObject(Exception exception) => _eventFactory(this, exception);
        }

        internal class BackgroundProgress<TState, TEvent, TOperation, TResultEvent> : BackgroundProgress<TEvent, TOperation, TResultEvent>, IBackgroundProgress<TState, TEvent>, IBackgroundProgressInfo<TState>
            where TEvent : IBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, IBackgroundOperationCompletedEvent<TState>
            where TOperation : IBackgroundOperation<TState>
        {
            public TState AsyncState { get; }

            internal BackgroundProgress(BackgroundProgressService service, Func<BackgroundProgress<TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TOperation> operationFactory,
                Func<IBackgroundProgressInfo<TState>, Exception, TEvent> eventFactory, string activity, string statusDescription, TState state, Guid? parentId, params CancellationToken[] tokens)
                : base(service, (p, t1, t2) => operationFactory((BackgroundProgress<TState, TEvent, TOperation, TResultEvent>)p, t1, t2), (p, e) => eventFactory((IBackgroundProgressInfo<TState>)p, e), activity, statusDescription, parentId, tokens)
            {
                AsyncState = state;
            }
        }
    }
}
