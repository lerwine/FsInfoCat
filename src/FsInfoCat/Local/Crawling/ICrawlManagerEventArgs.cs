namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlManagerEventArgs : ICrawlActivityEventArgs
    {
        ILocalCrawlConfiguration Configuration { get; }
    }
}
