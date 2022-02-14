using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class PersonalVolumeTagListItem : ItemTagListItem, IEquatable<PersonalVolumeTagListItem>
    {
        public const string VIEW_NAME = "vPersonalVolumeTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalVolumeTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(PersonalVolumeTagListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
