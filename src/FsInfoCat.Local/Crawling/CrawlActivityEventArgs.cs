using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class CrawlActivityEventArgs : EventArgs, ICrawlActivityEventArgs
    {
        public CrawlActivityEventArgs([DisallowNull] IBgOperationEventArgs bgOperation, MessageCode statusDescription, string currentOperation)
        {
            ConcurrencyId = bgOperation.ConcurrencyId;
            Activity = bgOperation.Activity;
            StatusDescription = statusDescription;
            CurrentOperation = currentOperation;
            AsyncState = bgOperation.AsyncState;
            ParentOperation = bgOperation.ParentOperation;
        }

        public Guid ConcurrencyId { get; }

        public ActivityCode Activity { get; }

        public MessageCode StatusDescription { get; }

        public AsyncJobStatus Status { get; }

        ActivityCode? IAsyncOperationInfo.Activity => Activity;

        MessageCode? IAsyncOperationInfo.StatusDescription => StatusDescription;

        public string CurrentOperation { get; }

        public object AsyncState { get; }

        public IAsyncOperationInfo ParentOperation { get; }

        public override string ToString() => $@"ConcurrencyId = {ExtensionMethods.ToPseudoCsText(ConcurrencyId)},
  Status = {ExtensionMethods.ToPseudoCsText(Status)},
  Activity = {ExtensionMethods.ToPseudoCsText(Activity)},
  StatusDescription = {ExtensionMethods.ToPseudoCsText(StatusDescription)},
  CurrentOperation = {ExtensionMethods.ToPseudoCsText(CurrentOperation)},
  AsyncState = {ExtensionMethods.ToPseudoCsText(AsyncState)}";
    }
}
