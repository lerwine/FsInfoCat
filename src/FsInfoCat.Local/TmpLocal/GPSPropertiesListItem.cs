using M = FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    // TODO: Document GPSPropertiesListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class GPSPropertiesListItem : GPSPropertiesRow, ILocalGPSPropertiesListItem, IEquatable<GPSPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vGPSPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<GPSPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME).HasKey(nameof(Id));
            _ = builder.Property(nameof(VersionID)).HasConversion(ByteValues.Converter);
        }

        public bool Equals(GPSPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(M.IGPSPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is GPSPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalGPSPropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.IGPSPropertiesRow other)
        {
            if (other is null) return false;
            if (other is GPSPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalGPSPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.IGPSProperties other)
        {
            if (other is null) return false;
            if (other is GPSPropertiesListItem listItem) return Equals(listItem);
            if (other is M.IGPSPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalGPSPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is GPSPropertiesListItem listItem) return Equals(listItem);
            if (obj is M.IGPSPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalGPSPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is M.IGPSProperties properties && ArePropertiesEqual(properties);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
