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
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 61;
                    hash = hash * 71 + AncestorNames.GetHashCode();
                    hash = hash * 71 + DisplayName.GetHashCode();
                    hash = hash * 71 + MaxRecursionDepth.GetHashCode();
                    hash = MaxTotalItems.HasValue ? hash * 71 + (MaxTotalItems ?? default).GetHashCode() : hash * 71;
                    hash = TTL.HasValue ? hash * 71 + TTL.Value.GetHashCode() : hash * 71;
                    hash = hash * 71 + Notes.GetHashCode();
                    hash = hash * 71 + StatusValue.GetHashCode();
                    hash = LastCrawlStart.HasValue ? hash * 71 + (LastCrawlStart ?? default).GetHashCode() : hash * 71;
                    hash = LastCrawlEnd.HasValue ? hash * 71 + (LastCrawlEnd ?? default).GetHashCode() : hash * 71;
                    hash = NextScheduledStart.HasValue ? hash * 71 + (NextScheduledStart ?? default).GetHashCode() : hash * 71;
                    hash = RescheduleInterval.HasValue ? hash * 71 + (RescheduleInterval ?? default).GetHashCode() : hash * 71;
                    hash = hash * 71 + RescheduleFromJobEnd.GetHashCode();
                    hash = hash * 71 + RescheduleAfterFail.GetHashCode();
                    hash = RootId.Equals(Guid.Empty) ? hash * 71 : hash * 71 + RootId.GetHashCode();
                    hash = UpstreamId.HasValue ? hash * 71 + (UpstreamId ?? default).GetHashCode() : hash * 71;
                    hash = LastSynchronizedOn.HasValue ? hash * 71 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 71;
                    hash = hash * 71 + CreatedOn.GetHashCode();
                    hash = hash * 71 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
