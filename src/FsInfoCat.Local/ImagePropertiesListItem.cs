using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ImagePropertiesListItem : ImagePropertiesRow, ILocalImagePropertiesListItem, IEquatable<ImagePropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vImagePropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<ImagePropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public bool Equals(ImagePropertiesListItem other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IImagePropertiesListItem other)
        {
            if (other is null) return false;
            if (other is ImagePropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalImagePropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IImagePropertiesRow other)
        {
            if (other is null) return false;
            if (other is ImagePropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalImagePropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IImageProperties other)
        {
            if (other is null) return false;
            if (other is ImagePropertiesListItem listItem) return Equals(listItem);
            if (other is IImagePropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalImagePropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is ImagePropertiesListItem listItem) return Equals(listItem);
            if (obj is IImagePropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalImagePropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is IImageProperties properties && ArePropertiesEqual(properties);
        }
    }
}
