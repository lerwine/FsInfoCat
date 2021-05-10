using FsInfoCat.Util;
using System.Collections.Generic;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsNode : INamedComponent
    {
        IList<CrawlMessage> Messages { get; set; }
    }
}
