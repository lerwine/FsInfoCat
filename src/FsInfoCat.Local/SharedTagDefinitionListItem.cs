using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SharedTagDefinitionListItem : SharedTagDefinitionRow, ILocalTagDefinitionListItem
    {
        public const string VIEW_NAME = "vSharedTagDefinitionListing";

        public long FileTagCount { get; set; }

        public long SubdirectoryTagCount { get; set; }

        public long VolumeTagCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<SharedTagDefinitionListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
