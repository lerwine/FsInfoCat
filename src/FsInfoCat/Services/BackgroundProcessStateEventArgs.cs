using System;

namespace FsInfoCat.Services
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

        protected BackgroundProcessStateEventArgs(IBackgroundOperation operation, MessageCode? messageCode)
        {
            // TODO: Populate properties
        }
    }
}
