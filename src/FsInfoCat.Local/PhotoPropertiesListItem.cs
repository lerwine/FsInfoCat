using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class PhotoPropertiesListItem : PhotoPropertiesRow, ILocalPhotoPropertiesListItem, IEquatable<PhotoPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vPhotoPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<PhotoPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Event)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(PeopleNames)).HasConversion(MultiStringValue.Converter);
        }

        public bool Equals(PhotoPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IPhotoPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is PhotoPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalPhotoPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IPhotoPropertiesRow other)
        {
            if (other is null) return false;
            if (other is PhotoPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalPhotoPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IPhotoProperties other)
        {
            if (other is null) return false;
            if (other is PhotoPropertiesListItem listItem) return Equals(listItem);
            if (other is IPhotoPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalPhotoPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is PhotoPropertiesListItem listItem) return Equals(listItem);
            if (obj is IPhotoPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalPhotoPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is IPhotoProperties properties && ArePropertiesEqual(properties);
        }
    }
}
