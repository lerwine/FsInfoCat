using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsDirectory : IFsNode
    {
        Collection<IFsChildNode> ChildNodes { get; set; }
    }
}
