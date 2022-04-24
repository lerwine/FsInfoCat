using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class MediaPropertiesListItem : MediaPropertiesRow, ILocalMediaPropertiesListItem, IEquatable<MediaPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vMediaPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<MediaPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Producer)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Writer)).HasConversion(MultiStringValue.Converter);
        }

        public bool Equals(MediaPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IMediaPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is MediaPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalMediaPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IMediaPropertiesRow other)
        {
            if (other is null) return false;
            if (other is MediaPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalMediaPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IMediaProperties other)
        {
            if (other is null) return false;
            if (other is MediaPropertiesListItem listItem) return Equals(listItem);
            if (other is IMediaPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalMediaPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is MediaPropertiesListItem listItem) return Equals(listItem);
            if (obj is IMediaPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalMediaPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is IMediaProperties properties && ArePropertiesEqual(properties);
        }
    }
}
