using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlErrorEventArgs : FileCrawlEndEventArgs, ICrawlErrorEventArgs
    {
        public Exception Exception { get; }

        public FileCrawlErrorEventArgs([DisallowNull] Exception exception, [DisallowNull] FileCrawlEventArgs args, string message = null, StatusMessageLevel level = StatusMessageLevel.Error, AsyncJobStatus status = AsyncJobStatus.Faulted)
            : base(args, string.IsNullOrWhiteSpace(message) ? (string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message) : message, level, status)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }
    }
}
