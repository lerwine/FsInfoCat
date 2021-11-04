namespace FsInfoCat.Local.Crawling
{
    public interface IFsItemCrawlEventArgs : ICurrentItem, ICrawlActivityEventArgs
    {
        DirectoryCrawlEventArgs Parent { get; }
    }
}
