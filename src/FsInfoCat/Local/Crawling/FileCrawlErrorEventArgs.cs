using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlErrorEventArgs : FileCrawlEventArgs, ICrawlErrorEventArgs
    {
        public Exception Exception { get; }

        public FileCrawlErrorEventArgs([DisallowNull] Exception exception, [DisallowNull] FileCrawlErrorEventArgs args, string message,
            StatusMessageLevel level = StatusMessageLevel.Error) : base(args, message, level)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }
    }
}
