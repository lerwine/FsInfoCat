using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class PersonalFileTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vPersonalFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }
}
