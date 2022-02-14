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
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
