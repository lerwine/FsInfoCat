using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity that associates a <see cref="PersonalTagDefinition"/> with an <see cref="DbFile"/>, <see cref="Subdirectory"/> or <see cref="Volume"/>.
    /// </summary>
    /// <seealso cref="PersonalFileTag" />
    /// <seealso cref="PersonalSubdirectoryTagListItem" />
    /// <seealso cref="PersonalVolumeTagListItem" />
    /// <seealso cref="LocalDbContext.PersonalFileTagListing" />
    public class PersonalFileTagListItem : ItemTagListItem, IEquatable<PersonalFileTagListItem>
    {
        private const string VIEW_NAME = "vPersonalFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
