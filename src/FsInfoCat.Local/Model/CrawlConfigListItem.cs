using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    /// List item DB entity that specifies the configuration of a file system crawl.
    /// </summary>
    public class CrawlConfigListItem : CrawlConfigListItemBase, IEquatable<CrawlConfigListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vCrawlConfigListing";

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlConfigListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME)
            .Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(CrawlConfigListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public override bool Equals(ICrawlConfigurationListItem other)
        {
            if (other is null) return false;
            if (other is CrawlConfigListItem crawlConfigListItem) return Equals(crawlConfigListItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalCrawlConfigurationListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
