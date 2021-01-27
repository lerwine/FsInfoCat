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

}
