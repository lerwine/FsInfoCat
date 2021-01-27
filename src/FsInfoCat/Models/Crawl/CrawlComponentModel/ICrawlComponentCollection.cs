using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface ICrawlComponentCollection : ICollection, IList
    {
        new INestedCrawlComponent this[int index] { get; set; }
        void Add(INestedCrawlComponent value);
        bool Contains(INestedCrawlComponent value);
        int IndexOf(INestedCrawlComponent value);
        void Insert(int index, INestedCrawlComponent value);
        void Remove(INestedCrawlComponent value);
    }

    public interface ICrawlComponentCollection<TComponent> : ICollection<TComponent>, IEnumerable<TComponent>, IEnumerable, IList<TComponent>, IReadOnlyCollection<TComponent>, IReadOnlyList<TComponent>, ICrawlComponentCollection
        where TComponent : INestedCrawlComponent
    {
    }

}
