namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface INestedCrawlComponent : ICrawlComponent
    {
         new INestedCrawlSite Site { get; }
    }

    public interface INestedCrawlComponent<TOwner> : INestedCrawlComponent
        where TOwner : ICrawlComponent
    {
         new INestedCrawlSite<TOwner> Site { get; }
    }

}
