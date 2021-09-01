using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class VolumeListItem : VolumeRow, ILocalVolumeListItem
    {
        public const string VIEW_NAME = "vVolumeListing";

        private readonly IPropertyChangeTracker<long> _accessErrorCount;
        private readonly IPropertyChangeTracker<long> _personalTagCount;
        private readonly IPropertyChangeTracker<long> _sharedTagCount;
        private readonly IPropertyChangeTracker<string> _rootPath;
        private readonly IPropertyChangeTracker<long> _rootSubdirectoryCount;
        private readonly IPropertyChangeTracker<long> _rootFileCount;

        public string RootPath { get => _rootPath.GetValue(); set => _rootPath.SetValue(value); }

        public long AccessErrorCount { get => _accessErrorCount.GetValue(); set => _accessErrorCount.SetValue(value); }

        public long PersonalTagCount { get => _personalTagCount.GetValue(); set => _personalTagCount.SetValue(value); }

        public long SharedTagCount { get => _sharedTagCount.GetValue(); set => _sharedTagCount.SetValue(value); }

        public long RootSubdirectoryCount { get => _rootSubdirectoryCount.GetValue(); set => _rootSubdirectoryCount.SetValue(value); }

        public long RootFileCount { get => _rootFileCount.GetValue(); set => _rootFileCount.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<VolumeListItem> builder)
        {
            builder.ToView(VIEW_NAME);
            builder.Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);
        }

        public VolumeListItem()
        {
            _rootPath = AddChangeTracker(nameof(RootPath), "", NonNullStringCoersion.Default);
            _accessErrorCount = AddChangeTracker(nameof(AccessErrorCount), 0L);
            _personalTagCount = AddChangeTracker(nameof(PersonalTagCount), 0L);
            _sharedTagCount = AddChangeTracker(nameof(SharedTagCount), 0L);
            _rootSubdirectoryCount = AddChangeTracker(nameof(RootSubdirectoryCount), 0L);
            _rootFileCount = AddChangeTracker(nameof(RootFileCount), 0L);
        }
    }
}
