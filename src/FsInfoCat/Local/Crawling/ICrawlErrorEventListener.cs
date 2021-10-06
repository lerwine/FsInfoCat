namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlErrorEventListener
    {
        void OnCrawlErrorEvent(ICrawlErrorEventArgs args);
    }
}
