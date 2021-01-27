using System;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public partial class CrawlComponentCollectionContainer<TOwner> : NestedCrawlComponentContainer<TOwner>, ICrawlComponentCollectionContainer
        where TOwner : ICrawlComponent
    {
        private CrawlComponentCollection _collection;
        public CrawlComponentCollection Items
        {
            get => _collection;
            set => _collection = value;
        }
        ICrawlComponentCollection ICrawlComponentCollectionContainer.Items { get => _collection; set => Items = (CrawlComponentCollection)value; }
    }

    public class CrawlComponentCollectionContainer<TOwner, TComponent> : CrawlComponentCollectionContainer<TOwner>, ICrawlComponentCollectionContainer<TOwner, TComponent>
        where TOwner : ICrawlComponent
        where TComponent : INestedCrawlComponent<TOwner>
    {
        public new CrawlComponentCollection<TComponent> Items
        {
            get => (CrawlComponentCollection<TComponent>)base.Items;
            set => base.Items = value;
        }
        ICrawlComponentCollection<TComponent> ICrawlComponentCollectionContainer<TOwner, TComponent>.Items { get => Items; set => Items = (CrawlComponentCollection<TComponent>)value; }
    }
}
