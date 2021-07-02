using System;

namespace FsInfoCat.Local
{
    public class CrawlEventArgs : EventArgs
    {
        public TimeSpan Time { get; }

        public string WorkerName { get; }

        public ulong ItemCount { get; }

        public CrawlEventArgs(CrawlTaskManager crawlWorker)
        {
            Time = (crawlWorker ?? throw new ArgumentNullException(nameof(crawlWorker))).Elapsed;
            WorkerName = crawlWorker.DisplayName;
            ItemCount = crawlWorker.TotalItems;
        }
    }
}
