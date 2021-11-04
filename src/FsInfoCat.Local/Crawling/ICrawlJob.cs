using FsInfoCat.Services;
using System;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJob : IQueuedBgOperation<CrawlTerminationReason>
    {
        Guid ConcurrencyId { get; }

        ICurrentItem CurrentItem { get; }
    }
}
