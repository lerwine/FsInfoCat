using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class CrawlConfigReportItem : CrawlConfigListItemBase, ILocalCrawlConfigReportItem
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
    }
}
