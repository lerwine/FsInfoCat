using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public abstract class TimedBackgroundProcessStateEventArgs : BackgroundProcessStateEventArgs, ITimedBackgroundProgressEvent
    {
        public TimeSpan Duration { get; }

        protected TimedBackgroundProcessStateEventArgs([DisallowNull] ITimedBackgroundOperation operation, MessageCode? messageCode, string statusDescription = null)
            : base(operation, messageCode, statusDescription)
        {
            Duration = operation.Duration;
        }
    }
}
