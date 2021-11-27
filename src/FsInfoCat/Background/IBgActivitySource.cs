using FsInfoCat.AsyncOps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public interface IBgActivitySource : IReadOnlyCollection<IAsyncAction>
    {
        IAsyncAction<TState> InvokeAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<IBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate);

        IAsyncAction<TState> InvokeAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<IBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate);

        IAsyncAction<TState> InvokeAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate);

        ITimedAsyncAction<TState> InvokeTimedAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<ITimedBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate);

        ITimedAsyncAction<TState> InvokeTimedAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<ITimedBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate);

        ITimedAsyncAction<TState> InvokeTimedAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate);

        IAsyncAction InvokeAsync(ActivityCode activityCode, string initialStatusMessage, IObserver<IBgStatusEventArgs> observer, Func<IBgActivityProgress, Task> asyncMethodDelegate);

        IAsyncAction InvokeAsync(ActivityCode activityCode, string initialStatusMessage, IProgress<IBgStatusEventArgs> listener, Func<IBgActivityProgress, Task> asyncMethodDelegate);

        IAsyncAction InvokeAsync(ActivityCode activityCode, string initialStatusMessage, Func<IBgActivityProgress, Task> asyncMethodDelegate);

        ITimedAsyncAction InvokeTimedAsync(ActivityCode activityCode, string initialStatusMessage, IObserver<ITimedBgStatusEventArgs> observer, Func<IBgActivityProgress, Task> asyncMethodDelegate);

        ITimedAsyncAction InvokeTimedAsync(ActivityCode activityCode, string initialStatusMessage, IProgress<ITimedBgStatusEventArgs> listener, Func<IBgActivityProgress, Task> asyncMethodDelegate);

        ITimedAsyncAction InvokeTimedAsync(ActivityCode activityCode, string initialStatusMessage, Func<IBgActivityProgress, Task> asyncMethodDelegate);

        IAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<IBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);

        IAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<IBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);

        IAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);

        ITimedAsyncFunc<TState, TResult> InvokeTimedAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<ITimedBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);

        ITimedAsyncFunc<TState, TResult> InvokeTimedAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<ITimedBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);

        ITimedAsyncFunc<TState, TResult> InvokeTimedAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);

        IAsyncFunc<TResult> InvokeAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, IObserver<IBgStatusEventArgs> observer, Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate);

        IAsyncFunc<TResult> InvokeAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, IProgress<IBgStatusEventArgs> listener, Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate);

        IAsyncFunc<TResult> InvokeAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate);

        ITimedAsyncFunc<TResult> InvokeTimedAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, IObserver<ITimedBgStatusEventArgs> observer, Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate);

        ITimedAsyncFunc<TResult> InvokeTimedAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, IProgress<ITimedBgStatusEventArgs> listener, Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate);

        ITimedAsyncFunc<TResult> InvokeTimedAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate);
    }
}
