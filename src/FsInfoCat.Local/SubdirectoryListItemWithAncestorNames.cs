using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class SubdirectoryListItemWithAncestorNames : SubdirectoryListItem, ILocalSubdirectoryListItemWithAncestorNames
    {
        public const string VIEW_NAME_WITH_ANCESTOR_NAMES = "vSubdirectoryListingWithAncestorNames";

        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value.AsNonNullTrimmed(); }

        public Guid EffectiveVolumeId { get; set; }

        public string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value.AsWsNormalizedOrEmpty(); }

        public string VolumeName { get => _volumeName; set => _volumeName = value.AsNonNullTrimmed(); }

        public VolumeIdentifier VolumeIdentifier { get; set; }

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItemWithAncestorNames> builder)
        {
            _ = builder.ToView(VIEW_NAME_WITH_ANCESTOR_NAMES);
            _ = builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
        }
    }
}
