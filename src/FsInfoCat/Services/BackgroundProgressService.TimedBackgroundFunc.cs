using FsInfoCat.AsyncOps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class TimedBackgroundFunc<TResult> : TimedBackgroundOperation<ITimedBackgroundProgressEvent, ITimedBackgroundProgress<ITimedBackgroundProgressEvent>,
            TimedBackgroundProgress<ITimedBackgroundProgressEvent, ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>>, Task<TResult>, ITimedBackgroundFunc<TResult>,
            ITimedBackgroundOperationResultEvent<TResult>>, ITimedBackgroundFunc<TResult>
        {
            internal TimedBackgroundFunc(TimedBackgroundProgress<ITimedBackgroundProgressEvent, ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> progress,
                Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, async p =>
                {
                    progress.StartTimer();
                    TResult result;
                    try { result = await (asyncMethodDelegate(p) ?? throw new InvalidOperationException()); }
                    finally { progress.StopTimer(); }
                    return result;
                }, tokenSource)
            { }
        }

        class TimedBackgroundFunc<TState, TResult> : TimedBackgroundOperation<ITimedBackgroundProgressEvent<TState>, ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>,
            TimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>, Task<TResult>,
            ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>, ITimedBackgroundFunc<TState, TResult>
        {
            public TState AsyncState { get; }

            internal TimedBackgroundFunc(TimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>, ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> progress,
                Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, async p =>
                {
                    progress.StartTimer();
                    TResult result;
                    try { result = await (asyncMethodDelegate(p) ?? throw new InvalidOperationException()); }
                    finally { progress.StopTimer(); }
                    return result;
                }, tokenSource)
            {
                AsyncState = progress.AsyncState;
            }

            public IDisposable Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer)
                => ObserverSubscriptionRelay<ITimedBackgroundProgressEvent<TState>, IBackgroundProgressEvent<TState>>.Create(this, observer);
        }
    }
}
