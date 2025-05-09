using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// DB entity for a crawl configuration report.
    /// </summary>
    /// <seealso cref="CrawlConfigListItem" />
    /// <seealso cref="CrawlConfiguration" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class CrawlConfigReportItem : CrawlConfigListItemBase, ILocalCrawlConfigReportItem, IEquatable<CrawlConfigReportItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vCrawlConfigReport";

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public long SucceededCount { get; set; }

        public long TimedOutCount { get; set; }

        public long ItemLimitReachedCount { get; set; }

        public long CanceledCount { get; set; }

        public long FailedCount { get; set; }

        public long? AverageDuration { get; set; }

        public long? MaxDuration { get; set; }

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlConfigReportItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME)
            .Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalCrawlConfigReportItem" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfigReportItem other) => ArePropertiesEqual((ILocalCrawlConfigurationListItem)other) &&
            SucceededCount == other.SucceededCount &&
            TimedOutCount == other.TimedOutCount &&
            ItemLimitReachedCount == other.ItemLimitReachedCount &&
            CanceledCount == other.CanceledCount &&
            FailedCount == other.FailedCount &&
            AverageDuration == other.AverageDuration &&
            MaxDuration == other.MaxDuration;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ICrawlConfigReportItem" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ICrawlConfigReportItem other) => ArePropertiesEqual((ICrawlConfigurationListItem)other) &&
            SucceededCount == other.SucceededCount &&
            TimedOutCount == other.TimedOutCount &&
            ItemLimitReachedCount == other.ItemLimitReachedCount &&
            CanceledCount == other.CanceledCount &&
            FailedCount == other.FailedCount &&
            AverageDuration == other.AverageDuration &&
            MaxDuration == other.MaxDuration;

        public bool Equals(CrawlConfigReportItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(ICrawlConfigReportItem other)
        {
            if (other is null) return false;
            if (other is CrawlConfigReportItem crawlConfigReportItem) return Equals(crawlConfigReportItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalCrawlConfigReportItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(ICrawlConfigurationListItem other)
        {
            if (other is null) return false;
            if (other is CrawlConfigReportItem crawlConfigReportItem) return Equals(crawlConfigReportItem);
            if (TryGetId(out Guid id)) id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalCrawlConfigReportItem localReportItem)
                return ArePropertiesEqual(localReportItem);
            if (other is ICrawlConfigReportItem reportItem)
                return ArePropertiesEqual(reportItem);
            if (other is ILocalCrawlConfigurationListItem localListItem)
                return ArePropertiesEqual(localListItem);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is CrawlConfigReportItem crawlConfigReportItem) return Equals(crawlConfigReportItem);
            if (obj is ICrawlConfigurationRow row)
            {
                if (TryGetId(out Guid id)) id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalCrawlConfigReportItem localReportItem)
                    return ArePropertiesEqual(localReportItem);
                if (row is ICrawlConfigReportItem reportItem)
                    return ArePropertiesEqual(reportItem);
                if (row is ILocalCrawlConfigurationListItem localListItem)
                    return ArePropertiesEqual(localListItem);
                if (row is ICrawlConfigurationListItem listItem)
                    return ArePropertiesEqual(listItem);
                if (row is (ILocalCrawlConfigurationRow localRow))
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return false;
        }

        protected override string PropertiesToString() => $@"{base.PropertiesToString()},
    SucceededCount={SucceededCount}, TimedOutCount={TimedOutCount}, ItemLimitReachedCount={ItemLimitReachedCount}, CanceledCount={CanceledCount},
    FailedCount={FailedCount}, AverageDuration={AverageDuration}, MaxDuration={MaxDuration}";
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
