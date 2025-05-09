using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="ILocalMediaPropertiesRow" />
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
            _ = builder.Property(nameof(Producer)).HasConversion(MultiStringValue.Converter, MultiStringValue.Comparer);
            _ = builder.Property(nameof(Writer)).HasConversion(MultiStringValue.Converter, MultiStringValue.Comparer);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(MediaPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IMediaPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is MediaPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalMediaPropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IMediaPropertiesRow other)
        {
            if (other is null) return false;
            if (other is MediaPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalMediaPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IMediaProperties other)
        {
            if (other is null) return false;
            if (other is MediaPropertiesListItem listItem) return Equals(listItem);
            if (other is IMediaPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalMediaPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is MediaPropertiesListItem listItem) return Equals(listItem);
            if (obj is IMediaPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalMediaPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is IMediaProperties properties && ArePropertiesEqual(properties);
        }

        protected override string PropertiesToString() => $@"{base.PropertiesToString()},
    ExistingFileCount={ExistingFileCount}, TotalFileCount={TotalFileCount}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
