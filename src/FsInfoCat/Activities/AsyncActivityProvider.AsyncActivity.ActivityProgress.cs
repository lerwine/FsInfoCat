using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal partial class AsyncActivity<TEvent, TTask>
            where TTask : Task
            where TEvent : IOperationEvent
        {
            internal abstract class ActivityProgress : IActivityProgress
            {
                private readonly AsyncActivity<TEvent, TTask> _activity;

                public ActivityStatus StatusValue => _activity.StatusValue;

                public string CurrentOperation => _activity.CurrentOperation;

                public int PercentComplete => _activity.PercentComplete;

                public Guid ActivityId => _activity.ActivityId;

                public Guid? ParentActivityId => _activity.ParentActivityId;

                public string ShortDescription => _activity.ShortDescription;

                public string StatusMessage => _activity.StatusMessage;

                public IObservable<IAsyncActivity> StateChangeObservable => _activity.StateChangeObservable;

                public int Count => _activity.Count;

                public CancellationToken Token { get; }

                internal ActivityProgress(AsyncActivity<TEvent, TTask> activity) => (_activity, Token) = (activity, _activity.TokenSource.Token);

                public IEnumerator<IAsyncActivity> GetEnumerator() => _activity.GetEnumerator();

                public IAsyncAction<IOperationEvent> InvokeAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
                {
                    throw new NotImplementedException();
                }

                public IAsyncFunc<IOperationEvent, TResult> InvokeAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
                {
                    throw new NotImplementedException();
                }

                public IAsyncAction<IOperationEvent<TState>, TState> InvokeAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
                {
                    throw new NotImplementedException();
                }

                public IAsyncFunc<IOperationEvent<TState>, TState, TResult> InvokeAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
                {
                    throw new NotImplementedException();
                }

                public ITimedAsyncAction<ITimedOperationEvent> InvokeTimedAsync([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task> asyncMethodDelegate)
                {
                    throw new NotImplementedException();
                }

                public ITimedAsyncFunc<ITimedOperationEvent, TResult> InvokeTimedAsync<TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, [DisallowNull] Func<IActivityProgress, Task<TResult>> asyncMethodDelegate)
                {
                    throw new NotImplementedException();
                }

                public ITimedAsyncAction<ITimedOperationEvent<TState>, TState> InvokeTimedAsync<TState>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state, [DisallowNull] Func<IActivityProgress<TState>, Task> asyncMethodDelegate)
                {
                    throw new NotImplementedException();
                }

                public ITimedAsyncFunc<ITimedOperationEvent<TState>, TState, TResult> InvokeTimedAsync<TState, TResult>([DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage, TState state, [DisallowNull] Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
                {
                    throw new NotImplementedException();
                }

                public void Report([DisallowNull] Exception error, string statusDescription, string currentOperation, int percentComplete)
                {
                    throw new NotImplementedException();
                }

                public void Report([DisallowNull] string statusDescription, string currentOperation, int percentComplete)
                {
                    throw new NotImplementedException();
                }

                public void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, string currentOperation)
                {
                    throw new NotImplementedException();
                }

                public void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, int percentComplete)
                {
                    throw new NotImplementedException();
                }

                public void Report([DisallowNull] string statusDescription, string currentOperation)
                {
                    throw new NotImplementedException();
                }

                public void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription)
                {
                    throw new NotImplementedException();
                }

                public void Report(string value)
                {
                    throw new NotImplementedException();
                }

                public void Report(Exception value)
                {
                    throw new NotImplementedException();
                }

                public void Report(int value)
                {
                    throw new NotImplementedException();
                }

                public void ReportCurrentOperation([DisallowNull] Exception error, string currentOperation, int percentComplete)
                {
                    throw new NotImplementedException();
                }

                public void ReportCurrentOperation(string currentOperation, int percentComplete)
                {
                    throw new NotImplementedException();
                }

                public void ReportCurrentOperation([DisallowNull] Exception error, string currentOperation)
                {
                    throw new NotImplementedException();
                }

                public void ReportCurrentOperation(string currentOperation)
                {
                    throw new NotImplementedException();
                }

                public IDisposable SubscribeStateChange([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving)
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
