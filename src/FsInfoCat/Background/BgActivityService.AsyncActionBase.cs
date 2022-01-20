using FsInfoCat.AsyncOps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public partial class BgActivityService
    {
        [Obsolete("Use FsInfoCat.Activities.*, instead.")]
        class AsyncActionBase<TState, TEvent, TTask> : IAsyncAction<TState>
            where TTask : Task
            where TEvent : IBgStatusEventArgs<TState>
        {
            public TTask Task => throw new NotImplementedException();

            public ActivityCode Activity => throw new NotImplementedException();

            public TState AsyncState => throw new NotImplementedException();

            Task IAsyncAction.Task => Task;

            object IAsyncResult.AsyncState => throw new NotImplementedException();

            object IBgActivityObject.AsyncState => throw new NotImplementedException();

            WaitHandle IAsyncResult.AsyncWaitHandle => throw new NotImplementedException();

            bool IAsyncResult.CompletedSynchronously => throw new NotImplementedException();

            bool IAsyncResult.IsCompleted => throw new NotImplementedException();

            public void Cancel()
            {
                throw new NotImplementedException();
            }

            public void CancelAfter(long millisecondsDelay)
            {
                throw new NotImplementedException();
            }

            public void CancelAfter(TimeSpan delay)
            {
                throw new NotImplementedException();
            }

            internal AsyncActionBase(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, TTask> asyncMethodDelegate, BgActivityService bgActivityService)
            {
            }

            internal AsyncActionBase(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<TEvent> observer, Func<IBgActivityProgress<TState>, TTask> asyncMethodDelegate, BgActivityService bgActivityService)
            {
            }

            internal AsyncActionBase(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<TEvent> listener, Func<IBgActivityProgress<TState>, TTask> asyncMethodDelegate, BgActivityService bgActivityService)
            {
            }

            public IDisposable Subscribe(IObserver<IBgStatusEventArgs> observer)
            {
                throw new NotImplementedException();
            }

            public IDisposable Subscribe(IObserver<IBgStatusEventArgs<TState>> observer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
