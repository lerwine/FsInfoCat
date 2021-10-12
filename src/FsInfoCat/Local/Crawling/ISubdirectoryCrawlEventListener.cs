namespace FsInfoCat.Local.Crawling
{
    public interface ISubdirectoryCrawlEventListener
    {
        void OnSubdirectoryCrawlEvent(DirectoryCrawlEventArgs args);
    }
}
