using System;

namespace FsInfoCat
{
    public interface IFileSystemListItem : IFileSystemRow, IEquatable<IFileSystemListItem>
    {
        Guid? PrimarySymbolicNameId { get; }

        string PrimarySymbolicName { get; }

        long SymbolicNameCount { get; }

        long VolumeCount { get; }
    }
}
