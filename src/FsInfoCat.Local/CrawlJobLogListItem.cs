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
