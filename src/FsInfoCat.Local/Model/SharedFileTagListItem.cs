using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity that associates a <see cref="SharedTagDefinition"/> with a <see cref="DbFile"/>.
    /// </summary>
    /// <seealso cref="SharedFileTag" />
    /// <seealso cref="SharedSubdirectoryTagListItem" />
    /// <seealso cref="SharedVolumeTagListItem" />
    /// <seealso cref="LocalDbContext.SharedFileTagListing" />
    public class SharedFileTagListItem : ItemTagListItem, IEquatable<SharedFileTagListItem>
    {
        private const string VIEW_NAME = "vSharedFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

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
