using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class SharedVolumeTagListItem : ItemTagListItem, IEquatable<SharedVolumeTagListItem>
    {
        public const string VIEW_NAME = "vSharedVolumeTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedVolumeTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(SharedVolumeTagListItem other)
        {
            // TODO: Implement Equals(SharedVolumeTagListItem)
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