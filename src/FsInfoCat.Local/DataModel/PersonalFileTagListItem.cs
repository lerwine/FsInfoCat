using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    // TODO: Document PersonalFileTagListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete("Use FsInfoCat.Local.Model.PersonalFileTagListItem")]
    public class PersonalFileTagListItem : ItemTagListItem, IEquatable<PersonalFileTagListItem>
    {
        public const string VIEW_NAME = "vPersonalFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(PersonalFileTagListItem other)
        {
            // TODO: Implement Equals(PersonalFileTagListItem)
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
