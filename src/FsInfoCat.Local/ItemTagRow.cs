using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public abstract class ItemTagRow : LocalDbEntity, ILocalItemTagRow
    {
        private string _notes = string.Empty;

        [Required]
        public virtual Guid TaggedId { get; set; }

        [Required]
        public virtual Guid DefinitionId { get; set; }

        [Required(AllowEmptyStrings = true)]
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
    }
}