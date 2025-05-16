using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity that represents a subdirectory and contains the names of the ancestor subdirectories.
    /// </summary>
    /// <seealso cref="Subdirectory" />
    /// <seealso cref="LocalDbContext.SubdirectoryListingWithAncestorNames" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    public class SubdirectoryListItemWithAncestorNames : SubdirectoryListItem, ILocalSubdirectoryListItemWithAncestorNames, IEquatable<SubdirectoryListItemWithAncestorNames>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME_WITH_ANCESTOR_NAMES = "vSubdirectoryListingWithAncestorNames";

        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

        [NotNull]
        [BackingField(nameof(_ancestorNames))]
        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value.AsNonNullTrimmed(); }

        public Guid EffectiveVolumeId { get; set; }

        [NotNull]
        [BackingField(nameof(_volumeDisplayName))]
        public string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_volumeName))]
        public string VolumeName { get => _volumeName; set => _volumeName = value.AsNonNullTrimmed(); }

        public VolumeIdentifier VolumeIdentifier { get; set; }

        [NotNull]
        [BackingField(nameof(_fileSystemDisplayName))]
        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_fileSystemSymbolicName))]
        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItemWithAncestorNames> builder)
        {
            _ = builder.ToView(VIEW_NAME_WITH_ANCESTOR_NAMES);
            _ = builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(SubdirectoryListItemWithAncestorNames other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public override bool Equals(SubdirectoryListItem other) => other is not null && ((other is SubdirectoryListItemWithAncestorNames item) ? Equals(item) :
            base.Equals(other));

        public bool Equals(ISubdirectoryAncestorName other)
        {
            // TODO: Implement Equals(ISubdirectoryAncestorName)
            throw new NotImplementedException();
        }

        public bool Equals(ISubdirectoryListItemWithAncestorNames other)
        {
            if (other is null) return false;
            if (other is SubdirectoryListItemWithAncestorNames listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSubdirectoryListItemWithAncestorNames local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(ISubdirectoryListItem other)
        {
            // TODO: Implement Equals(ISubdirectoryListItem)
            return base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        protected override string PropertiesToString()
        {
            return $@"AncestorNames={ExtensionMethods.EscapeCsString(_ancestorNames)}, {base.PropertiesToString()},
    VolumeIdentifier={VolumeIdentifier}, EffectiveVolumeId={EffectiveVolumeId},
    VolumeDisplayName={ExtensionMethods.EscapeCsString(_volumeDisplayName)}, VolumeName={ExtensionMethods.EscapeCsString(_volumeName)},
    FileSystemDisplayName={ExtensionMethods.EscapeCsString(_fileSystemDisplayName)}, FileSystemSymbolicName={ExtensionMethods.EscapeCsString(_fileSystemSymbolicName)}";
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
