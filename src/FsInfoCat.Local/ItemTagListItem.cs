using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public abstract class ItemTagListItem : ItemTagRow, ILocalItemTagListItem, IEquatable<ItemTagListItem>
    {
        private string _name = string.Empty;
        private string _description = string.Empty;

        [Required]
        public virtual string Name { get => _name; set => _name = value.AsWsNormalizedOrEmpty(); }

        [Required(AllowEmptyStrings = true)]
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
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            Guid taggedId = TaggedId;
            Guid definitionId = DefinitionId;
            if (taggedId.Equals(Guid.Empty) && DefinitionId.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + Name.GetHashCode();
                    hash = hash * 23 + Description.GetHashCode();
                    hash = hash * 23 + Notes.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 23);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 23);
                    hash = hash * 23 + CreatedOn.GetHashCode();
                    hash = hash * 23 + ModifiedOn.GetHashCode();
                    return hash;
                }
            unchecked
            {
                return EntityExtensions.HashGuid(definitionId, EntityExtensions.HashGuid(taggedId, 3, 7), 7);
            }
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
