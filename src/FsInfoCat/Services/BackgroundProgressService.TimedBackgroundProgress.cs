using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics;
using System.Threading;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        internal class TimedBackgroundProgress<TEvent, TOperation, TResultEvent> : BackgroundProgressBase<TEvent, TOperation, TResultEvent>, ITimedBackgroundProgress<TEvent>
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent
            where TOperation : ITimedBackgroundOperation
        {
            private readonly Func<ITimedBackgroundProgressInfo, Exception, TEvent> _eventFactory;
            private readonly Stopwatch _stopwatch = new();

            public TimeSpan Duration => _stopwatch.Elapsed;

            internal TimedBackgroundProgress(BackgroundProgressService service,
                Func<TimedBackgroundProgress<TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TOperation> operationFactory,
                Func<ITimedBackgroundProgressInfo, Exception, TEvent> eventFactory, string activity, string statusDescription, Guid? parentId, params CancellationToken[] tokens)
                : base(service, (p, t1, t2) => operationFactory((TimedBackgroundProgress<TEvent, TOperation, TResultEvent>)p, t1, t2), activity, statusDescription, parentId, tokens)
            {
                _eventFactory = eventFactory;
            }

            internal void StopTimer() => _stopwatch.Stop();

            internal void StartTimer() => _stopwatch.Start();

            protected override TEvent CreateEventObject(Exception exception) => _eventFactory(this, exception);
        }

        internal class TimedBackgroundProgress<TState, TEvent, TOperation, TResultEvent> : TimedBackgroundProgress<TEvent, TOperation, TResultEvent>, ITimedBackgroundProgress<TState, TEvent>, ITimedBackgroundProgressInfo<TState>
            where TEvent : ITimedBackgroundProgressEvent<TState>
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent<TState>
            where TOperation : ITimedBackgroundOperation<TState>
        {
            public TState AsyncState { get; }

            internal TimedBackgroundProgress(BackgroundProgressService service, Func<TimedBackgroundProgress<TEvent, TOperation, TResultEvent>, CancellationTokenSource, CancellationTokenSource, TOperation> operationFactory,
                Func<ITimedBackgroundProgressInfo<TState>, Exception, TEvent> eventFactory, string activity, string statusDescription, TState state, Guid? parentId, params CancellationToken[] tokens)
                : base(service, (p, t1, t2) => operationFactory((TimedBackgroundProgress<TState, TEvent, TOperation, TResultEvent>)p, t1, t2), (p, e) => eventFactory((ITimedBackgroundProgressInfo<TState>)p, e), activity, statusDescription, parentId, tokens)
            {
                AsyncState = state;
            }
        }
    }
}
