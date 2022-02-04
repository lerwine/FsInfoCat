using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract partial class TimedAsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask> : AsyncActivity<TBaseEvent, TOperationEvent, TResultEvent, TTask>, ITimedAsyncActivity
            where TTask : Task
            where TBaseEvent : ITimedActivityEvent
            where TOperationEvent : TBaseEvent, ITimedOperationEvent
            where TResultEvent : TBaseEvent, ITimedActivityCompletedEvent
        {
            private readonly Stopwatch _stopwatch = new();

            public DateTime Started { get; private set; } = DateTime.Now;

            public TimeSpan Duration => _stopwatch.Elapsed;

            protected TimedAsyncActivity([DisallowNull] AsyncActivityProvider provider, [DisallowNull] string activityDescription, [DisallowNull] string initialStatusMessage)
                : base(provider, activityDescription, initialStatusMessage)
            {
            }

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
