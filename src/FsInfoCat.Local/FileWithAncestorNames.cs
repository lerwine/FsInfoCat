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
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 89;
                    hash = hash * 101 + Status.GetHashCode();
                    hash = hash * 101 + Options.GetHashCode();
                    hash = hash * 101 + LastAccessed.GetHashCode();
                    hash = LastHashCalculation.HasValue ? hash * 101 + (LastHashCalculation ?? default).GetHashCode() : hash * 101;
                    hash = hash * 101 + Notes.GetHashCode();
                    hash = hash * 101 + CreationTime.GetHashCode();
                    hash = hash * 101 + LastWriteTime.GetHashCode();
                    hash = hash * 101 + ParentId.GetHashCode();
                    hash = hash * 101 + BinaryPropertySetId.GetHashCode();
                    hash = SummaryPropertySetId.HasValue ? hash * 101 + (SummaryPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = DocumentPropertySetId.HasValue ? hash * 101 + (DocumentPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = AudioPropertySetId.HasValue ? hash * 101 + (AudioPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = DRMPropertySetId.HasValue ? hash * 101 + (DRMPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = GPSPropertySetId.HasValue ? hash * 101 + (GPSPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = ImagePropertySetId.HasValue ? hash * 101 + (ImagePropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = MediaPropertySetId.HasValue ? hash * 101 + (MediaPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = MusicPropertySetId.HasValue ? hash * 101 + (MusicPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = PhotoPropertySetId.HasValue ? hash * 101 + (PhotoPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = RecordedTVPropertySetId.HasValue ? hash * 101 + (RecordedTVPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = VideoPropertySetId.HasValue ? hash * 101 + (VideoPropertySetId ?? default).GetHashCode() : hash * 101;
                    hash = UpstreamId.HasValue ? hash * 101 + (UpstreamId ?? default).GetHashCode() : hash * 101;
                    hash = LastSynchronizedOn.HasValue ? hash * 101 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 101;
                    hash = hash * 101 + CreatedOn.GetHashCode();
                    hash = hash * 101 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
