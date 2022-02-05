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

            public DateTime Started { get; private set; } = DateTime.Now;

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

            protected override void OnStarted()
            {
                Started = DateTime.Now;
                _stopwatch.Start();
                base.OnStarted();
            }

            protected void StopTimer() => _stopwatch.Stop();
        }
    }
}
