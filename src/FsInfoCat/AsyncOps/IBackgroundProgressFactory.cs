using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundProgressFactory
    {
        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state,
            params CancellationToken[] tokens);

        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state,
            params CancellationToken[] tokens);

        ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state,
            params CancellationToken[] tokens);

        IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state);

        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>> onCompleted, string activity, string statusDescription, TState state);

        ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state);

        IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate,
            Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>> onCompleted, string activity, string statusDescription, TState state);

        ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
            Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription, params CancellationToken[] tokens);

        ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription);

        IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate,
            Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>> onCompleted, string activity, string statusDescription);

        ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate,
            Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription);

        IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate,
            Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted, string activity, string statusDescription);

        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            string activity, string statusDescription, TState state, params CancellationToken[] tokens);

        ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
            string statusDescription, TState state, params CancellationToken[] tokens);

        IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
            string statusDescription, TState state, params CancellationToken[] tokens);

        ITimedBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            string activity, string statusDescription, TState state);

        IBackgroundFunc<TState, TResult> InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>> asyncMethodDelegate,
            string activity, string statusDescription, TState state);

        ITimedBackgroundOperation<TState> InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
            string statusDescription, TState state);

        IBackgroundOperation<TState> InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task> asyncMethodDelegate, string activity,
            string statusDescription, TState state);

        ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity,
            string statusDescription, params CancellationToken[] tokens);

        IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription,
            params CancellationToken[] tokens);

        ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription,
            params CancellationToken[] tokens);

        IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription,
            params CancellationToken[] tokens);

        ITimedBackgroundFunc<TResult> InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity,
            string statusDescription);

        IBackgroundFunc<TResult> InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>> asyncMethodDelegate, string activity, string statusDescription);

        ITimedBackgroundOperation InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription);

        IBackgroundOperation InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate, string activity, string statusDescription);
    }
}
