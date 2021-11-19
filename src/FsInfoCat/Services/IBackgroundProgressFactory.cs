using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    public interface IBackgroundProgressFactory
    {
        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, Func<IBackgroundOperation<TState>, IBackgroundOperationCompleteEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state);

        IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, Func<IBackgroundOperation<TState>, IBackgroundOperationCompleteEvent<TState>> onCompleted, string activity, string statusDescription, TState state);

        IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription);

        IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription);

        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, TState state);

        IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity, string statusDescription, TState state);

        IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens);

        IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription, params CancellationToken[] tokens);

        IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription);

        IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription);
    }
}
