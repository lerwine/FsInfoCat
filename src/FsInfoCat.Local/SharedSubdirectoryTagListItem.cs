using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SharedSubdirectoryTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vSharedSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }
}
