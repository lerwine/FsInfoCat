using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for an entity that associates a <see cref="SharedTagDefinitionRow"/> or <see cref="PersonalTagDefinitionRow"/> with a <see cref="DbFile"/>, <see cref="Subdirectory"/>
    /// or <see cref="Volume"/>.
    /// </summary>
    /// <seealso cref="ItemTag" />
    /// <seealso cref="ItemTagListItem" />
    /// <seealso cref="LocalDbContext.PersonalFileTags" />
    /// <seealso cref="LocalDbContext.PersonalSubdirectoryTags" />
    /// <seealso cref="LocalDbContext.PersonalVolumeTags" />
    /// <seealso cref="LocalDbContext.PersonalFileTagListing" />
    /// <seealso cref="LocalDbContext.PersonalSubdirectoryTagListing" />
    /// <seealso cref="LocalDbContext.PersonalVolumeTagListing" />
    public abstract class ItemTagRow : LocalDbEntity, ILocalItemTagRow
    {
        private string _notes = string.Empty;

        [Required]
        public abstract Guid TaggedId { get; set; }

        [Required]
        public abstract Guid DefinitionId { get; set; }

        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        IEnumerable<Guid> IHasCompoundIdentifier.Id
        {
            get
            {
                yield return DefinitionId;
                yield return TaggedId;
            }
        }

        (Guid, Guid) IHasIdentifierPair.Id => (DefinitionId, TaggedId);

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalItemTagRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalItemTagRow other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalItemTagRow)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IItemTagRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] IItemTagRow other)
        {
            // TODO: Implement ArePropertiesEqual(IItemTagRow)
            throw new NotImplementedException();
        }
    }
}
