using FsInfoCat.AsyncOps;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public partial class BgActivityService
    {
        [Obsolete("Use FsInfoCat.Activities.*, instead.")]
        class AsyncFunc<TState, TResult> : AsyncActionBase<TState, IBgStatusEventArgs<TState>, Task<TResult>>, IAsyncFunc<TState, TResult>
        {
            internal AsyncFunc(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, asyncMethodDelegate, bgActivityService)
            {
            }

            internal AsyncFunc(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<IBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, observer, asyncMethodDelegate, bgActivityService)
            {
            }

            internal AsyncFunc(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<IBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate, BgActivityService bgActivityService)
                : base(activityCode, initialStatusMessage, state, listener, asyncMethodDelegate, bgActivityService)
            {
            }
        }
    }
}
