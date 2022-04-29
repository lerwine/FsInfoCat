using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
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

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalItemTagRow other)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] IItemTagRow other)
        {
            throw new NotImplementedException();
        }
    }
}
