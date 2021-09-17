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

    public class SharedVolumeTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vSharedVolumeTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedVolumeTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }

    public class PersonalSubdirectoryTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vPersonalSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }

    public class SharedSubdirectoryTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vSharedSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }

    public class PersonalFileTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vPersonalFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }

    public class SharedFileTagListItem : ItemTagListItem
    {
        public const string VIEW_NAME = "vSharedFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));
    }
}
