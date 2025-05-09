using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity that associates a <see cref="PersonalTagDefinition"/> with a <see cref="Subdirectory"/>.
    /// </summary>
    /// <seealso cref="PersonalSubdirectoryTag" />
    /// <seealso cref="PersonalFileTagListItem" />
    /// <seealso cref="PersonalVolumeTagListItem" />
    /// <seealso cref="LocalDbContext.PersonalSubdirectoryTagListing" />
    public class PersonalSubdirectoryTagListItem : ItemTagListItem, IEquatable<PersonalSubdirectoryTagListItem>
    {
        private const string VIEW_NAME = "vPersonalSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
