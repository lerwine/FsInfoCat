using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class GPSPropertiesListItem : GPSPropertiesRow, ILocalGPSPropertiesListItem
    {
        public const string VIEW_NAME = "vGPSPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<GPSPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME).HasKey(nameof(Id));
            _ = builder.Property(nameof(VersionID)).HasConversion(ByteValues.Converter);
        }
    }
}
