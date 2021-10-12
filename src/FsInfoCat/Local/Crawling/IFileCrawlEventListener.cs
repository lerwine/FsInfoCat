namespace FsInfoCat.Local.Crawling
{
    public interface IFileCrawlEventListener
    {
        void OnFileCrawlEvent(FileCrawlEventArgs args);
    }
}
