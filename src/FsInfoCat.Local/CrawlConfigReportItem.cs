using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class CrawlConfigReportItem : CrawlConfigListItemBase, ILocalCrawlConfigReportItem, IEquatable<CrawlConfigReportItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vCrawlConfigReport";

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

        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfigReportItem other) => ArePropertiesEqual((ILocalCrawlConfigurationListItem)other) &&
            SucceededCount == other.SucceededCount &&
            TimedOutCount == other.TimedOutCount &&
            ItemLimitReachedCount == other.ItemLimitReachedCount &&
            CanceledCount == other.CanceledCount &&
            FailedCount == other.FailedCount &&
            AverageDuration == other.AverageDuration &&
            MaxDuration == other.MaxDuration;

        protected bool ArePropertiesEqual([DisallowNull] ICrawlConfigReportItem other) => ArePropertiesEqual((ICrawlConfigurationListItem)other) &&
            SucceededCount == other.SucceededCount &&
            TimedOutCount == other.TimedOutCount &&
            ItemLimitReachedCount == other.ItemLimitReachedCount &&
            CanceledCount == other.CanceledCount &&
            FailedCount == other.FailedCount &&
            AverageDuration == other.AverageDuration &&
            MaxDuration == other.MaxDuration;

        public bool Equals(CrawlConfigReportItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(ICrawlConfigReportItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(ICrawlConfigurationListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
