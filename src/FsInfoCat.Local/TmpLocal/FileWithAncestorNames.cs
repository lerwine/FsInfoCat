using M = FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    // TODO: Document FileWithAncestorNames class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class FileWithAncestorNames : DbFileRow, ILocalFileListItemWithAncestorNames, IEquatable<FileWithAncestorNames>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vFileListingWithAncestorNames";

        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        [NotNull]
        [BackingField(nameof(_ancestorNames))]
        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value.EmptyIfNullOrWhiteSpace(); }

        public Guid EffectiveVolumeId { get; set; }

        [NotNull]
        [BackingField(nameof(_volumeDisplayName))]
        public string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value.EmptyIfNullOrWhiteSpace(); }

        [NotNull]
        [BackingField(nameof(_volumeName))]
        public string VolumeName { get => _volumeName; set => _volumeName = value.EmptyIfNullOrWhiteSpace(); }

        public M.VolumeIdentifier VolumeIdentifier { get; set; } = M.VolumeIdentifier.Empty;

        [NotNull]
        [BackingField(nameof(_fileSystemDisplayName))]
        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.EmptyIfNullOrWhiteSpace(); }

        [NotNull]
        [BackingField(nameof(_fileSystemSymbolicName))]
        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value.EmptyIfNullOrWhiteSpace(); }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithAncestorNames> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(VolumeIdentifier)).HasConversion(M.VolumeIdentifier.Converter);
        }

        public bool Equals(FileWithAncestorNames other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(M.IFileListItemWithAncestorNames other)
        {
            if (other is null) return false;
            if (other is FileWithAncestorNames listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFileListItemWithAncestorNames local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is FileWithAncestorNames listItem) return Equals(listItem);
            if (obj is M.IFileListItemWithAncestorNames other)
            {
                if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
                return !other.TryGetId(out _) && (other is ILocalFileListItemWithAncestorNames local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
            }
            return false;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
