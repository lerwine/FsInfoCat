namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface ICrawlComponentCollectionContainer : INestedCrawlComponentContainer
    {
        ICrawlComponentCollection Items { get; set; }
    }

    public interface ICrawlComponentCollectionContainer<TOwner, TComponent> : ICrawlComponentCollectionContainer, INestedCrawlComponentContainer<TOwner>
        where TOwner : ICrawlComponent
        where TComponent : INestedCrawlComponent<TOwner>
    {
        new ICrawlComponentCollection<TComponent> Items { get; set; }
    }

}
