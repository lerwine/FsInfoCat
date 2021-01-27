namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public abstract class NestedCrawlComponent : CrawlComponent, INestedCrawlComponent
    {
        protected new NestedCrawlSite Site => throw new System.NotImplementedException();

        INestedCrawlSite INestedCrawlComponent.Site => throw new System.NotImplementedException();
    }

    public abstract class NestedCrawlComponent<TOwner> : NestedCrawlComponent, INestedCrawlComponent<TOwner>
        where TOwner : ICrawlComponent
    {
        protected new NestedCrawlSite<TOwner> Site => throw new System.NotImplementedException();

        INestedCrawlSite<TOwner> INestedCrawlComponent<TOwner>.Site => throw new System.NotImplementedException();
    }
}
