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

        public byte? PercentComplete { get; }

        protected BackgroundProcessStateEventArgs([DisallowNull] IBackgroundOperation operation, MessageCode? messageCode, string statusDescription)
        {
            Code = messageCode;
            OperationId = (operation ?? throw new ArgumentNullException(nameof(operation))).OperationId;
            Activity = operation.Activity ?? "";
            if (string.IsNullOrWhiteSpace(statusDescription))
            {
                StatusDescription = operation.StatusDescription ?? "";
                CurrentOperation = operation.CurrentOperation ?? "";
            }
            else
            {
                StatusDescription = statusDescription;
                CurrentOperation = "";
            }
            ParentId = operation.ParentId;
            PercentComplete = operation.PercentComplete;
        }
    }
}
