using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public abstract class ItemTag : LocalDbEntity, ILocalItemTag
    {
        private readonly IPropertyChangeTracker<Guid> _taggedId;
        private readonly IPropertyChangeTracker<Guid> _definitionId;
        private readonly IPropertyChangeTracker<string> _notes;

        protected abstract ILocalDbEntity GetTagged();

        protected abstract ILocalTagDefinition GetDefinition();

        [Key]
        public virtual Guid TaggedId
        {
            get => _taggedId.GetValue();
            set
            {
                if (_taggedId.SetValue(value))
                    OnTaggedIdChanged(value);
            }
        }

        [Key]
        public virtual Guid DefinitionId
        {
            get => _definitionId.GetValue();
            set
            {
                if (_definitionId.SetValue(value))
                    OnDefinitionIdChanged(value);
            }
        }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        IDbEntity IItemTag.Tagged => GetTagged();

        ITagDefinition IItemTag.Definition => GetDefinition();

        ILocalDbEntity ILocalItemTag.Tagged => GetTagged();

        ILocalTagDefinition ILocalItemTag.Definition => GetDefinition();

        IEnumerable<Guid> IHasCompoundIdentifier.Id
        {
            get
            {
                yield return DefinitionId;
                yield return TaggedId;
            }
        }

        (Guid , Guid) IHasIdentifierPair.Id => (DefinitionId, TaggedId);

        protected abstract void OnTaggedIdChanged(Guid value);

        protected abstract void OnDefinitionIdChanged(Guid value);

        protected void SetTaggedId(Guid value) => _taggedId.SetValue(value);

        protected void SetDefinitionId(Guid value) => _definitionId.SetValue(value);

        public ItemTag()
        {
            _taggedId = AddChangeTracker(nameof(TaggedId), Guid.Empty);
            _definitionId = AddChangeTracker(nameof(DefinitionId), Guid.Empty);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
        }
    }
}
