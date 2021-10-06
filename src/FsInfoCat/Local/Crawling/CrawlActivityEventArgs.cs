using System;

namespace FsInfoCat.Local.Crawling
{
    public class CrawlActivityEventArgs : EventArgs, ICrawlActivityEventArgs
    {
        public string Message { get; }

        public StatusMessageLevel MessageLevel { get; }

        public Guid ConcurrencyId { get; }

        public AsyncJobStatus JobStatus { get; }

        public CrawlActivityEventArgs(string message, StatusMessageLevel level, AsyncJobStatus status, Guid concurrencyId)
        {
            Message = message ?? "";
            MessageLevel = level;
            JobStatus = status;
            ConcurrencyId = concurrencyId;
        }
    }
}
