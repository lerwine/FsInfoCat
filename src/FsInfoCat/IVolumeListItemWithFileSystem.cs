using System;

namespace FsInfoCat
{
    public interface IVolumeListItemWithFileSystem : IVolumeListItem, IEquatable<IVolumeListItemWithFileSystem>
    {
        string FileSystemDisplayName { get; }

        bool EffectiveReadOnly { get; }

        uint EffectiveMaxNameLength { get; }
    }
}
