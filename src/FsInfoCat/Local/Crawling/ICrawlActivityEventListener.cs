namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlActivityEventListener
    {
        void OnCrawlActivity(ICrawlActivityEventArgs args);
    }
}
