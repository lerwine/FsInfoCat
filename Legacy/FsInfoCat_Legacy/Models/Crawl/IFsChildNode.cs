using System;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsChildNode : IFsNode, IEquatable<IFsChildNode>, IComparable<IFsChildNode>
    {
        new string Name { get; set; }
        DateTime CreationTime { get; set; }
        DateTime LastWriteTime { get; set; }
        int Attributes { get; set; }
    }
}
