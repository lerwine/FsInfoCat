using M = FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    // TODO: Document SummaryPropertiesListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class SummaryPropertiesListItem : SummaryPropertiesRow, ILocalSummaryPropertiesListItem, IEquatable<SummaryPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vSummaryPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<SummaryPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Author)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Keywords)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(ItemAuthors)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Kind)).HasConversion(MultiStringValue.Converter);
        }

        public virtual bool Equals(SummaryPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public virtual bool Equals(M.ISummaryPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is SummaryPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSummaryPropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.ISummaryPropertiesRow other)
        {
            if (other is null) return false;
            if (other is SummaryPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSummaryPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.ISummaryProperties other)
        {
            if (other is null) return false;
            if (other is SummaryPropertiesListItem listItem) return Equals(listItem);
            if (other is M.ISummaryPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalSummaryPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SummaryPropertiesListItem listItem) return Equals(listItem);
            if (obj is M.ISummaryPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalSummaryPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is M.ISummaryProperties properties && ArePropertiesEqual(properties);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
