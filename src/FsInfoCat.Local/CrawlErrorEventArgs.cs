using System;

namespace FsInfoCat.Local
{
    public class CrawlErrorEventArgs : EventArgs
    {
        public AggregateException Exception { get; }
        [Obsolete]
        public CrawlErrorEventArgs(CrawlWorker crawlWorker, AggregateException exception)
        {
        }
        public CrawlErrorEventArgs(AggregateException exception)
        {
            Exception = exception;
        }
    }
}
