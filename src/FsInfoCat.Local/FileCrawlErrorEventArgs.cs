using System;
using System.IO;

namespace FsInfoCat.Local
{
    public class FileCrawlErrorEventArgs : FileCrawlEventArgs
    {
        public AccessErrorCode ErrorCode { get; }

        public Exception Exception { get; }

        public FileCrawlErrorEventArgs(CrawlWorker.CrawlContext crawlContext, AccessErrorCode errorCode, FileInfo fs, DbFile db, Exception exception)
            : base(crawlContext, fs, db)
        {
            ErrorCode = errorCode;
            Exception = exception;
        }
    }
}
