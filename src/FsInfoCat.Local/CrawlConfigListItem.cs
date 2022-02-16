using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class CrawlConfigListItem : CrawlConfigListItemBase, IEquatable<CrawlConfigListItem>
    {
        private const string VIEW_NAME = "vCrawlConfigListing";

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlConfigListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME)
            .Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);

        public bool Equals(CrawlConfigListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

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
                    hash = TTL.HasValue ? hash * 71 + (TTL ?? default).GetHashCode() : hash * 71;
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
