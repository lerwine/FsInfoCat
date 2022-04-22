using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class SharedFileTagListItem : ItemTagListItem, IEquatable<SharedFileTagListItem>
    {
        public const string VIEW_NAME = "vSharedFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(SharedFileTagListItem other)
        {
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
                // TODO: Implement Equals(object)
                throw new NotImplementedException();
            return HashCode.Combine(taggedId, definitionId);
        }
    }
}
