using System;

namespace FsInfoCat.Local
{
    public class DirectoryCrawlErrorEventArgs : DirectoryCrawlEventArgs
    {
        public AccessErrorCode ErrorCode { get; }

        public Exception Exception { get; }

        public DirectoryCrawlErrorEventArgs(CrawlWorker.CrawlContext crawlContext, AccessErrorCode errorCode, Exception exception) : base(crawlContext)
        {
            ErrorCode = errorCode;
            Exception = exception;
        }
    }
}
