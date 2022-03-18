using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class FileWithAncestorNames : DbFileRow, ILocalFileListItemWithAncestorNames, IEquatable<FileWithAncestorNames>
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

        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value.EmptyIfNullOrWhiteSpace(); }

        public Guid EffectiveVolumeId { get; set; }

        public string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value.EmptyIfNullOrWhiteSpace(); }

        public string VolumeName { get => _volumeName; set => _volumeName = value.EmptyIfNullOrWhiteSpace(); }

        public VolumeIdentifier VolumeIdentifier { get; set; } = VolumeIdentifier.Empty;

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.EmptyIfNullOrWhiteSpace(); }

        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value.EmptyIfNullOrWhiteSpace(); }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithAncestorNames> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalFileListItemWithAncestorNames other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IFileListItemWithAncestorNames other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(FileWithAncestorNames other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

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
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 89;
                    hash = hash * 101 + Status.GetHashCode();
                    hash = hash * 101 + Options.GetHashCode();
                    hash = hash * 101 + LastAccessed.GetHashCode();
                    hash = EntityExtensions.HashNullable(LastHashCalculation, hash, 101);
                    hash = hash * 101 + Notes.GetHashCode();
                    hash = hash * 101 + CreationTime.GetHashCode();
                    hash = hash * 101 + LastWriteTime.GetHashCode();
                    hash = hash * 101 + ParentId.GetHashCode();
                    hash = hash * 101 + BinaryPropertySetId.GetHashCode();
                    hash = EntityExtensions.HashNullable(SummaryPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(DocumentPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(AudioPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(DRMPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(GPSPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(ImagePropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(MediaPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(MusicPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(PhotoPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(RecordedTVPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(VideoPropertySetId, hash, 101);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 101);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 101);
                    hash = hash * 101 + CreatedOn.GetHashCode();
                    hash = hash * 101 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
