using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SharedVolumeTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vSharedVolumeTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedVolumeTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }
}
