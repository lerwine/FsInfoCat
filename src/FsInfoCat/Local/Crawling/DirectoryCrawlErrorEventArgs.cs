using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlErrorEventArgs : DirectoryCrawlEndEventArgs, ICrawlErrorEventArgs
    {
        public Exception Exception { get; }

        public DirectoryCrawlErrorEventArgs([DisallowNull] Exception exception, [DisallowNull] DirectoryCrawlEventArgs args, string message,
            StatusMessageLevel level = StatusMessageLevel.Error) : base(args, message, level)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }
    }
}
