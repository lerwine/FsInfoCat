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
        void Add(TComponent component);
        void Add(TComponent component, string name);
        void Remove(TComponent component);
    }

    // public abstract class CrawlComponentCollectionContainer : NestedCrawlComponentContainer, ICrawlComponentCollectionContainer
    // {
    //     public CrawlComponentCollection Items { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    //     ICrawlComponentCollection ICrawlComponentCollectionContainer.Items { get => Items; set => Items = (CrawlComponentCollection)value; }
    //     protected CrawlComponentCollectionContainer(ICrawlComponent owner) : base(owner) { }
    // }

    public abstract class CrawlComponentCollectionContainer<TOwner, TComponent> : NestedCrawlComponentContainer<TOwner>, ICrawlComponentCollectionContainer<TOwner, TComponent>
        where TOwner : ICrawlComponent
        where TComponent : NestedCrawlComponent<TOwner>
    {
        public CrawlComponentCollection<TComponent> Items { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ICrawlComponentCollection<TComponent> ICrawlComponentCollectionContainer<TOwner, TComponent>.Items { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        ICrawlComponentCollection ICrawlComponentCollectionContainer.Items { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        // public new TOwner Owner => (TOwner)base.Owner;

        protected CrawlComponentCollectionContainer(TOwner owner) : base(owner) { }

        public void Add(TComponent component) => base.Add(component);

        void INestedCrawlComponentContainer<TOwner>.Add(INestedCrawlComponent<TOwner> component) => base.Add((TComponent)component);

        void ICrawlComponentCollectionContainer<TOwner, TComponent>.Add(TComponent component, string name) => base.Add(component, name);

        void INestedCrawlComponentContainer<TOwner>.Add(INestedCrawlComponent<TOwner> component, string name) => base.Add((TComponent)component, name);

        protected abstract NestedCrawlSite<TOwner, TComponent> CreateCrawlSite(TComponent component);

        protected override NestedCrawlSite<TOwner> CreateCrawlSite(NestedCrawlComponent<TOwner> component) => CreateCrawlSite((TComponent)component);

        public bool Remove(TComponent component) => base.Remove(component);

        void ICrawlComponentCollectionContainer<TOwner, TComponent>.Remove(TComponent component) => base.Remove(component);

        void INestedCrawlComponentContainer<TOwner>.Remove(INestedCrawlComponent<TOwner> component) => base.Remove((TComponent)component);
    }

    public abstract class CrawlComponentCollectionContainer<TOwner, TComponent, TCollection> : CrawlComponentCollectionContainer<TOwner, TComponent>
        where TOwner : ICrawlComponent
        where TComponent : NestedCrawlComponent<TOwner>
        where TCollection : CrawlComponentCollection<TComponent>, new()
    {
        public new TCollection Items { get => (TCollection)base.Items; set => base.Items = value; }

        protected CrawlComponentCollectionContainer(TOwner owner) : base(owner) { }
    }
}
