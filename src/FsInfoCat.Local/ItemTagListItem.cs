﻿using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public abstract class ItemTagListItem : ItemTagRow, ILocalItemTagListItem
    {
        private string _name = string.Empty;
        private string _description = string.Empty;

        [Required]
        public virtual string Name { get => _name; set => _name = value.AsWsNormalizedOrEmpty(); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Description { get => _description; set => _description = value.AsWsNormalizedOrEmpty(); }
    }
}