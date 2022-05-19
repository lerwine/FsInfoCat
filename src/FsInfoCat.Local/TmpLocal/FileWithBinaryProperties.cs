using M = FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    // TODO: Document FileWithBinaryProperties class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class FileWithBinaryProperties : DbFileRow, ILocalFileListItemWithBinaryProperties, IEquatable<FileWithBinaryProperties>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vFileListingWithBinaryProperties";

        public long Length { get; set; }

        public M.MD5Hash? Hash { get; set; }

        public long RedundancyCount { get; set; }

        public long ComparisonCount { get; set; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithBinaryProperties> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Hash)).HasConversion(M.MD5Hash.Converter);
        }

        public virtual bool Equals(FileWithBinaryProperties other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public virtual bool Equals(M.IFileListItemWithBinaryProperties other)
        {
            if (other is null) return false;
            if (other is FileWithBinaryProperties listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFileListItemWithBinaryProperties local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is FileSystemListItem listItem) return Equals(listItem);
            if (obj is M.IFileListItemWithBinaryProperties other)
            {
                if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
                return !other.TryGetId(out _) && (other is ILocalFileListItemWithBinaryProperties local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
            }
            return false;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
