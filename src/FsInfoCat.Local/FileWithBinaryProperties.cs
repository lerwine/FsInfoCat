using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class FileWithBinaryProperties : DbFileRow, ILocalFileListItemWithBinaryProperties
    {
        private const string VIEW_NAME = "vFileListingWithBinaryProperties";

        private readonly IPropertyChangeTracker<long> _length;
        private readonly IPropertyChangeTracker<MD5Hash?> _hash;
        private readonly IPropertyChangeTracker<long> _redundancyCount;
        private readonly IPropertyChangeTracker<long> _comparisonCount;
        private readonly IPropertyChangeTracker<long> _accessErrorCount;
        private readonly IPropertyChangeTracker<long> _personalTagCount;
        private readonly IPropertyChangeTracker<long> _sharedTagCount;

        public long Length { get => _length.GetValue(); set => _length.SetValue(value); }

        public MD5Hash? Hash { get => _hash.GetValue(); set => _hash.SetValue(value); }

        public long RedundancyCount { get => _redundancyCount.GetValue(); set => _redundancyCount.SetValue(value); }

        public long ComparisonCount { get => _comparisonCount.GetValue(); set => _comparisonCount.SetValue(value); }

        public long AccessErrorCount { get => _accessErrorCount.GetValue(); set => _accessErrorCount.SetValue(value); }

        public long PersonalTagCount { get => _personalTagCount.GetValue(); set => _personalTagCount.SetValue(value); }

        public long SharedTagCount { get => _sharedTagCount.GetValue(); set => _sharedTagCount.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithBinaryProperties> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
        }

        public FileWithBinaryProperties()
        {
            _length = AddChangeTracker(nameof(Length), 0L);
            _hash = AddChangeTracker<MD5Hash?>(nameof(Hash), null);
            _redundancyCount = AddChangeTracker(nameof(RedundancyCount), 0L);
            _comparisonCount = AddChangeTracker(nameof(ComparisonCount), 0L);
            _accessErrorCount = AddChangeTracker(nameof(AccessErrorCount), 0L);
            _personalTagCount = AddChangeTracker(nameof(PersonalTagCount), 0L);
            _sharedTagCount = AddChangeTracker(nameof(SharedTagCount), 0L);
        }
    }
}
