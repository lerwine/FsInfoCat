using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class SubdirectoryListItemWithAncestorNames : SubdirectoryListItem, ILocalSubdirectoryListItemWithAncestorNames
    {
        public const string VIEW_NAME_WITH_ANCESTOR_NAMES = "vSubdirectoryListingWithAncestorNames";

        private readonly IPropertyChangeTracker<string> _ancestorNames;
        private readonly IPropertyChangeTracker<Guid> _effectiveVolumeId;
        private readonly IPropertyChangeTracker<string> _volumeDisplayName;
        private readonly IPropertyChangeTracker<string> _volumeName;
        private readonly IPropertyChangeTracker<VolumeIdentifier> _volumeIdentifier;

        public string AncestorNames { get => _ancestorNames.GetValue(); set => _ancestorNames.SetValue(value); }

        public Guid EffectiveVolumeId { get => _effectiveVolumeId.GetValue(); set => _effectiveVolumeId.SetValue(value); }

        public string VolumeDisplayName { get => _volumeDisplayName.GetValue(); set => _volumeDisplayName.SetValue(value); }

        public string VolumeName { get => _volumeName.GetValue(); set => _volumeName.SetValue(value); }

        public VolumeIdentifier VolumeIdentifier { get => _volumeIdentifier.GetValue(); set => _volumeIdentifier.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItemWithAncestorNames> builder)
        {
            builder.ToView(VIEW_NAME_WITH_ANCESTOR_NAMES);
            builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
        }

        public SubdirectoryListItemWithAncestorNames()
        {
            _ancestorNames = AddChangeTracker(nameof(AncestorNames), "", NonNullStringCoersion.Default);
            _effectiveVolumeId = AddChangeTracker(nameof(EffectiveVolumeId), Guid.Empty);
            _volumeDisplayName = AddChangeTracker(nameof(VolumeDisplayName), "", NonNullStringCoersion.Default);
            _volumeName = AddChangeTracker(nameof(VolumeName), "", NonNullStringCoersion.Default);
            _volumeIdentifier = AddChangeTracker(nameof(VolumeIdentifier), VolumeIdentifier.Empty);
        }
    }
}
