using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        [Obsolete("Use nested operation progress classes")]
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

            ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TState>(Func<ITimedBackgroundProgress<TState, TEvent1>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, ITimedBackgroundProgress<TState, TEvent1>> eventFactory, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
            {
                throw new NotImplementedException();
            }

            ITimedBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TState>(Func<ITimedBackgroundProgress<TState, TEvent1>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, ITimedBackgroundProgress<TState, TEvent1>> eventFactory, string activity, string statusDescription, TState state)
            {
                throw new NotImplementedException();
            }

            ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1>(Func<ITimedBackgroundProgress<TEvent1>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, ITimedBackgroundProgress<TEvent1>> eventFactory, string activity, string statusDescription, params CancellationToken[] tokens)
            {
                throw new NotImplementedException();
            }

            ITimedBackgroundOperation IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1>(Func<ITimedBackgroundProgress<TEvent1>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, ITimedBackgroundProgress<TEvent1>> eventFactory, string activity, string statusDescription)
            {
                throw new NotImplementedException();
            }

            IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TState>(Func<IBackgroundProgress<TState, TEvent1>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, IBackgroundProgress<TState, TEvent1>> eventFactory, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
            {
                throw new NotImplementedException();
            }

            IBackgroundOperation<TState> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TState>(Func<IBackgroundProgress<TState, TEvent1>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, IBackgroundProgress<TState, TEvent1>> eventFactory, string activity, string statusDescription, TState state)
            {
                throw new NotImplementedException();
            }

            IBackgroundOperation IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1>(Func<IBackgroundProgress<TEvent1>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, IBackgroundProgress<TEvent1>> eventFactory, string activity, string statusDescription, params CancellationToken[] tokens)
            {
                throw new NotImplementedException();
            }

            IBackgroundOperation IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1>(Func<IBackgroundProgress<TEvent1>, Task> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, IBackgroundProgress<TEvent1>> eventFactory, string activity, string statusDescription)
            {
                throw new NotImplementedException();
            }

            ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TState, TResult>(Func<ITimedBackgroundProgress<TState, TEvent1>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, ITimedBackgroundProgress<TState, TEvent1>, TResult> eventFactory, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
            {
                throw new NotImplementedException();
            }

            ITimedBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TState, TResult>(Func<ITimedBackgroundProgress<TState, TEvent1>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, ITimedBackgroundProgress<TState, TEvent1>, TResult> eventFactory, string activity, string statusDescription, TState state)
            {
                throw new NotImplementedException();
            }

            ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TResult>(Func<ITimedBackgroundProgress<TEvent1>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, ITimedBackgroundProgress<TEvent1>, TResult> eventFactory, string activity, string statusDescription, params CancellationToken[] tokens)
            {
                throw new NotImplementedException();
            }

            ITimedBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TResult>(Func<ITimedBackgroundProgress<TEvent1>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, ITimedBackgroundProgress<TEvent1>, TResult> eventFactory, string activity, string statusDescription)
            {
                throw new NotImplementedException();
            }

            IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TState, TResult>(Func<IBackgroundProgress<TState, TEvent1>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, IBackgroundProgress<TState, TEvent1>, TResult> eventFactory, string activity, string statusDescription, TState state, params CancellationToken[] tokens)
            {
                throw new NotImplementedException();
            }

            IBackgroundFunc<TState, TResult> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TState, TResult>(Func<IBackgroundProgress<TState, TEvent1>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, IBackgroundProgress<TState, TEvent1>, TResult> eventFactory, string activity, string statusDescription, TState state)
            {
                throw new NotImplementedException();
            }

            IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TResult>(Func<IBackgroundProgress<TEvent1>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, IBackgroundProgress<TEvent1>, TResult> eventFactory, string activity, string statusDescription, params CancellationToken[] tokens)
            {
                throw new NotImplementedException();
            }

            IBackgroundFunc<TResult> IBackgroundProgressFactory.InvokeAsync<TEvent1, TResultEvent1, TResult>(Func<IBackgroundProgress<TEvent1>, Task<TResult>> asyncMethodDelegate, IBackgroundEventFactory<TEvent1, TResultEvent1, IBackgroundProgress<TEvent1>, TResult> eventFactory, string activity, string statusDescription)
            {
                throw new NotImplementedException();
            }
        }

        [Obsolete("Use nested operation progress classes")]
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
