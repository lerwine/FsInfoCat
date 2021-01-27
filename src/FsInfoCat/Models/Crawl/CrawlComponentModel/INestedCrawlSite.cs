using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface INestedCrawlSite : ICrawlSite, INestedSite
    {
        new INestedCrawlComponent Component { get; }
        new INestedCrawlComponentContainer Container { get; }
    }

    public interface INestedCrawlSite<TOwner> : INestedCrawlSite
        where TOwner : ICrawlComponent
    {
        new INestedCrawlComponent<TOwner> Component { get; }
        new INestedCrawlComponentContainer<TOwner> Container { get; }
    }

}
