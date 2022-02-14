using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class CrawlConfigReportItem : CrawlConfigListItemBase, ILocalCrawlConfigReportItem, IEquatable<CrawlConfigReportItem>
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfigReportItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ICrawlConfigReportItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(CrawlConfigReportItem other)
        {
            throw new NotImplementedException();
        }

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

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
