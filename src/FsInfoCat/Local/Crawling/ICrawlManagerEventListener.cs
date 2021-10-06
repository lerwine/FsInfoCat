namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlManagerEventListener
    {
        void OnCrawlManagerEvent(ICrawlManagerEventArgs args);
    }
}
