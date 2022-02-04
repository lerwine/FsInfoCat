using FsInfoCat.Activities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class BackgroundJobVM
    {
        /// <summary>
        /// Timed operation event observer for <see cref="BackgroundJobVM"/> items associated with an <see cref="ITimedAsyncAction{ITimedActivityEvent}" /> activity.
        /// </summary>
        /// <seealso cref="ItemEventObserver{ITimedActivityEvent}" />
        internal class TimedItemEventObserver : ItemEventObserver<ITimedActivityEvent>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TimedItemEventObserver"/> class.
            /// </summary>
            /// <param name="target">The target view model.</param>
            /// <param name="onCompleted">The action to be invoked when <see cref="IObserver{ITimedActivityEvent}.OnCompleted"/> is called.</param>
            internal TimedItemEventObserver([DisallowNull] BackgroundJobVM target, [DisallowNull] Action onCompleted) : base(target, onCompleted) { }

            /// <summary>
            /// Called from the <see cref="Dispatcher"/> thread to update the <see cref="Target"/> view model.
            /// </summary>
            /// <param name="activityEvent">The timed activity event.</param>
            protected override void OnNextEvent([DisallowNull] ITimedActivityEvent activityEvent)
            {
                base.OnNextEvent(activityEvent);
                Target.Started = activityEvent.Started;
                Target.Duration = activityEvent.Duration;
            }
        }
    }
}
