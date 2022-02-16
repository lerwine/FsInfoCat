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
            unchecked
            {
                int hash = 23;
                hash = hash * 31 + TaggedId.GetHashCode();
                hash = hash * 31 + DefinitionId.GetHashCode();
                hash = hash * 31 + Name.GetHashCode();
                hash = hash * 31 + Description.GetHashCode();
                hash = hash * 31 + Notes.GetHashCode();
                hash = UpstreamId.HasValue ? hash * 31 + (UpstreamId ?? default).GetHashCode() : hash * 31;
                hash = LastSynchronizedOn.HasValue ? hash * 31 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 31;
                hash = hash * 31 + CreatedOn.GetHashCode();
                hash = hash * 31 + ModifiedOn.GetHashCode();
                return hash;
            }
        }
    }
}
