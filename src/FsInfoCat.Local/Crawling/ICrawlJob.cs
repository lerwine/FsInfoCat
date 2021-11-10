using FsInfoCat.Services;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJob : IQueuedBgProducer<CrawlTerminationReason>
    {
        ICurrentItem CurrentItem { get; }
    }
}
