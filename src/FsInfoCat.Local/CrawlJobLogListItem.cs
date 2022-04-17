using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class CrawlJobLogListItem : CrawlJobLogRow, ILocalCrawlJobListItem, IEquatable<CrawlJobLogListItem>
    {
        private string _configurationDisplayName = string.Empty;
        private const string VIEW_NAME = "vCrawlJobListing";

        [NotNull]
        [BackingField(nameof(_configurationDisplayName))]
        public string ConfigurationDisplayName { get => _configurationDisplayName; set => _configurationDisplayName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlJobLogListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME);

        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlJobListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ICrawlJobListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(CrawlJobLogListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(ICrawlJobListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(ICrawlJob other)
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
                    int hash = EntityExtensions.HashGuid(ConfigurationId, 23, 31);
                    hash = hash * 31 + RootPath.GetHashCode();
                    hash = hash * 31 + MaxRecursionDepth.GetHashCode();
                    hash = EntityExtensions.HashNullable(MaxTotalItems, hash, 31);
                    hash = EntityExtensions.HashNullable(TTL, hash, 31);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 31);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 31);
                    hash = hash * 31 + CreatedOn.GetHashCode();
                    hash = hash * 31 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
