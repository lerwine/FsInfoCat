namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJob : IAsyncJob
    {
        ICurrentItem CurrentItem { get; }
    }
}
