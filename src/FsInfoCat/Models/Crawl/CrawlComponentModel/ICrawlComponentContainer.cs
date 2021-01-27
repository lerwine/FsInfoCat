using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface ICrawlComponentContainer : IContainer, IServiceProvider
    {
        void Add(ICrawlComponent component);
        void Add(ICrawlComponent component, string name);
        void Remove(ICrawlComponent component);
    }

}
