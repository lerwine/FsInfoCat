using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class MusicPropertiesListItem : MusicPropertiesRow, ILocalMusicPropertiesListItem
    {
        public const string VIEW_NAME = "vMusicPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<MusicPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Artist)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Composer)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Conductor)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter);
        }
    }
}
