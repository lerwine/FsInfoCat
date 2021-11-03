using System;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlActivityEventArgs
    {
        string Message { get; }

        StatusMessageLevel MessageLevel { get; }

        Guid ConcurrencyId { get; }

        AsyncJobStatus JobStatus { get; }
    }
}
