using M = FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    // TODO: Document ImagePropertiesListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ImagePropertiesListItem : ImagePropertiesRow, ILocalImagePropertiesListItem, IEquatable<ImagePropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vImagePropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<ImagePropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public bool Equals(ImagePropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(M.IImagePropertiesListItem other)
        {
            if (other is null) return false;
            if (other is ImagePropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalImagePropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.IImagePropertiesRow other)
        {
            if (other is null) return false;
            if (other is ImagePropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalImagePropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.IImageProperties other)
        {
            if (other is null) return false;
            if (other is ImagePropertiesListItem listItem) return Equals(listItem);
            if (other is M.IImagePropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalImagePropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is ImagePropertiesListItem listItem) return Equals(listItem);
            if (obj is M.IImagePropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalImagePropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is M.IImageProperties properties && ArePropertiesEqual(properties);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
