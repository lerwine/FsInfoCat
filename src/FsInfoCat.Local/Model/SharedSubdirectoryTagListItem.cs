using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity that associates a <see cref="SharedTagDefinition"/> with a <see cref="Subdirectory"/>.
    /// </summary>
    /// <seealso cref="SharedSubdirectoryTag" />
    /// <seealso cref="SharedFileTagListItem" />
    /// <seealso cref="SharedVolumeTagListItem" />
    public class SharedSubdirectoryTagListItem : ItemTagListItem, IEquatable<SharedFileTagListItem>
    {
        public const string VIEW_NAME = "vSharedSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(SharedFileTagListItem other)
        {
            // TODO: Implement Equals(SharedFileTagListItem)
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
