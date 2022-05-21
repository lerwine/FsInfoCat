using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    // TODO: Document PersonalVolumeTagListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PersonalVolumeTagListItem : ItemTagListItem, IEquatable<PersonalVolumeTagListItem>
    {
        public const string VIEW_NAME = "vPersonalVolumeTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalVolumeTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(PersonalVolumeTagListItem other)
        {
            // TODO: Implement Equals(PersonalVolumeTagListItem)
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            Guid taggedId = TaggedId;
            Guid definitionId = DefinitionId;
            if (taggedId.Equals(Guid.Empty) && DefinitionId.Equals(Guid.Empty))
                // TODO: Implement GetHashCode()
                throw new NotImplementedException();
            return HashCode.Combine(taggedId, definitionId);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
