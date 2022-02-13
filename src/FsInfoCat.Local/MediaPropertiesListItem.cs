using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class MediaPropertiesListItem : MediaPropertiesRow, ILocalMediaPropertiesListItem
    {
        public const string VIEW_NAME = "vMediaPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<MediaPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Producer)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Writer)).HasConversion(MultiStringValue.Converter);
        }
    }
}
