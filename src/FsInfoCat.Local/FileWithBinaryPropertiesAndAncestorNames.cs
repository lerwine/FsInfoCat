using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class FileWithBinaryPropertiesAndAncestorNames : FileWithBinaryProperties, ILocalFileListItemWithBinaryPropertiesAndAncestorNames, IEquatable<FileWithBinaryPropertiesAndAncestorNames>
    {
        private const string VIEW_NAME = "vFileListingWithBinaryPropertiesAndAncestorNames";

        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value.EmptyIfNullOrWhiteSpace(); }

        public Guid EffectiveVolumeId { get; set; }

        public string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value.EmptyIfNullOrWhiteSpace(); }

        public string VolumeName { get => _volumeName; set => _volumeName = value.EmptyIfNullOrWhiteSpace(); }

        public VolumeIdentifier VolumeIdentifier { get; set; } = VolumeIdentifier.Empty;

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.EmptyIfNullOrWhiteSpace(); }

        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value.EmptyIfNullOrWhiteSpace(); }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithBinaryPropertiesAndAncestorNames> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
            _ = builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalFileListItemWithBinaryProperties other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IFileListItemWithBinaryProperties other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(FileWithBinaryPropertiesAndAncestorNames other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(FileWithBinaryProperties other)
        {
            return base.Equals(other);
        }

        public override bool Equals(IFileListItemWithBinaryProperties other)
        {
            return base.Equals(other);
        }

        public bool Equals(IFileListItemWithBinaryPropertiesAndAncestorNames other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IFileListItemWithAncestorNames other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
