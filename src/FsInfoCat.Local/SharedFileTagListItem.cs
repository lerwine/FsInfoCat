using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SharedFileTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vSharedFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }
}
