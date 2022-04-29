using System;

namespace FsInfoCat.Local
{
    public abstract class ItemTag : ItemTagRow, ILocalItemTag
    {
        protected abstract ILocalDbEntity GetTagged();

        protected abstract ILocalTagDefinition GetDefinition();

        public abstract bool TryGetDefinitionId(out Guid definitionId);

        public abstract bool TryGetTaggedId(out Guid taggedId);

        IDbEntity IItemTag.Tagged => GetTagged();

        ITagDefinition IItemTag.Definition => GetDefinition();

        ILocalDbEntity ILocalItemTag.Tagged => GetTagged();

        ILocalTagDefinition ILocalItemTag.Definition => GetDefinition();
    }
}
