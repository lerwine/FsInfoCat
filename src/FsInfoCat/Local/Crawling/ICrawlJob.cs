using FsInfoCat.Services;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJob : IQueuedBgOperation<CrawlTerminationReason>
    {
        ICurrentItem CurrentItem { get; }
    }
}
