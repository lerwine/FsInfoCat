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

    public abstract class NestedCrawlComponent : CrawlComponent, INestedCrawlComponent
    {
        protected internal new NestedCrawlSite Site
        {
            get => (NestedCrawlSite)base.Site;
            protected set => base.Site = value;
        }

        INestedCrawlSite INestedCrawlComponent.Site => Site;
    }

    public abstract class NestedCrawlComponent<TOwner> : NestedCrawlComponent, INestedCrawlComponent<TOwner>
        where TOwner : ICrawlComponent
    {
        protected internal new NestedCrawlSite<TOwner> Site
        {
            get => (NestedCrawlSite<TOwner>)base.Site;
            protected set => base.Site = value;
        }

        INestedCrawlSite<TOwner> INestedCrawlComponent<TOwner>.Site => Site;
    }
}
