using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProgressEventArgs : IBackgroundProgressEvent
    {
        public MessageCode? Code { get; }

        public Guid OperationId { get; }

        public string Activity { get; }

        public string StatusDescription { get; }

        public string CurrentOperation { get; }

        public Guid? ParentId { get; }

        public BackgroundProgressEventArgs([DisallowNull] IBackgroundProgressInfo progress)
        {
            OperationId=(progress??throw new ArgumentNullException(nameof(progress))).OperationId;
            Activity=progress.Activity;
            StatusDescription=progress.StatusDescription;
            CurrentOperation=progress.CurrentOperation;
            ParentId=progress.ParentId;
        }

        public BackgroundProgressEventArgs([DisallowNull] IBackgroundProgressInfo progress, MessageCode messageCode) : this(progress) { Code=messageCode; }
    }

    public class BackgroundProgressEventArgs<T> : BackgroundProgressEventArgs, IBackgroundProgressEvent<T>
    {
        public T AsyncState { get; }

        public BackgroundProgressEventArgs([DisallowNull] IBackgroundProgressInfo<T> progress) : base(progress)
        {
            AsyncState=progress.AsyncState;
        }
    }
}
