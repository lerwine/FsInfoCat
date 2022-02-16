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

        public bool Equals(CrawlJobLogListItem other)
        {
            throw new NotImplementedException();
        }

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
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 23;
                    hash = ConfigurationId.Equals(Guid.Empty) ? hash * 109 : hash * 109 + ConfigurationId.GetHashCode();
                    hash = hash * 31 + RootPath.GetHashCode();
                    hash = hash * 31 + MaxRecursionDepth.GetHashCode();
                    hash = MaxTotalItems.HasValue ? hash * 31 + (MaxTotalItems ?? default).GetHashCode() : hash * 31;
                    hash = TTL.HasValue ? hash * 31 + TTL.Value.GetHashCode() : hash * 31;
                    hash = UpstreamId.HasValue ? hash * 31 + (UpstreamId ?? default).GetHashCode() : hash * 31;
                    hash = LastSynchronizedOn.HasValue ? hash * 31 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 31;
                    hash = hash * 31 + CreatedOn.GetHashCode();
                    hash = hash * 31 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
