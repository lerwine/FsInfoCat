using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public abstract class BackgroundProcessStateEventArgs : EventArgs, IBackgroundProgressEvent
    {
        public MessageCode? Code { get; }

        public Guid OperationId { get; }

        public string Activity { get; }

        public string StatusDescription { get; }

        public string CurrentOperation { get; }

        public Guid? ParentId { get; }

        public IBackgroundProgressService Source { get; }

        protected BackgroundProcessStateEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation operation, MessageCode? messageCode)
        {
            Source=source??throw new ArgumentNullException(nameof(source));
            Code=messageCode;
            OperationId=(operation??throw new ArgumentNullException(nameof(operation))).OperationId;
            Activity=operation.Activity??"";
            StatusDescription=operation.StatusDescription??"";
            CurrentOperation=operation.CurrentOperation??"";
            ParentId=operation.ParentId;
        }
    }
}
