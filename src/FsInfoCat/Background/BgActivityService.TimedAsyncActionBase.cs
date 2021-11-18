using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public partial class BgActivityService
    {
        class TimedAsyncActionBase<TState, TTask> : AsyncActionBase<TState, ITimedBgStatusEventArgs<TState>, TTask>, ITimedAsyncAction<TState>
            where TTask : Task
        {
            public DateTime Started => throw new NotImplementedException();

            public TimeSpan Duration => throw new NotImplementedException();

            internal TimedAsyncActionBase(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, TTask> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, asyncMethodDelegate, bgActivityService)
            {
            }

            internal TimedAsyncActionBase(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<ITimedBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, TTask> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, observer, asyncMethodDelegate, bgActivityService)
            {
            }

            internal TimedAsyncActionBase(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<ITimedBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, TTask> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, listener, asyncMethodDelegate, bgActivityService)
            {
            }

            public IDisposable Subscribe(IObserver<ITimedBgStatusEventArgs> observer)
            {
                throw new NotImplementedException();
            }

            public IDisposable Subscribe(IObserver<ITimedBgStatusEventArgs<TState>> observer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
