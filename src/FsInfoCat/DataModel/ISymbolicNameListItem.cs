using System;

namespace FsInfoCat
{
    public interface ISymbolicNameListItem : ISymbolicNameRow, IEquatable<ISymbolicNameListItem>
    {
        string FileSystemDisplayName { get; }
    }
}
