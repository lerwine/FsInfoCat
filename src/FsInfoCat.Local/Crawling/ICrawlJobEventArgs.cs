namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJobEventArgs : ICrawlActivityEventArgs
    {
        [System.Obsolete("Use CrawlJob")]
        ILocalCrawlConfiguration Configuration { get; }

        ICrawlJob CrawlJob { get; }
    }
}
