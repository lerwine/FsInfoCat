using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SharedTagDefinitionListItem : SharedTagDefinitionRow, ILocalTagDefinitionListItem
    {
        public const string VIEW_NAME = "vSharedTagDefinitionListing";

        private readonly IPropertyChangeTracker<long> _fileTagCount;
        private readonly IPropertyChangeTracker<long> _subdirectoryTagCount;
        private readonly IPropertyChangeTracker<long> _volumeTagCount;

        public long FileTagCount { get => _fileTagCount.GetValue(); set => _fileTagCount.SetValue(value); }

        public long SubdirectoryTagCount { get => _subdirectoryTagCount.GetValue(); set => _subdirectoryTagCount.SetValue(value); }

        public long VolumeTagCount { get => _volumeTagCount.GetValue(); set => _volumeTagCount.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<SharedTagDefinitionListItem> builder) => builder.ToView(VIEW_NAME);

        public SharedTagDefinitionListItem()
        {
            _fileTagCount = AddChangeTracker(nameof(FileTagCount), 0L);
            _subdirectoryTagCount = AddChangeTracker(nameof(SubdirectoryTagCount), 0L);
            _volumeTagCount = AddChangeTracker(nameof(VolumeTagCount), 0L);
        }
    }
}