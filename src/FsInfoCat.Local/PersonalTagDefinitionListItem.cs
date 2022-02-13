using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class PersonalTagDefinitionListItem : PersonalTagDefinitionRow, ILocalTagDefinitionListItem
    {
        public const string VIEW_NAME = "vPersonalTagDefinitionListing";

        public long FileTagCount { get; set; }

        public long SubdirectoryTagCount { get; set; }

        public long VolumeTagCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalTagDefinitionListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
