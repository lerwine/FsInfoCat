using M = FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    // TODO: Document PhotoPropertiesListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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

        public bool Equals(PhotoPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(M.IPhotoPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is PhotoPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalPhotoPropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.IPhotoPropertiesRow other)
        {
            if (other is null) return false;
            if (other is PhotoPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalPhotoPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.IPhotoProperties other)
        {
            if (other is null) return false;
            if (other is PhotoPropertiesListItem listItem) return Equals(listItem);
            if (other is M.IPhotoPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalPhotoPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is PhotoPropertiesListItem listItem) return Equals(listItem);
            if (obj is M.IPhotoPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalPhotoPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is M.IPhotoProperties properties && ArePropertiesEqual(properties);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
