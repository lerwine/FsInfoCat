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

    public abstract class NestedCrawlSite : CrawlComponent.CrawlSite, INestedCrawlSite
    {
        public new NestedCrawlComponent Component => (NestedCrawlComponent)base.Component;

        INestedCrawlComponent INestedCrawlSite.Component => Component;

        public new INestedCrawlComponentContainer Container => (NestedCrawlComponentContainer)base.Container;

        // INestedCrawlComponentContainer INestedCrawlSite.Container => Container;

        string INestedSite.FullName => throw new System.NotImplementedException();

        protected NestedCrawlSite(NestedCrawlComponentContainer container, NestedCrawlComponent component) : base(container, component) { }
    }

    public abstract class NestedCrawlSite<TOwner> : NestedCrawlSite, INestedCrawlSite<TOwner>
        where TOwner : ICrawlComponent
    {
        public new NestedCrawlComponent<TOwner> Component => (NestedCrawlComponent<TOwner>)base.Component;
        INestedCrawlComponent<TOwner> INestedCrawlSite<TOwner>.Component => Component;

        public new INestedCrawlComponentContainer<TOwner> Container => (NestedCrawlComponentContainer<TOwner>)base.Container;

        // INestedCrawlComponentContainer<TOwner> INestedCrawlSite<TOwner>.Container => Container;

        protected NestedCrawlSite(NestedCrawlComponentContainer<TOwner> container, NestedCrawlComponent<TOwner> component) : base(container, component) { }
    }

    public class NestedCrawlSite<TOwner, TComponent> : NestedCrawlSite<TOwner>
        where TOwner : ICrawlComponent
        where TComponent : NestedCrawlComponent<TOwner>
    {
        public new TComponent Component => (TComponent)base.Component;
        public NestedCrawlSite(NestedCrawlComponentContainer<TOwner> container, TComponent component) : base(container, component) { }
    }
}
