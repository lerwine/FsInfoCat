using System;
using System.IO;

namespace FsInfoCat.Local
{
    public class CrawlEventArgs : EventArgs
    {
        public TimeSpan Time { get; }

        public string WorkerName { get; }

        public ulong ItemNumber { get; }

        public int Depth { get; }

        public string ParentPath { get; }

        public string ItemName { get; }

        public string CrawlRelativeParentPath { get; }

        public string CrawlRelativeName => string.IsNullOrEmpty(CrawlRelativeParentPath) ? (ItemName ?? "") : string.IsNullOrEmpty(ItemName) ? CrawlRelativeParentPath : Path.Combine(CrawlRelativeParentPath, ItemName);

        public string FullName => string.IsNullOrEmpty(ParentPath) ? (ItemName ?? "") : string.IsNullOrEmpty(ItemName) ? ParentPath : Path.Combine(ParentPath, ItemName);

        public AccessErrorCode? ErrorCode { get; }

        public string ErrorMessage { get; }

        public string ErrorDetails { get; }

        internal CrawlEventArgs(CrawlManagerService.CrawlWorker worker, CrawlManagerService.Context context, IAccessError accessError = null)
        {
            CrawlRelativeParentPath = context.GetCrawlRelativeParentPath();
            ParentPath = context.GetParentPath();
            ItemName = context.GetParentPath();
            Time = worker.Duration;
            WorkerName = worker.DisplayName;
            ItemNumber = worker.ItemsProcessed;
            Depth = context.Depth;
            if (accessError != null)
            {
                ErrorCode = accessError.ErrorCode;
                ErrorMessage = accessError.Message ?? "";
                ErrorDetails = accessError.Details ?? "";
            }
        }
    }
}
