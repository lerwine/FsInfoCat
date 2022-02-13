using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class VideoPropertiesListItem : VideoPropertiesRow, ILocalVideoPropertiesListItem
    {
        public const string VIEW_NAME = "vVideoPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<VideoPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME).HasKey(nameof(Id));
            _ = builder.Property(nameof(Director)).HasConversion(MultiStringValue.Converter);
        }
    }
}
