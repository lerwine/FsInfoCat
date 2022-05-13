using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class PersonalSubdirectoryTagListItem : ItemTagListItem, IEquatable<PersonalSubdirectoryTagListItem>
    {
        public const string VIEW_NAME = "vPersonalSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(PersonalSubdirectoryTagListItem other)
        {
            // TODO: Implement Equals(PersonalSubdirectoryTagListItem)
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
}
