using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    partial class AsyncActivityProvider
    {
        internal abstract partial class TimedAsyncActivity<TEvent, TTask> : AsyncActivity<TEvent, TTask>, ITimedAsyncActivity
            where TTask : Task
            where TEvent : ITimedOperationEvent
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

            protected override void OnCanceled(LinkedListNode<IAsyncActivity> node)
            {
                _stopwatch.Stop();
                base.OnCanceled(node);
            }

            protected override void OnFaulted(LinkedListNode<IAsyncActivity> node, Exception exception)
            {
                _stopwatch.Stop();
                base.OnFaulted(node, exception);
            }

            protected override void OnRanToCompletion(LinkedListNode<IAsyncActivity> node)
            {
                _stopwatch.Stop();
                base.OnRanToCompletion(node);
            }
        }
    }
}
