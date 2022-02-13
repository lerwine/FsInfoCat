using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class VolumeListItemWithFileSystem : VolumeListItem, ILocalVolumeListItemWithFileSystem
    {
        public const string VIEW_NAME_WITH_FILESYSTEM = "vVolumeListingWithFileSystem";

        private string _fileSystemDisplayName = string.Empty;

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        public bool EffectiveReadOnly { get; set; }

        public uint EffectiveMaxNameLength { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<VolumeListItemWithFileSystem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME_WITH_FILESYSTEM).Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);
    }
}
