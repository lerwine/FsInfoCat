using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class PhotoPropertiesListItem : PhotoPropertiesRow, ILocalPhotoPropertiesListItem
    {
        public const string VIEW_NAME = "vPhotoPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<PhotoPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Event)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(PeopleNames)).HasConversion(MultiStringValue.Converter);
        }
    }
}
