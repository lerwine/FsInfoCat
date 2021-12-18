using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        abstract class TimedBackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask> : BackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask>,
            ITimedBackgroundProgressInfo
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent
            where TProgress : ITimedBackgroundProgress<TEvent>
            where TTask : Task
        {
            public TimeSpan Duration => Progress.Duration;

            protected Stopwatch Stopwatch { get; } = new();

            protected TimedBackgroundOperationInfo(BackgroundProgressService service, CancellationToken[] linkedTokens) : base(service, linkedTokens) { }

            internal abstract class TimedBackgroundProgress : BackgroundProgress, ITimedBackgroundProgress<TEvent>
            {
                private readonly Stopwatch _stopwatch;

                public TimeSpan Duration => _stopwatch.Elapsed;

                protected TimedBackgroundProgress([DisallowNull] BackgroundOperationInfo operation, [DisallowNull] string activity, [DisallowNull] string initialStatusDescription, Guid? parentId, [DisallowNull] Stopwatch stopwatch,
                    CancellationToken token)
                    : base(operation, activity, initialStatusDescription, parentId, token) => _stopwatch = stopwatch;
            }
        }
    }
}
