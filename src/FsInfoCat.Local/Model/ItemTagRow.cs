using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    // TODO: Document ItemTagRow class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
