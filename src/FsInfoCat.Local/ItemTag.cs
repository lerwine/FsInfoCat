namespace FsInfoCat.Local
{
    public abstract class ItemTag : ItemTagRow, ILocalItemTag
    {
        protected abstract ILocalDbEntity GetTagged();

        protected abstract ILocalTagDefinition GetDefinition();

        IDbEntity IItemTag.Tagged => GetTagged();

        ITagDefinition IItemTag.Definition => GetDefinition();

        ILocalDbEntity ILocalItemTag.Tagged => GetTagged();

        ILocalTagDefinition ILocalItemTag.Definition => GetDefinition();
    }
}
