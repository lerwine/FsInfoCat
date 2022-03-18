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
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 101;
                    hash = hash * 107 + Length.GetHashCode();
                    hash = EntityExtensions.HashNullable(Hash, hash, 107);
                    hash = hash * 107 + Status.GetHashCode();
                    hash = hash * 107 + Options.GetHashCode();
                    hash = hash * 107 + LastAccessed.GetHashCode();
                    hash = EntityExtensions.HashNullable(LastHashCalculation, hash, 107);
                    hash = hash * 107 + Notes.GetHashCode();
                    hash = hash * 107 + CreationTime.GetHashCode();
                    hash = hash * 107 + LastWriteTime.GetHashCode();
                    hash = hash * 107 + ParentId.GetHashCode();
                    hash = hash * 107 + BinaryPropertySetId.GetHashCode();
                    hash = EntityExtensions.HashNullable(SummaryPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(DocumentPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(AudioPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(DRMPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(GPSPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(ImagePropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(MediaPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(MusicPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(PhotoPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(RecordedTVPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(VideoPropertySetId, hash, 107);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 107);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 107);
                    hash = hash * 107 + CreatedOn.GetHashCode();
                    hash = hash * 107 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
