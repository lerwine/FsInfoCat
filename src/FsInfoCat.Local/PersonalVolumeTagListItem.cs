using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class PersonalVolumeTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vPersonalVolumeTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalVolumeTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }
}
