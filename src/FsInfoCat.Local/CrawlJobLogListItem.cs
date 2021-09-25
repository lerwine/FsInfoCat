using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class CrawlJobLogListItem : CrawlJobLogRow, ILocalCrawlJobListItem
    {
        private readonly IPropertyChangeTracker<string> _configurationDisplayName;
        private const string VIEW_NAME = "vCrawlJobListing";

        public string ConfigurationDisplayName { get => _configurationDisplayName.GetValue(); set => _configurationDisplayName.SetValue(value); }

        public CrawlJobLogListItem()
        {
            _configurationDisplayName = AddChangeTracker(nameof(ConfigurationDisplayName), "", TrimmedNonNullStringCoersion.Default);
        }

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlJobLogListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME);
    }
}
