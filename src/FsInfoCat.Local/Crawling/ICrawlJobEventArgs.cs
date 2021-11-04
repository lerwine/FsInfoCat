namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJobEventArgs : ICrawlActivityEventArgs
    {
        ICrawlJob CrawlJob { get; }
    }
}
