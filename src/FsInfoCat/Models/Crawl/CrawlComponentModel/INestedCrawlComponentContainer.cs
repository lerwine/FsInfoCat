using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface INestedCrawlComponentContainer : ICrawlComponentContainer, INestedContainer
    {
        new ICrawlComponent Owner { get; }
        void Add(INestedCrawlComponent component);
        void Add(INestedCrawlComponent component, string name);
        void Remove(INestedCrawlComponent component);
    }

    public interface INestedCrawlComponentContainer<TOwner> : INestedCrawlComponentContainer
        where TOwner : ICrawlComponent
    {
        new TOwner Owner { get; }
        void Add(INestedCrawlComponent<TOwner> component);
        void Add(INestedCrawlComponent<TOwner> component, string name);
        void Remove(INestedCrawlComponent<TOwner> component);
    }

    public abstract class NestedCrawlComponentContainer : CrawlComponent.CrawlSite.CrawlComponentContainer, INestedCrawlComponentContainer
    {
        public ICrawlComponent Owner { get; }

        // ICrawlComponent INestedCrawlComponentContainer.Owner => Owner;

        IComponent INestedContainer.Owner => Owner;

        protected NestedCrawlComponentContainer(ICrawlComponent owner)
        {
            if (null == owner)
                throw new ArgumentNullException(nameof(owner));
            Owner = owner;
        }

        protected void Add(NestedCrawlComponent component) => base.Add(component);

        void INestedCrawlComponentContainer.Add(INestedCrawlComponent component) => base.Add((NestedCrawlComponent)component);

        void INestedCrawlComponentContainer.Add(INestedCrawlComponent component, string name) => Add((NestedCrawlComponent)component, name);

        protected abstract NestedCrawlSite CreateCrawlSite(NestedCrawlComponent component);

        protected override CrawlComponent.CrawlSite CreateCrawlSite(CrawlComponent component) => CreateCrawlSite((NestedCrawlComponent)component);

        protected bool Remove(NestedCrawlComponent component) => base.Remove(component as NestedCrawlComponent);

        void INestedCrawlComponentContainer.Remove(INestedCrawlComponent component) => base.Remove(component as NestedCrawlComponent);
    }

    public abstract class NestedCrawlComponentContainer<TOwner> : NestedCrawlComponentContainer, INestedCrawlComponentContainer<TOwner>
        where TOwner : ICrawlComponent
    {
        public new TOwner Owner => (TOwner)base.Owner;

        protected NestedCrawlComponentContainer(ICrawlComponent owner) : base(owner) { }

        protected void Add(NestedCrawlComponent<TOwner> component) => base.Add(component);

        void INestedCrawlComponentContainer<TOwner>.Add(INestedCrawlComponent<TOwner> component) => base.Add((NestedCrawlComponent<TOwner>)component);

        void INestedCrawlComponentContainer<TOwner>.Add(INestedCrawlComponent<TOwner> component, string name) => base.Add((NestedCrawlComponent<TOwner>)component, name);

        protected abstract NestedCrawlSite<TOwner> CreateCrawlSite(NestedCrawlComponent<TOwner> component);

        protected override NestedCrawlSite CreateCrawlSite(NestedCrawlComponent component) => CreateCrawlSite((NestedCrawlComponent<TOwner>)component);

        protected bool Remove(NestedCrawlComponent<TOwner> component) => base.Remove(component);

        void INestedCrawlComponentContainer<TOwner>.Remove(INestedCrawlComponent<TOwner> component) => base.Remove(component as NestedCrawlComponent<TOwner>);
    }
}
