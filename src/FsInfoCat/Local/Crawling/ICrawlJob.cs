using System;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJob : IAsyncJob
    {
        DateTime Started { get; }

        ICurrentItem CurrentItem { get; }
    }
}
