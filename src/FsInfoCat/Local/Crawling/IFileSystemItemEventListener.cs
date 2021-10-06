namespace FsInfoCat.Local.Crawling
{
    public interface IFileSystemItemEventListener
    {
        void OnFileSystemItemEvent(ICrawlManagerFsItemEventArgs args);
    }
    public interface ISubdirectoryCrawlEventListener
    {
        void OnFileCrawlEvent(DirectoryCrawlEventArgs args);
    }
    public interface IFileCrawlEventListener
    {
        void OnFileCrawlEvent(FileCrawlEventArgs args);
    }
}
