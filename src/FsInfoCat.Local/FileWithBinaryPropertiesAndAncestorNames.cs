using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class FileWithBinaryPropertiesAndAncestorNames : FileWithBinaryProperties, ILocalFileListItemWithBinaryPropertiesAndAncestorNames, IEquatable<FileWithBinaryPropertiesAndAncestorNames>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vFileListingWithBinaryPropertiesAndAncestorNames";

        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

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

        public VolumeIdentifier VolumeIdentifier { get; set; } = VolumeIdentifier.Empty;

        [NotNull]
        [BackingField(nameof(_fileSystemDisplayName))]
        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.EmptyIfNullOrWhiteSpace(); }

        [NotNull]
        [BackingField(nameof(_fileSystemSymbolicName))]
        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value.EmptyIfNullOrWhiteSpace(); }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithBinaryPropertiesAndAncestorNames> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
            _ = builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
        }

        public bool Equals(FileWithBinaryPropertiesAndAncestorNames other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public override bool Equals(FileWithBinaryProperties other) => other is not null && ((other is FileWithBinaryPropertiesAndAncestorNames item) ? Equals(item) :
            base.Equals(other));

        public override bool Equals(IFileListItemWithBinaryProperties other)
        {
            return base.Equals(other);
        }

        public bool Equals(IFileListItemWithBinaryPropertiesAndAncestorNames other)
        {
            if (other is null) return false;
            if (other is FileWithBinaryPropertiesAndAncestorNames listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFileListItemWithBinaryPropertiesAndAncestorNames local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public bool Equals(IFileListItemWithAncestorNames other)
        {
            // TODO: Implement Equals(IFileListItemWithAncestorNames)
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }
    }
}
