using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class MusicPropertiesListItem : MusicPropertiesRow, ILocalMusicPropertiesListItem
    {
        public const string VIEW_NAME = "vMusicPropertiesListing";

        private readonly IPropertyChangeTracker<long> _existingFileCount;
        private readonly IPropertyChangeTracker<long> _totalFileCount;

        public long ExistingFileCount { get => _existingFileCount.GetValue(); set => _existingFileCount.SetValue(value); }

        public long TotalFileCount { get => _totalFileCount.GetValue(); set => _totalFileCount.SetValue(value); }

        public MusicPropertiesListItem()
        {
            _existingFileCount = AddChangeTracker(nameof(ExistingFileCount), 0L);
            _totalFileCount = AddChangeTracker(nameof(TotalFileCount), 0L);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<MusicPropertiesListItem> builder)
        {
            builder.ToView(VIEW_NAME);
            builder.Property(nameof(Artist)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Composer)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Conductor)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter);
        }
    }
}
