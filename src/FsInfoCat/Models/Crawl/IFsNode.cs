using System.Collections.Generic;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsNode : INamedComponent
    {
         IList<CrawlMessage> Messages { get; set; }
    }
}
