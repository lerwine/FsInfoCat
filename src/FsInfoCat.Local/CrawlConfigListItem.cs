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
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 61;
                    hash = hash * 71 + AncestorNames.GetHashCode();
                    hash = hash * 71 + DisplayName.GetHashCode();
                    hash = hash * 71 + MaxRecursionDepth.GetHashCode();
                    hash = EntityExtensions.HashNullable(MaxTotalItems, hash, 71);
                    hash = EntityExtensions.HashNullable(TTL, hash, 71);
                    hash = hash * 71 + Notes.GetHashCode();
                    hash = hash * 71 + StatusValue.GetHashCode();
                    hash = EntityExtensions.HashNullable(LastCrawlStart, hash, 71);
                    hash = EntityExtensions.HashNullable(LastCrawlEnd, hash, 71);
                    hash = EntityExtensions.HashNullable(NextScheduledStart, hash, 71);
                    hash = EntityExtensions.HashNullable(RescheduleInterval, hash, 71);
                    hash = hash * 71 + RescheduleFromJobEnd.GetHashCode();
                    hash = hash * 71 + RescheduleAfterFail.GetHashCode();
                    hash = EntityExtensions.HashGuid(RootId, hash, 71);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 71);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 71);
                    hash = hash * 71 + CreatedOn.GetHashCode();
                    hash = hash * 71 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
