using System;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlProgress : IProgress<CrawlJobStartEventArgs>, IProgress<CrawlJobEndEventArgs>, IProgress<DirectoryCrawlStartEventArgs>, IProgress<DirectoryCrawlEndEventArgs>, IProgress<DirectoryCrawlErrorEventArgs>, IProgress<FileCrawlStartEventArgs>,
        IProgress<FileCrawlEndEventArgs>, IProgress<FileCrawlErrorEventArgs> { }
}
