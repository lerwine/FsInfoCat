using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsDirectory : IFsNode
    {
        Collection<IFsChildNode> ChildNodes { get; set; }
        bool TryFindPartialCrawl(out PartialCrawlWarning message, out IEnumerable<IFsDirectory> segments);
    }
}
