using FsInfoCat.AsyncOps;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public partial class BgActivityService
    {
        class TimedAsyncFunc<TState, TResult> : TimedAsyncActionBase<TState, Task<TResult>>, ITimedAsyncFunc<TState, TResult>
        {
            internal TimedAsyncFunc(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, asyncMethodDelegate, bgActivityService)
            {
            }

            internal TimedAsyncFunc(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<ITimedBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, observer, asyncMethodDelegate, bgActivityService)
            {
            }

            internal TimedAsyncFunc(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<ITimedBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, listener, asyncMethodDelegate, bgActivityService)
            {
            }
        }
    }
}
