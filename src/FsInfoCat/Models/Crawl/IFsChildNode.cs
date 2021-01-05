using System;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsChildNode : IFsNode
    {
        string Name { get; set; }
        DateTime CreationTime { get; set; }
        DateTime LastWriteTime { get; set; }
        int Attributes { get; set; }
    }
}
