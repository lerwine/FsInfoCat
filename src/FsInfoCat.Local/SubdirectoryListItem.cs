namespace FsInfoCat.Local
{
    public class SubdirectoryListItem : SubdirectoryRow, ILocalSubdirectoryListItem
    {
        private readonly IPropertyChangeTracker<long> _subdirectoryCount;
        private readonly IPropertyChangeTracker<long> _fileCount;
        private readonly IPropertyChangeTracker<long> _accessErrorCount;
        private readonly IPropertyChangeTracker<long> _personalTagCount;
        private readonly IPropertyChangeTracker<long> _sharedTagCount;

        public long SubdirectoryCount { get => _subdirectoryCount.GetValue(); set => _subdirectoryCount.SetValue(value); }

        public long FileCount { get => _fileCount.GetValue(); set => _fileCount.SetValue(value); }

        public long AccessErrorCount { get => _accessErrorCount.GetValue(); set => _accessErrorCount.SetValue(value); }

        public long PersonalTagCount { get => _personalTagCount.GetValue(); set => _personalTagCount.SetValue(value); }

        public long SharedTagCount { get => _sharedTagCount.GetValue(); set => _sharedTagCount.SetValue(value); }

        public SubdirectoryListItem()
        {
            _subdirectoryCount = AddChangeTracker(nameof(SubdirectoryCount), 0L);
            _fileCount = AddChangeTracker(nameof(FileCount), 0L);
            _accessErrorCount = AddChangeTracker(nameof(AccessErrorCount), 0L);
            _personalTagCount = AddChangeTracker(nameof(PersonalTagCount), 0L);
            _sharedTagCount = AddChangeTracker(nameof(SharedTagCount), 0L);
        }
    }
}
