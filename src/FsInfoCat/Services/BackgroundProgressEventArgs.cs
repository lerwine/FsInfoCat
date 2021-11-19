using System;

namespace FsInfoCat.Services
{
    public class BackgroundProgressEventArgs : IBackgroundProgressEvent
    {
        public BackgroundProgressEventArgs(IBackgroundProgressInfo progress)
        {
            OperationId = progress.OperationId;
            Activity = progress.Activity;
            StatusDescription = progress.StatusDescription;
            CurrentOperation = progress.CurrentOperation;
            ParentId = progress.ParentId;
        }
        public BackgroundProgressEventArgs(IBackgroundProgressInfo progress, MessageCode messageCode) : this(progress) { Code = messageCode; }

        public MessageCode? Code { get; }

        public Guid OperationId { get; }

        public string Activity { get; }

        public string StatusDescription { get; }

        public string CurrentOperation { get; }

        public Guid? ParentId { get; }
    }

    public class BackgroundProgressEventArgs<T> : BackgroundProgressEventArgs, IBackgroundProgressEvent<T>
    {
        public BackgroundProgressEventArgs(IBackgroundProgressInfo<T> progress) : base(progress)
        {
            AsyncState = progress.AsyncState;
        }

        public T AsyncState { get; }
    }

    public class BackgroundProgressErrorEventArgs : BackgroundProgressEventArgs, IBackgroundOperationErrorEvent
    {
        public BackgroundProgressErrorEventArgs(IBackgroundProgressInfo progress, Exception exception, ErrorCode errorCode)
            : base(progress, errorCode.ToMessageCode())
        {
            Error = exception;
        }

        public BackgroundProgressErrorEventArgs(IBackgroundProgressInfo progress, Exception exception)
            : base(progress)
        {
            // TODO: Attempt to get error code from exception
            Error = exception;
        }

        public Exception Error { get; }

        MessageCode IBackgroundOperationErrorEvent.Code => Code ?? MessageCode.UnexpectedError;
    }
}
