using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SubdirectoryListItem : SubdirectoryRow, ILocalSubdirectoryListItem
    {
        public const string VIEW_NAME = "vSubdirectoryListing";

        private string _crawlConfigDisplayName;

        public long SubdirectoryCount { get; set; }

        public long FileCount { get; set; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        public string CrawlConfigDisplayName { get => _crawlConfigDisplayName; set => _crawlConfigDisplayName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
