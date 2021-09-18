using System;

namespace FsInfoCat
{
    public interface IDbFsItemListItemWithAncestorNames : IDbFsItemListItem, IDbFsItemAncestorName
    {
        Guid EffectiveVolumeId { get; }

        string VolumeDisplayName { get; }

        string VolumeName { get; }

        VolumeIdentifier VolumeIdentifier { get; }

        string FileSystemDisplayName { get; }

        string FileSystemSymbolicName { get; }
    }
}

