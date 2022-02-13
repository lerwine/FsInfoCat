using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class ImagePropertiesListItem : ImagePropertiesRow, ILocalImagePropertiesListItem
    {
        public const string VIEW_NAME = "vImagePropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<ImagePropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
