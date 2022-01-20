using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class BackgroundProgressEventArgs : IBackgroundProgressEvent
    {
        public MessageCode? Code { get; }

        public Guid OperationId { get; }

        public string Activity { get; }

        public string StatusDescription { get; }

        public string CurrentOperation { get; }

        public Guid? ParentId { get; }

        public byte? PercentComplete { get; }

        public BackgroundProgressEventArgs([DisallowNull] IBackgroundProgressInfo progress)
        {
            OperationId = (progress ?? throw new ArgumentNullException(nameof(progress))).OperationId;
            Activity = progress.Activity;
            StatusDescription = progress.StatusDescription;
            CurrentOperation = progress.CurrentOperation;
            ParentId = progress.ParentId;
            PercentComplete = progress.PercentComplete;
        }

        public BackgroundProgressEventArgs([DisallowNull] IBackgroundProgressInfo progress, MessageCode messageCode) : this(progress) { Code = messageCode; }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class BackgroundProgressEventArgs<TState> : BackgroundProgressEventArgs, IBackgroundProgressEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProgressEventArgs([DisallowNull] IBackgroundProgressInfo<TState> progress) : base(progress)
        {
            AsyncState = progress.AsyncState;
        }
    }
}
