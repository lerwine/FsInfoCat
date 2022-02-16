using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class FileWithBinaryProperties : DbFileRow, ILocalFileListItemWithBinaryProperties, IEquatable<FileWithBinaryProperties>
    {
        private const string VIEW_NAME = "vFileListingWithBinaryProperties";

        public long Length { get; set; }

        public MD5Hash? Hash { get; set; }

        public long RedundancyCount { get; set; }

        public long ComparisonCount { get; set; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithBinaryProperties> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalFileListItemWithBinaryProperties other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IFileListItemWithBinaryProperties other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(FileWithBinaryProperties other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public virtual bool Equals(IFileListItemWithBinaryProperties other)
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
                    int hash = 101;
                    hash = hash * 107 + Length.GetHashCode();
                    hash = Hash.HasValue ? hash * 107 + (Hash ?? default).GetHashCode() : hash * 107;
                    hash = hash * 107 + Status.GetHashCode();
                    hash = hash * 107 + Options.GetHashCode();
                    hash = hash * 107 + LastAccessed.GetHashCode();
                    hash = LastHashCalculation.HasValue ? hash * 107 + (LastHashCalculation ?? default).GetHashCode() : hash * 107;
                    hash = hash * 107 + Notes.GetHashCode();
                    hash = hash * 107 + CreationTime.GetHashCode();
                    hash = hash * 107 + LastWriteTime.GetHashCode();
                    hash = hash * 107 + ParentId.GetHashCode();
                    hash = hash * 107 + BinaryPropertySetId.GetHashCode();
                    hash = SummaryPropertySetId.HasValue ? hash * 107 + (SummaryPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = DocumentPropertySetId.HasValue ? hash * 107 + (DocumentPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = AudioPropertySetId.HasValue ? hash * 107 + (AudioPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = DRMPropertySetId.HasValue ? hash * 107 + (DRMPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = GPSPropertySetId.HasValue ? hash * 107 + (GPSPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = ImagePropertySetId.HasValue ? hash * 107 + (ImagePropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = MediaPropertySetId.HasValue ? hash * 107 + (MediaPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = MusicPropertySetId.HasValue ? hash * 107 + (MusicPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = PhotoPropertySetId.HasValue ? hash * 107 + (PhotoPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = RecordedTVPropertySetId.HasValue ? hash * 107 + (RecordedTVPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = VideoPropertySetId.HasValue ? hash * 107 + (VideoPropertySetId ?? default).GetHashCode() : hash * 107;
                    hash = UpstreamId.HasValue ? hash * 107 + (UpstreamId ?? default).GetHashCode() : hash * 107;
                    hash = LastSynchronizedOn.HasValue ? hash * 107 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 107;
                    hash = hash * 107 + CreatedOn.GetHashCode();
                    hash = hash * 107 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
