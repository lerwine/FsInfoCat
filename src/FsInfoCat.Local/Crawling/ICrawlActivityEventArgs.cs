namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlActivityEventArgs : IAsyncOperationInfo
    {
        new ActivityCode Activity { get; }

        new MessageCode StatusDescription { get; }
    }
}
