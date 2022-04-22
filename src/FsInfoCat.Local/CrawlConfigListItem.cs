using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class CrawlConfigListItem : CrawlConfigListItemBase, IEquatable<CrawlConfigListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vCrawlConfigListing";

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlConfigListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME)
            .Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);

        public bool Equals(CrawlConfigListItem other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (TryGetId(out Guid id))
                return other.TryGetId(out Guid g) && id.Equals(g);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public override bool Equals(ICrawlConfigurationListItem other)
        {
            if (other is null) return false;
            if (other is CrawlConfigListItem crawlConfigListItem) return Equals(crawlConfigListItem);
            if (TryGetId(out Guid id)) id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalCrawlConfigurationListItem localsListItem)
                return ArePropertiesEqual(localsListItem);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is CrawlConfigListItem crawlConfigListItem) return Equals(crawlConfigListItem);
            if (obj is ICrawlConfigurationRow row)
            {
                if (TryGetId(out Guid id)) id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalCrawlConfigurationListItem localsListItem)
                    return ArePropertiesEqual(localsListItem);
                if (row is ICrawlConfigurationListItem listItem)
                    return ArePropertiesEqual(listItem);
                if (row is (ILocalCrawlConfigurationRow localRow))
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return false;
        }
    }
}
