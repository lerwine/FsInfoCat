using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SubdirectoryListItem : SubdirectoryRow, ILocalSubdirectoryListItem
    {
        public const string VIEW_NAME = "vSubdirectoryListing";

        private readonly IPropertyChangeTracker<long> _subdirectoryCount;
        private readonly IPropertyChangeTracker<long> _fileCount;
        private readonly IPropertyChangeTracker<long> _accessErrorCount;
        private readonly IPropertyChangeTracker<long> _personalTagCount;
        private readonly IPropertyChangeTracker<long> _sharedTagCount;
        private readonly IPropertyChangeTracker<string> _crawlConfigDisplayName;

        public long SubdirectoryCount { get => _subdirectoryCount.GetValue(); set => _subdirectoryCount.SetValue(value); }

        public long FileCount { get => _fileCount.GetValue(); set => _fileCount.SetValue(value); }

        public long AccessErrorCount { get => _accessErrorCount.GetValue(); set => _accessErrorCount.SetValue(value); }

        public long PersonalTagCount { get => _personalTagCount.GetValue(); set => _personalTagCount.SetValue(value); }

        public long SharedTagCount { get => _sharedTagCount.GetValue(); set => _sharedTagCount.SetValue(value); }

        public string CrawlConfigDisplayName { get => _crawlConfigDisplayName.GetValue(); set => _crawlConfigDisplayName.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public SubdirectoryListItem()
        {
            _subdirectoryCount = AddChangeTracker(nameof(SubdirectoryCount), 0L);
            _fileCount = AddChangeTracker(nameof(FileCount), 0L);
            _accessErrorCount = AddChangeTracker(nameof(AccessErrorCount), 0L);
            _personalTagCount = AddChangeTracker(nameof(PersonalTagCount), 0L);
            _sharedTagCount = AddChangeTracker(nameof(SharedTagCount), 0L);
            _crawlConfigDisplayName = AddChangeTracker(nameof(CrawlConfigDisplayName), "", TrimmedNonNullStringCoersion.Default);
        }
    }
}
