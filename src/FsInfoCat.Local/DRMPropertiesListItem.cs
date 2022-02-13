using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class DRMPropertiesListItem : DRMPropertiesRow, ILocalDRMPropertiesListItem
    {
        public const string VIEW_NAME = "vDRMPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<DRMPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
