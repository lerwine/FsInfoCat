using FsInfoCat.AsyncOps;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public partial class BgActivityService
    {
        [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
        class TimedAsyncAction<TState> : TimedAsyncActionBase<TState, Task>
        {
            internal TimedAsyncAction(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, asyncMethodDelegate, bgActivityService)
            {
            }

            internal TimedAsyncAction(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<ITimedBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, observer, asyncMethodDelegate, bgActivityService)
            {
            }

            internal TimedAsyncAction(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<ITimedBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, listener, asyncMethodDelegate, bgActivityService)
            {
            }
        }
    }
}
