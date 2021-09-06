using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class VolumeListItemWithFileSystem : VolumeListItem, ILocalVolumeListItemWithFileSystem
    {
        public const string VIEW_NAME_WITH_FILESYSTEM = "vVolumeListingWithFileSystem";

        private readonly IPropertyChangeTracker<string> _fileSystemDisplayName;
        private readonly IPropertyChangeTracker<bool> _effectiveReadOnly;
        private readonly IPropertyChangeTracker<uint> _effectiveMaxNameLength;

        public string FileSystemDisplayName { get => _fileSystemDisplayName.GetValue(); set => _fileSystemDisplayName.SetValue(value); }

        public bool EffectiveReadOnly { get => _effectiveReadOnly.GetValue(); set => _effectiveReadOnly.SetValue(value); }

        public uint EffectiveMaxNameLength { get => _effectiveMaxNameLength.GetValue(); set => _effectiveMaxNameLength.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<VolumeListItemWithFileSystem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME_WITH_FILESYSTEM).Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);

        public VolumeListItemWithFileSystem()
        {
            _fileSystemDisplayName = AddChangeTracker(nameof(FileSystemDisplayName), "", TrimmedNonNullStringCoersion.Default);
            _effectiveReadOnly = AddChangeTracker(nameof(EffectiveReadOnly), false);
            _effectiveMaxNameLength = AddChangeTracker(nameof(EffectiveMaxNameLength), 0u);
        }
    }
}
