using System;

namespace FsInfoCat
{
    public interface IFileAncestorName : IDbFsItemAncestorName
    {
        Guid ParentId { get; }
    }
}
