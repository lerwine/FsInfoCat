using System;

namespace FsInfoCat.Local
{
    public class CrawlErrorEventArgs : EventArgs
    {
        public AggregateException Exception { get; }

        public CrawlErrorEventArgs(CrawlTaskManager crawlWorker, AggregateException exception)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public CrawlErrorEventArgs(AggregateException exception)
        {
            Exception = exception;
        }
    }
}
