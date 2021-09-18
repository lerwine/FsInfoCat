using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class PersonalSubdirectoryTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vPersonalSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }
}
