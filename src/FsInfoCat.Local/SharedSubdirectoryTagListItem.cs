using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class SharedSubdirectoryTagListItem : ItemTagListItem, IEquatable<SharedFileTagListItem>
    {
        public const string VIEW_NAME = "vSharedSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(SharedFileTagListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            Guid taggedId = TaggedId;
            Guid definitionId = DefinitionId;
            if (taggedId.Equals(Guid.Empty) && DefinitionId.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + Name.GetHashCode();
                    hash = hash * 23 + Description.GetHashCode();
                    hash = hash * 23 + Notes.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 23);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 23);
                    hash = hash * 23 + CreatedOn.GetHashCode();
                    hash = hash * 23 + ModifiedOn.GetHashCode();
                    return hash;
                }
            unchecked
            {
                return EntityExtensions.HashGuid(definitionId, EntityExtensions.HashGuid(taggedId, 3, 7), 7);
            }
        }
    }
}
