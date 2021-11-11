using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class CrawlConfigReportItem : CrawlConfigListItemBase, ILocalCrawlConfigReportItem
    {
        private const string VIEW_NAME = "vCrawlConfigReport";

        private readonly IPropertyChangeTracker<long> _succeededCount;
        private readonly IPropertyChangeTracker<long> _timedOutCount;
        private readonly IPropertyChangeTracker<long> _itemLimitReachedCount;
        private readonly IPropertyChangeTracker<long> _canceledCount;
        private readonly IPropertyChangeTracker<long> _failedCount;
        private readonly IPropertyChangeTracker<long?> _averageDuration;
        private readonly IPropertyChangeTracker<long?> _maxDuration;

        public long SucceededCount { get => _succeededCount.GetValue(); set => _succeededCount.SetValue(value); }

        public long TimedOutCount { get => _timedOutCount.GetValue(); set => _timedOutCount.SetValue(value); }

        public long ItemLimitReachedCount { get => _itemLimitReachedCount.GetValue(); set => _itemLimitReachedCount.SetValue(value); }

        public long CanceledCount { get => _canceledCount.GetValue(); set => _canceledCount.SetValue(value); }

        public long FailedCount { get => _failedCount.GetValue(); set => _failedCount.SetValue(value); }

        public long? AverageDuration { get => _averageDuration.GetValue(); set => _averageDuration.SetValue(value); }

        public long? MaxDuration { get => _maxDuration.GetValue(); set => _maxDuration.SetValue(value); }

        public CrawlConfigReportItem()
        {
            _succeededCount = AddChangeTracker(nameof(SucceededCount), 0L);
            _timedOutCount = AddChangeTracker(nameof(TimedOutCount), 0L);
            _itemLimitReachedCount = AddChangeTracker(nameof(ItemLimitReachedCount), 0L);
            _canceledCount = AddChangeTracker(nameof(CanceledCount), 0L);
            _failedCount = AddChangeTracker(nameof(FailedCount), 0L);
            _averageDuration = AddChangeTracker<long?>(nameof(AverageDuration), null);
            _maxDuration = AddChangeTracker<long?>(nameof(MaxDuration), null);
        }

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlConfigReportItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME)
            .Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
    }
}
