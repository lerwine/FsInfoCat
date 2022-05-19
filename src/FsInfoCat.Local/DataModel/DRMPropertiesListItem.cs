using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    // TODO: Document DRMPropertiesListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    [Obsolete("Use FsInfoCat.Local.Model.DRMPropertiesListItem")]
    public class DRMPropertiesListItem : DRMPropertiesRow, ILocalDRMPropertiesListItem, IEquatable<DRMPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vDRMPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<DRMPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public bool Equals(DRMPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IDRMPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is DRMPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalDRMPropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IDRMPropertiesRow other)
        {
            if (other is null) return false;
            if (other is DRMPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalDRMPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IDRMProperties other)
        {
            if (other is null) return false;
            if (other is DRMPropertiesListItem listItem) return Equals(listItem);
            if (other is IDRMPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalDRMPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is DRMPropertiesListItem listItem) return Equals(listItem);
            if (obj is IDRMPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalDRMPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is IDRMProperties properties && ArePropertiesEqual(properties);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
