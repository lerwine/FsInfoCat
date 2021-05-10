using System.Collections.Generic;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsDirectory : IFsNode
    {
        IList<IFsChildNode> ChildNodes { get; set; }
        bool TryFindPartialCrawl(out PartialCrawlWarning message, out IEnumerable<IFsDirectory> segments);
    }
}
