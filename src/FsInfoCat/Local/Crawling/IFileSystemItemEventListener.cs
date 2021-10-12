namespace FsInfoCat.Local.Crawling
{
    public interface IFileSystemItemEventListener
    {
        void OnFileSystemItemEvent(ICrawlManagerFsItemEventArgs args);
    }
}
