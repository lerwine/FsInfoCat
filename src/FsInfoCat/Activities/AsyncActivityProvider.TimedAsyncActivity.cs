using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        /// <summary>
        /// Base class for timed asynchronous activity objects.
        /// </summary>
        /// <typeparam name="TBaseEvent">The base type for all observed <see cref="ITimedActivityEvent"/> objects.</typeparam>
        /// <typeparam name="TOperationEvent">The type of the <typeparamref name="TBaseEvent"/> operation object which implements <see cref="ITimedOperationEvent"/>.</typeparam>
        /// <typeparam name="TResultEvent">The type of the <typeparamref name="TBaseEvent"/> result object which implements <see cref="ITimedActivityCompletedEvent"/>.</typeparam>
        /// <typeparam name="TTask">The type of the <see cref="Task"/> that implements the activity.</typeparam>
        /// <seealso cref="AsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, TTask}" />
        /// <seealso cref="ITimedAsyncActivity" />
        internal abstract partial class TimedAsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask> : AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask>, ITimedAsyncActivity
            where TTask : Task
            where TBaseEvent : ITimedActivityEvent
            where TOperationEvent : TBaseEvent, ITimedOperationEvent
            where TResultEvent : TBaseEvent, ITimedActivityCompletedEvent
        {
            private readonly Stopwatch _stopwatch = new();

            /// <summary>
            /// Gets the start time.
            /// </summary>
            /// <value>The date and time when the activity was started started.</value>
            /// <remarks>If <see cref="AsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, TTask}.StatusValue"/> is <see cref="ActivityStatus.WaitingToRun"/>, this will be the date and time when this object was instantiated.</remarks>
            public DateTime Started { get; private set; } = DateTime.Now;

            /// <summary>
            /// Gets the duration of the activity.
            /// </summary>
            /// <value>The duration of the activity.</value>
            public TimeSpan Duration => _stopwatch.Elapsed;

            /// <summary>
            /// Initializes a new instance of the <see cref="TimedAsyncActivity{TBaseEvent, TOperationEvent, TResultEvent, TTask}"/> class.
            /// </summary>
            /// <param name="owner">The owner activity provider.</param>
            /// <param name="activityDescription">The description to use for the asynchronous activity.</param>
            /// <param name="initialStatusMessage">The activity status message that indicates the activity is waiting to start.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentException"><paramref name="activityDescription"/> or <paramref name="initialStatusMessage"/> is null, empty or whitespace.</exception>
            protected TimedAsyncActivity([DisallowNull] AsyncActivityProvider owner, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(owner, activityDescription, initialStatusMessage) { }

            /// <summary>
            /// Called when the associated task is about to be run.
            /// </summary>
            protected override void OnBeforeAwaitTask()
            {
                Started = DateTime.Now;
                _stopwatch.Start();
                base.OnBeforeAwaitTask();
            }

            /// <summary>
            /// Called when the associated task has completed.
            /// </summary>
            protected void StopTimer() => _stopwatch.Stop();
        }
    }
}
