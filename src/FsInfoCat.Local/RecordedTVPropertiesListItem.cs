using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class RecordedTVPropertiesListItem : RecordedTVPropertiesRow, ILocalRecordedTVPropertiesListItem
    {
        public const string VIEW_NAME = "vRecordedTVPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<RecordedTVPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
