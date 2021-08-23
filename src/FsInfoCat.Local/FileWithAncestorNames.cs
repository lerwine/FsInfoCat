using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class FileWithAncestorNames : DbFileRow, ILocalFileListItemWithAncestorNames
    {
        private const string VIEW_NAME = "vFileListingWithAncestorNames";

        private readonly IPropertyChangeTracker<long> _accessErrorCount;
        private readonly IPropertyChangeTracker<long> _personalTagCount;
        private readonly IPropertyChangeTracker<long> _sharedTagCount;
        private readonly IPropertyChangeTracker<string> _ancestorNames;
        private readonly IPropertyChangeTracker<Guid> _effectiveVolumeId;
        private readonly IPropertyChangeTracker<string> _volumeDisplayName;
        private readonly IPropertyChangeTracker<string> _volumeName;
        private readonly IPropertyChangeTracker<VolumeIdentifier> _volumeIdentifier;

        public long AccessErrorCount { get => _accessErrorCount.GetValue(); set => _accessErrorCount.SetValue(value); }

        public long PersonalTagCount { get => _personalTagCount.GetValue(); set => _personalTagCount.SetValue(value); }

        public long SharedTagCount { get => _sharedTagCount.GetValue(); set => _sharedTagCount.SetValue(value); }

        public string AncestorNames { get => _ancestorNames.GetValue(); set => _ancestorNames.SetValue(value); }

        public Guid EffectiveVolumeId { get => _effectiveVolumeId.GetValue(); set => _effectiveVolumeId.SetValue(value); }

        public string VolumeDisplayName { get => _volumeDisplayName.GetValue(); set => _volumeDisplayName.SetValue(value); }

        public string VolumeName { get => _volumeName.GetValue(); set => _volumeName.SetValue(value); }

        public VolumeIdentifier VolumeIdentifier { get => _volumeIdentifier.GetValue(); set => _volumeIdentifier.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithAncestorNames> builder)
        {
            builder.ToView(VIEW_NAME);
            builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
        }

        public FileWithAncestorNames()
        {
            _accessErrorCount = AddChangeTracker(nameof(AccessErrorCount), 0L);
            _personalTagCount = AddChangeTracker(nameof(PersonalTagCount), 0L);
            _sharedTagCount = AddChangeTracker(nameof(SharedTagCount), 0L);
            _ancestorNames = AddChangeTracker(nameof(AncestorNames), "", NonNullStringCoersion.Default);
            _effectiveVolumeId = AddChangeTracker(nameof(EffectiveVolumeId), Guid.Empty);
            _volumeDisplayName = AddChangeTracker(nameof(VolumeDisplayName), "", NonNullStringCoersion.Default);
            _volumeName = AddChangeTracker(nameof(VolumeName), "", NonNullStringCoersion.Default);
            _volumeIdentifier = AddChangeTracker(nameof(VolumeIdentifier), VolumeIdentifier.Empty);
        }
    }
}
