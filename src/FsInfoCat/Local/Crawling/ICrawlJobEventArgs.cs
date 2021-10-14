namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJobEventArgs : ICrawlActivityEventArgs
    {
        ILocalCrawlConfiguration Configuration { get; }
    }
}
