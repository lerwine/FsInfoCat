namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlManagerFsItemEventArgs : ICurrentItem, ICrawlActivityEventArgs
    {
        DirectoryCrawlEventArgs Parent { get; }
    }
}
