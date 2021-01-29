using System.Collections.ObjectModel;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsNode : INamedComponent
    {
         Collection<CrawlMessage> Messages { get; set; }
    }
}
