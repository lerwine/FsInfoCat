using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
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

        public virtual bool Equals(SummaryPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public virtual bool Equals(ISummaryPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is SummaryPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalSummaryPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(ISummaryPropertiesRow other)
        {
            if (other is null) return false;
            if (other is SummaryPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalSummaryPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(ISummaryProperties other)
        {
            if (other is null) return false;
            if (other is SummaryPropertiesListItem listItem) return Equals(listItem);
            if (other is ISummaryPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalSummaryPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SummaryPropertiesListItem listItem) return Equals(listItem);
            if (obj is ISummaryPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalSummaryPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is ISummaryProperties properties && ArePropertiesEqual(properties);
        }
    }
}
