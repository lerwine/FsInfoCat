using System;

namespace FsInfoCat.Local.Crawling
{
    public abstract class CrawlActivityEventArgs : EventArgs, ICrawlActivityEventArgs
    {
        public Guid ConcurrencyId { get; }

        public ActivityCode Activity { get; }

        public MessageCode StatusDescription { get; }

        public AsyncJobStatus Status { get; }

        ActivityCode? IAsyncOperationInfo.Activity => Activity;

        MessageCode? IAsyncOperationInfo.StatusDescription => StatusDescription;

        public string CurrentOperation { get; }

        public object AsyncState { get; }

        protected CrawlActivityEventArgs(ICrawlActivityEventArgs source)
        {
            ConcurrencyId = source.ConcurrencyId;
            Status = source.Status;
            Activity = source.Activity;
            StatusDescription = source.StatusDescription;
            CurrentOperation = source.CurrentOperation;
            AsyncState = source.AsyncState;
        }

        protected CrawlActivityEventArgs(ICrawlJob source, AsyncJobStatus status, MessageCode? statusDescription = null, string currentOperation = null)
        {
            ConcurrencyId = source.ConcurrencyId;
            Status = status;
            Activity = source.Activity;
            StatusDescription = statusDescription ?? source.StatusDescription;
            CurrentOperation = currentOperation ?? source.CurrentOperation;
            AsyncState = ((IAsyncOperationInfo)source).AsyncState;
        }

        protected CrawlActivityEventArgs(Guid concurrencyId, AsyncJobStatus status, ActivityCode activity, MessageCode statusDescription, string currentOperation = null,
            object asyncState = null)
        {
            ConcurrencyId = concurrencyId;
            Status = status;
            Activity = activity;
            StatusDescription = statusDescription;
            CurrentOperation = currentOperation ?? "";
            AsyncState = asyncState;
        }

        protected CrawlActivityEventArgs(Guid concurrencyId, AsyncJobStatus status, ActivityCode activity, OperationStatus operationStatus,
            object asyncState = null)
        {
            ConcurrencyId = concurrencyId;
            Status = status;
            Activity = activity;
            StatusDescription = operationStatus.StatusDescription;
            CurrentOperation = operationStatus.CurrentOperation ?? "";
            AsyncState = asyncState;
        }

        public override string ToString() => $@"ConcurrencyId = {ExtensionMethods.ToPseudoCsText(ConcurrencyId)},
  Status = {ExtensionMethods.ToPseudoCsText(Status)},
  Activity = {ExtensionMethods.ToPseudoCsText(Activity)},
  StatusDescription = {ExtensionMethods.ToPseudoCsText(StatusDescription)},
  CurrentOperation = {ExtensionMethods.ToPseudoCsText(CurrentOperation)},
  AsyncState = {ExtensionMethods.ToPseudoCsText(AsyncState)}";
    }
}
