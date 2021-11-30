using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public abstract class TimedBackgroundProcessStateEventArgs : BackgroundProcessStateEventArgs, ITimedBackgroundProgressEvent
    {
        public TimeSpan Duration { get; }

        protected TimedBackgroundProcessStateEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation operation, MessageCode? messageCode)
            : base(source, operation, messageCode)
        {
            Duration = operation.Duration;
        }
    }
}
