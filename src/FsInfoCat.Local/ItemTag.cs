using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public abstract class ItemTagListItem : ItemTagRow, ILocalItemTagListItem
    {
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<string> _description;
        private readonly IPropertyChangeTracker<Guid> _taggedId;
        private readonly IPropertyChangeTracker<Guid> _definitionId;

        [Required]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Description { get => _description.GetValue(); set => _description.SetValue(value); }

        public override Guid TaggedId { get => _taggedId.GetValue(); set => _taggedId.SetValue(value); }

        public override Guid DefinitionId { get => _definitionId.GetValue(); set => _definitionId.SetValue(value); }

        public ItemTagListItem()
        {
            _name = AddChangeTracker(nameof(Name), "", NormalizedOrEmptyStringCoersion.Default);
            _description = AddChangeTracker(nameof(Description), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _taggedId = AddChangeTracker(nameof(TaggedId), Guid.Empty);
            _definitionId = AddChangeTracker(nameof(DefinitionId), Guid.Empty);
        }
    }
    public abstract class ItemTagRow : LocalDbEntity, ILocalItemTagRow
    {
        private readonly IPropertyChangeTracker<string> _notes;

        [Required]
        public abstract Guid TaggedId { get; set; }

        [Required]
        public abstract Guid DefinitionId { get; set; }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        IEnumerable<Guid> IHasCompoundIdentifier.Id
        {
            get
            {
                yield return DefinitionId;
                yield return TaggedId;
            }
        }

        (Guid, Guid) IHasIdentifierPair.Id => (DefinitionId, TaggedId);

        public ItemTagRow()
        {
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
        }
    }
    public abstract class ItemTag : ItemTagRow, ILocalItemTag
    {
        private readonly IPropertyChangeTracker<Guid> _taggedId;
        private readonly IPropertyChangeTracker<Guid> _definitionId;

        public override Guid TaggedId
        {
            get => _taggedId.GetValue();
            set
            {
                if (_taggedId.SetValue(value))
                    OnTaggedIdChanged(value);
            }
        }

        public override Guid DefinitionId
        {
            get => _definitionId.GetValue();
            set
            {
                if (_definitionId.SetValue(value))
                    OnDefinitionIdChanged(value);
            }
        }

        protected abstract ILocalDbEntity GetTagged();

        protected abstract ILocalTagDefinition GetDefinition();

        IDbEntity IItemTag.Tagged => GetTagged();

        ITagDefinition IItemTag.Definition => GetDefinition();

        ILocalDbEntity ILocalItemTag.Tagged => GetTagged();

        ILocalTagDefinition ILocalItemTag.Definition => GetDefinition();

        protected abstract void OnTaggedIdChanged(Guid value);

        protected abstract void OnDefinitionIdChanged(Guid value);

        protected void SetTaggedId(Guid value) => _taggedId.SetValue(value);

        protected void SetDefinitionId(Guid value) => _definitionId.SetValue(value);

        public ItemTag()
        {
            _taggedId = AddChangeTracker(nameof(TaggedId), Guid.Empty);
            _definitionId = AddChangeTracker(nameof(DefinitionId), Guid.Empty);
        }
    }
}
