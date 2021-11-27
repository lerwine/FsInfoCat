using FsInfoCat.AsyncOps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        class BackgroundFunc<TResult> : BackgroundOperation<IBackgroundProgressEvent, IBackgroundProgress<IBackgroundProgressEvent>, BackgroundProgress<IBackgroundProgressEvent, IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>, Task<TResult>, IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>, IBackgroundFunc<TResult>
        {
            internal BackgroundFunc(BackgroundProgress<IBackgroundProgressEvent, IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> progress,
                Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, asyncMethodDelegate, tokenSource) { }
        }

        class BackgroundFunc<TState, TResult> : BackgroundOperation<IBackgroundProgressEvent<TState>, IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, BackgroundProgress<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>, Task<TResult>, IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>, IBackgroundFunc<TState, TResult>
        {
            public TState AsyncState { get; }

            internal BackgroundFunc(BackgroundProgress<TState, IBackgroundProgressEvent<TState>, IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> progress,
                Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, CancellationTokenSource tokenSource)
                : base(progress, asyncMethodDelegate, tokenSource)
            {
                AsyncState = progress.AsyncState;
            }
        }
    }
}
