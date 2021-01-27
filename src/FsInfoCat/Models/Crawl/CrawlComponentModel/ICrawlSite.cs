using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface ICrawlSite : ISite, IEquatable<ICrawlSite>
    {
        new string Name { get; }
        new ICrawlComponent Component { get; }
        new ICrawlComponentContainer Container { get; }
    }

}
