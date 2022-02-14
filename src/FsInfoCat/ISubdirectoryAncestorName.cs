using System;

namespace FsInfoCat
{
    public interface ISubdirectoryAncestorName : IDbFsItemAncestorName, IEquatable<ISubdirectoryAncestorName>
    {
        Guid? ParentId { get; }
    }
}
