using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class CrawlJobLogListItem : CrawlJobLogRow, ILocalCrawlJobListItem
    {
        private string _configurationDisplayName = string.Empty;
        private const string VIEW_NAME = "vCrawlJobListing";

        public string ConfigurationDisplayName { get => _configurationDisplayName; set => _configurationDisplayName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlJobLogListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME);
    }
}
