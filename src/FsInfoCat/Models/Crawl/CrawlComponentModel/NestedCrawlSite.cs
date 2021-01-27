using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public class NestedCrawlSite : CrawlComponent.CrawlSite, INestedCrawlSite
    {
        public new NestedCrawlComponent Component { get; }
        INestedCrawlComponent INestedCrawlSite.Component => throw new System.NotImplementedException();

        public new NestedCrawlComponentContainer Container { get; }
        INestedCrawlComponentContainer INestedCrawlSite.Container => throw new System.NotImplementedException();

        string INestedSite.FullName => throw new System.NotImplementedException();
    }

    public class NestedCrawlSite<TOwner> : NestedCrawlSite, INestedCrawlSite<TOwner>
        where TOwner : ICrawlComponent
    {
        public new NestedCrawlComponent<TOwner> Component => throw new System.NotImplementedException();

        public new NestedCrawlComponentContainer<TOwner> Container => throw new System.NotImplementedException();

        INestedCrawlComponent<TOwner> INestedCrawlSite<TOwner>.Component => throw new System.NotImplementedException();

        INestedCrawlComponentContainer<TOwner> INestedCrawlSite<TOwner>.Container => throw new System.NotImplementedException();
    }
}
