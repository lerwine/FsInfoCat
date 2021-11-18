using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public partial class BgActivityService
    {
        class AsyncAction<TState> : AsyncActionBase<TState, IBgStatusEventArgs<TState>, Task>
        {
            internal AsyncAction(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, asyncMethodDelegate, bgActivityService)
            {
            }

            internal AsyncAction(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<IBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, observer, asyncMethodDelegate, bgActivityService)
            {
            }

            internal AsyncAction(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<IBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, listener, asyncMethodDelegate, bgActivityService)
            {
            }
        }
    }
}
