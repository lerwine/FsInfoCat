using System;

namespace FsInfoCat
{
    public interface IFileListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, IFileRow, IFileAncestorName, IEquatable<IFileListItemWithAncestorNames> { }
}
