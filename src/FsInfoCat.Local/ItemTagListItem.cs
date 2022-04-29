using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class ItemTagListItem : ItemTagRow, ILocalItemTagListItem, IEquatable<ItemTagListItem>
    {
        private string _name = string.Empty;
        private string _description = string.Empty;

        public override Guid TaggedId { get; set; }

        public override Guid DefinitionId { get; set; }

        [Required]
        [NotNull]
        [BackingField(nameof(_name))]
        public virtual string Name { get => _name; set => _name = value.AsWsNormalizedOrEmpty(); }

        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_description))]
        public virtual string Description { get => _description; set => _description = value.AsWsNormalizedOrEmpty(); }

        public bool Equals(ItemTagListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IItemTagListItem other)
        {
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
            if (taggedId.Equals(Guid.Empty) && definitionId.Equals(Guid.Empty))
                throw new NotImplementedException();
            // TODO: Implement GetHashCode()
            return HashCode.Combine(taggedId, definitionId);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
