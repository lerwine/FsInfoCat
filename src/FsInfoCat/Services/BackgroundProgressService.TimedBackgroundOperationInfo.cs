using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    partial class BackgroundProgressService
    {
        abstract class TimedBackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask> : BackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask>, ITimedBackgroundProgressInfo
            where TEvent : ITimedBackgroundProgressEvent
            where TResultEvent : TEvent, ITimedBackgroundOperationCompletedEvent
            where TProgress : TimedBackgroundOperationInfo<TEvent, TResultEvent, TProgress, TTask>.TimedBackgroundProgress
            where TTask : Task
        {
            public TimeSpan Duration => Progress.Duration;

            protected Stopwatch Stopwatch { get; } = new();

            internal class TimedBackgroundProgress : BackgroundProgress, ITimedBackgroundProgress<TEvent>
            {
                private readonly Stopwatch _stopwatch;

                public TimeSpan Duration => _stopwatch.Elapsed;

                protected TimedBackgroundProgress(string activity, string initialStatusDescription, Guid? parentId, Stopwatch stopwatch, CancellationToken token)
                    : base(activity, initialStatusDescription, parentId, token) => _stopwatch = stopwatch;

                protected override TEvent CreateEvent(string statusDescription, string currentOperation, MessageCode? code, byte? percentComplete, Exception error)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
