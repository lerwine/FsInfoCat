namespace FsInfoCat.Local
{
    public class FileWithAncestorNames : DbFileRow, ILocalFileListItemWithAncestorNames
    {
        private readonly IPropertyChangeTracker<long> _accessErrorCount;
        private readonly IPropertyChangeTracker<long> _personalTagCount;
        private readonly IPropertyChangeTracker<long> _sharedTagCount;
        private readonly IPropertyChangeTracker<string> _ancestorNames;

        public long AccessErrorCount { get => _accessErrorCount.GetValue(); set => _accessErrorCount.SetValue(value);  }

        public long PersonalTagCount { get => _personalTagCount.GetValue(); set => _personalTagCount.SetValue(value); }

        public long SharedTagCount { get => _sharedTagCount.GetValue(); set => _sharedTagCount.SetValue(value); }

        public string AncestorNames { get => _ancestorNames.GetValue(); set => _ancestorNames.SetValue(value); }

        public FileWithAncestorNames()
        {
            _accessErrorCount = AddChangeTracker(nameof(AccessErrorCount), 0L);
            _personalTagCount = AddChangeTracker(nameof(PersonalTagCount), 0L);
            _sharedTagCount = AddChangeTracker(nameof(SharedTagCount), 0L);
            _ancestorNames = AddChangeTracker(nameof(AncestorNames), "", NonNullStringCoersion.Default);
        }
    }
}
